using Banking.Contracts.Response.IdentityServer;
using Banking.Contracts.V1;
using Finance.Contracts.Response.Reports;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Puchase_and_payables.Contracts.Response.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Reporter.Controllers
{
    public class ReporterController : Controller
    {
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int maxRetryTimes = 10;
        private readonly IHttpContextAccessor _accessor;
        private readonly IHttpClientFactory _httpClientFactory;
        public ReporterController(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _accessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy.Handle<HttpRequestException>()

             .WaitAndRetryAsync(maxRetryTimes, times =>

             TimeSpan.FromSeconds(times * 2));
        }
        [HttpGet] 
        public IActionResult GetOfferLetterReport()
        {
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Cities.xlsx");
            byte[] bytes = System.IO.File.ReadAllBytes(pdfPath);
            return File(bytes, "application/xlsx");
        }

        [HttpGet]
        public  IActionResult OfferLetterReport(string applicationRefNo)
        {
            OfferLetterDetailObj response1 = new OfferLetterDetailObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            ReportRespObj response2 = new ReportRespObj();
            if (string.IsNullOrEmpty(applicationRefNo))
            { 
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false} });
            }
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result1 = new HttpResponseMessage();
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result1 = gosGatewayClient.GetAsync($"/loanapplication/generate/offerletter?ApplicationReference={applicationRefNo}").Result;
                    if (!result1.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data1 = result1.Content.ReadAsStringAsync().Result;
                    response1 = JsonConvert.DeserializeObject<OfferLetterDetailObj>(data1);
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/generate/offerletter/schedule?ApplicationRef={applicationRefNo}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        OfferLetterDetails = response1,
                        OfferLetterRepayments = response2.OfferLetterRepayments,
                        ProductFees = response2.ProductFees
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult OfferLetterReportLMS(string applicationRefNo)
        {
            ReportRespObj response2 = new ReportRespObj();
            if (string.IsNullOrEmpty(applicationRefNo))
            {
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result1 = new HttpResponseMessage();
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/generate/offerletter/lms?ApplicationRef={applicationRefNo}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        OfferLetterDetails = response2.OfferLetterDetails,
                        OfferLetterRepayments = response2.OfferLetterRepayments,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult OfferLetterReportInvestment(string applicationRefNo)
        {
            ReportRespObj response2 = new ReportRespObj();
            if (string.IsNullOrEmpty(applicationRefNo))
            {
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result1 = new HttpResponseMessage();
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/generate/offerletter/investment?ApplicationRef={applicationRefNo}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        OfferLetterDetails = response2.OfferLetterDetails,
                        OfferLetterRepayments = response2.OfferLetterRepayments,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult InvestmentReport(DateTime date1, DateTime date2)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result1 = new HttpResponseMessage();
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/investment/report?date1={date1}&date2={date2}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        Investments = response2.Investments,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult LoanReport(DateTime date1, DateTime date2)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result1 = new HttpResponseMessage();
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/loan/report?date1={date1}&date2={date2}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        Loans = response2.Loans,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult LoanIndividualCustomerReport(DateTime date1, DateTime date2, int customerTypeId)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/loan/individual/customer/report?date1={date1}&date2={date2}&customerTypeId={customerTypeId}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        IndividualCustomers = response2.IndividualCustomers,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult LoanCorporateCustomerReport(DateTime date1, DateTime date2, int customerTypeId)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/loan/corporate/customer/report?date1={date1}&date2={date2}&customerTypeId={customerTypeId}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        CorporateCustomers = response2.CorporateCustomers,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult InvestmentIndividualCustomerReport(DateTime date1, DateTime date2, int customerTypeId)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/investment/individual/customer/report?date1={date1}&date2={date2}&customerTypeId={customerTypeId}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        IndividualInvestorCustomers = response2.IndividualInvestorCustomers,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult InvestmentCorporateCustomerReport(DateTime date1, DateTime date2, int customerTypeId)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/loanapplication/investment/corporate/customer/report?date1={date1}&date2={date2}&customerTypeId={customerTypeId}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        CorporateInvestorCustomers = response2.CorporateInvestorCustomers,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult FinancialStatementReport(DateTime date1, DateTime date2)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result1 = new HttpResponseMessage();
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/gl/generate/fsreport?date1={date1}&date2={date2}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        FSreports = response2.FSreports,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }

        public IActionResult ProfitandLossReport(DateTime date1, DateTime date2)
        {
            ReportRespObj response2 = new ReportRespObj();
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                HttpResponseMessage result1 = new HttpResponseMessage();
                HttpResponseMessage result2 = new HttpResponseMessage();
                try
                {
                    result2 = gosGatewayClient.GetAsync($"/gl/generate/plreport?date1={date1}&date2={date2}").Result;
                    if (!result2.IsSuccessStatusCode)
                    {
                        return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
                    }
                    var data2 = result2.Content.ReadAsStringAsync().Result;
                    response2 = JsonConvert.DeserializeObject<ReportRespObj>(data2);
                }
                catch (Exception ex) { throw new HttpRequestException(); }

                if (response2.Status.IsSuccessful)
                {
                    var dto = new ReportRespObj
                    {
                        PLreports = response2.PLreports,
                    };
                    return View(dto);
                }
                return View(new ReportRespObj { Status = new APIResponseStatus { IsSuccessful = false } });
            }
            catch (Exception ex)
            {
                return View(response2.Status);
            }
        }   

        public IActionResult LPO(int lpoid)
        {
            var  response = new LPOReport { Phases = new List<Phases>(),
                ServiceTerm = new ServiceTerm {  Content = "", Header = ""},
                From = new From { SupplierId = 0, Number = "", Name ="", Address =""},
                To = new To { Number = "", Name = "", Address = "" ,},
                Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = ""}, IsSuccessful = true,},
            };

            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                var result = gosGatewayClient.GetAsync($"/report/get/lpo/report?LPOId={lpoid}").Result;
               // var result = gosGatewayClient.GetAsync($"{Puchase_and_payables.Contracts.V1.ApiRoutes.Report.LPO_REPORT}?LPOId={lpoid}").Result;
                var stringdata = result.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<LPOReport>(stringdata);
                if(response.ServiceTerm == null)
                {
                    response.ServiceTerm = new ServiceTerm();
                }
                if (response.To == null)
                {
                    response.To = new To();
                }
                if (response.From == null)
                {
                    response.From = new From();
                }
              
                if (!result.IsSuccessStatusCode)
                {
                    return View(response);
                }; 
                return View(response);
            }
            catch (Exception ex)
            {
                return View(response);
            }
        }

        public IActionResult PurchaseAndPayablesReport(PurchAndPayaReportResp report)
        {  
            try
            {   
                return View(report);
            }
            catch (Exception ex)
            {
                return View(new PurchAndPayaReportResp());
            }
        }
    }
}
