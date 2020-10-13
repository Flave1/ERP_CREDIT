using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class LoanScheduleObjs
    {

        public class LoanScheduleRespObj
        {
            public IEnumerable<LoanPaymentSchedulePeriodicObj> LoanPaymentSchedule { get; set; }
            public byte[] export { get; set; }
            public bool response { get; set; }
            public APIResponseStatus Status { get; set; }
        }
        public class LoanScheduleRegRespObj
        {
            public bool response { get; set; }
            public APIResponseStatus Status { get; set; }
        }
        public class LoanScheduleSearchObj
        {
            public int ScheduleMethodId { get; set; }
            public LoanPaymentScheduleInputObj loanInput { get; set; }
            public int LoanId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteLoanScheduleCommand : IRequest<DeleteRespObj>
        {
            public List<int> ScheduleMethodIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanPaymentScheduleInputObj : GeneralEntity
        {
            public int ScheduleMethodId { get; set; }
            public Double PrincipalAmount { get; set; }
            public Double OldBalance { get; set; }
            public Double OldInterest { get; set; }
            public DateTime EffectiveDate { get; set; }
            public double InterestRate { get; set; }
            public int? PrincipalFrequency { get; set; }
            public int? InterestFrequency { get; set; }

            private int _Tenor;

            public int Tenor
            {
                get { return (MaturityDate - EffectiveDate).Days; }
                set { _Tenor = value; }
            }

            public DateTime PrincipalFirstpaymentDate { get; set; }
            public DateTime InterestFirstpaymentDate { get; set; }
            public DateTime MaturityDate { get; set; }
            public int AccurialBasis { get; set; }
            public double IntegralFeeAmount { get; set; }
            public int FirstDayType { get; set; }
            public int StaffId { get; set; }
            public int CompanyId { get; set; }
            public int CustomerId { get; set; }
            public int ProductId { get; set; }
            public int CurrencyId { get; set; }
            public List<IrregularLoanScheduleInputViewModel> IrregularPaymentSchedule { get; set; }
        }

        public class IrregularLoanScheduleInputViewModel
        {
            public DateTime PaymentDate { get; set; }
            public Double PaymentAmount { get; set; }
        }

        public class LoanOperationTypeObj
        {
            public int OperationTypeId { get; set; }
            public string OperationTypeName { get; set; }
        }

        public class LoanPaymentSchedulePeriodicObj : GeneralEntity
        {
            public int PaymentNumber { get; set; }
            public DateTime PaymentDate { get; set; }
            public double StartPrincipalAmount { get; set; }
            public double PeriodPaymentAmount { get; set; }
            public double PeriodInterestAmount { get; set; }
            public double PeriodPrincipalAmount { get; set; }
            public double EndPrincipalAmount { get; set; }
            public double InterestRate { get; set; }
            public double AmortisedStartPrincipalAmount { get; set; }
            public double AmortisedPeriodPaymentAmount { get; set; }
            public double AmortisedPeriodInterestAmount { get; set; }
            public double AmortisedPeriodPrincipalAmount { get; set; }
            public double AmortisedEndPrincipalAmount { get; set; }
            public double EffectiveInterestRate { get; set; }
            public int LoanId { get; set; }
            public double? PreviousInterestAmount { get; set; }
            public double? PreviousPrincipalAmount { get; set; }
            public double? Pmt { get; set; }
            public double? Irr { get; set; }
            public DateTime? NextpaymentDate { get; set; }
        }


        public class LoanPaymentScheduleDailyObj : GeneralEntity
        {
            public int PaymentNumber { get; set; }
            public DateTime Date { get; set; }
            public DateTime PaymentDate { get; set; }

            public double OpeningBalance { get; set; }
            public double StartPrincipalAmount { get; set; }
            public double DailyPaymentAmount { get; set; }
            public double DailyInterestAmount { get; set; }
            public double DailyPrincipalAmount { get; set; }
            public double ClosingBalance { get; set; }
            public double EndPrincipalAmount { get; set; }
            public double AccruedInterest { get; set; }
            public double AmortisedCost { get; set; }
            public double NorminalInterestRate { get; set; }
            public double AmOpeningBalance { get; set; }
            public double AmStartPrincipalAmount { get; set; }
            public double AmDailyPaymentAmount { get; set; }
            public double AmDailyInterestAmount { get; set; }
            public double AmDailyPrincipalAmount { get; set; }
            public double AmClosingBalance { get; set; }
            public double AmEndPrincipalAmount { get; set; }
            public double AmAccruedInterest { get; set; }
            public double AmAmortisedCost { get; set; }

            public double BalloonAmt { get; set; }
            public double DiscountPremium { get; set; }
            public double UnEarnedFee { get; set; }
            public double EarnedFee { get; set; }
            public double EffectiveInterestRate { get; set; }
            public int NumberOfPeriods { get; set; }
            public int LoanId { get; set; }
            public double BallonAmount { get; set; }
            public double? PreviousInterestAmount { get; set; }
            public double? PreviousPrincipalAmount { get; set; }

        }


        public class LoanRepaymentObj : GeneralEntity
        {
            public int LoanId { get; set; }
            public string LoanRefNo { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal PeriodInterestAmount { get; set; }
            public decimal PeriodPrincipalAmount { get; set; }
            public double InterestRate { get; set; }
            public int ProductId { get; set; }
            public int CompanyId { get; set; }
            public int CategoryId { get; set; }
            public double ExchangeRate { get; set; }
            public int BranchId { get; set; }
            public int CurrencyId { get; set; }
            public decimal TotalAmount { get; set; }
            public int CasaAccountId { get; set; }
            public byte TransactionTypeId { get; set; }
            public int LoanApplicationNumberId { get; set; }
            public int ChecklistId { get; set; }
            public decimal ClosingBalance { get; set; }
            public string CasaAccountNumber { get; set; }
        }

        public class LoanPaymentRestructureScheduleInputObj : LoanPaymentScheduleInputObj

        {
            public int? ProposedTenor { get; set; }
            public int LoanChangeType { get; set; }
            public int LoanId { get; set; }
            public Double PayAmount { get; set; }
            public Double NewAmount { get; set; }
            public int? NewPrincipalFrequency { get; set; }
            public int? NewInterestFrequency { get; set; }
            public DateTime? NewPrincipalFirstpaymentDate { get; set; }
            public DateTime? NewInterestFirstpaymentDate { get; set; }
            public Double PayInterest { get; set; }
            public Double NewInterest { get; set; }
            public int NewTenor { get { return (MaturityDate - NewEffectiveDate).Days; } }
            public DateTime NewEffectiveDate { get; set; }
            public decimal Prepayment { get; set; }
            public int OperationId { get; set; }
            public bool IsManagementInterestRate { get; set; }
            public DateTime? NewMaturityDate { get; set; }
            public int NewTenorPrepayment { get { return ((DateTime)NewMaturityDate - NewEffectiveDate).Days; } }
        }
    }
}
