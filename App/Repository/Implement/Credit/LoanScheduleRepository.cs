using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Enum;
using Banking.Repository.Interface.Credit;
using GOSLibraries.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using NodaTime;
using OfficeOpenXml;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;
using static Banking.Contracts.Response.Credit.LookUpViewObjs;
using LoanScheduleTypeEnum = GOSLibraries.Enums.LoanScheduleTypeEnum;
using TenorModeEnum = GOSLibraries.Enums.TenorModeEnum;
using Banking.Requests;

namespace Banking.Repository.Implement.Credit
{
    public class LoanScheduleRepository : ILoanScheduleRepository
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly IIdentityServerRequest _serverRequest;
        public LoanScheduleRepository(DataContext dataContext, 
            IIdentityService identityService, 
            IConfiguration configuration, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;
            _identityService = identityService;
            _configuration = configuration;
        }
        public async Task<bool> AddLoanSchedule(int loanId, LoanPaymentScheduleInputObj loanInput)
        {
            bool output = false;
            var user = await _serverRequest.UserDataAsync();
            var createdBy =  user.UserName;
            var applicationDate = DateTime.Now.Date;
            int staffId = user.StaffId;

            //---------------save irregular loan schedule input---------------------------
            List<credit_loanscheduleirrigular> tblIrregularSchedule = new List<credit_loanscheduleirrigular>();
            LoanScheduleTypeEnum scheduleMethod = (LoanScheduleTypeEnum)loanInput.ScheduleMethodId;
            if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
            {
                var data = loanInput.IrregularPaymentSchedule.OrderBy(x => x.PaymentDate);
                foreach (var item in data)
                {
                    credit_loanscheduleirrigular schedule = new credit_loanscheduleirrigular();
                    schedule.LoanId = loanId;
                    schedule.PaymentDate = item.PaymentDate;
                    schedule.PaymentAmount = Convert.ToDecimal(item.PaymentAmount);
                    schedule.StaffId = staffId;
                    schedule.CreatedOn = applicationDate;

                    tblIrregularSchedule.Add(schedule);
                }

            }

            //----------generate and save periodic loan schedule -----------------------------------
            List<LoanPaymentSchedulePeriodicObj> periodicSchedule = GeneratePeriodicLoanSchedule(loanInput);

            List<credit_loanscheduleperiodic> tblPeriodicSchedule = new List<credit_loanscheduleperiodic>();
            foreach (var item in periodicSchedule)
            {
                credit_loanscheduleperiodic schedule = new credit_loanscheduleperiodic();
                schedule.LoanId = loanId;
                schedule.PaymentNumber = item.PaymentNumber;
                schedule.PaymentDate = item.PaymentDate;
                schedule.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                schedule.PeriodPaymentAmount = Convert.ToDecimal(item.PeriodPaymentAmount);
                schedule.PeriodInterestAmount = Convert.ToDecimal(item.PeriodInterestAmount);
                schedule.PeriodPrincipalAmount = Convert.ToDecimal(item.PeriodPrincipalAmount);
                schedule.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                schedule.InterestRate = loanInput.InterestRate;

                schedule.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmortisedStartPrincipalAmount);
                schedule.AmortisedPeriodPaymentAmount = Convert.ToDecimal(item.AmortisedPeriodPaymentAmount);
                schedule.AmortisedPeriodInterestAmount = Convert.ToDecimal(item.AmortisedPeriodInterestAmount);
                schedule.AmortisedPeriodPrincipalAmount = Convert.ToDecimal(item.AmortisedPeriodPrincipalAmount);
                schedule.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmortisedEndPrincipalAmount);
                schedule.EffectiveInterestRate = item.EffectiveInterestRate;
                schedule.StaffId = staffId;
                schedule.CreatedOn = applicationDate;
                schedule.Deleted = false;

                tblPeriodicSchedule.Add(schedule);
            }

            //----------generate and save daily loan schedule -----------------------------------
            List<LoanPaymentScheduleDailyObj> dailySchedule = GenerateDailyLoanSchedule(loanInput);

            List<credit_loanscheduledaily> tblDailySchedule = new List<credit_loanscheduledaily>();

            foreach (var item in dailySchedule)
            {
                credit_loanscheduledaily schedule = new credit_loanscheduledaily();

                schedule.LoanId = loanId;
                schedule.PaymentNumber = item.PaymentNumber;
                schedule.Date = item.Date;
                schedule.PaymentDate = item.PaymentDate;
                schedule.OpeningBalance = Convert.ToDecimal(item.OpeningBalance);
                schedule.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                schedule.DailyPaymentAmount = Convert.ToDecimal(item.DailyPaymentAmount);
                schedule.DailyInterestAmount = Convert.ToDecimal(item.DailyInterestAmount);
                schedule.DailyPrincipalAmount = Convert.ToDecimal(item.DailyPrincipalAmount);
                schedule.ClosingBalance = Convert.ToDecimal(item.ClosingBalance);
                schedule.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                schedule.AccruedInterest = Convert.ToDecimal(item.AccruedInterest);
                schedule.AmortisedCost = Convert.ToDecimal(item.AmortisedCost);
                schedule.InterestRate = item.NorminalInterestRate;

                schedule.AmortisedOpeningBalance = Convert.ToDecimal(item.AmOpeningBalance);
                schedule.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmStartPrincipalAmount);
                schedule.AmortisedDailyPaymentAmount = Convert.ToDecimal(item.AmDailyPaymentAmount);
                schedule.AmortisedDailyInterestAmount = Convert.ToDecimal(item.AmDailyInterestAmount);
                schedule.AmortisedDailyPrincipalAmount = Convert.ToDecimal(item.AmDailyPrincipalAmount);
                schedule.AmortisedClosingBalance = Convert.ToDecimal(item.AmClosingBalance);
                schedule.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmEndPrincipalAmount);
                schedule.AmortisedAccruedInterest = Convert.ToDecimal(item.AmAccruedInterest);
                schedule.Amortised_AmortisedCost = Convert.ToDecimal(item.AmAmortisedCost);
                schedule.UnearnedFee = Convert.ToDecimal(item.UnEarnedFee);
                schedule.EarnedFee = Convert.ToDecimal(item.EarnedFee);
                schedule.EffectiveInterestRate = item.EffectiveInterestRate;
                schedule.StaffId = staffId;
                schedule.CreatedOn = applicationDate;
                schedule.Deleted = false;

                tblDailySchedule.Add(schedule);
            }

            //------------adding records to the database--------------------------

            this._dataContext.credit_loanscheduleperiodic.AddRange(tblPeriodicSchedule);

            this._dataContext.credit_loanscheduledaily.AddRange(tblDailySchedule);

            //----------update loan details -----------------------------------
            var loan = this._dataContext.credit_loan.FirstOrDefault(x => x.LoanId == loanId);
            loan.EffectiveDate = loanInput.EffectiveDate;
            loan.PrincipalAmount = (decimal)loanInput.PrincipalAmount;
            loan.MaturityDate = periodicSchedule.Max(x => x.PaymentDate);
            loan.OutstandingInterest = (decimal)(periodicSchedule.Select(x => x.PeriodInterestAmount)).Sum();
            loan.OutstandingPrincipal = (decimal)(loanInput.PrincipalAmount - loanInput.IntegralFeeAmount);
            loan.OutstandingAmortisedPrincipal = (decimal)(loanInput.PrincipalAmount);
            loan.OutstandingOldPrincipal = loan.OutstandingPrincipal + loan.OutstandingInterest;
            loan.OutstandingOldInterest = loan.OutstandingInterest;

            loan.FirstInterestPaymentDate = loanInput.InterestFirstpaymentDate;
            loan.FirstPrincipalPaymentDate = loanInput.PrincipalFirstpaymentDate;
            loan.AccrualBasis = loanInput.AccurialBasis;
            //loan.StaffId = staffId;
            //-------------------------------------------------


            //------------------Loan Repayment------------------------------//

            credit_loan_repayment repayment = new credit_loan_repayment();
            repayment.LoanId = loanId;
            repayment.Date = loanInput.EffectiveDate;
            repayment.InterestAmount = (decimal)(periodicSchedule.Select(x => x.PeriodInterestAmount)).Sum();
            repayment.PrincipalAmount = (decimal)loanInput.PrincipalAmount;
            repayment.ClosingBalance = (decimal)loanInput.PrincipalAmount + (decimal)(periodicSchedule.Select(x => x.PeriodInterestAmount)).Sum();

            //-------------------------------------------------//
            _dataContext.SaveChanges();
            //-------------------------------------------------------

            output = true;

            return output;
        }

        public async Task<bool> AddReviewedLoanSchedule(int loanId, LoanPaymentScheduleInputObj loanInput)
        {
            bool output = false;
            var user = await _serverRequest.UserDataAsync();
            var createdBy = user.UserName;
            var applicationDate = DateTime.Now.Date;
            int staffId = user.StaffId;

            //---------------save irregular loan schedule input---------------------------
            List<credit_loanscheduleirrigular> tblIrregularSchedule = new List<credit_loanscheduleirrigular>();
            LoanScheduleTypeEnum scheduleMethod = (LoanScheduleTypeEnum)loanInput.ScheduleMethodId;
            if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
            {
                var data = loanInput.IrregularPaymentSchedule.OrderBy(x => x.PaymentDate);
                foreach (var item in data)
                {
                    credit_loanscheduleirrigular schedule = new credit_loanscheduleirrigular();
                    schedule.LoanId = loanId;
                    schedule.PaymentDate = item.PaymentDate;
                    schedule.PaymentAmount = Convert.ToDecimal(item.PaymentAmount);
                    schedule.StaffId = staffId;
                    schedule.CreatedOn = applicationDate;

                    tblIrregularSchedule.Add(schedule);
                }

            }
            //----------------------------------------------


            //----------generate and save periodic loan schedule -----------------------------------
            List<LoanPaymentSchedulePeriodicObj> periodicSchedule = GeneratePeriodicLoanSchedule(loanInput);

            List<credit_loanscheduleperiodic> tblPeriodicSchedule = new List<credit_loanscheduleperiodic>();
            foreach (var item in periodicSchedule)
            {
                credit_loanscheduleperiodic schedule = new credit_loanscheduleperiodic();
                schedule.LoanId = loanId;
                schedule.PaymentNumber = item.PaymentNumber;
                schedule.PaymentDate = item.PaymentDate;
                schedule.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                schedule.PeriodPaymentAmount = Convert.ToDecimal(item.PeriodPaymentAmount);
                schedule.PeriodInterestAmount = Convert.ToDecimal(item.PeriodInterestAmount);
                schedule.PeriodPrincipalAmount = Convert.ToDecimal(item.PeriodPrincipalAmount);
                schedule.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                schedule.InterestRate = loanInput.InterestRate;

                schedule.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmortisedStartPrincipalAmount);
                schedule.AmortisedPeriodPaymentAmount = Convert.ToDecimal(item.AmortisedPeriodPaymentAmount);
                schedule.AmortisedPeriodInterestAmount = Convert.ToDecimal(item.AmortisedPeriodInterestAmount);
                schedule.AmortisedPeriodPrincipalAmount = Convert.ToDecimal(item.AmortisedPeriodPrincipalAmount);
                schedule.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmortisedEndPrincipalAmount);
                schedule.EffectiveInterestRate = item.EffectiveInterestRate;
                schedule.StaffId = staffId;
                schedule.CreatedOn = applicationDate;
                schedule.Deleted = false;

                tblPeriodicSchedule.Add(schedule);
            }

            //----------generate and save daily loan schedule -----------------------------------
            List<LoanPaymentScheduleDailyObj> dailySchedule = GenerateDailyLoanSchedule(loanInput);

            List<credit_loanscheduledaily> tblDailySchedule = new List<credit_loanscheduledaily>();

            foreach (var item in dailySchedule)
            {
                credit_loanscheduledaily schedule = new credit_loanscheduledaily();

                schedule.LoanId = loanId;
                schedule.PaymentNumber = item.PaymentNumber;
                schedule.Date = item.Date;
                schedule.PaymentDate = item.PaymentDate;
                schedule.OpeningBalance = Convert.ToDecimal(item.OpeningBalance);
                schedule.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                schedule.DailyPaymentAmount = Convert.ToDecimal(item.DailyPaymentAmount);
                schedule.DailyInterestAmount = Convert.ToDecimal(item.DailyInterestAmount);
                schedule.DailyPrincipalAmount = Convert.ToDecimal(item.DailyPrincipalAmount);
                schedule.ClosingBalance = Convert.ToDecimal(item.ClosingBalance);
                schedule.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                schedule.AccruedInterest = Convert.ToDecimal(item.AccruedInterest);
                schedule.AmortisedCost = Convert.ToDecimal(item.AmortisedCost);
                schedule.InterestRate = item.NorminalInterestRate;

                schedule.AmortisedOpeningBalance = Convert.ToDecimal(item.AmOpeningBalance);
                schedule.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmStartPrincipalAmount);
                schedule.AmortisedDailyPaymentAmount = Convert.ToDecimal(item.AmDailyPaymentAmount);
                schedule.AmortisedDailyInterestAmount = Convert.ToDecimal(item.AmDailyInterestAmount);
                schedule.AmortisedDailyPrincipalAmount = Convert.ToDecimal(item.AmDailyPrincipalAmount);
                schedule.AmortisedClosingBalance = Convert.ToDecimal(item.AmClosingBalance);
                schedule.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmEndPrincipalAmount);
                schedule.AmortisedAccruedInterest = Convert.ToDecimal(item.AmAccruedInterest);
                schedule.Amortised_AmortisedCost = Convert.ToDecimal(item.AmAmortisedCost);
                schedule.UnearnedFee = Convert.ToDecimal(item.UnEarnedFee);
                schedule.EarnedFee = Convert.ToDecimal(item.EarnedFee);
                schedule.EffectiveInterestRate = item.EffectiveInterestRate;
                schedule.StaffId = staffId;
                schedule.CreatedOn = applicationDate;
                schedule.Deleted = false;

                tblDailySchedule.Add(schedule);
            }
            //----------------------------------------------------------------


            //------------adding records to the database--------------------------


            this._dataContext.credit_loanscheduleperiodic.AddRange(tblPeriodicSchedule);

            this._dataContext.credit_loanscheduledaily.AddRange(tblDailySchedule);

            //----------update loan details -----------------------------------
            var loan = this._dataContext.credit_loan.FirstOrDefault(x => x.LoanId == loanId);
            loan.EffectiveDate = loanInput.EffectiveDate;
            loan.PrincipalAmount = (decimal)loanInput.PrincipalAmount;
            loan.MaturityDate = periodicSchedule.Max(x => x.PaymentDate);
            loan.OutstandingInterest = (decimal)(periodicSchedule.Select(x => x.PeriodInterestAmount)).Sum();
            loan.OutstandingPrincipal = (decimal)(loanInput.PrincipalAmount - loanInput.IntegralFeeAmount);
            loan.OutstandingAmortisedPrincipal = (decimal)(loanInput.PrincipalAmount);
            loan.OutstandingOldPrincipal = (decimal)loanInput.OldBalance;
            loan.OutstandingOldInterest = (decimal)loanInput.OldInterest;

            loan.FirstInterestPaymentDate = loanInput.InterestFirstpaymentDate;
            loan.FirstPrincipalPaymentDate = loanInput.PrincipalFirstpaymentDate;
            loan.AccrualBasis = loanInput.AccurialBasis;
            //loan.StaffId = staffId;
            //-------------------------------------------------


            //------------------Loan Repayment------------------------------//

            credit_loan_repayment repayment = new credit_loan_repayment();
            repayment.LoanId = loanId;
            repayment.Date = loanInput.EffectiveDate;
            repayment.InterestAmount = (decimal)(periodicSchedule.Select(x => x.PeriodInterestAmount)).Sum();
            repayment.PrincipalAmount = (decimal)loanInput.PrincipalAmount;
            repayment.ClosingBalance = (decimal)loanInput.PrincipalAmount + (decimal)(periodicSchedule.Select(x => x.PeriodInterestAmount)).Sum();

            //-------------------------------------------------//
            _dataContext.SaveChanges();
            //-------------------------------------------------------

            output = true;

            return output;
        }

        public async Task<bool> AddTempLoanApplicationSchedule(int loanApplicationId, LoanPaymentScheduleInputObj loanInput)
        {
            try
            {
                bool output = false;
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var applicationDate = DateTime.Now.Date;
                int staffId = user.StaffId;

                //---------------save irregular loan schedule input---------------------------
                List<credit_loanscheduleirrigular> tblIrregularSchedule = new List<credit_loanscheduleirrigular>();
                LoanScheduleTypeEnum scheduleMethod = (LoanScheduleTypeEnum)loanInput.ScheduleMethodId;
                if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
                {
                    var data = loanInput.IrregularPaymentSchedule.OrderBy(x => x.PaymentDate);
                    foreach (var item in data)
                    {
                        credit_loanscheduleirrigular schedule = new credit_loanscheduleirrigular();
                        schedule.LoanId = loanApplicationId;
                        schedule.PaymentDate = item.PaymentDate;
                        schedule.PaymentAmount = Convert.ToDecimal(item.PaymentAmount);
                        schedule.StaffId = staffId;
                        schedule.CreatedOn = applicationDate;

                        tblIrregularSchedule.Add(schedule);
                    }

                }
                //----------------------------------------------


                //----------generate and save periodic loan schedule -----------------------------------
                List<LoanPaymentSchedulePeriodicObj> periodicSchedule = GeneratePeriodicLoanSchedule(loanInput);

                List<tmp_loanapplicationscheduleperiodic> tblPeriodicSchedule = new List<tmp_loanapplicationscheduleperiodic>();
                foreach (var item in periodicSchedule)
                {
                    tmp_loanapplicationscheduleperiodic schedule = new tmp_loanapplicationscheduleperiodic();
                    schedule.LoanApplicationId = loanApplicationId;
                    schedule.PaymentNumber = item.PaymentNumber;
                    schedule.PaymentDate = item.PaymentDate;
                    schedule.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                    schedule.PeriodPaymentAmount = Convert.ToDecimal(item.PeriodPaymentAmount);
                    schedule.PeriodInterestAmount = Convert.ToDecimal(item.PeriodInterestAmount);
                    schedule.PeriodPrincipalAmount = Convert.ToDecimal(item.PeriodPrincipalAmount);
                    schedule.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                    schedule.InterestRate = loanInput.InterestRate;

                    schedule.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmortisedStartPrincipalAmount);
                    schedule.AmortisedPeriodPaymentAmount = Convert.ToDecimal(item.AmortisedPeriodPaymentAmount);
                    schedule.AmortisedPeriodInterestAmount = Convert.ToDecimal(item.AmortisedPeriodInterestAmount);
                    schedule.AmortisedPeriodPrincipalAmount = Convert.ToDecimal(item.AmortisedPeriodPrincipalAmount);
                    schedule.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmortisedEndPrincipalAmount);
                    schedule.EffectiveInterestRate = item.EffectiveInterestRate;
                    schedule.StaffId = staffId;
                    schedule.CreatedOn = applicationDate;
                    //schedule.Deleted = false;

                    tblPeriodicSchedule.Add(schedule);
                }
                //-------------------------------------------------------------------------------------

                //------------adding records to the database--------------------------

                this._dataContext.tmp_loanapplicationscheduleperiodic.AddRange(tblPeriodicSchedule);


                var res  =  _dataContext.SaveChanges() > 0;
                //-------------------------------------------------------

                output = true;

                return output;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<bool> AddTempLoanSchedule(int loanId, LoanPaymentScheduleInputObj loanInput)
        {
            try
            {
                bool output = false;
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var applicationDate = DateTime.Now.Date;
                int staffId = user.StaffId;

                //---------------save irregular loan schedule input---------------------------
                List<credit_temp_loanscheduleirrigular> tblIrregularSchedule = new List<credit_temp_loanscheduleirrigular>();
                LoanScheduleTypeEnum scheduleMethod = (LoanScheduleTypeEnum)loanInput.ScheduleMethodId;
                if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
                {
                    var data = loanInput.IrregularPaymentSchedule.OrderBy(x => x.PaymentDate);
                    foreach (var item in data)
                    {
                        credit_temp_loanscheduleirrigular schedule = new credit_temp_loanscheduleirrigular();
                        schedule.LoanId = loanId;
                        schedule.PaymentDate = item.PaymentDate;
                        schedule.PaymentAmount = Convert.ToDecimal(item.PaymentAmount);
                        schedule.StaffId = staffId;
                        schedule.CreatedOn = applicationDate;

                        tblIrregularSchedule.Add(schedule);
                    }

                }
                //----------------------------------------------


                //----------generate and save periodic loan schedule -----------------------------------
                List<LoanPaymentSchedulePeriodicObj> periodicSchedule = GeneratePeriodicLoanSchedule(loanInput);

                List<credit_temp_loanscheduleperiodic> tblPeriodicSchedule = new List<credit_temp_loanscheduleperiodic>();
                foreach (var item in periodicSchedule)
                {
                    credit_temp_loanscheduleperiodic schedule = new credit_temp_loanscheduleperiodic();
                    schedule.LoanId = loanId;
                    schedule.PaymentNumber = item.PaymentNumber;
                    schedule.PaymentDate = item.PaymentDate;
                    schedule.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                    schedule.PeriodPaymentAmount = Convert.ToDecimal(item.PeriodPaymentAmount);
                    schedule.PeriodInterestAmount = Convert.ToDecimal(item.PeriodInterestAmount);
                    schedule.PeriodPrincipalAmount = Convert.ToDecimal(item.PeriodPrincipalAmount);
                    schedule.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                    schedule.InterestRate = loanInput.InterestRate;

                    schedule.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmortisedStartPrincipalAmount);
                    schedule.AmortisedPeriodPaymentAmount = Convert.ToDecimal(item.AmortisedPeriodPaymentAmount);
                    schedule.AmortisedPeriodInterestAmount = Convert.ToDecimal(item.AmortisedPeriodInterestAmount);
                    schedule.AmortisedPeriodPrincipalAmount = Convert.ToDecimal(item.AmortisedPeriodPrincipalAmount);
                    schedule.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmortisedEndPrincipalAmount);
                    schedule.EffectiveInterestRate = item.EffectiveInterestRate;
                    schedule.StaffId = staffId;
                    schedule.CreatedOn = applicationDate;

                    tblPeriodicSchedule.Add(schedule);
                }
                //-------------------------------------------------------------------------------------


                //----------generate and save daily loan schedule -----------------------------------
                List<LoanPaymentScheduleDailyObj> dailySchedule = GenerateDailyLoanSchedule(loanInput);

                List<credit_temp_loanscheduledaily> tblDailySchedule = new List<credit_temp_loanscheduledaily>();

                foreach (var item in dailySchedule)
                {
                    credit_temp_loanscheduledaily schedule = new credit_temp_loanscheduledaily();

                    schedule.LoanId = loanId;
                    schedule.PaymentNumber = item.PaymentNumber;
                    schedule.Date = item.Date;
                    schedule.PaymentDate = item.PaymentDate;
                    schedule.OpeningBalance = Convert.ToDecimal(item.OpeningBalance);
                    schedule.StartPrincipalAmount = Convert.ToDecimal(item.StartPrincipalAmount);
                    schedule.DailyPaymentAmount = Convert.ToDecimal(item.DailyPaymentAmount);
                    schedule.DailyInterestAmount = Convert.ToDecimal(item.DailyInterestAmount);
                    schedule.DailyPrincipalAmount = Convert.ToDecimal(item.DailyPrincipalAmount);
                    schedule.ClosingBalance = Convert.ToDecimal(item.ClosingBalance);
                    schedule.EndPrincipalAmount = Convert.ToDecimal(item.EndPrincipalAmount);
                    schedule.AccruedInterest = Convert.ToDecimal(item.AccruedInterest);
                    schedule.AmortisedCost = Convert.ToDecimal(item.AmortisedCost);
                    schedule.InterestRate = item.NorminalInterestRate;

                    schedule.AmortisedOpeningBalance = Convert.ToDecimal(item.AmOpeningBalance);
                    schedule.AmortisedStartPrincipalAmount = Convert.ToDecimal(item.AmStartPrincipalAmount);
                    schedule.AmortisedDailyPaymentAmount = Convert.ToDecimal(item.AmDailyPaymentAmount);
                    schedule.AmortisedDailyInterestAmount = Convert.ToDecimal(item.AmDailyInterestAmount);
                    schedule.AmortisedDailyPrincipalAmount = Convert.ToDecimal(item.AmDailyPrincipalAmount);
                    schedule.AmortisedClosingBalance = Convert.ToDecimal(item.AmClosingBalance);
                    schedule.AmortisedEndPrincipalAmount = Convert.ToDecimal(item.AmEndPrincipalAmount);
                    schedule.AmortisedAccruedInterest = Convert.ToDecimal(item.AmAccruedInterest);
                    schedule.Amortised_AmortisedCost = Convert.ToDecimal(item.AmAmortisedCost);
                    schedule.UnearnedFee = Convert.ToDecimal(item.UnEarnedFee);
                    schedule.EarnedFee = Convert.ToDecimal(item.EarnedFee);
                    schedule.EffectiveInterestRate = item.EffectiveInterestRate;
                    schedule.StaffId = staffId;
                    schedule.CreatedOn = applicationDate;
                    schedule.Deleted = false;

                    tblDailySchedule.Add(schedule);
                }
                //----------------------------------------------------------------


                //------------adding records to the database--------------------------

                //if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
                //{ this.context.tbl_Loan_Schedule_Irregular_Input.AddRange(tblIrregularSchedule); }


                this._dataContext.credit_temp_loanscheduleperiodic.AddRange(tblPeriodicSchedule);

                this._dataContext.credit_temp_loanscheduledaily.AddRange(tblDailySchedule);

                //----------update loan details -----------------------------------
                //-------------------------------------------------

                _dataContext.SaveChanges();
                //-------------------------------------------------------

                output = true;

                return output;
            }
            catch (Exception ex)
            {

                throw ex;
            }        
        }

        public DateTime CalculateFirstPayDate(DateTime effectiveDate, int frequencyTypeId)
        {
            var frequencyValue = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == frequencyTypeId).Value;

            DateTime output = effectiveDate.AddMonths(12 / (int)frequencyValue);
            return output;
        }

        public int CalculateNumberOfInstallments(TenorModeEnum tenorModeId, int frequencyTypeId, int tenor)
        {
            double totalTenor = 0;

            if (tenorModeId == TenorModeEnum.Days)
                totalTenor = tenor / 365; //365 days in a year
            else if (tenorModeId == TenorModeEnum.Months)
                totalTenor = tenor / 12; //12 = months in a year
            else if (tenorModeId == TenorModeEnum.Years)
                totalTenor = tenor;

            if (frequencyTypeId == 10 || frequencyTypeId == 11) // 10 = end of period and 11 = now
                return 1;

            var frequencyValue = _dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == frequencyTypeId).Value;

            var installments = totalTenor * frequencyValue;

            return (int)installments;
        }
        public int CalculateNumberOfInstallments(DateTime firstPaymentDate, DateTime maturityDate, FrequencyTypeEnum frequencyType)
        {
            var startdate = LocalDateTime.FromDateTime(firstPaymentDate);
            var endDate = LocalDateTime.FromDateTime(maturityDate);

            double numberOfpayments = 0;

            if (frequencyType == FrequencyTypeEnum.Daily)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Days).Days;
            else if (frequencyType == FrequencyTypeEnum.Monthly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Months).Months;
            else if (frequencyType == FrequencyTypeEnum.Quarterly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Months).Months / 3.0;
            else if (frequencyType == FrequencyTypeEnum.SixTimesYearly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Months).Months / 6.0;
            else if (frequencyType == FrequencyTypeEnum.ThriceYearly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Months).Months / 4.0;
            else if (frequencyType == FrequencyTypeEnum.TwiceMonthly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Months).Months * 2.0;
            else if (frequencyType == FrequencyTypeEnum.TwiceYearly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Months).Months / 6.0;
            else if (frequencyType == FrequencyTypeEnum.Weekly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Weeks).Weeks;
            else if (frequencyType == FrequencyTypeEnum.Yearly)
                numberOfpayments = Period.Between(startdate, endDate, PeriodUnits.Years).Years;

            return Convert.ToInt32(numberOfpayments + 1);
        }

        public List<LoanPaymentScheduleDailyObj> GenerateDailyLoanSchedule(LoanPaymentScheduleInputObj loanInput)
        {
            List<LoanPaymentSchedulePeriodicObj> periodicSchedule = GeneratePeriodicLoanSchedule(loanInput);

            List<LoanPaymentScheduleDailyObj> output = new List<LoanPaymentScheduleDailyObj>();

            var numberOfPeriods = periodicSchedule.Count() - 1;

            DateTime previousPaymentDate = loanInput.EffectiveDate;

            int dailyScheduleRowCount = 0;
            int paymentNumber = 1;
            foreach (var item in periodicSchedule)
            {
                if (paymentNumber > 1)
                {
                    var dateDifferenceCount = (item.PaymentDate - previousPaymentDate).TotalDays;
                    var currentDate = previousPaymentDate; // item.paymentDate;
                    double previousPrincipalAmount = item.StartPrincipalAmount;
                    double amPreviousPrincipalAmount = item.AmortisedStartPrincipalAmount;
                    double accuredInterest = 0;
                    double amAccuredInterest = 0;

                    for (int counter = 1; counter <= dateDifferenceCount; counter++)
                    {
                        LoanPaymentScheduleDailyObj dayValues = new LoanPaymentScheduleDailyObj();
                        dayValues.PaymentNumber = dailyScheduleRowCount;
                        dayValues.PaymentDate = item.PaymentDate;
                        dayValues.Date = currentDate;

                        dayValues.OpeningBalance = previousPrincipalAmount;
                        dayValues.StartPrincipalAmount = item.StartPrincipalAmount;
                        dayValues.DailyPrincipalAmount = item.PeriodPrincipalAmount / dateDifferenceCount;
                        dayValues.DailyInterestAmount = item.PeriodInterestAmount / dateDifferenceCount;
                        dayValues.DailyPaymentAmount = dayValues.DailyPrincipalAmount + dayValues.DailyInterestAmount;
                        dayValues.ClosingBalance = dayValues.OpeningBalance - dayValues.DailyPrincipalAmount;
                        dayValues.EndPrincipalAmount = item.EndPrincipalAmount;
                        dayValues.AccruedInterest = accuredInterest + dayValues.DailyInterestAmount;
                        dayValues.AmortisedCost = dayValues.StartPrincipalAmount + dayValues.AccruedInterest;
                        dayValues.NorminalInterestRate = loanInput.InterestRate;

                        dayValues.AmOpeningBalance = amPreviousPrincipalAmount;
                        dayValues.AmStartPrincipalAmount = item.AmortisedStartPrincipalAmount;
                        dayValues.AmDailyPrincipalAmount = item.AmortisedPeriodPrincipalAmount / dateDifferenceCount;
                        dayValues.AmDailyInterestAmount = item.AmortisedPeriodInterestAmount / dateDifferenceCount;
                        dayValues.AmDailyPaymentAmount = dayValues.AmDailyPrincipalAmount + dayValues.AmDailyInterestAmount;
                        dayValues.AmClosingBalance = dayValues.AmOpeningBalance - dayValues.AmDailyPrincipalAmount;
                        dayValues.AmEndPrincipalAmount = item.AmortisedEndPrincipalAmount;
                        dayValues.AmAccruedInterest = amAccuredInterest + dayValues.AmDailyInterestAmount;
                        dayValues.AmAmortisedCost = dayValues.AmStartPrincipalAmount + dayValues.AmAccruedInterest;

                        dayValues.DiscountPremium = dayValues.AmDailyInterestAmount - dayValues.DailyInterestAmount;
                        dayValues.UnEarnedFee = dayValues.AmClosingBalance - dayValues.ClosingBalance;
                        dayValues.EarnedFee = loanInput.IntegralFeeAmount - dayValues.UnEarnedFee;
                        dayValues.EffectiveInterestRate = item.EffectiveInterestRate;
                        dayValues.NumberOfPeriods = numberOfPeriods;

                        //public double balloonAmt { get; set; }


                        output.Add(dayValues);

                        accuredInterest = dayValues.AccruedInterest;
                        amAccuredInterest = dayValues.AmAccruedInterest;

                        previousPrincipalAmount = dayValues.ClosingBalance;
                        amPreviousPrincipalAmount = dayValues.AmClosingBalance;

                        currentDate = currentDate.AddDays(1);
                        dailyScheduleRowCount += 1;

                    }

                    previousPaymentDate = item.PaymentDate;
                }

                paymentNumber += 1;
            }

            return output;
        }

        public List<LoanPaymentSchedulePeriodicObj> GeneratePeriodicLoanSchedule(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.PrincipalAmount <= 0)
                throw new Exception("Please Enter Loan Amount");
            if (loanInput.InterestRate < 0)
                throw new Exception("Please Enter Loan Interest Amount");
            List<LoanPaymentSchedulePeriodicObj> output = null; // new List<LoanPaymentSchedulePeriodicViewModel>();
            LoanScheduleTypeEnum scheduleMethod = (LoanScheduleTypeEnum)loanInput.ScheduleMethodId;

            if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
                output = GenerateIrregularPeriodicScheduleWithAmortisedCost(loanInput).ToList();
            else if (scheduleMethod == LoanScheduleTypeEnum.Annuity)
            {
                if (loanInput.InterestFirstpaymentDate == loanInput.PrincipalFirstpaymentDate && loanInput.InterestFrequency == loanInput.PrincipalFrequency)
                    output = GenerateNormalAnnuityPeriodicSchedule(loanInput);
                else
                    output = GenerateMoratoriumAnnuityPeriodicSchedule(loanInput);
            }
            else if (scheduleMethod == LoanScheduleTypeEnum.ReducingBalance)
                output = GenerateReducingBalancePeriodicSchedule(loanInput);
            else if (scheduleMethod == LoanScheduleTypeEnum.BulletPayment)
                output = GenerateBulletPeriodicScheduleWithAmortisedCost(loanInput);
            else if (scheduleMethod == LoanScheduleTypeEnum.ConstantPrincipalAndInterest)
                output = GenerateConstantPrincipalAndInterestPeriodicScheduleWithAmortisedCost(loanInput);
            else if (scheduleMethod == LoanScheduleTypeEnum.BallonPayment)
                output = GenerateBallonPeriodicSchedule(loanInput);

            return output;
        }

        public byte[] GeneratePeriodicLoanScheduleExport(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.PrincipalAmount <= 0)
                throw new Exception("Please Enter Loan Amount");
            if (loanInput.InterestRate < 0)
                throw new Exception("Please Enter Loan Interest Amount");
            List<LoanPaymentSchedulePeriodicObj> output = null; // new List<LoanPaymentSchedulePeriodicViewModel>();
            LoanScheduleTypeEnum scheduleMethod = (LoanScheduleTypeEnum)loanInput.ScheduleMethodId;

            if (scheduleMethod == LoanScheduleTypeEnum.IrregularSchedule)
                output = GenerateIrregularPeriodicScheduleWithAmortisedCost(loanInput).ToList();
            else if (scheduleMethod == LoanScheduleTypeEnum.Annuity)
            {
                if (loanInput.InterestFirstpaymentDate == loanInput.PrincipalFirstpaymentDate && loanInput.InterestFrequency == loanInput.PrincipalFrequency)
                    output = GenerateNormalAnnuityPeriodicSchedule(loanInput);
                else
                    output = GenerateMoratoriumAnnuityPeriodicSchedule(loanInput);
            }
            else if (scheduleMethod == LoanScheduleTypeEnum.ReducingBalance)
                output = GenerateReducingBalancePeriodicSchedule(loanInput);
            else if (scheduleMethod == LoanScheduleTypeEnum.BulletPayment)
                output = GenerateBulletPeriodicScheduleWithAmortisedCost(loanInput);
            else if (scheduleMethod == LoanScheduleTypeEnum.ConstantPrincipalAndInterest)
                output = GenerateConstantPrincipalAndInterestPeriodicScheduleWithAmortisedCost(loanInput);
            else if (scheduleMethod == LoanScheduleTypeEnum.BallonPayment)
                output = GenerateBallonPeriodicSchedule(loanInput);

            DataTable dt = new DataTable();
            dt.Columns.Add("S/N");
            dt.Columns.Add("Payment Date");
            dt.Columns.Add("Start Principal");
            dt.Columns.Add("Periodic Amount");
            dt.Columns.Add("Principal Amount");
            dt.Columns.Add("Interest Amount");
            dt.Columns.Add("Balance");
            dt.Columns.Add("AM Start Amount");
            dt.Columns.Add("AM Periodic Amount");
            dt.Columns.Add("AM Principal Amount");
            dt.Columns.Add("AM Interest Amount");
            dt.Columns.Add("AM Balance");
            dt.Columns.Add("IRR");
            foreach (var kk in output)
            {
                var row = dt.NewRow();
                row["S/N"] = kk.PaymentNumber;
                row["Payment Date"] = kk.PaymentDate;
                row["Start Principal"] = kk.StartPrincipalAmount;
                row["Periodic Amount"] = kk.PeriodPrincipalAmount;
                row["Interest Amount"] = kk.PeriodInterestAmount;
                row["Balance"] = kk.EndPrincipalAmount;
                row["AM Start Amount"] = kk.AmortisedStartPrincipalAmount;
                row["AM Periodic Amount"] = kk.AmortisedPeriodPaymentAmount;
                row["AM Principal Amount"] = kk.AmortisedStartPrincipalAmount;
                row["AM Interest Amount"] = kk.AmortisedPeriodInterestAmount;
                row["AM Balance"] = kk.AmortisedEndPrincipalAmount;
                row["IRR"] = kk.Irr;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (output != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Schedule");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public IEnumerable<LookupObj> GetAllDayCount()
        {
            return (from data in _dataContext.credit_daycountconvention
                    select new LookupObj()
                    {
                        LookupId = data.DayCountConventionId,
                        LookupName = data.DayCountConventionName
                    });
        }

        public IEnumerable<LookupObj> GetAllFrequencyTypes()
        {
            return (from data in _dataContext.credit_frequencytype
                    select new LookupObj()
                    {
                        LookupId = data.FrequencyTypeId,
                        LookupName = data.Mode,
                        LookupTypeId = data.Days ?? 0
                    });
        }

        public IEnumerable<LookupObj> GetAllLoanScheduleCategory()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LookupObj> GetAllLoanScheduleType()
        {
            return (from data in _dataContext.credit_loanscheduletype
                    where data.Deleted == false
                    select new LookupObj()
                    {
                        LookupId = data.LoanScheduleTypeId,
                        LookupName = data.LoanScheduleTypeName,
                        LookupTypeId = data.LoanScheduleCategoryId,
                        LookupTypeName = data.credit_loanschedulecategory.LoanScheduleCategoryName,
                    });
        }

        public IEnumerable<LookupObj> GetLoanScheduleTypeByCategory(int categoryId)
        {
            return (from data in _dataContext.credit_loanscheduletype
                    where data.LoanScheduleCategoryId == categoryId
                    select new LookupObj()
                    {
                        LookupId = data.LoanScheduleTypeId,
                        LookupName = data.LoanScheduleTypeName,
                        LookupTypeId = data.LoanScheduleCategoryId,
                        LookupTypeName = data.credit_loanschedulecategory.LoanScheduleCategoryName,
                    });
        }

        public IEnumerable<LoanPaymentSchedulePeriodicObj> GetPeriodicScheduleByLoaanId(int loanId)
        {
            var data = (from a in _dataContext.credit_loanscheduleperiodic
                        where a.LoanId == loanId && a.Deleted == false
                        select new LoanPaymentSchedulePeriodicObj
                        {
                            LoanId = a.LoanId,
                            PaymentNumber = a.PaymentNumber,
                            PaymentDate = a.PaymentDate,
                            StartPrincipalAmount = (double)a.StartPrincipalAmount,
                            PeriodPaymentAmount = (double)a.PeriodPaymentAmount,
                            PeriodInterestAmount = (double)a.PeriodInterestAmount,
                            PeriodPrincipalAmount = (double)a.PeriodPrincipalAmount,
                            EndPrincipalAmount = (double)a.EndPrincipalAmount,
                            InterestRate = a.InterestRate,

                            AmortisedStartPrincipalAmount = (double)a.AmortisedStartPrincipalAmount,
                            AmortisedPeriodPaymentAmount = (double)a.AmortisedPeriodPaymentAmount,
                            AmortisedPeriodInterestAmount = (double)a.AmortisedPeriodInterestAmount,
                            AmortisedPeriodPrincipalAmount = (double)a.AmortisedPeriodPrincipalAmount,
                            AmortisedEndPrincipalAmount = (double)a.AmortisedEndPrincipalAmount,
                            EffectiveInterestRate = a.EffectiveInterestRate,

                        }).ToList().OrderBy(x => x.PaymentNumber);
            return data;
        }

        public IEnumerable<LoanPaymentSchedulePeriodicObj> GetPeriodicScheduleByLoaanIdDeleted(int loanId)
        {
            var data = (from a in _dataContext.credit_loanscheduleperiodic
                        where a.LoanId == loanId && a.Deleted == true
                        select new LoanPaymentSchedulePeriodicObj
                        {
                            LoanId = a.LoanId,
                            PaymentNumber = a.PaymentNumber,
                            PaymentDate = a.PaymentDate,
                            StartPrincipalAmount = (double)a.StartPrincipalAmount,
                            PeriodPaymentAmount = (double)a.PeriodPaymentAmount,
                            PeriodInterestAmount = (double)a.PeriodInterestAmount,
                            PeriodPrincipalAmount = (double)a.PeriodPrincipalAmount,
                            EndPrincipalAmount = (double)a.EndPrincipalAmount,
                            InterestRate = a.InterestRate,

                            AmortisedStartPrincipalAmount = (double)a.AmortisedStartPrincipalAmount,
                            AmortisedPeriodPaymentAmount = (double)a.AmortisedPeriodPaymentAmount,
                            AmortisedPeriodInterestAmount = (double)a.AmortisedPeriodInterestAmount,
                            AmortisedPeriodPrincipalAmount = (double)a.AmortisedPeriodPrincipalAmount,
                            AmortisedEndPrincipalAmount = (double)a.AmortisedEndPrincipalAmount,
                            EffectiveInterestRate = a.EffectiveInterestRate,

                        }).ToList().OrderBy(x=>x.PaymentNumber);
            return data;
        }



        //Private Methods
        //////////////////////////////////////////////////////////////////////
        private int GetDaysInAYear(DayCountConventionEnum dayCountId)
        {
            if (dayCountId == DayCountConventionEnum.Actual_Actual)
            {
                var currentDate = DateTime.Now;
                var firstDate = new DateTime(currentDate.Year, 1, 1); //  DateTime.ParseExact(user, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var lastdate = new DateTime(currentDate.Year, 12, 31);
                var difference = (lastdate - firstDate).TotalDays;

                return Convert.ToInt32(difference);
            }

            var value = _dataContext.credit_daycountconvention.FirstOrDefault(x => x.DayCountConventionId == (int)dayCountId).DaysInAYear;

            return value;
        }

        private double CalculateIRR(LoanPaymentScheduleInputObj loanInput, List<LoanPaymentSchedulePeriodicObj> cashflow, int numberOfPayments, int numberOfPaymentsInAYear,
                                    int daysInAYear, Double FV, string interestRule, DateTime maturityDate)
        {
            double output = 0;


            if (loanInput.IntegralFeeAmount == 0)
                output = loanInput.InterestRate / 100;
            else if (numberOfPayments < 2 && loanInput.InterestRate > 0)
            {

                List<double> cashflowAmounts = new List<double>();
                List<int> paymentNumbers = new List<int>();

                cashflowAmounts.Add((loanInput.PrincipalAmount - loanInput.IntegralFeeAmount) * -1);
                paymentNumbers.Add(0);

                var counter = 0;
                foreach (var payment in cashflow)
                {
                    if (counter > 0)
                    {
                        cashflowAmounts.Add(payment.PeriodPaymentAmount);
                        paymentNumbers.Add(payment.PaymentNumber);
                    }

                    counter += 1;
                }

                output = IRR(cashflowAmounts, paymentNumbers, 0);
            }
            else if (numberOfPayments > 1 && loanInput.InterestRate > 0)
            {
                //var pmt = wct.LPMT(loanInput.principalAmount, loanInput.effectiveDate, loanInput.interestRate / 100.0, loanInput.interestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear,
                //                   daysInAYear, FV, interestRule);

                //output = wct.LRATE(loanInput.principalAmount - loanInput.integralFeeAmount, loanInput.effectiveDate, pmt, loanInput.interestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear,
                //                      daysInAYear, 0, interestRule, wct.NULL_DOUBLE);

                var pmt = LPMT(loanInput.PrincipalAmount, loanInput.EffectiveDate, loanInput.InterestRate / 100.0, loanInput.InterestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear,
                                    daysInAYear, 0, "U");

                output = LRATE(loanInput.PrincipalAmount - loanInput.IntegralFeeAmount, loanInput.EffectiveDate, pmt, loanInput.InterestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear,
                                      daysInAYear, 0, "U", 0);

                //  SELECT @IRR = wct.LRATE(
                // @la - @fa--PV
                //, @sd--Loan Date
                //,[wct].[LPMT](@la, @sd, cast(cast(@rt as float) / cast(100 as float) as float), @fpd, @np, @rfqy, @NDY, NULL, 'U')--PMT
                //, @fpd--FirstPayment Date
                //, @np--Number ofPaymens
                //, @rfqy--Payments peryear
                //, @NDY--Days in Year
                //, 0--FV
                //, 'U'--InterestRule
                //, NULL--Guess
                //)

            }
            else if (loanInput.InterestRate <= 0)
                output = 0;

            if (loanInput.InterestFirstpaymentDate == maturityDate)
            {
                Period period = Period.Between(LocalDateTime.FromDateTime(loanInput.InterestFirstpaymentDate), LocalDateTime.FromDateTime(maturityDate));
                if (period.Days == 0)
                {
                    output = output * (daysInAYear / 1);
                }
                else
                {
                    output = output * (daysInAYear / period.Days);
                }

            }

            return output * 100;
        }


        /// <summary>
        /// USE FOR NORMAL ANNUITY SCHEDULE AS SHOWN WHERE INTEREST AND PRINCIPAL DROPS THE SAME DAY
        /// </summary>
        /// <param name="loanInput"></param>
        /// <returns></returns>         
        public List<LoanPaymentSchedulePeriodicObj> GenerateNormalAnnuityPeriodicSchedule(LoanPaymentScheduleInputObj loanInput)
        {
            try
            {
                if (loanInput.EffectiveDate == null)
                    throw new Exception("Please Enter Effective Date");
                if (loanInput.MaturityDate == null)
                    throw new Exception("Please Enter Maturity Date");
                if (loanInput.InterestFirstpaymentDate == null)
                    throw new Exception("Please Enter First Interest Payment Date");
                if (loanInput.PrincipalFirstpaymentDate == null)
                    throw new Exception("Please Enter First Principal Payment Date");
                if (loanInput.PrincipalAmount <= 0)
                    throw new Exception("Please Enter Loan Amount");
                if (loanInput.EffectiveDate >= loanInput.MaturityDate)
                    throw new Exception("Effective Date must be less than the maturity date");
                if (loanInput.InterestFirstpaymentDate >= loanInput.MaturityDate)
                    throw new Exception("First Interest Payment Date must be less than the maturity date");
                if (loanInput.PrincipalFirstpaymentDate >= loanInput.MaturityDate)
                    throw new Exception("First Principal Payment Date must be less than the maturity date");
                if (loanInput.EffectiveDate >= loanInput.PrincipalFirstpaymentDate)
                    throw new Exception("First Principal Payment Date cannot be less than the effective date");
                if (loanInput.EffectiveDate >= loanInput.InterestFirstpaymentDate)
                    throw new Exception("First Interest Payment Date cannot be less than the effective date");

                List<LoanPaymentSchedulePeriodicObj> output = new List<LoanPaymentSchedulePeriodicObj>();

                int daysInAYear = GetDaysInAYear((DayCountConventionEnum)loanInput.AccurialBasis);
                int numberOfPayments = CalculateNumberOfInstallments(loanInput.InterestFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);
                int numberOfPaymentsInAYear = Convert.ToInt32(_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.InterestFrequency).Value);

                Double FV;
                //FinancialTypes.InterestRuleType IntRule;
                //FinancialTypes.AMORTSCHED_table result;
                List<LoanPaymentSchedulePeriodicObj> result = new List<LoanPaymentSchedulePeriodicObj>();

                //double? dddd;
                //dddd = null;
                FV = 0;
                //IntRule = FinancialTypes.InterestRuleType.US;

                //result = wct.AMORTSCHED(PV, LoanDate, rate, FirstPayDate, NumPmts, Pmtpyr, DaysInYr, FV, IntRule);

                result = AMORTSCHED(loanInput.PrincipalAmount, loanInput.EffectiveDate, (loanInput.InterestRate / 100.0), loanInput.InterestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear, daysInAYear, 0, "U");

                //result = wct.AMORTSCHED(loanInput.principalAmount, loanInput.effectiveDate, (loanInput.interestRate / 100.0), loanInput.interestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear, daysInAYear, FV, IntRule);


                int counter = 0;

                foreach (var row in result)
                {
                    LoanPaymentSchedulePeriodicObj payment = new LoanPaymentSchedulePeriodicObj();
                    //payment.paymentNumber = Convert.ToInt32(row["num_pmt"]);
                    //payment.paymentDate = Convert.ToDateTime(row["date_pmt"]);
                    //payment.startPrincipalAmount = Convert.ToDouble(row["amt_prin_init"]);
                    //payment.periodPaymentAmount = Convert.ToDouble(row["amt_pmt"]);
                    //payment.periodInterestAmount = Convert.ToDouble(row["amt_int_pay"]);
                    //payment.periodPrincipalAmount = Convert.ToDouble(row["amt_prin_pay"]);
                    //payment.endPrincipalAmount = Convert.ToDouble(row["amt_prin_end"]);


                    payment.PaymentNumber = row.PaymentNumber;
                    payment.PaymentDate = row.PaymentDate;
                    payment.StartPrincipalAmount = row.StartPrincipalAmount;
                    payment.PeriodPaymentAmount = row.PeriodPaymentAmount;
                    payment.PeriodInterestAmount = row.PeriodInterestAmount;
                    payment.PeriodPrincipalAmount = row.PeriodPrincipalAmount;
                    payment.EndPrincipalAmount = row.EndPrincipalAmount;
                    payment.InterestRate = loanInput.InterestRate;

                    output.Add(payment);

                    counter += 1;
                }

                var maturityDate = output.Max(x => x.PaymentDate); //loanInput.maturityDate

                var internalRateOfReturn = CalculateIRR(loanInput, output, numberOfPayments, numberOfPaymentsInAYear,
                                                 daysInAYear, FV, "U", maturityDate);

                //FinancialTypes.AMORTSCHED_table amortisedResult;
                List<LoanPaymentSchedulePeriodicObj> amortisedResult = new List<LoanPaymentSchedulePeriodicObj>();
                amortisedResult = AMORTSCHEDIRR(loanInput.PrincipalAmount - loanInput.IntegralFeeAmount, loanInput.EffectiveDate, (internalRateOfReturn / 100.0), loanInput.InterestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear, daysInAYear, 0, "U");

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(amortisedResult);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                counter = 0;
                foreach (var payment in output)
                {
                    //payment.amortisedStartPrincipalAmount = Convert.ToDouble(amortisedResult.Rows[counter]["amt_prin_init"]);
                    //payment.amortisedPeriodPaymentAmount = Convert.ToDouble(amortisedResult.Rows[counter]["amt_pmt"]);
                    //payment.amortisedPeriodInterestAmount = Convert.ToDouble(amortisedResult.Rows[counter]["amt_int_pay"]);
                    //payment.amortisedPeriodPrincipalAmount = Convert.ToDouble(amortisedResult.Rows[counter]["amt_prin_pay"]);
                    //payment.amortisedEndPrincipalAmount = Convert.ToDouble(amortisedResult.Rows[counter]["amt_prin_end"]);
                    //LoanPaymentSchedulePeriodicViewModel payment = new LoanPaymentSchedulePeriodicViewModel();
                    payment.AmortisedStartPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedStartPrincipalAmount"]);
                    payment.AmortisedPeriodPaymentAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPaymentAmount"]);
                    payment.AmortisedPeriodInterestAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodInterestAmount"]);
                    payment.AmortisedPeriodPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPrincipalAmount"]);
                    payment.AmortisedEndPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedEndPrincipalAmount"]);
                    payment.EffectiveInterestRate = internalRateOfReturn;

                    counter += 1;
                }

                return output;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public double LPMT(double PV, DateTime loanDate, double rate, DateTime firstPayDate, int numberOfPayments, int numberOfPaymentsInAYear, int daysInAYear, double FV, string IntRule)
        {
            //  var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            double PMT = 0;
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_LPMT", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Rate",
                    Value = rate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPayDate",
                    Value = firstPayDate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumPmts",
                    Value = numberOfPayments
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Pmtpyr",
                    Value = numberOfPaymentsInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@IntRule",
                    Value = IntRule
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                //payment.paymentNumber = Convert.ToInt32(row["num_pmt"]);
                //payment.paymentDate = Convert.ToDateTime(row["date_pmt"]);
                //payment.startPrincipalAmount = Convert.ToDouble(row["amt_prin_init"]);
                //payment.periodPaymentAmount = Convert.ToDouble(row["amt_pmt"]);
                //payment.periodInterestAmount = Convert.ToDouble(row["amt_int_pay"]);
                //payment.periodPrincipalAmount = Convert.ToDouble(row["amt_prin_pay"]);
                //payment.endPrincipalAmount = Convert.ToDouble(row["amt_prin_end"]);


                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["pmt"] != DBNull.Value)
                        candidate.Pmt = double.Parse(reader["pmt"].ToString());
                    candidates.Add(candidate);
                }
                PMT = (double)candidates.FirstOrDefault().Pmt;
                con.Close();
            }

            return PMT;
        }

        public double LRATE(double PV, DateTime loanDate, double pmt, DateTime firstPayDate, int numberOfPayments, int numberOfPaymentsInAYear, int daysInAYear, double FV, string IntRule, double guess)
        {
            // var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            double EIR = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_LRATE", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate.ToString("MMMM dd, yyyy")
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Pmt",
                    Value = pmt
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPayDate",
                    Value = firstPayDate.ToString("MMMM dd, yyyy")
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumPmts",
                    Value = numberOfPayments
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Pmtpyr",
                    Value = numberOfPaymentsInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@IntRule",
                    Value = IntRule
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Guess",
                    Value = guess
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["eir"] != DBNull.Value)
                        candidate.Irr = double.Parse(reader["eir"].ToString());
                    candidates.Add(candidate);
                }
                var irrCandidate = candidates.FirstOrDefault();
                if (irrCandidate.Irr != null)
                {
                    EIR = (double)irrCandidate.Irr;
                }
                con.Close();
            }

            return EIR;
        }

        public double XIRR(List<double> CF, List<DateTime> CFdate, double guess)
        {
            // var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            double EIR = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_XIRR", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CF",
                    Value = CF,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CFdate",
                    Value = CFdate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@guess",
                    Value = guess
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["IRR"] != DBNull.Value)
                        candidate.Irr = double.Parse(reader["IRR"].ToString());
                    candidates.Add(candidate);
                }
                EIR = (double)candidates.FirstOrDefault().Irr;
                con.Close();
            }

            return EIR;
        }

        public double IRR(List<double> CF, List<int> Per, double guess)
        {
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            double IRR = 0;
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            SqlConnection connection = new SqlConnection(connectionString);
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_IRR", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CF",
                    Value = CF,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Per",
                    Value = Per
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@guess",
                    Value = guess
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["IRR"] != DBNull.Value)
                        candidate.Irr = double.Parse(reader["IRR"].ToString());
                    candidates.Add(candidate);
                }
                IRR = (double)candidates.FirstOrDefault().Irr;
                con.Close();
            }

            return IRR;
        }

        public DateTime NPD(DateTime SettDate, DateTime FirstPayDate, int Pmtpyr, int NumPmts)
        {
            //var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            DateTime NxtPayDate;
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            SqlCommand sqlCommand = new SqlCommand();

            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_NPD", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SettDate",
                    Value = SettDate,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPayDate",
                    Value = FirstPayDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Pmtpyr",
                    Value = Pmtpyr
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumPmts",
                    Value = NumPmts
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["NextPaymentDate"] != DBNull.Value)
                        candidate.NextpaymentDate = DateTime.Parse(reader["NextPaymentDate"].ToString());
                    candidates.Add(candidate);
                }
                NxtPayDate = (DateTime)candidates.FirstOrDefault().NextpaymentDate;
                con.Close();
            }

            return NxtPayDate;
        }

        public List<LoanPaymentSchedulePeriodicObj> AMORTSCHED(double PV, DateTime loanDate, double rate, DateTime firstPayDate, int numberOfPayments, int numberOfPaymentsInAYear, int daysInAYear, double FV, string IntRule)
        {
            // var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            //SqlConnection connection = new SqlConnection(connectionString);

            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand("calculate_AMORTSCHED", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Rate",
                    Value = rate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPayDate",
                    Value = firstPayDate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumPmts",
                    Value = numberOfPayments
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Pmtpyr",
                    Value = numberOfPaymentsInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@IntRule",
                    Value = IntRule
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                //payment.paymentNumber = Convert.ToInt32(row["num_pmt"]);
                //payment.paymentDate = Convert.ToDateTime(row["date_pmt"]);
                //payment.startPrincipalAmount = Convert.ToDouble(row["amt_prin_init"]);
                //payment.periodPaymentAmount = Convert.ToDouble(row["amt_pmt"]);
                //payment.periodInterestAmount = Convert.ToDouble(row["amt_int_pay"]);
                //payment.periodPrincipalAmount = Convert.ToDouble(row["amt_prin_pay"]);
                //payment.endPrincipalAmount = Convert.ToDouble(row["amt_prin_end"]);


                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["num_pmt"] != DBNull.Value)
                        candidate.PaymentNumber = int.Parse(reader["num_pmt"].ToString());

                    if (reader["date_pmt"] != DBNull.Value)
                        candidate.PaymentDate = DateTime.Parse(reader["date_pmt"].ToString());

                    if (reader["amt_prin_init"] != DBNull.Value)
                        candidate.StartPrincipalAmount = double.Parse(reader["amt_prin_init"].ToString());

                    if (reader["amt_pmt"] != DBNull.Value)
                        candidate.PeriodPaymentAmount = double.Parse(reader["amt_pmt"].ToString());

                    if (reader["amt_int_pay"] != DBNull.Value)
                        candidate.PeriodInterestAmount = double.Parse(reader["amt_int_pay"].ToString());

                    if (reader["amt_prin_pay"] != DBNull.Value)
                        candidate.PeriodPrincipalAmount = double.Parse(reader["amt_prin_pay"].ToString());

                    if (reader["amt_prin_end"] != DBNull.Value)
                        candidate.EndPrincipalAmount = double.Parse(reader["amt_prin_end"].ToString());
                    candidates.Add(candidate);
                }

                con.Close();
            }

            return candidates;
        }

        public List<LoanPaymentSchedulePeriodicObj> AMORTSCHEDIRR(double PV, DateTime loanDate, double rate, DateTime firstPayDate, int numberOfPayments, int numberOfPaymentsInAYear, int daysInAYear, double FV, string IntRule)
        {
            //var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);

            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_AMORTSCHED", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Rate",
                    Value = rate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPayDate",
                    Value = firstPayDate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumPmts",
                    Value = numberOfPayments
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Pmtpyr",
                    Value = numberOfPaymentsInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@IntRule",
                    Value = IntRule
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["num_pmt"] != DBNull.Value)
                        candidate.PaymentNumber = int.Parse(reader["num_pmt"].ToString());

                    if (reader["date_pmt"] != DBNull.Value)
                        candidate.PaymentDate = DateTime.Parse(reader["date_pmt"].ToString());

                    if (reader["amt_prin_init"] != DBNull.Value)
                        candidate.AmortisedStartPrincipalAmount = double.Parse(reader["amt_prin_init"].ToString());

                    if (reader["amt_pmt"] != DBNull.Value)
                        candidate.AmortisedPeriodPaymentAmount = double.Parse(reader["amt_pmt"].ToString());

                    if (reader["amt_int_pay"] != DBNull.Value)
                        candidate.AmortisedPeriodInterestAmount = double.Parse(reader["amt_int_pay"].ToString());

                    if (reader["amt_prin_pay"] != DBNull.Value)
                        candidate.AmortisedPeriodPrincipalAmount = double.Parse(reader["amt_prin_pay"].ToString());

                    if (reader["amt_prin_end"] != DBNull.Value)
                        candidate.AmortisedEndPrincipalAmount = double.Parse(reader["amt_prin_end"].ToString());
                    candidates.Add(candidate);
                }

                con.Close();
            }

            return candidates;



        }

        public List<LoanPaymentSchedulePeriodicObj> MoratoriumAnnuityAmortSched(double PV, double rate, DateTime loanDate, int interestFreq, DateTime firstPayDate,
            int daysInAYear, int PrinPaymentMultiple, int FirstPrinPayNo, int numberOfPayments, int LastPaymentNumber, double FV, bool IsRegPay)
        {
            //var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);

            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_UNEQUALLOANPAYMENTS", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Rate",
                    Value = rate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@InterestFrequency",
                    Value = interestFreq
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPaymentDate",
                    Value = firstPayDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PrinPaymentMultiple",
                    Value = PrinPaymentMultiple
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPrinPayNo",
                    Value = FirstPrinPayNo
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumberOfPayments",
                    Value = FirstPrinPayNo
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LastPaymentNumber",
                    Value = LastPaymentNumber
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@IsRegPay",
                    Value = IsRegPay
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["num_pmt"] != DBNull.Value)
                        candidate.PaymentNumber = int.Parse(reader["num_pmt"].ToString());

                    if (reader["date_pmt"] != DBNull.Value)
                        candidate.PaymentDate = DateTime.Parse(reader["date_pmt"].ToString());

                    if (reader["amt_prin_init"] != DBNull.Value)
                        candidate.StartPrincipalAmount = double.Parse(reader["amt_prin_init"].ToString());

                    if (reader["amt_pmt"] != DBNull.Value)
                        candidate.PeriodPaymentAmount = double.Parse(reader["amt_pmt"].ToString());

                    if (reader["amt_int_pay"] != DBNull.Value)
                        candidate.PeriodInterestAmount = double.Parse(reader["amt_int_pay"].ToString());

                    if (reader["amt_prin_pay"] != DBNull.Value)
                        candidate.PeriodPrincipalAmount = double.Parse(reader["amt_prin_pay"].ToString());

                    if (reader["amt_prin_end"] != DBNull.Value)
                        candidate.EndPrincipalAmount = double.Parse(reader["amt_prin_end"].ToString());
                    candidates.Add(candidate);
                }

                con.Close();
            }

            return candidates;
        }

        public List<LoanPaymentSchedulePeriodicObj> MoratoriumAnnuityAmortSchedIRR(double PV, double rate, DateTime loanDate, int interestFreq, DateTime firstPayDate,
          int daysInAYear, int PrinPaymentMultiple, int FirstPrinPayNo, int numberOfPayments, int LastPaymentNumber, double FV, bool IsRegPay)
        {
            // var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_UNEQUALLOANPAYMENTS", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Rate",
                    Value = rate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@InterestFrequency",
                    Value = interestFreq
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPaymentDate",
                    Value = firstPayDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PrinPaymentMultiple",
                    Value = PrinPaymentMultiple
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPrinPayNo",
                    Value = FirstPrinPayNo
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumberOfPayments",
                    Value = FirstPrinPayNo
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LastPaymentNumber",
                    Value = LastPaymentNumber
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@IsRegPay",
                    Value = IsRegPay
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["num_pmt"] != DBNull.Value)
                        candidate.PaymentNumber = int.Parse(reader["num_pmt"].ToString());

                    if (reader["date_pmt"] != DBNull.Value)
                        candidate.PaymentDate = DateTime.Parse(reader["date_pmt"].ToString());

                    if (reader["amt_prin_init"] != DBNull.Value)
                        candidate.AmortisedStartPrincipalAmount = double.Parse(reader["amt_prin_init"].ToString());

                    if (reader["amt_pmt"] != DBNull.Value)
                        candidate.AmortisedPeriodPaymentAmount = double.Parse(reader["amt_pmt"].ToString());

                    if (reader["amt_int_pay"] != DBNull.Value)
                        candidate.AmortisedPeriodInterestAmount = double.Parse(reader["amt_int_pay"].ToString());

                    if (reader["amt_prin_pay"] != DBNull.Value)
                        candidate.AmortisedPeriodPrincipalAmount = double.Parse(reader["amt_prin_pay"].ToString());

                    if (reader["amt_prin_end"] != DBNull.Value)
                        candidate.AmortisedEndPrincipalAmount = double.Parse(reader["amt_prin_end"].ToString());
                    candidates.Add(candidate);
                }

                con.Close();
            }

            return candidates;
        }

        public List<LoanPaymentSchedulePeriodicObj> CONSTPRINAMORT(double PV, double rate, DateTime loanDate, int NumPmtsPerYear, DateTime firstPayDate,
       int daysInAYear, int NumberOfPayments, int LastPaymentNumber, int FirstPrinPayNo, double FV, double PPMT, bool eom)
        {
            // var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_CONSTPRINAMORT", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Rate",
                    Value = rate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumPmtsPerYear",
                    Value = NumPmtsPerYear
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPaymentDate",
                    Value = firstPayDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumberOfPayments",
                    Value = NumberOfPayments
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LastPaymentNumber",
                    Value = LastPaymentNumber
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPrinPayNo ",
                    Value = FirstPrinPayNo
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PPMT",
                    Value = PPMT
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@eom",
                    Value = eom
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["num_pmt"] != DBNull.Value)
                        candidate.PaymentNumber = int.Parse(reader["num_pmt"].ToString());

                    if (reader["date_pmt"] != DBNull.Value)
                        candidate.PaymentDate = DateTime.Parse(reader["date_pmt"].ToString());

                    if (reader["amt_prin_init"] != DBNull.Value)
                        candidate.StartPrincipalAmount = double.Parse(reader["amt_prin_init"].ToString());

                    if (reader["amt_pmt"] != DBNull.Value)
                        candidate.PeriodPaymentAmount = double.Parse(reader["amt_pmt"].ToString());

                    if (reader["amt_int_pay"] != DBNull.Value)
                        candidate.PeriodInterestAmount = double.Parse(reader["amt_int_pay"].ToString());

                    if (reader["amt_prin_pay"] != DBNull.Value)
                        candidate.PeriodPrincipalAmount = double.Parse(reader["amt_prin_pay"].ToString());

                    if (reader["amt_prin_end"] != DBNull.Value)
                        candidate.EndPrincipalAmount = double.Parse(reader["amt_prin_end"].ToString());
                    candidates.Add(candidate);
                }

                con.Close();
            }

            return candidates;
        }

        public List<LoanPaymentSchedulePeriodicObj> CONSTPRINAMORTIRR(double PV, double rate, DateTime loanDate, int NumPmtsPerYear, DateTime firstPayDate,
     int daysInAYear, int NumberOfPayments, int LastPaymentNumber, int FirstPrinPayNo, double FV, double PPMT, bool eom)
        {
            // var connectionString = "Data Source=DESKTOP-GVL0L2K;Database=GOSFINANCIAL;Trusted_Connection=True";           
            string connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);
            var candidates = new List<LoanPaymentSchedulePeriodicObj>();
            using (var con = connection)
            {
                var cmd = new SqlCommand("calculate_CONSTPRINAMORT", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PV",
                    Value = PV,
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Rate",
                    Value = rate
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LoanDate",
                    Value = loanDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumPmtsPerYear",
                    Value = NumPmtsPerYear
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPaymentDate",
                    Value = firstPayDate
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DaysInYr",
                    Value = daysInAYear
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@NumberOfPayments",
                    Value = NumberOfPayments
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@LastPaymentNumber",
                    Value = LastPaymentNumber
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FirstPrinPayNo ",
                    Value = FirstPrinPayNo
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FV",
                    Value = FV
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@PPMT",
                    Value = PPMT
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@eom",
                    Value = eom
                });
                cmd.CommandTimeout = 0;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var candidate = new LoanPaymentSchedulePeriodicObj();

                    if (reader["num_pmt"] != DBNull.Value)
                        candidate.PaymentNumber = int.Parse(reader["num_pmt"].ToString());

                    if (reader["date_pmt"] != DBNull.Value)
                        candidate.PaymentDate = DateTime.Parse(reader["date_pmt"].ToString());

                    if (reader["amt_prin_init"] != DBNull.Value)
                        candidate.AmortisedStartPrincipalAmount = double.Parse(reader["amt_prin_init"].ToString());

                    if (reader["amt_pmt"] != DBNull.Value)
                        candidate.AmortisedPeriodPaymentAmount = double.Parse(reader["amt_pmt"].ToString());

                    if (reader["amt_int_pay"] != DBNull.Value)
                        candidate.AmortisedPeriodInterestAmount = double.Parse(reader["amt_int_pay"].ToString());

                    if (reader["amt_prin_pay"] != DBNull.Value)
                        candidate.AmortisedPeriodPrincipalAmount = double.Parse(reader["amt_prin_pay"].ToString());

                    if (reader["amt_prin_end"] != DBNull.Value)
                        candidate.AmortisedEndPrincipalAmount = double.Parse(reader["amt_prin_end"].ToString());
                    candidates.Add(candidate);
                }

                con.Close();
            }

            return candidates;
        }


        /// <summary>
        /// USE FOR ANNUITY SCHEDULE WHERE THERE IS A MORATORIUM 
        /// </summary>
        /// <param name="loanInput"></param>
        /// <returns></returns>         
        private List<LoanPaymentSchedulePeriodicObj> GenerateMoratoriumAnnuityPeriodicSchedule(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.EffectiveDate == null)
                throw new Exception("Please Enter Effective Date");
            if (loanInput.MaturityDate == null)
                throw new Exception("Please Enter Maturity Date");
            if (loanInput.InterestFirstpaymentDate == null)
                throw new Exception("Please Enter First Interest Payment Date");
            if (loanInput.PrincipalFirstpaymentDate == null)
                throw new Exception("Please Enter First Principal Payment Date");
            if (loanInput.PrincipalAmount <= 0)
                throw new Exception("Please Enter Loan Amount");
            if (loanInput.PrincipalFrequency <= 0)
                throw new Exception("Please Select Principal Frequency");
            if (loanInput.InterestFrequency <= 0)
                throw new Exception("Please Select Interest Frequency");
            if (loanInput.EffectiveDate >= loanInput.MaturityDate)
                throw new Exception("Effective Date must be less than the maturity date");
            if (loanInput.InterestFirstpaymentDate >= loanInput.MaturityDate)
                throw new Exception("First Interest Payment Date must be less than the maturity date");
            if (loanInput.PrincipalFirstpaymentDate >= loanInput.MaturityDate)
                throw new Exception("First Principal Payment Date must be less than the maturity date");
            if (loanInput.EffectiveDate >= loanInput.PrincipalFirstpaymentDate)
                throw new Exception("First Principal Payment Date cannot be less than the effective date");
            if (loanInput.EffectiveDate >= loanInput.InterestFirstpaymentDate)
                throw new Exception("First Interest Payment Date cannot be less than the effective date");
            List<LoanPaymentSchedulePeriodicObj> output = new List<LoanPaymentSchedulePeriodicObj>();

            int daysInAYear = GetDaysInAYear((DayCountConventionEnum)loanInput.AccurialBasis);
            int numberOfPayments = CalculateNumberOfInstallments(loanInput.InterestFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);

            int numberOfPrincipalPayments = CalculateNumberOfInstallments(loanInput.PrincipalFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);

            int numberOfPaymentsInAYear = (int)_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.InterestFrequency).Value;

            var principalPaymentsInAYear = (int)_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.PrincipalFrequency).Value;

            int principalPaymentMultiple = Convert.ToInt32(12 / principalPaymentsInAYear);

            var principalFirstPaymentNumber = (numberOfPayments - numberOfPrincipalPayments) + 1;

            Double FV = 0;
            //int? xxx;
            //xxx = null;
            //FinancialTypes.UNEQUALLOANPAYMENTS_table result;


            //result = wct.AMORTSCHED(loanInput.principalAmount, loanInput.effectiveDate, (loanInput.interestRate / 100.0), loanInput.interestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear, daysInAYear, FV, IntRule);

            //result = wct.UNEQUALLOANPAYMENTS(loanInput.principalAmount, (loanInput.interestRate / 100.0), loanInput.effectiveDate, numberOfPaymentsInAYear, loanInput.interestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, wct.NULL_INT, FV, true);

            List<LoanPaymentSchedulePeriodicObj> result = new List<LoanPaymentSchedulePeriodicObj>();
            result = MoratoriumAnnuityAmortSched(loanInput.PrincipalAmount, (loanInput.InterestRate / 100.0), loanInput.EffectiveDate, numberOfPaymentsInAYear, loanInput.InterestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, numberOfPayments, FV, true);

            int counter = 0;
            foreach (var row in result)
            {
                LoanPaymentSchedulePeriodicObj payment = new LoanPaymentSchedulePeriodicObj();
                payment.PaymentNumber = row.PaymentNumber;
                payment.PaymentDate = row.PaymentDate;
                payment.StartPrincipalAmount = row.StartPrincipalAmount;
                payment.PeriodPaymentAmount = row.PeriodPaymentAmount;
                payment.PeriodInterestAmount = row.PeriodInterestAmount;
                payment.PeriodPrincipalAmount = row.PeriodPrincipalAmount;
                payment.EndPrincipalAmount = row.EndPrincipalAmount;
                payment.InterestRate = loanInput.InterestRate;

                output.Add(payment);

                counter += 1;
            }

            var maturityDate = output.Max(x => x.PaymentDate); //loanInput.maturityDate

            var internalRateOfReturn = CalculateIRR(loanInput, output, numberOfPayments, numberOfPaymentsInAYear,
                                             daysInAYear, FV, "U", maturityDate);

            //FinancialTypes.UNEQUALLOANPAYMENTS_table amortisedResult;

            List<LoanPaymentSchedulePeriodicObj> amortisedResult = new List<LoanPaymentSchedulePeriodicObj>();

            amortisedResult = MoratoriumAnnuityAmortSchedIRR(loanInput.PrincipalAmount - loanInput.IntegralFeeAmount, (internalRateOfReturn / 100.0), loanInput.EffectiveDate, numberOfPaymentsInAYear, loanInput.InterestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, numberOfPayments, FV, true);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(amortisedResult);
            DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);
            counter = 0;
            foreach (var payment in output)
            {
                payment.AmortisedStartPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedStartPrincipalAmount"]);
                payment.AmortisedPeriodPaymentAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPaymentAmount"]);
                payment.AmortisedPeriodInterestAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodInterestAmount"]);
                payment.AmortisedPeriodPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPrincipalAmount"]);
                payment.AmortisedEndPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedEndPrincipalAmount"]);
                payment.EffectiveInterestRate = internalRateOfReturn;

                counter += 1;
            }

            return output;

        }

        private List<LoanPaymentSchedulePeriodicObj> GenerateBallonPeriodicSchedule(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.EffectiveDate == null)
                throw new Exception("Please Enter Effective Date");
            if (loanInput.MaturityDate == null)
                throw new Exception("Please Enter Maturity Date");
            if (loanInput.PrincipalFirstpaymentDate == null)
                throw new Exception("Please Enter First Principal Payment Date");
            if (loanInput.PrincipalAmount <= 0)
                throw new Exception("Please Enter Loan Amount");
            if (loanInput.InterestFrequency <= 0)
                throw new Exception("Please Select Interest Frequency");
            if (loanInput.EffectiveDate >= loanInput.MaturityDate)
                throw new Exception("Effective Date must be less than the maturity date");
            if (loanInput.PrincipalFirstpaymentDate >= loanInput.MaturityDate)
                throw new Exception("First Principal Payment Date must be less than the maturity date");
            if (loanInput.EffectiveDate >= loanInput.PrincipalFirstpaymentDate)
                throw new Exception("First Principal Payment Date cannot be less than the effective date");
            List<LoanPaymentSchedulePeriodicObj> output = new List<LoanPaymentSchedulePeriodicObj>();

            int daysInAYear = GetDaysInAYear((DayCountConventionEnum)loanInput.AccurialBasis);
            int numberOfPayments = CalculateNumberOfInstallments(loanInput.InterestFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);

            int numberOfPaymentsInAYear = (int)_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.InterestFrequency).Value;

            int principalPaymentMultiple = 1;

            var principalFirstPaymentNumber = numberOfPayments;

            Double FV = 0;

            // FinancialTypes.UNEQUALLOANPAYMENTS_table result;
            //result = wct.UNEQUALLOANPAYMENTS(loanInput.principalAmount, (loanInput.interestRate / 100.0), loanInput.effectiveDate, numberOfPaymentsInAYear, loanInput.interestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, wct.NULL_INT, FV, true);
            //int?;
            //xxx = null;
            List<LoanPaymentSchedulePeriodicObj> result = new List<LoanPaymentSchedulePeriodicObj>();
            result = MoratoriumAnnuityAmortSched(loanInput.PrincipalAmount, (loanInput.InterestRate / 100.0), loanInput.EffectiveDate, numberOfPaymentsInAYear, loanInput.InterestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, numberOfPayments, FV, true);

            int counter = 0;
            foreach (var row in result)
            {
                LoanPaymentSchedulePeriodicObj payment = new LoanPaymentSchedulePeriodicObj();
                payment.PaymentNumber = row.PaymentNumber;
                payment.PaymentDate = row.PaymentDate;
                payment.StartPrincipalAmount = row.StartPrincipalAmount;
                payment.PeriodPaymentAmount = row.PeriodPaymentAmount;
                payment.PeriodInterestAmount = row.PeriodInterestAmount;
                payment.PeriodPrincipalAmount = row.PeriodPrincipalAmount;
                payment.EndPrincipalAmount = row.EndPrincipalAmount;
                payment.InterestRate = loanInput.InterestRate;

                output.Add(payment);

                counter += 1;
            }

            var maturityDate = output.Max(x => x.PaymentDate); //loanInput.maturityDate

            var internalRateOfReturn = CalculateIRR(loanInput, output, numberOfPayments, numberOfPaymentsInAYear,
                                             daysInAYear, FV, "U", maturityDate);


            // FinancialTypes.UNEQUALLOANPAYMENTS_table amortisedResult;

            //amortisedResult = wct.UNEQUALLOANPAYMENTS(loanInput.principalAmount - loanInput.integralFeeAmount, (internalRateOfReturn / 100.0), loanInput.effectiveDate, numberOfPaymentsInAYear, loanInput.interestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, wct.NULL_INT, FV, true);
            List<LoanPaymentSchedulePeriodicObj> amortisedResult = new List<LoanPaymentSchedulePeriodicObj>();
            amortisedResult = MoratoriumAnnuityAmortSchedIRR(loanInput.PrincipalAmount - loanInput.IntegralFeeAmount, (internalRateOfReturn / 100.0), loanInput.EffectiveDate, numberOfPaymentsInAYear, loanInput.InterestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, numberOfPayments, FV, true);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(amortisedResult);
            DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);
            counter = 0;
            foreach (var payment in output)
            {
                payment.AmortisedStartPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedStartPrincipalAmount"]);
                payment.AmortisedPeriodPaymentAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPaymentAmount"]);
                payment.AmortisedPeriodInterestAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodInterestAmount"]);
                payment.AmortisedPeriodPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPrincipalAmount"]);
                payment.AmortisedEndPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedEndPrincipalAmount"]);
                payment.EffectiveInterestRate = internalRateOfReturn;

                counter += 1;
            }

            return output;

        }
        /// <summary>
        /// USE FOR REDUCING BALANCE
        /// </summary>
        /// <param name="loanInput"></param>
        /// <returns></returns>         
        private List<LoanPaymentSchedulePeriodicObj> GenerateReducingBalancePeriodicSchedule(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.EffectiveDate == null)
                throw new Exception("Please Enter Effective Date");
            if (loanInput.MaturityDate == null)
                throw new Exception("Please Enter Maturity Date");
            if (loanInput.InterestFirstpaymentDate == null)
                throw new Exception("Please Enter First Interest Payment Date");
            if (loanInput.PrincipalFirstpaymentDate == null)
                throw new Exception("Please Enter First Principal Payment Date");
            if (loanInput.PrincipalAmount <= 0)
                throw new Exception("Please Enter Loan Amount");
            if (loanInput.PrincipalFrequency <= 0)
                throw new Exception("Please Select Principal Frequency");
            if (loanInput.InterestFrequency <= 0)
                throw new Exception("Please Select Interest Frequency");
            if (loanInput.EffectiveDate >= loanInput.MaturityDate)
                throw new Exception("Effective Date must be less than the maturity date");
            if (loanInput.InterestFirstpaymentDate >= loanInput.MaturityDate)
                throw new Exception("First Interest Payment Date must be less than the maturity date");
            if (loanInput.PrincipalFirstpaymentDate >= loanInput.MaturityDate)
                throw new Exception("First Principal Payment Date must be less than the maturity date");
            if (loanInput.EffectiveDate >= loanInput.PrincipalFirstpaymentDate)
                throw new Exception("First Principal Payment Date cannot be less than the effective date");
            if (loanInput.EffectiveDate >= loanInput.InterestFirstpaymentDate)
                throw new Exception("First Interest Payment Date cannot be less than the effective date");
            List<LoanPaymentSchedulePeriodicObj> output = new List<LoanPaymentSchedulePeriodicObj>();

            int daysInAYear = GetDaysInAYear((DayCountConventionEnum)loanInput.AccurialBasis);
            int numberOfPayments = CalculateNumberOfInstallments(loanInput.InterestFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);

            int numberOfPrincipalPayments = CalculateNumberOfInstallments(loanInput.PrincipalFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);

            int numberOfPaymentsInAYear = (int)_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.InterestFrequency).Value;

            var principalPaymentsInAYear = (int)_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.PrincipalFrequency).Value;

            int principalPaymentMultiple = Convert.ToInt32(12 / principalPaymentsInAYear);

            var principalFirstPaymentNumber = (numberOfPayments - numberOfPrincipalPayments) + 1;



            int lastPaymentNumber = 0;
            lastPaymentNumber = numberOfPayments;
            Double FV = 0;
            Double PPMT = 0;
            Boolean eom = true;

            // FinancialTypes.CONSTPRINAMORT_table result;
            if (daysInAYear == 364)
            {
                daysInAYear = daysInAYear + 1;
            }
            //result = wct.UNEQUALLOANPAYMENTS(loanInput.principalAmount, (loanInput.interestRate / 100.0), loanInput.effectiveDate, numberOfPaymentsInAYear, loanInput.interestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, wct.NULL_INT, FV, true);
            List<LoanPaymentSchedulePeriodicObj> result = new List<LoanPaymentSchedulePeriodicObj>();
            //result = wct.CONSTPRINAMORT(loanInput.principalAmount, (loanInput.interestRate / 100.0), loanInput.effectiveDate, numberOfPaymentsInAYear, loanInput.interestFirstpaymentDate, daysInAYear, numberOfPayments, lastPaymentNumber, principalFirstPaymentNumber, FV, PPMT, eom);
            result = CONSTPRINAMORT(loanInput.PrincipalAmount, (loanInput.InterestRate / 100.0), loanInput.EffectiveDate, numberOfPaymentsInAYear, loanInput.InterestFirstpaymentDate, daysInAYear, numberOfPayments, lastPaymentNumber, principalFirstPaymentNumber, FV, PPMT, eom);
            int counter = 0;
            foreach (var row in result)
            {
                LoanPaymentSchedulePeriodicObj payment = new LoanPaymentSchedulePeriodicObj();
                payment.PaymentNumber = row.PaymentNumber;
                payment.PaymentDate = row.PaymentDate;
                payment.StartPrincipalAmount = row.StartPrincipalAmount;
                payment.PeriodPaymentAmount = row.PeriodPaymentAmount;
                payment.PeriodInterestAmount = row.PeriodInterestAmount;
                payment.PeriodPrincipalAmount = row.PeriodPrincipalAmount;
                payment.EndPrincipalAmount = row.EndPrincipalAmount;
                payment.InterestRate = loanInput.InterestRate;

                output.Add(payment);

                counter += 1;
            }

            var maturityDate = output.Max(x => x.PaymentDate); //loanInput.maturityDate

            var internalRateOfReturn = CalculateIRR(loanInput, output, numberOfPayments, numberOfPaymentsInAYear,
                                             daysInAYear, FV, "U", maturityDate);

            //FinancialTypes.CONSTPRINAMORT_table amortisedResult;

            List<LoanPaymentSchedulePeriodicObj> amortisedResult = new List<LoanPaymentSchedulePeriodicObj>();

            //amortisedResult = wct.UNEQUALLOANPAYMENTS(loanInput.principalAmount - loanInput.integralFeeAmount, (internalRateOfReturn / 100.0), loanInput.effectiveDate, numberOfPaymentsInAYear, loanInput.interestFirstpaymentDate, daysInAYear, principalPaymentMultiple, principalFirstPaymentNumber, numberOfPayments, wct.NULL_INT, FV, true);

            //amortisedResult = wct.CONSTPRINAMORT(loanInput.principalAmount - loanInput.integralFeeAmount, (internalRateOfReturn / 100.0), loanInput.effectiveDate, numberOfPaymentsInAYear, loanInput.interestFirstpaymentDate, daysInAYear, numberOfPayments, lastPaymentNumber, principalFirstPaymentNumber, FV, PPMT, eom);
            amortisedResult = CONSTPRINAMORTIRR(loanInput.PrincipalAmount - loanInput.IntegralFeeAmount, (internalRateOfReturn / 100.0), loanInput.EffectiveDate, numberOfPaymentsInAYear, loanInput.InterestFirstpaymentDate, daysInAYear, numberOfPayments, lastPaymentNumber, principalFirstPaymentNumber, FV, PPMT, eom);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(amortisedResult);
            DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);
            counter = 0;
            foreach (var payment in output)
            {
                payment.AmortisedStartPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedStartPrincipalAmount"]);
                payment.AmortisedPeriodPaymentAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPaymentAmount"]);
                payment.AmortisedPeriodInterestAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodInterestAmount"]);
                payment.AmortisedPeriodPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedPeriodPrincipalAmount"]);
                payment.AmortisedEndPrincipalAmount = Convert.ToDouble(pDt.Rows[counter]["amortisedEndPrincipalAmount"]);
                payment.EffectiveInterestRate = internalRateOfReturn;

                counter += 1;
            }

            return output;

        }
        
        private List<LoanPaymentSchedulePeriodicObj> GenerateIrregularPeriodicScheduleWithAmortisedCost(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.IrregularPaymentSchedule.Count() == 0)
                throw new Exception("Specify a repayment schedule");

            if (loanInput.PrincipalAmount != (loanInput.IrregularPaymentSchedule.Sum(x => x.PaymentAmount)))
                throw new Exception("Payment Amount is not equal to the principal Amount");

            if (loanInput.EffectiveDate > (loanInput.IrregularPaymentSchedule.Min(x => x.PaymentDate)))
                throw new Exception("Effective Date should be less than the payment date(s)");

            List<LoanPaymentSchedulePeriodicObj> paymentSchedule = GenerateIrregularPeriodicSchedule(loanInput, false);

            double internalRateOfReturn = 0;
            if (loanInput.IntegralFeeAmount > 0)
            {
                var amounts = paymentSchedule.Select(x => x.PeriodPaymentAmount).ToList();  //paymentSchedule.Where(x => x.paymentNumber > 0).Select(x => x.periodPaymentAmount).ToList();
                var dates = paymentSchedule.Select(x => x.PaymentDate).ToList();

                amounts[0] = loanInput.PrincipalAmount * -1;
                internalRateOfReturn = XIRR(amounts, dates, 0) * 100;
            }
            else
                internalRateOfReturn = loanInput.InterestRate;

            //FinancialTypes.AMORTSCHED_table amortisedResult;
            //amortisedResult = wct.AMORTSCHED(loanInput.principalAmount - loanInput.integralFeeAmount, loanInput.effectiveDate, (internalRateOfReturn / 100.0), loanInput.interestFirstpaymentDate, numberOfPayments, numberOfPaymentsInAYear, daysInAYear, FV, IntRule);

            loanInput.PrincipalAmount = loanInput.PrincipalAmount - loanInput.IntegralFeeAmount;
            loanInput.InterestRate = internalRateOfReturn;
            List<LoanPaymentSchedulePeriodicObj> paymentScheduleArmotised = GenerateIrregularPeriodicSchedule(loanInput, true);

            int counter = 0;
            foreach (var payment in paymentSchedule)
            {
                payment.AmortisedStartPrincipalAmount = paymentScheduleArmotised[counter].StartPrincipalAmount;
                payment.AmortisedPeriodPaymentAmount = paymentScheduleArmotised[counter].PeriodPaymentAmount;
                payment.AmortisedPeriodInterestAmount = paymentScheduleArmotised[counter].PeriodInterestAmount;
                payment.AmortisedPeriodPrincipalAmount = paymentScheduleArmotised[counter].PeriodPrincipalAmount;
                payment.AmortisedEndPrincipalAmount = paymentScheduleArmotised[counter].EndPrincipalAmount;
                payment.EffectiveInterestRate = internalRateOfReturn;

                counter += 1;
            }

            return paymentSchedule;
        }

        private List<LoanPaymentSchedulePeriodicObj> GenerateIrregularPeriodicSchedule(LoanPaymentScheduleInputObj loanInput, bool isArmotisedSchedule)
        {
            List<LoanPaymentSchedulePeriodicObj> output = new List<LoanPaymentSchedulePeriodicObj>();

            output.Add(new LoanPaymentSchedulePeriodicObj
            {
                PaymentNumber = 0,
                PaymentDate = loanInput.EffectiveDate,
                StartPrincipalAmount = 0,
                PeriodPrincipalAmount = 0,
                AmortisedPeriodInterestAmount = 0,
                PeriodPaymentAmount = 0,
                EndPrincipalAmount = loanInput.PrincipalAmount
            });


            var data = loanInput.IrregularPaymentSchedule.OrderBy(x => x.PaymentDate);


            double previousPrincipalAmount = loanInput.PrincipalAmount;
            DateTime previousPaymentDate = loanInput.EffectiveDate;
            int daysInAYear = GetDaysInAYear((DayCountConventionEnum)loanInput.AccurialBasis);

            int paymentNumber = 1;
            foreach (var item in data)
            {
                LoanPaymentSchedulePeriodicObj loanPeriod = new LoanPaymentSchedulePeriodicObj();
                loanPeriod.PaymentNumber = paymentNumber;
                loanPeriod.PaymentDate = item.PaymentDate;
                loanPeriod.StartPrincipalAmount = previousPrincipalAmount;

                if (isArmotisedSchedule == false)
                { loanPeriod.PeriodPrincipalAmount = item.PaymentAmount; }
                else
                {
                    if (loanInput.IntegralFeeAmount > 0)
                    {
                        var feeDifferential = loanInput.PrincipalAmount / (loanInput.PrincipalAmount + loanInput.IntegralFeeAmount);
                        loanPeriod.PeriodPrincipalAmount = item.PaymentAmount * feeDifferential;
                    }
                    else
                        loanPeriod.PeriodPrincipalAmount = item.PaymentAmount;
                }

                var dateDifferenceCount = (item.PaymentDate - previousPaymentDate).TotalDays;

                loanPeriod.PeriodInterestAmount = (previousPrincipalAmount * (loanInput.InterestRate / 100.0)) * (dateDifferenceCount / daysInAYear);
                loanPeriod.PeriodPaymentAmount = loanPeriod.PeriodPrincipalAmount + loanPeriod.PeriodInterestAmount;
                loanPeriod.EndPrincipalAmount = loanPeriod.StartPrincipalAmount - loanPeriod.PeriodPrincipalAmount;

                output.Add(loanPeriod);

                previousPrincipalAmount = loanPeriod.EndPrincipalAmount;
                previousPaymentDate = loanPeriod.PaymentDate;
                paymentNumber += 1;
            }
            return output;
        }

        private List<LoanPaymentSchedulePeriodicObj> GenerateBulletPeriodicSchedule(LoanPaymentScheduleInputObj loanInput)
        {

            List<LoanPaymentSchedulePeriodicObj> output = new List<LoanPaymentSchedulePeriodicObj>();

            output.Add(new LoanPaymentSchedulePeriodicObj
            {
                PaymentNumber = 0,
                PaymentDate = loanInput.EffectiveDate,
                StartPrincipalAmount = 0,
                PeriodPrincipalAmount = 0,
                AmortisedPeriodInterestAmount = 0,
                PeriodPaymentAmount = 0,
                EndPrincipalAmount = loanInput.PrincipalAmount
            });


            //double previousPrincipalAmount = loanInput.principalAmount;
            //DateTime previousPaymentDate = loanInput.effectiveDate;
            int daysInAYear = GetDaysInAYear((DayCountConventionEnum)loanInput.AccurialBasis);


            LoanPaymentSchedulePeriodicObj loanPeriod = new LoanPaymentSchedulePeriodicObj();
            loanPeriod.PaymentNumber = 1;
            loanPeriod.PaymentDate = loanInput.MaturityDate;
            loanPeriod.StartPrincipalAmount = loanInput.PrincipalAmount;
            loanPeriod.PeriodPrincipalAmount = loanInput.PrincipalAmount;

            var dateDifferenceCount = (loanInput.MaturityDate - loanInput.EffectiveDate).TotalDays;

            loanPeriod.PeriodInterestAmount = (loanInput.PrincipalAmount * (loanInput.InterestRate / 100.0)) * (dateDifferenceCount / daysInAYear);
            loanPeriod.PeriodPaymentAmount = loanPeriod.PeriodPrincipalAmount + loanPeriod.PeriodInterestAmount;
            loanPeriod.EndPrincipalAmount = loanPeriod.StartPrincipalAmount - loanPeriod.PeriodPrincipalAmount;

            output.Add(loanPeriod);

            return output;
        }

        private List<LoanPaymentSchedulePeriodicObj> GenerateBulletPeriodicScheduleWithAmortisedCost(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.EffectiveDate == null)
                throw new Exception("Please Enter Effective Date");
            if (loanInput.MaturityDate == null)
                throw new Exception("Please Enter Maturity Date");
            if (loanInput.PrincipalAmount <= 0)
                throw new Exception("Please Enter Loan Amount");
            if (loanInput.EffectiveDate >= loanInput.MaturityDate)
                throw new Exception("Effective Date must be less than the maturity date");

            List<LoanPaymentSchedulePeriodicObj> paymentSchedule = GenerateBulletPeriodicSchedule(loanInput);

            double internalRateOfReturn = 0;
            loanInput.IntegralFeeAmount = 0;
            if (loanInput.IntegralFeeAmount > 0)
            {
                var amounts = paymentSchedule.Select(x => x.PeriodPaymentAmount).ToList();  //paymentSchedule.Where(x => x.paymentNumber > 0).Select(x => x.periodPaymentAmount).ToList();
                var dates = paymentSchedule.Select(x => x.PaymentDate).ToList();

                amounts[0] = loanInput.PrincipalAmount * -1;
                internalRateOfReturn = XIRR(amounts, dates, 0) * 100;
                //internalRateOfReturn = XIRR(amounts, dates, wct.NULL_DOUBLE) * 100;


            }
            else
                internalRateOfReturn = loanInput.InterestRate;

            loanInput.PrincipalAmount = loanInput.PrincipalAmount - loanInput.IntegralFeeAmount;
            loanInput.InterestRate = internalRateOfReturn;
            List<LoanPaymentSchedulePeriodicObj> paymentScheduleArmotised = GenerateBulletPeriodicSchedule(loanInput);

            int counter = 0;
            foreach (var payment in paymentSchedule)
            {
                payment.AmortisedStartPrincipalAmount = paymentScheduleArmotised[counter].StartPrincipalAmount;
                payment.AmortisedPeriodPaymentAmount = paymentScheduleArmotised[counter].PeriodPaymentAmount;
                payment.AmortisedPeriodInterestAmount = paymentScheduleArmotised[counter].PeriodInterestAmount;
                payment.AmortisedPeriodPrincipalAmount = paymentScheduleArmotised[counter].PeriodPrincipalAmount;
                payment.AmortisedEndPrincipalAmount = paymentScheduleArmotised[counter].EndPrincipalAmount;
                payment.EffectiveInterestRate = internalRateOfReturn;

                counter += 1;
            }

            return paymentSchedule;
        }

        private List<LoanPaymentSchedulePeriodicObj> GenerateConstantPrincipalAndInterestPeriodicSchedule(LoanPaymentScheduleInputObj loanInput)
        {

            int daysInAYear = GetDaysInAYear((DayCountConventionEnum)loanInput.AccurialBasis);

            int numberOfPayments = CalculateNumberOfInstallments(loanInput.InterestFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);

            int numberOfPrincipalPayments = CalculateNumberOfInstallments(loanInput.PrincipalFirstpaymentDate, loanInput.MaturityDate, (FrequencyTypeEnum)loanInput.InterestFrequency);

            int numberOfPaymentsInAYear = (int)_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.InterestFrequency).Value;

            var principalPaymentsInAYear = (int)_dataContext.credit_frequencytype.FirstOrDefault(x => x.FrequencyTypeId == loanInput.PrincipalFrequency).Value;

            int principalPaymentMultiple = Convert.ToInt32(12 / principalPaymentsInAYear);

            var principalFirstPaymentNumber = (numberOfPayments - numberOfPrincipalPayments) + 1;

            List<LoanPaymentSchedulePeriodicObj> output = new List<LoanPaymentSchedulePeriodicObj>();

            output.Add(new LoanPaymentSchedulePeriodicObj
            {
                PaymentNumber = 0,
                PaymentDate = loanInput.EffectiveDate,
                StartPrincipalAmount = 0,
                PeriodPrincipalAmount = 0,
                AmortisedPeriodInterestAmount = 0,
                PeriodPaymentAmount = 0,
                EndPrincipalAmount = loanInput.PrincipalAmount
            });


            double previousPrincipalAmount = loanInput.PrincipalAmount;
            DateTime nextPaymentDate = loanInput.InterestFirstpaymentDate;
            double periodPrincipalAmount = loanInput.PrincipalAmount / numberOfPayments;
            double interestRate = loanInput.InterestRate / 100.0;
            double periodInterestAmount = loanInput.PrincipalAmount * (interestRate / numberOfPaymentsInAYear) * numberOfPayments;

            for (int i = 1; i <= numberOfPayments; i++)
            {
                LoanPaymentSchedulePeriodicObj loanPeriod = new LoanPaymentSchedulePeriodicObj();
                loanPeriod.PaymentNumber = i;
                loanPeriod.PaymentDate = nextPaymentDate;
                loanPeriod.StartPrincipalAmount = previousPrincipalAmount;
                loanPeriod.PeriodPrincipalAmount = periodPrincipalAmount;

                loanPeriod.PeriodInterestAmount = periodInterestAmount;
                loanPeriod.PeriodPaymentAmount = loanPeriod.PeriodPrincipalAmount + loanPeriod.PeriodInterestAmount;
                loanPeriod.EndPrincipalAmount = loanPeriod.StartPrincipalAmount - loanPeriod.PeriodPrincipalAmount;

                output.Add(loanPeriod);

                previousPrincipalAmount = loanPeriod.EndPrincipalAmount;
                nextPaymentDate = NPD(loanPeriod.PaymentDate, loanInput.InterestFirstpaymentDate, numberOfPaymentsInAYear, numberOfPayments);
                //nextPaymentDate = wct.NPD(loanPeriod.paymentDate, loanInput.interestFirstpaymentDate, numberOfPaymentsInAYear, numberOfPayments);
            }
            return output;
        }

        private List<LoanPaymentSchedulePeriodicObj> GenerateConstantPrincipalAndInterestPeriodicScheduleWithAmortisedCost(LoanPaymentScheduleInputObj loanInput)
        {
            if (loanInput.EffectiveDate == null)
                throw new Exception("Please Enter Effective Date");
            if (loanInput.MaturityDate == null)
                throw new Exception("Please Enter Maturity Date");
            if (loanInput.InterestFirstpaymentDate == null)
                throw new Exception("Please Enter First Interest Payment Date");
            if (loanInput.PrincipalFirstpaymentDate == null)
                throw new Exception("Please Enter First Principal Payment Date");
            if (loanInput.PrincipalAmount <= 0)
                throw new Exception("Please Enter Loan Amount");
            if (loanInput.EffectiveDate >= loanInput.MaturityDate)
                throw new Exception("Effective Date must be less than the maturity date");
            if (loanInput.InterestFirstpaymentDate >= loanInput.MaturityDate)
                throw new Exception("First Interest Payment Date must be less than the maturity date");
            if (loanInput.PrincipalFirstpaymentDate >= loanInput.MaturityDate)
                throw new Exception("First Principal Payment Date must be less than the maturity date");
            if (loanInput.EffectiveDate >= loanInput.PrincipalFirstpaymentDate)
                throw new Exception("First Principal Payment Date cannot be less than the effective date");
            if (loanInput.EffectiveDate >= loanInput.InterestFirstpaymentDate)
                throw new Exception("First Interest Payment Date cannot be less than the effective date");

            List<LoanPaymentSchedulePeriodicObj> paymentSchedule = GenerateConstantPrincipalAndInterestPeriodicSchedule(loanInput);

            double internalRateOfReturn = 0;
            loanInput.IntegralFeeAmount = 0;
            if (loanInput.IntegralFeeAmount > 0)
            {
                var amounts = paymentSchedule.Select(x => x.PeriodPaymentAmount).ToList();  //paymentSchedule.Where(x => x.paymentNumber > 0).Select(x => x.periodPaymentAmount).ToList();
                var dates = paymentSchedule.Select(x => x.PaymentDate).ToList();

                amounts[0] = loanInput.PrincipalAmount * -1;
                internalRateOfReturn = XIRR(amounts, dates, 0) * 100;
                //internalRateOfReturn = XIRR(amounts, dates, 1) * 100;

                //internalRateOfReturn = XIRR();
            }
            else
                internalRateOfReturn = loanInput.InterestRate;

            loanInput.PrincipalAmount = loanInput.PrincipalAmount - loanInput.IntegralFeeAmount;
            loanInput.InterestRate = internalRateOfReturn;
            List<LoanPaymentSchedulePeriodicObj> paymentScheduleArmotised = GenerateConstantPrincipalAndInterestPeriodicSchedule(loanInput);

            int counter = 0;
            foreach (var payment in paymentSchedule)
            {
                payment.AmortisedStartPrincipalAmount = paymentScheduleArmotised[counter].StartPrincipalAmount;
                payment.AmortisedPeriodPaymentAmount = paymentScheduleArmotised[counter].PeriodPaymentAmount;
                payment.AmortisedPeriodInterestAmount = paymentScheduleArmotised[counter].PeriodInterestAmount;
                payment.AmortisedPeriodPrincipalAmount = paymentScheduleArmotised[counter].PeriodPrincipalAmount;
                payment.AmortisedEndPrincipalAmount = paymentScheduleArmotised[counter].EndPrincipalAmount;
                payment.EffectiveInterestRate = internalRateOfReturn;

                counter += 1;
            }

            return paymentSchedule;
        }

    }
}
