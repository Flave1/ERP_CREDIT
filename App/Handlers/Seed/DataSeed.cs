using Banking.Data;
using Banking.DomainObjects.Credit;
using GODP.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace Banking.Handlers.Deposit
{
    public static class DataSeed
    {
         
        public static void Seedrepayment(DataContext context)
        {
            var repayments = new List<credit_repaymenttype>
            { 
                new credit_repaymenttype{RepaymentTypeId = 1, RepaymentTypeName = "Upfront", Active = true, Deleted = false, },
                new credit_repaymenttype{RepaymentTypeId = 2, RepaymentTypeName = "Bullet", Active = true, Deleted = false, },
                new credit_repaymenttype{RepaymentTypeId = 3, RepaymentTypeName = "Spread", Active = true, Deleted = false, },
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in repayments)
                    {
                        if (!context.credit_repaymenttype.Any(w => w.RepaymentTypeId == item.RepaymentTypeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }
         
        public static void SeedFrequencyType(DataContext context)
        {
            var frequencytype = new List<credit_frequencytype>
            {
                new credit_frequencytype{ FrequencyTypeId = 1, Value = 1, Days = 365, IsVisible = true, Mode = "Yearly", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 2, Value = 2, Days = 182, IsVisible = true, Mode = "Twice-Yearly", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 3, Value = 4, Days = 91, IsVisible = true, Mode = "Quarterly", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 4, Value = 6, Days = 61, IsVisible = true, Mode = "Six- times-Yearly", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 5, Value = 12, Days = 30, IsVisible = true, Mode = "Monthly", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 6, Value = 24, Days = 15, IsVisible = true, Mode = "Twice-Monthly", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 7, Value = 52, Days = 7, IsVisible = true, Mode = "Weekly", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 8, Value = 365, Days = 1, IsVisible = true, Mode = "Daily", Active = true, Deleted = false, },
                new credit_frequencytype{ FrequencyTypeId = 9, Value = 3, Days = 122, IsVisible = true, Mode = "Thrice-Yearly", Active = true, Deleted = false, }
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in frequencytype)
                    {
                        if (!context.credit_frequencytype.Any(w => w.FrequencyTypeId == item.FrequencyTypeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedLoanScheduleType(DataContext context)
        {
            var loanschedule = new List<credit_loanscheduletype>
            {
                new credit_loanscheduletype{ LoanScheduleTypeId = 1, LoanScheduleTypeName = "Annuity", LoanScheduleCategoryId = 1,  Active = true, Deleted = false, },
                new credit_loanscheduletype{ LoanScheduleTypeId = 2, LoanScheduleTypeName = "Reducing Balance", LoanScheduleCategoryId = 1,  Active = true, Deleted = false, },
                new credit_loanscheduletype{ LoanScheduleTypeId = 3, LoanScheduleTypeName = "Bullet Payment", LoanScheduleCategoryId = 1,  Active = true, Deleted = false, },
                new credit_loanscheduletype{ LoanScheduleTypeId = 4, LoanScheduleTypeName = "Irregular Schedule", LoanScheduleCategoryId = 2,  Active = true, Deleted = false, },
                new credit_loanscheduletype{ LoanScheduleTypeId = 5, LoanScheduleTypeName = "Constant Principal and Interest", LoanScheduleCategoryId = 1,  Active = true, Deleted = false, },
                new credit_loanscheduletype{ LoanScheduleTypeId = 6, LoanScheduleTypeName = "Ballon Payment", LoanScheduleCategoryId = 1,  Active = true, Deleted = false, },
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in loanschedule)
                    {
                        if (!context.credit_loanscheduletype.Any(w => w.LoanScheduleTypeId == item.LoanScheduleTypeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedCreditRiskCategory(DataContext context)
        {
            var creditriskcategory = new List<credit_creditriskcategory>
            {
                new credit_creditriskcategory{ CreditRiskCategoryId = 1, CreditRiskCategoryName = "Application Score Cards", Description = "Score at initial recognition", UseInOrigination = true,  Active = true, Deleted = false, },
                new credit_creditriskcategory{ CreditRiskCategoryId = 2, CreditRiskCategoryName = "Behavioural Score Card", Description = "Behaviour of the facility", UseInOrigination = false,  Active = true, Deleted = false, },

            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in creditriskcategory)
                    {
                        if (!context.credit_creditriskcategory.Any(w => w.CreditRiskCategoryId == item.CreditRiskCategoryId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedDepositAccountSetup(DataContext context)
        {
            var depositAccount = new List<deposit_accountsetup>
            {
                new deposit_accountsetup{ DepositAccountId = 1, AccountName = "Agric Development Bank", AccountTypeId = 1, CurrencyId=1, DormancyDays="2", InitialDeposit= 5000,  CategoryId = 1, GLMapping=1, BankGl=1, InterestRate=5, InterestType="Simple", CheckCollecting=true, MaturityType="Fixed", InUse=true, Active = true, Deleted = false, },              
                new deposit_accountsetup{ DepositAccountId = 2, AccountName = "Student School Fees", AccountTypeId = 1, CurrencyId=1, DormancyDays="2", InitialDeposit= 5000,  CategoryId = 1, GLMapping=1, BankGl=1, InterestRate=5, InterestType="Simple", CheckCollecting=true, MaturityType="Fixed", InUse=true, Active = true, Deleted = false, },              
                new deposit_accountsetup{ DepositAccountId = 3, AccountName = "Operating Account", AccountTypeId = 2, CurrencyId=1, DormancyDays="2", InitialDeposit= 10000,  CategoryId = 1, GLMapping=1, BankGl=1, InterestRate=5, InterestType="Simple", CheckCollecting=true, MaturityType="Fixed", InUse=true, Active = true, Deleted = false, },              
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in depositAccount)
                    {
                        if (!context.deposit_accountsetup.Any(w => w.DepositAccountId == item.DepositAccountId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedDepositAccountType(DataContext context)
        {
            var depositAccount = new List<deposit_accountype>
            {
                new deposit_accountype{ AccountTypeId = 1, Name = "Saving Account", Description = "Saving Account",  Active = true, Deleted = false, },
                new deposit_accountype{ AccountTypeId = 2, Name = "Current Account", Description = "Current Account",  Active = true, Deleted = false, },
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in depositAccount)
                    {
                        if (!context.deposit_accountype.Any(w => w.AccountTypeId == item.AccountTypeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();


                Seedrepayment(context);
                SeedFrequencyType(context);
                SeedLoanScheduleType(context);
                SeedCreditRiskCategory(context);
                SeedDepositAccountSetup(context);
                SeedDepositAccountType(context);
                SeedExposureParameter(context);
                SeedSystemAttribute(context);
                SeedDayCount(context);
            }
            return host;
        }

        public static void SeedExposureParameter(DataContext context)
        {
            var exposure = new List<credit_exposureparament>
            {
                new credit_exposureparament{ ExposureParameterId = 1, CustomerTypeId = 1, Percentage = 80, ShareHolderAmount =5000000, Description = "Share Holder's Funds", Amount =4000000,  Active = true, Deleted = false, },
                new credit_exposureparament{ ExposureParameterId = 2, CustomerTypeId = 2, Percentage = 80, ShareHolderAmount =5000000, Description = "Share Holder's Funds", Amount =4000000,  Active = true, Deleted = false, },
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in exposure)
                    {
                        if (!context.credit_exposureparament.Any(w => w.ExposureParameterId == item.ExposureParameterId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedSystemAttribute(DataContext context)
        {
            var systemattributes = new List<credit_systemattribute>
            {
                new credit_systemattribute{ SystemAttributeId = 1, SystemAttributeName = "Age", Active = true, Deleted = false, },
                new credit_systemattribute{ SystemAttributeId = 2, SystemAttributeName = "Gender", Active = true, Deleted = false, },
                new credit_systemattribute{ SystemAttributeId = 3, SystemAttributeName = "No of days after default", Active = true, Deleted = false, },
                new credit_systemattribute{ SystemAttributeId = 4, SystemAttributeName = "Others", Active = true, Deleted = false, },
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in systemattributes)
                    {
                        if (!context.credit_systemattribute.Any(w => w.SystemAttributeId == item.SystemAttributeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedDayCount(DataContext context)
        {
            var daycount = new List<credit_daycountconvention>
            {
                new credit_daycountconvention{ DayCountConventionId = 1, DayCountConventionName = "US (NASD) 30/360", DaysInAYear = 360, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 2, DayCountConventionName = "Actual/Actual", DaysInAYear = -1, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 3, DayCountConventionName = "Actual/360", DaysInAYear = 360, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 4, DayCountConventionName = "Actual/365", DaysInAYear = 365, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 5, DayCountConventionName = "European 30/360", DaysInAYear = 360, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 6, DayCountConventionName = "30/360 ISDA", DaysInAYear = 360, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 7, DayCountConventionName = "No Leap Year /365", DaysInAYear = 365, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 8, DayCountConventionName = "No Leap Year /360", DaysInAYear = 360, IsVisible = true, Active = true, Deleted = false, },
                new credit_daycountconvention{ DayCountConventionId = 9, DayCountConventionName = "Actual/364", DaysInAYear = 364, IsVisible = true, Active = true, Deleted = false, },

            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in daycount)
                    {
                        if (!context.credit_daycountconvention.Any(w => w.DayCountConventionId == item.DayCountConventionId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }
    }
}
