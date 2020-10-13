using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Banking.Contracts.Response.Credit
{
    public class LoanObjs
    {
        public class LoanObj : GeneralEntity
        {
            public string ApprovedComments { get; set; }

            public string CasaAccountNumber { get; set; }

            public DateTime ApprovedDate { get; set; }

            public string DisbursedComments { get; set; }

            public DateTime? DisbursedDate { get; set; }


            public int TargetId { get; set; }
            public int ApprovalStatusId { get; set; }
            public string Comment { get; set; }


            public string LoanRefNumber { get; set; }


            public int? CustomerRiskRatingId { get; set; }

            public int? AccrualBasis { get; set; }

            public int TrailApprovalStatus { get; set; }
            public string NostroAccount { get; set; }
            public string CurrencyCode { get; set; }

            public string ApprovalStatus { get; set; }
            public string ApprovedByName { get; set; }

            public int RequestStatusId { get; set; }
            public bool RequestDeleted { get; set; }
            public string CasaAccountDetails { get; set; }
            public int CompanyId { get; set; }
            public string CompanyName { get; set; }

            public decimal DisbursableAmount { get; set; }
            public string LoanStatusName { get; set; }
            public bool IsBidbond { get; set; }
            public bool IsOverdraft { get; set; }
            public object CommercialPrincipal { get; set; }

            public int NotificationDuration { get; set; }
            public int LoanId { get; set; }
            public int CustomerId { get; set; }
            public int ProductId { get; set; }
            public double ProductPriceIndexRate { get; set; }
            public int CasaAccountId { get; set; }
            public int? CasaAccountId2 { get; set; }
            public int LoanApplicationId { get; set; }
            public int LoanApplicationDetailId { get; set; }

            public int BranchId { get; set; }
            public string LoanReferenceNumber { get; set; }
            public string RelatedloanReferenceNumber { get; set; }
            public string ApplicationReferenceNumber { get; set; }
            public int Tenor { get { return (this.MaturityDate - this.EffectiveDate).Days; } }
            public int TenorUsed { get { return (DateTime.Now - this.EffectiveDate).Days; } }
            public int? PrincipalFrequencyTypeId { get; set; }
            public int? InterestFrequencyTypeId { get; set; }
            public int PrincipalNumberOfInstallment { get; set; }
            public int InterestNumberOfInstallment { get; set; }
            public int RelationshipOfficerId { get; set; }
            public int RelationshipManagerId { get; set; }
            public string MisCode { get; set; }
            public string TeamMiscode { get; set; }
            public double? InterestRate { get; set; }
            public DateTime EffectiveDate { get; set; }
            public DateTime MaturityDate { get; set; }
            public DateTime BookingDate { get; set; }
            public decimal PrincipalAmount { get; set; }
            public decimal InterestIncome { get; set; }
            public decimal PrincipalAmountIR { get; set; }
            public decimal InterestAmountIR { get; set; }
            public int PrincipalInstallmentLeft { get; set; }
            public int InterestInstallmentLeft { get; set; }
            public int? ApprovedBy { get; set; }
            public string ApproverComment { get; set; }
            public DateTime? DateApproved { get; set; }
            public int LoanStatusId { get; set; }
            public string LoanStatus { get; set; }
            public int ScheduleTypeId { get; set; }
            public string ScheduleTypeName { get; set; }
            public bool ShouldDisbursed { get; set; }
            public bool IsDisbursed { get; set; }
            public int? DisbursedBy { get; set; }
            public string DisburserComment { get; set; }
            public DateTime? DisburseDate { get; set; }
            public decimal? ApprovedAmount { get; set; }
            public bool CreditAppraisalCompleted { get; set; }
            public int? OperationId { get; set; }
            public int? OperationTypeId { get; set; }
            public string OperationName { get; set; }
            public int? CustomerGroupId { get; set; }
            public int LoanTypeId { get; set; }
            public decimal OverDraft { get; set; }
            public string ArchiveCode { get; set; }


            public decimal EquityContribution { get; set; }
            public int SubSectorId { get; set; }
            public DateTime? FirstPrincipalPaymentDate { get; set; }
            public DateTime? FirstInterestPaymentDate { get; set; }
            public decimal OutstandingPrincipal { get; set; }
            public decimal OutstandingInterest { get; set; }
            public int? PrincipalAdditionCount { get; set; }
            public int? PrincipalReductionCount { get; set; }
            public bool FixedPrincipal { get; set; }
            public bool ProfileLoan { get; set; }
            public bool DischargeLetter { get; set; }
            public bool SuspendInterest { get; set; }
            public bool Booked { get; set; }
            public bool? Scheduled { get; set; }
            public bool? IsScheduledPrepayment { get; set; }
            public decimal? ScheduledPrepaymentAmount { get; set; }
            public DateTime? ScheduledPrepaymentDate { get; set; }
            public int? ScheduledPrepaymentFrequencyTypeId { get; set; }
            public int CustomerSensitivityLevelId { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string CustomerCode { get; set; }
            public int CurrencyId { get; set; }
            public string Currency { get; set; }
            public int AccurialBasis { get; set; }
            public decimal IntegralFeeAmount { get; set; }
            public int FirstDayType { get; set; }
            public bool IsCamsol { get; set; }
            public int? InternalPrudentialGuidelineStatusId { get; set; }
            public int? ExternalPrudentialGuidelineStatusId { get; set; }
            public int? UserPrudentialGuidelineStatusId { get; set; }
            public string InternalPrudentialGuidelineStatus { get; set; }
            public string ExternalPrudentialGuidelineStatus { get; set; }
            public string UserPrudentialGuidelineStatus { get; set; }
            public DateTime? NplDate { get; set; }
            public int ScheduleDayCountConventionId { get; set; }
            public int ScheduleDayInterestTypeId { get; set; }
            public int Teno { get; set; }
            public bool AllowForceDebitRepayment { get; set; }

            //.............Fee Attribute.....................//
            public double ExchangeRate { get; set; }

            public DateTime PaymentDate { get; set; }
            public decimal TotalAmount { get; set; }
            public int ChargeFeeId { get; set; }

            //...................For Loan Review................//
            public int LoanReviewOperationTypeId { get; set; }
            public string ReviewDetails { get; set; }
            //.............Other Attributes................//
            public int? ProductTypeId { get; set; }

            public string ProductTypeName { get; set; }
            public string CreatorName { get; set; }
            public string ProductAccountName { get; set; }
            public string LoanTypeName { get; set; }
            public string BranchName { get; set; }
            public string SubSectorName { get; set; }
            public string SectorName { get; set; }
            public string RelationshipOfficerName { get; set; }
            public string RelationshipManagerName { get; set; }
            public string ProductName { get; set; }
            public string CustomerSensitivityLevelName { get; set; }
            public string CustomerName { get; set; }
            public string PricipalFrequencyTypeName { get; set; }
            public string InterestFrequencyTypeName { get; set; }
            public string RelationshipManagerEmail { get; set; }
            public string RelationshipOfficerEmail { get; set; }
            public decimal CustomerAvailableAmount { get; set; }
            public bool FeeOverride { get; set; }
            public int LoanBookingRequestId { get; set; }
            public int StaffId { get; set; }
            public int StaffName { get; set; }
            public bool HasLein { get; set; }
            public int PostNoStatusId { get; set; }
            public int LmsApplicationDetailId { get; set; }

            public decimal OverdraftLimit { get; set; }
            public bool MaintainTenor { get; set; }
            public decimal AccrualedAmount { get; set; }
            public int Newtenor { get; set; }
            public int ScheduleTypeCategoryId { get; set; }
            public DateTime PreviousEffectiveDate { get; set; }
            public decimal PastDueTotal { get; set; }
            public decimal OverDraftCheckAmount { get; set; }

            public decimal PastDuePrincipal { get; set; }
            public decimal PastDueInterest { get; set; }
            public decimal InterestOnPastDuePrincipal { get; set; }
            public decimal InterestOnPastDueInterest { get; set; }
            public decimal PenalChargeAmount { get; set; }
            public DateTime LastRestructureDate { get; set; }
            public int LoanSystemTypeId { get; set; }
            public bool IsPerforming { get; set; }
            public object LoanPrincipal { get; set; }
            public string ApprovedComment { get; set; }
            public decimal ScheduleDayCountConvention { get; set; }
            public string IsDisbursedState { get; set; }
            public string ProductPriceIndexName { get; set; }
            public string RevolvingType { get; set; }
            public string Istenored { get; set; }
            public string IsbankFormat { get; set; }
            public int LoadArchiveId { get; set; }
            public bool IsTermLoam { get; set; }
            public bool IsOD { get; set; }
            public int PostedByStaffId { get; set; }
            public string BatchNo { get; set; }
            public decimal CreditAmount { get; set; }
            public decimal DebitAmount { get; set; }
            public string Description { get; set; }
            public DateTime ValueDate { get; set; }
            public DateTime PostedDate { get; set; }
            public DateTime PostedTime { get; set; }
            public double CurrencyRate { get; set; }
            public string PostCurrency { get; set; }
            public string SourceReferenceNumber { get; set; }
            public decimal RequestedAmount { get; set; }
            public string Remark { get; set; }
            public string NostroAccountId { get; set; }
            public string NostroRateCode { get; set; }
            public string NotstroCurrency { get; set; }
            public decimal? NostroRateAmount { get; set; }
            public string BaseReferenceNumber { get; set; }
            public string CategoryName { get; set; }
            public string CurrencyName { get; set; }
            public decimal DailyAccrualAmount { get; set; }
            public DateTime Date { get; set; }
            public decimal MainAmount { get; set; }
            public DateTime? SystemCurrentDate { get; set; }
            public double? FeeAmount { get; set; }
        }

        public class CreditLoanCommentObj : GeneralEntity
        {
            public int LoanCommentId { get; set; }

            public DateTime? Date { get; set; }

            public string Comment { get; set; }

            public string NextStep { get; set; }

            public int LoanId { get; set; }

            public int LoanScheduleId { get; set; }
        }

        public class CreditLoanDecisionObj : GeneralEntity
        {
            public int LoanDecisionId { get; set; }

            public DateTime? Date { get; set; }

            public string Decision { get; set; }

            public int LoanId { get; set; }

            public int LoanScheduleId { get; set; }

        }

        public class LoanListObj
        {
            public int LoanId { get; set; }

            public int LoanScheduleId { get; set; }

            public int CustomerId { get; set; }

            public string CustomerName { get; set; }
            public string WorkflowToken { get; set; }
            public string LoanStatus { get; set; }

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public int LoanApplicationId { get; set; }

            public int? PrincipalFrequencyTypeId { get; set; }

            public string PrincipalFrequencyType { get; set; }
            public int? InterestFrequencyTypeId { get; set; }
            public string InterestFrequencyType { get; set; }
            public int? ScheduleTypeId { get; set; }
            public string ScheduleType { get; set; }
            public int CurrencyId { get; set; }
            public string Currency { get; set; }
            public double ExchangeRate { get; set; }

            public DateTime BookingDate { get; set; }

            public DateTime EffectiveDate { get; set; }

            public DateTime MaturityDate { get; set; }

            public int? LoanStatusId { get; set; }

            public bool IsDisbursed { get; set; }
            public bool Commented { get; set; }
            public bool Decided { get; set; }


            public int? CompanyId { get; set; }

            public string LoanRefNumber { get; set; }
            public string Description { get; set; }

            public decimal PrincipalAmount { get; set; }
            public decimal? OutstandingBalance { get; set; }
            public decimal? Repayment { get; set; }
            public decimal? RepaymentActual { get; set; }
            public decimal? RepaymentPending { get; set; }
            public decimal? OperatingAccountBal { get; set; }
            public decimal? PastDueAmount { get; set; }
            public int? DaysInOverdue { get; set; }
            public int PastDueId { get; set; }
            public double? LateRepaymentCharge { get; set; }
            public DateTime? PaymentDate { get; set; }
            public DateTime? PaymentDateDefault { get; set; }

            public decimal? EquityContribution { get; set; }

            public DateTime? FirstPrincipalPaymentDate { get; set; }

            public DateTime? FirstInterestPaymentDate { get; set; }

            public decimal? OutstandingPrincipal { get; set; }
            public decimal? OutstandingAmortisedPrincipal { get; set; }

            public decimal? OutstandingInterest { get; set; }
            public decimal? OutstandingAmortisedInterest { get; set; }

            public DateTime? NPLDate { get; set; }

            public int? AccrualBasis { get; set; }

            public int? FirstDayType { get; set; }

            public decimal? CreditScore { get; set; }

            public decimal? ProbabilityOfDefault { get; set; }         

        }

        public class LoanReviewOperationObj : GeneralEntity
        {
            public int LoanReviewOperationsId { get; set; }

            public int LoanId { get; set; }

            public int ProductTypeId { get; set; }

            public int OperationTypeId { get; set; }

            public DateTime ProposedEffectiveDate { get; set; }

            public string ReviewDetails { get; set; }

            public decimal? InterateRate { get; set; }

            public decimal Prepayment { get; set; }

            public int? PrincipalFrequencyTypeId { get; set; }

            public int? InterestFrequencyTypeId { get; set; }

            public DateTime? PrincipalFirstPaymentDate { get; set; }

            public DateTime? InterestFirstPaymentDate { get; set; }

            public DateTime? MaturityDate { get; set; }

            public int? Tenor { get; set; }

            public int? CASA_AccountId { get; set; }

            public decimal? OverDraftTopup { get; set; }

            public decimal? Fee_Charges { get; set; }

            public string TerminationAndReBook { get; set; }

            public string CompleteWriteOff { get; set; }

            public string CancelUndisbursedLoan { get; set; }

            public int ApprovalStatusId { get; set; }

            public bool IsManagementRate { get; set; }

            public bool OperationCompleted { get; set; }

            public int? ScheduleTypeId { get; set; }

            public int? ScheduleDayCountId { get; set; }

            public int? InterestTypeId { get; set; }

            public int LmsApplicationDetailId { get; set; }

            public int StaffId { get; set; }

            public string LoanReferenceNumber { get; set; }
            public int CompanyId { get; set; }
            public decimal PrincipalAmount { get; set; }
            public int ProductId { get; set; }
            public int OperationId { get; set; }
            public int CurrencyId { get; set; }
            public decimal InterestIncome { get; set; }
            public decimal PrincipalAmountIR { get; set; }
            public decimal InterestAmountIR { get; set; }

            public List<LoanReviewIrregularScheduleObj> ReviewIrregularSchedule { get; set; }
        }

        public class LoanReviewIrregularScheduleObj
        {
            public int IrregularScheduleInputId { get; set; }

            public int LoanReviewOperationId { get; set; }

            public DateTime PaymentDate { get; set; }

            public decimal PaymentAmount { get; set; }

            public int CreatedBy { get; set; }

            public DateTime DateTimeCreated { get; set; }
        }

        public class LoanReviewOperationApprovalObj
        {
            public string CurrencyCode { get; set; }
            public bool IsBankFormat { get; set; }
            public int CompanyId { get; set; }
            public string CreatedByName { get; set; }
            public DateTime DateTimeCreated { get; set; }
            public decimal MaturityAmount { get; set; }
            public string RelatedReferenceNumber { get; set; }
            public decimal InterestAmount { get; set; }

            public int LoanId { get; set; }
            public int CustomerId { get; set; }
            public int ProductId { get; set; }
            public decimal ProductPriceIndexRate { get; set; }
            public int CasaAccountId { get; set; }
            public int LoanApplicationDetailId { get; set; }

            public int BranchId { get; set; }
            public string LoanReferenceNumber { get; set; }
            public string ApplicationReferenceNumber { get; set; }
            public int Tenor { get { return (this.MaturityDate - this.EffectiveDate).Days; } }
            public int PrincipalFrequencyTypeId { get; set; }
            public int InterestFrequencyTypeId { get; set; }
            public int PrincipalNumberOfInstallment { get; set; }
            public int InterestNumberOfInstallment { get; set; }
            public int RelationshipOfficerId { get; set; }
            public int RelationshipManagerId { get; set; }
            public string MisCode { get; set; }
            public string TeamMiscode { get; set; }
            public double InterestRate { get; set; }
            public DateTime EffectiveDate { get; set; }
            public DateTime MaturityDate { get; set; }
            public DateTime BookingDate { get; set; }
            public decimal PrincipalAmount { get; set; }
            public int PrincipalInstallmentLeft { get; set; }
            public int InterestInstallmentLeft { get; set; }
            public int ApprovalStatusId { get; set; }
            public string ApprovalStatusName { get; set; }
            public int? ApprovedBy { get; set; }
            public string ApproverComment { get; set; }
            public DateTime? DateApproved { get; set; }
            public int LoanStatusId { get; set; }
            public int ScheduleTypeId { get; set; }
            public bool IsDisbursed { get; set; }
            public int? DisbursedBy { get; set; }
            public string DisburserComment { get; set; }
            public DateTime? DisburseDate { get; set; }
            public decimal? ApprovedAmount { get; set; }
            public bool CreditAppraisalCompleted { get; set; }
            public int? OperationId { get; set; }
            public string OperationName { get; set; }
            public int? CustomerGroupId { get; set; }
            public int LoanTypeId { get; set; }

            public decimal EquityContribution { get; set; }
            public int SubSectorId { get; set; }
            public DateTime? FirstPrincipalPaymentDate { get; set; }
            public DateTime? FirstInterestPaymentDate { get; set; }
            public decimal OutstandingPrincipal { get; set; }
            public decimal OutstandingInterest { get; set; }
            public int? PrincipalAdditionCount { get; set; }
            public int? PrincipalReductionCount { get; set; }
            public bool FixedPrincipal { get; set; }
            public bool ProfileLoan { get; set; }
            public bool DischargeLetter { get; set; }
            public bool SuspendInterest { get; set; }
            public bool Booked { get; set; }
            public bool? Scheduled { get; set; }
            public bool? IsScheduledPrepayment { get; set; }
            public decimal? ScheduledPrepaymentAmount { get; set; }
            public DateTime? ScheduledPrepaymentDate { get; set; }
            public int ScheduledPrepaymentFrequencyTypeId { get; set; }
            public int CustomerSensitivityLevelId { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string CustomerCode { get; set; }
            public string ProductAccountNumber { get; set; }
            public int CurrencyId { get; set; }
            public string Currency { get; set; }
            public int AccurialBasis { get; set; }
            public double IntegralFeeAmount { get; set; }
            public int FirstDayType { get; set; }
            public bool IsCamsol { get; set; }
            public int InternalPrudentialGuidelineStatusId { get; set; }
            public int ExternalPrudentialGuidelineStatusId { get; set; }
            public DateTime NplDate { get; set; }
            public int ScheduleDayCountConventionId { get; set; }
            public int ScheduleDayInterestTypeId { get; set; }
            public int CustomerRiskRatingId { get; set; }

            // public double productPriceIndexRate { get; set; }
            public bool AllowForceDebitRepayment { get; set; }

            //.............Fee Attribute.....................//
            public double ExchangeRate { get; set; }

            public DateTime PaymentDate { get; set; }
            public decimal TotalAmount { get; set; }
            public int ChargeFeeId { get; set; }

            //.............Other Attributes................//
            public int ProductTypeId { get; set; }

            public string ProductTypeName { get; set; }
            public string CreatorName { get; set; }
            public string ProductAccountName { get; set; }
            public string LoanTypeName { get; set; }
            public string BranchName { get; set; }
            public string SubSectorName { get; set; }
            public string SectorName { get; set; }
            public string RelationshipOfficerName { get; set; }
            public string RelationshipManagerName { get; set; }
            public string ProductName { get; set; }
            public string CustomerSensitivityLevelName { get; set; }
            public string CustomerName { get; set; }
            public string PricipalFrequencyTypeName { get; set; }
            public string InterestFrequencyTypeName { get; set; }
            public string comment { get; set; }

            //Loan Review Operation
            public int LoanReviewOperationsId { get; set; }
            public int OperationTypeId { get; set; }
            public string OperationTypeName { get; set; }
            public DateTime NewEffectiveDate { get; set; }
            public string ReviewDetails { get; set; }
            public decimal? NewInterateRate { get; set; }
            public decimal? Prepayment { get; set; }
            public int? NewPrincipalFrequencyTypeId { get; set; }
            public int? NewInterestFrequencyTypeId { get; set; }
            public DateTime? NewPrincipalFirstPaymentDate { get; set; }
            public DateTime? NewInterestFirstPaymentDate { get; set; }
            public int? NewTenor { get; set; }
            public int? CASA_AccountId { get; set; }
            public decimal? OverDraftTopup { get; set; }
            public decimal? Fee_Charges { get; set; }
            public string TerminationAndReBook { get; set; }
            public string CompleteWriteOff { get; set; }
            public string CancelUndisbursedLoan { get; set; }
        }

        public class LoanRespObj
        {
            public IEnumerable<LoanObj> Loans { get; set; }
            public IEnumerable<LoanListObj> ManageLoans { get; set; }
            public IEnumerable<LoanReviewOperationApprovalObj>LoanReviewOperations { get; set; }
            public LoanListObj LoanDetail { get; set; }
            public IEnumerable<LoanOperationTypeObj> LoanOperationType { get; set; }
            public LoanPaymentScheduleInputObj LoanSchedule { get; set; }
            public byte[] Export { get; set; }
            public APIResponseStatus Status { get; set; }
            public string LoanRefNumber { get; set; }
        }

        public class LoanCommentRespObj
        {
            public IEnumerable<CreditLoanCommentObj> LoanComments { get; set; }
            public IEnumerable<CreditLoanDecisionObj> LoanDecisions { get; set; }
            public APIResponseStatus Status { get; set; }
        }

            public class LoanSearchObj
        {
            public int LoanApplicationId { get; set; }
            public int LoanId { get; set; }
            public int CustomerId { get; set; }
            public int LoanDecisionId { get; set; }
            public int LoanCommentId { get; set; }
            public int LoanScheduleId { get; set; }
            public int LoanChequeId { get; set; }
            public string LoanRefNumber { get; set; }
        }

        public class DeleteLoanCommand
        {
            public List<int> Ids { get; set; }
        }

        public class CreditLoanRespObj
        {
            public IEnumerable<credit_loan_obj> Loans { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class credit_loan_obj
        {
            public int LoanId { get; set; }

            public int CustomerId { get; set; }

            public int ProductId { get; set; }

            public int LoanApplicationId { get; set; }

            public int? PrincipalFrequencyTypeId { get; set; }

            public int? InterestFrequencyTypeId { get; set; }

            public int? ScheduleTypeId { get; set; }

            public int CurrencyId { get; set; }

            public double ExchangeRate { get; set; }

            public int ApprovalStatusId { get; set; }

            public int ApprovedBy { get; set; }

            public string ApprovedComments { get; set; }

            public DateTime ApprovedDate { get; set; }

            public DateTime BookingDate { get; set; }

            public DateTime EffectiveDate { get; set; }

            public DateTime MaturityDate { get; set; }

            public int? LoanStatusId { get; set; }

            public bool IsDisbursed { get; set; }

            public int? DisbursedBy { get; set; }

            public string DisbursedComments { get; set; }

            public DateTime? DisbursedDate { get; set; }

            public int? CompanyId { get; set; }

            public string LoanRefNumber { get; set; }

            public decimal PrincipalAmount { get; set; }

            public decimal? EquityContribution { get; set; }

            public DateTime? FirstPrincipalPaymentDate { get; set; }

            public DateTime? FirstInterestPaymentDate { get; set; }

            public decimal? OutstandingAmortisedPrincipal { get; set; }

            public decimal? OutstandingPrincipal { get; set; }
            public decimal? OutstandingOldPrincipal { get; set; }

            public decimal? OutstandingOldInterest { get; set; }

            public decimal? OutstandingInterest { get; set; }

            public int? AccrualBasis { get; set; }

            public int? FirstDayType { get; set; }

            public DateTime? NPLDate { get; set; }

            public int? CustomerRiskRatingId { get; set; }

            public int? LoanOperationId { get; set; }

            public int? StaffId { get; set; }

            public int? CasaAccountId { get; set; }

            public int? BranchId { get; set; }

            public decimal? PastDuePrincipal { get; set; }

            public decimal? PastDueInterest { get; set; }

            public double InterestRate { get; set; }

            public decimal? InterestOnPastDueInterest { get; set; }

            public decimal? InterestOnPastDuePrincipal { get; set; }

            public decimal? IntegralFeeAmount { get; set; }

            public string WorkflowToken { get; set; }

            public bool Active { get; set; }

            public bool? Deleted { get; set; }

            public string CreatedBy { get; set; }

            public DateTime? CreatedOn { get; set; }

            public string UpdatedBy { get; set; }

            public DateTime? UpdatedOn { get; set; }
        }

        public class loan_cheque_obj : GeneralEntity
        {
            public string Start { get; set; }
            public string End { get; set; }
            public string ChequeNo { get; set; }
            public byte [] GeneralUpload { get; set; }
            public byte [] SingleUpload { get; set; }
            public int Status { get; set; }
            public string StatusName { get; set; }
            public int LoanChequeId { get; set; }
            public int LoanId { get; set; }
            public int LoanChequeListId { get; set; }
        }

        public class LoanChequeRespObj
        {
            public IEnumerable<loan_cheque_obj> LoanCheque { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public byte[] Export { get; set; }
            public string FileExtension { get; set; }
            public string FileName { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class EndOfDayRequestObj
        {
            public DateTime RequestDate { get; set; }
        }

        public class repaymentObj
        {
            public string amount { get; set; }
            public int customerId { get; set; }
            public int loanScheduleId { get; set; }
            public int loanId { get; set; }
        }

        public class LoanRepaymentRespObj
        {
            public string Link { get; set; }
            public APIResponseStatus Status { get; set; }
        }
    }
}
