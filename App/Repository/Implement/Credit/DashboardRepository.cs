using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries.GOS_API_Response;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;
using static Banking.Contracts.Response.Credit.LoanObjs;

namespace Banking.Repository.Implement
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IIdentityServerRequest _serverRequest;
        private readonly DataContext _dataContext;
        private readonly bool _getAwaitingCount;
        private readonly Dictionary<int, string> MonthNumberPair;
        private readonly string[] BackgroundColor;
        private readonly string[] HoverBackgroundColor;
        private Int32 disburseCount = 0, creditAppraisalCount = 0, investmentCount1 = 0, liquidationCount1 = 0, collectionCount1 = 0;
        public DashboardRepository(DataContext context, IIdentityServerRequest serverRequest)
        {
            _dataContext = context;
            _serverRequest = serverRequest;

            _getAwaitingCount = GetAwaitingApproval().Result;
            MonthNumberPair = new Dictionary<int, string>
            {
                {1, "Jan" },
                {2, "Feb" },
                {3, "Mar" },
                {4, "Apr" },
                {5, "May" },
                {6, "Jun" },
                {7, "Jul" },
                {8, "Aug" },
                {9, "Sept" },
                {10, "Oct" },
                {11, "Nov" },
                {12, "Dec" }
            };

            BackgroundColor = new string[]
            {
                "#00ffbf","#00a689","#f44666","#bc3e55","#55ccb6",
                "#2a7c6d","#2592a3", "#00ff72","#aeff00","#ffe500",
                "#08fbbf","#f24366","#bc2c55","#49ccb6","#009f89",
                "#12bb72","#10fc6d","#1093a3","#bb4500", "#ea8f00",
            };
            HoverBackgroundColor = new string[]
            {
                "#00ffbf","#f44666","#bc3e55","#55ccb6","#00a689",
                "#00ff72","#2a7c6d","#2592a3","#ffe500", "#aeff00",
                "#2a7c6d","#2592a3", "#00ff72","#aeff00","#ffe500",
                "#12bb72","#10fc6d","#1093a3","#bb4500", "#ea8f00",
            };
        }
        public CustomerDashboardObj GetCustomerTransactionSummary(string accountNumber)
        {
            var inflow = _dataContext.fin_customertransaction
                .Where(o => o.TransactionDate.Value.Year == DateTime.Now.Year && o.AccountNumber == accountNumber && o.TransactionType == "Credit")
                .Select(x => new { x.TransactionDate, x.Amount }).GroupBy(y => new { y.TransactionDate.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    Count = group.Count(),
                    Sum = group.Sum(k => k.Amount)
                }).ToList();

            var outflow = _dataContext.fin_customertransaction
                .Where(o => o.TransactionDate.Value.Year == DateTime.Now.Year && o.AccountNumber == accountNumber && o.TransactionType == "Debit")
                .Select(x => new { x.TransactionDate, x.Amount }).GroupBy(y => new { y.TransactionDate.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    Count = group.Count(),
                    Sum = group.Sum(k => k.Amount)
                }).ToList();

            List<decimal> inflowCount = new List<decimal>();
            List<decimal> outflowCount = new List<decimal>();

            foreach (var app in MonthNumberPair)
            {
                var result = inflow.FirstOrDefault(x => x.MonthNumber == app.Key);
                decimal count = 0;

                if (result != null)
                {
                    count = result.Sum??0;
                }

                inflowCount.Add(count);
            }

            foreach (var app in MonthNumberPair)
            {
                var result = outflow.FirstOrDefault(x => x.MonthNumber == app.Key);
                decimal count = 0;

                if (result != null)
                {
                    count = result.Sum ?? 0;
                }

                outflowCount.Add(count);
            }


            var performanceMatrics = new CustomerDashboardObj
            {
                Labels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" },
                Datasets = new List<CustomerDatasetObj>
            {
                new CustomerDatasetObj
                {
                    Label = "Inflow",
                    BackgroundColor = "#42A5F5",
                    BorderColor = "1E88E5",
                    Data = inflowCount.ToArray(),
                },
                new CustomerDatasetObj
                {
                    Label = "Outflow",
                    BackgroundColor = "#9CCC65",
                    BorderColor = "7CB342",
                    Data = outflowCount.ToArray(),
                }
            }
            };
            return performanceMatrics;
        }

        public InvestmentApplicationDetailObj GetInvestmentApplicationDetails()
        {
            var pendingInvestment = _dataContext.inf_investorfund_website.Where(x => x.Deleted == false && x.ApprovalStatus == 1).Count();
            var pendingCollection = _dataContext.inf_collection_website.Where(x => x.Deleted == false && x.ApprovalStatus == 1).Count();
            var pendingLiquidation = _dataContext.inf_liquidation_website.Where(x => x.Deleted == false && x.ApprovalStatus == 1).Count();
            var pendingTopup = _dataContext.inf_investorfund_topup_website.Where(x => x.Deleted == false && x.ApprovalStatus == 1).Count();
            var pendingRollover = _dataContext.inf_investorfund_rollover_website.Where(x => x.Deleted == false && x.ApprovalStatus == 1).Count();
            var investmentAprraisal = investmentCount1;
            var liquidationAppraisal = liquidationCount1;
            var collectionAppraisal = collectionCount1;

            return new InvestmentApplicationDetailObj
            {
                PendingInvestment = pendingInvestment,
                PendingCollection = pendingCollection,
                PendingLiquidation = pendingLiquidation,
                InvestmentAppraisal = investmentAprraisal,
                LiquidationAppraisal = liquidationAppraisal,
                CollectionAppraisal = collectionAppraisal,
                TopUpAppraisal = pendingTopup,
                RollOverAppraisal = pendingRollover
            };
        }

        public LoanConcentrationObj GetInvestmentConcentrationDetails()
        {
            var productList = _dataContext.inf_product.Where(x => x.Deleted == false).ToList();
            var investmentList = _dataContext.inf_investorfund.Where(x => x.Deleted == false).ToList();

            var InvestmentConcentrations = productList.Select(a => new ProductVolume
            {
                ProductName = _dataContext.inf_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                TotalVolume = investmentList.Where(x => x.ProductId == a.ProductId).Sum(x => x.ApprovedAmount),
            }).ToList();
                var count = InvestmentConcentrations.Count();
                var usableColors = new List<string>();

                for (var i = 0; i < count; i++)
                {
                    usableColors.Add(BackgroundColor[i]);
                }

                var investorFundConcentration = new LoanConcentrationObj
                {
                    Labels = InvestmentConcentrations.Select(x => x.ProductName).ToArray(),
                    Datasets = new DatasetLoanConcentrationObj
                    {
                        BackgroundColor = usableColors.ToArray(),
                        HoverBackgroundColor = usableColors.ToArray(),
                        Data = InvestmentConcentrations.Select(x => x.TotalVolume).ToArray()
                    }
                };
                return investorFundConcentration;
        }

        public DashboardObj GetInvestmentPerformanceChart()
        {
            var InvestmentApplication = _dataContext.inf_investorfund
                .Where(o => o.CreatedOn.Value.Year == DateTime.Now.Year && o.Deleted == false)
                .Select(x => new { x.CreatedOn, x.InvestorFundId }).GroupBy(y => new { y.CreatedOn.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    Count = group.Count()
                }).ToList();

            var CollectionApplication = _dataContext.inf_collection
                .Where(o => o.CreatedOn.Value.Year == DateTime.Now.Year && o.Deleted == false)
                .Select(x => new { x.CreatedOn, x.CollectionId }).GroupBy(y => new { y.CreatedOn.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    Count = group.Count()
                }).ToList();

            var LiquidationApplication = _dataContext.inf_liquidation
                .Where(o => o.CreatedOn.Value.Year == DateTime.Now.Year && o.Deleted == false)
                .Select(x => new { x.CreatedOn, x.LiquidationId }).GroupBy(y => new { y.CreatedOn.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    Count = group.Count()
                }).ToList();

            List<int> investmentCount = new List<int>();
            List<int> collectionCount = new List<int>();
            List<int> liquidationCount = new List<int>();

            foreach (var app in MonthNumberPair)
            {
                var result = InvestmentApplication.FirstOrDefault(x => x.MonthNumber == app.Key);
                var count = 0;

                if (result != null)
                {
                    count = result.Count;
                }

                investmentCount.Add(count);
            }

            foreach (var app in MonthNumberPair)
            {
                var result = CollectionApplication.FirstOrDefault(x => x.MonthNumber == app.Key);
                var count = 0;

                if (result != null)
                {
                    count = result.Count;
                }

                collectionCount.Add(count);
            }

            foreach (var app in MonthNumberPair)
            {
                var result = LiquidationApplication.FirstOrDefault(x => x.MonthNumber == app.Key);
                var count = 0;

                if (result != null)
                {
                    count = result.Count;
                }

                liquidationCount.Add(count);
            }


            var performanceMatrics = new DashboardObj
            {
                Labels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" },
                Datasets = new List<Dataset>
            {
                new Dataset
                {
                    Label = "Investment Application",
                    BorderColor = "#4bc0c0",
                    Data = investmentCount.ToArray(),
                    Fill = false,

                },
                new Dataset
                {
                    Label = "Collection Application",
                    BorderColor = "#565656",
                    Data = collectionCount.ToArray(),
                    Fill = false,

                },
                new Dataset
                {
                    Label = "Liquidation Application",
                    BorderColor = "#903656",
                    Data = liquidationCount.ToArray(),
                    Fill = false,
                }
            }
            };

            return performanceMatrics;
        }

        public LoanConcentrationObj GetLoanConcentrationDetails()
        {
            var productList = _dataContext.credit_product.Where(x => x.Deleted == false).ToList();
            var loanList = _dataContext.credit_loan.Where(x => x.Deleted == false).ToList();

            var loanConcentrations = productList.Select(a => new ProductVolume
            {
                ProductName = _dataContext.credit_product.FirstOrDefault(x => x.ProductId == a.ProductId).ProductName,
                TotalVolume = loanList.Where(x => x.ProductId == a.ProductId).Sum(x => x.OutstandingPrincipal),
            }).ToList();
            var count = loanConcentrations.Count();
            var usableColors = new List<string>();

            for (var i = 0; i < count; i++)
            {
                usableColors.Add(BackgroundColor[i]);
            }

            var loanConcentration = new LoanConcentrationObj
            {
                Labels = loanConcentrations.Select(x => x.ProductName).ToArray(),
                Datasets = new DatasetLoanConcentrationObj
                {
                    BackgroundColor = usableColors.ToArray(),
                    HoverBackgroundColor = usableColors.ToArray(),
                    Data = loanConcentrations.Select(x => x.TotalVolume).ToArray()
                }
            };

            return loanConcentration;
        }

        public LoanApplicationDetailObj GetLoansApplicationDetails()
        {

            var totalLoanApplicationInAppraisal = creditAppraisalCount;

            var totalLoanApplicationDisbursed = disburseCount;

            var totalLoanApplication = _dataContext.credit_loanapplication.Where(x => x.Deleted == false && x.LoanApplicationStatusId == 1 || x.LoanApplicationStatusId == 14).Count();
            var totalLoanApplicationwebsite = _dataContext.credit_loanapplication_website.Where(x => x.Deleted == false && x.LoanApplicationStatusId == 1).Count();


            var now = DateTime.Now.Date;
            var totalLoanApplicationPaymentDue = (from a in _dataContext.credit_loan
                                                  join b in _dataContext.credit_loanscheduleperiodic on a.LoanId equals b.LoanId
                                                  where b.PaymentDate.Date == now && b.Deleted == false
                                                  select b.PaymentDate).Count();
            var totalLoanApplicationOverdue = (from a in _dataContext.credit_loan
                                               join b in _dataContext.credit_loan_past_due on a.LoanId equals b.LoanId
                                               where b.Date.Date < now
                                               select b.Date).Count();

            return new LoanApplicationDetailObj
            {
                TotalLoanApplication = totalLoanApplication,
                TotalAappraisalLoan = totalLoanApplicationInAppraisal,
                TotalDisbursementLoan = totalLoanApplicationDisbursed,
                TotalPaymentDue = totalLoanApplicationPaymentDue,
                TotalOverDue = totalLoanApplicationOverdue,
                TotalLoanApplicationWebsite = totalLoanApplicationwebsite
            };
        }

        public LoanConcentrationObj GetOverDueForDashboard()
        {
            var creditLoanOverDue = (from a in _dataContext.credit_loan_past_due
                                     join b in _dataContext.credit_loan on a.LoanId equals b.LoanId
                                     //where a.PrincipalAmount <= 0
                                     group new { b.PastDuePrincipal, a.LoanId } by new { a.Date, a.LoanId } into g
                                     select new
                                     {
                                         g.Key.Date,
                                         PastDuePrincipal = g.FirstOrDefault(x => x.LoanId == g.Key.LoanId)
                                     }).ToList();

            var creditLoanOverDueConcat = new List<LoanOverDueDBResult>();
            var ids = creditLoanOverDue.Select(x => x.PastDuePrincipal.LoanId).Distinct();

            foreach (var id in ids)
            {
                var repaymentToOverDue = creditLoanOverDue.Where(x => x.PastDuePrincipal.LoanId == id).Sum(y => y.PastDuePrincipal.PastDuePrincipal);
                var earliestDate = creditLoanOverDue.Where(x => x.PastDuePrincipal.LoanId == id).OrderBy(y => y.Date).First();

                creditLoanOverDueConcat.Add(new LoanOverDueDBResult
                {
                    TotalPastDuePayment = repaymentToOverDue,
                    Range = ((TimeSpan)(DateTime.Today - earliestDate.Date)).Days
                });
            }

            var creditClassif = (from a in _dataContext.credit_creditclassification
                                 where a.Deleted == false
                                 select new
                                 {
                                     a.LowerLimit,
                                     a.UpperLimit,
                                     a.ProvisioningRequirement,
                                 }).ToList();

            var classificationName = new List<string>();
            var pastDuePrinciples = new List<decimal?>();
            var usableColors = new List<string>();
            decimal? provisioning = 0;

            for (var i = 0; i < creditClassif.Count; i++)
            {
                usableColors.Add(BackgroundColor[i]);
            }

            foreach (var credit in creditClassif)
            {
                classificationName.Add($"{credit.LowerLimit}" + " - " + $"{credit.UpperLimit}" + " days");

                var overCal = creditLoanOverDueConcat
                    .Where(x => credit.LowerLimit <= x.Range && credit.UpperLimit <= x.Range)?
                    .Sum(y => y.TotalPastDuePayment);

                provisioning += (credit.ProvisioningRequirement * overCal) / 100;

                pastDuePrinciples.Add(overCal);
            }

            var overDueAnalysis = new LoanConcentrationObj
            {
                Labels = classificationName.ToArray(),
                Datasets = new DatasetLoanConcentrationObj
                {
                    BackgroundColor = usableColors.ToArray(),
                    HoverBackgroundColor = usableColors.ToArray(),
                    Data = pastDuePrinciples.ToArray()
                },
                Provisioning = provisioning
            };
            return overDueAnalysis;
        }

        public DashboardObj GetPARForDashboard()
        {
            var creditLoan = _dataContext.credit_loan
                .Where(o => o.CreatedOn.Value.Year == DateTime.Now.Year && o.Deleted == false)
                .Select(x => new { x.CreatedOn, x.LoanApplicationId, x.PastDuePrincipal, x.OutstandingPrincipal }).GroupBy(y => new { y.CreatedOn.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    TotalPastDuePrincipal = group.Sum(x => x.PastDuePrincipal),
                    TotalOutstandingPrincipal = group.Sum(x => x.OutstandingPrincipal),
                    Percent = 0
                }).ToList();

            List<int> loanCount = new List<int>();

            foreach (var app in MonthNumberPair)
            {
                var result = creditLoan.FirstOrDefault(x => x.MonthNumber == app.Key);
                decimal? par = 0;

                if (result != null)
                {
                    par = (result.TotalPastDuePrincipal / result.TotalOutstandingPrincipal) * 100;
                }

                int laonparCalc = Decimal.ToInt32(par.Value);

                loanCount.Add(laonparCalc);
            }

            var parAnalysis = new DashboardObj
            {
                Labels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" },
                Datasets = new List<Dataset>
                {
                    new Dataset
                    {
                        Label = "PAR (%)",
                        BorderColor = "#1E88E5",
                        BackgroundColor = "#42A5F5",
                        Data = loanCount.ToArray(),
                        Fill = false,
                    },
                }
            };

            return parAnalysis;
        }

        public DashboardObj GetPerformanceMatrics()
        {
            var creditLoanApplication = (from a in _dataContext.credit_loanapplication
                           where a.Deleted == false && a.CreatedOn.Value.Year == DateTime.Now.Year
                           select new
                           {
                               Month = a.CreatedOn.Value.Month,
                               LoanApplicationId = a.LoanApplicationId,
                           }).GroupBy(x => new { x.Month}, (key, group) => new
                           {
                               MonthNumber = key.Month,
                               Count = group.Count(),
                           }).ToList();

            var disbursement = _dataContext.credit_loan
                .Where(o => o.CreatedOn.Value.Year == DateTime.Now.Year && o.Deleted == false)
                .Select(x => new { x.CreatedOn, x.LoanApplicationId }).GroupBy(y => new { y.CreatedOn.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    Count = group.Count()
                }).ToList();

            var closedLoans = _dataContext.credit_loan
                .Where(o => o.CreatedOn.Value.Year == DateTime.Now.Year && o.Deleted == false && o.MaturityDate < DateTime.Now)
                .Select(x => new { x.CreatedOn, x.LoanApplicationId }).GroupBy(y => new { y.CreatedOn.Value.Month }, (key, group) => new
                {
                    MonthNumber = key.Month,
                    Count = group.Count()
                }).ToList();

            List<int> loanDisbursementAnnualCount = new List<int>();
            List<int> creditApplicationAnnualCount = new List<int>();
            List<int> closedLoanAnnualCount = new List<int>();

            foreach (var app in MonthNumberPair)
            {
                var result = creditLoanApplication.FirstOrDefault(x => x.MonthNumber == app.Key);
                var count = 0;

                if (result != null)
                {
                    count = result.Count;
                }

                creditApplicationAnnualCount.Add(count);
            }

            foreach (var app in MonthNumberPair)
            {
                var result = disbursement.FirstOrDefault(x => x.MonthNumber == app.Key);
                var count = 0;

                if (result != null)
                {
                    count = result.Count;
                }

                loanDisbursementAnnualCount.Add(count);
            }

            foreach (var app in MonthNumberPair)
            {
                var result = closedLoans.FirstOrDefault(x => x.MonthNumber == app.Key);
                var count = 0;

                if (result != null)
                {
                    count = result.Count;
                }

                closedLoanAnnualCount.Add(count);
            }


            var performanceMatrics = new DashboardObj
            {
                Labels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" },
                Datasets = new List<Dataset>
            {
                new Dataset
                {
                    Label = "Monthly Application",
                    BorderColor = "#4bc0c0",
                    Data = creditApplicationAnnualCount.ToArray(),
                    Fill = false,

                },
                new Dataset
                {
                    Label = "Monthly Disbursement",
                    BorderColor = "#565656",
                    Data = loanDisbursementAnnualCount.ToArray(),
                    Fill = false,

                },
                new Dataset
                {
                    Label = "Monthly Closed Loans",
                    BorderColor = "#903656",
                    Data = closedLoanAnnualCount.ToArray(),
                    Fill = false,
                }
            }
            };

            return performanceMatrics;
        }

        public CustomerDashboardObj LoanStaging()
        {
            List<stagingObj> LoanStagingCount = new List<stagingObj>();
            List<string> label = new List<string>();
            List<decimal> sum = new List<decimal>();
            var count = _dataContext.credit_impairment_final
                .Where(o => o.Stage != null)
                .Select(x => new { x.Stage, x.EAD }).GroupBy(y => new { y.Stage }, (key, group) => new stagingObj
                {
                    stage = key.Stage,
                    count = (int)group.Sum(k => k.EAD)
                }).ToList();
            LoanStagingCount.AddRange(count);

            foreach (var app in LoanStagingCount)
            {
                label.Add(app.stage);
                //var new_count = app.count.ToString("#,##0.00");
                sum.Add(app.count);
            }

            var performanceMatrics = new CustomerDashboardObj
            {
                Labels = label.ToArray(),
                Datasets = new List<CustomerDatasetObj>
            {
                new CustomerDatasetObj
                {
                    Label = "Loan Staging",
                    BackgroundColor = "#42A5F5",
                    BorderColor = "1E88E5",
                    Data = sum.ToArray(),
                },
            }
            };
            return performanceMatrics;
        }


        //////PRIVATE METHODS
        ///
        public class ProductVolume
        {
            public string ProductName { get; set; }
            public decimal? TotalVolume { get; set; }
        }

        public class LoanOverDueDBResult
        {
            public int Id { get; set; }
            public int Range { get; set; }
            public decimal? TotalPastDuePayment { get; set; }
        }

        public async Task<bool> GetAwaitingApproval()
        {
            try
            {
                var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                if (!result.IsSuccessStatusCode)
                {
                    return false;
                }

                var data = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);

                if (res == null)
                {
                    return false;
                }

                if (res.workflowTasks.Count() > 0)
                {                  
                    var targetIds = res.workflowTasks.Select(x => x.TargetId).ToList();
                    var tokens = res.workflowTasks.Select(d => d.WorkflowToken).ToList();

                    disburseCount = await GetLoansAwaitingApprovalAsync(targetIds, tokens);
                    creditAppraisalCount = await GetLoanApplicationAwaitingApprovalAsync(targetIds, tokens);
                    investmentCount1 = await GetInvestmentAwaitingApprovalAsync(targetIds, tokens);
                    liquidationCount1 = await GetLiquidationAwaitingApprovalAsync(targetIds, tokens);
                    collectionCount1 = await GetCollectionAwaitingApprovalAsync(targetIds, tokens);
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
               
        }

        private async Task<Int32> GetLoansAwaitingApprovalAsync(List<int> LoanIds, List<string> tokens)
        {
            var item = await _dataContext.credit_loan
                .Where(s => LoanIds.Contains(s.LoanId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item.Count();
        }

        private async Task<Int32> GetLoanApplicationAwaitingApprovalAsync(List<int> LoanApplicationIds, List<string> tokens)
        {
            var item = await _dataContext.credit_loanapplication
                .Where(s => LoanApplicationIds.Contains(s.LoanApplicationId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item.Count();
        }

        private async Task<Int32> GetInvestmentAwaitingApprovalAsync(List<int> InvestfundIds, List<string> tokens)
        {
            var item = await _dataContext.inf_investorfund
                .Where(s => InvestfundIds.Contains(s.InvestorFundId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item.Count();
        }

        private async Task<Int32> GetCollectionAwaitingApprovalAsync(List<int> CollectionIds, List<string> tokens)
        {
            var item = await _dataContext.inf_collection
                .Where(s => CollectionIds.Contains(s.CollectionId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item.Count();
        }

        private async Task<Int32> GetLiquidationAwaitingApprovalAsync(List<int> liquidationIds, List<string> tokens)
        {
            var item = await _dataContext.inf_liquidation
                .Where(s => liquidationIds.Contains(s.LiquidationId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item.Count();
        }
    }
}
