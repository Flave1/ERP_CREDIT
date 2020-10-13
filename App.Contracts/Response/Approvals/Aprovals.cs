using Banking.Contracts.Response.FlutterWave;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;

namespace Banking.Contracts.Response.Approvals
{
    public class GoForApprovalRequest
    {
        public int StaffId { get; set; }
        public int CompanyId { get; set; }
        public int StatusId { get; set; }
        public List<int> TargetId { get; set; }
        public string Comment { get; set; }
        public int OperationId { get; set; }
        public bool DeferredExecution { get; set; }
        public int WorkflowId { get; set; }
        public bool ExternalInitialization { get; set; }
        public bool EmailNotification { get; set; }
        public int WorkflowTaskId { get; set; }
        public int ApprovalStatus { get; set; }
    }
    public class ApprovalRegRespObj
    {
        public int ResponseId { get; set; } 
        public int AnyIdentifier { get; set; }
        public bool response { get; set; }
        public LoanPaymentScheduleInputObj loanPayment { get; set; }
        public CustomerTransactionObj DisbursementEntry { get; set; }
        public CustomerTransactionObj IntegralFeeEntry { get; set; }
        public BulkTransferObj FlutterObj { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class GoForApprovalRespObj
    {
        public int SupplierId { get; set; }
        public bool HasWorkflowAccess { get; set; }
        public bool EnableWorkflow { get; set; }
        public bool ApprovalProcessStarted { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowTaskRespObj
    {
        public List<WorkflowTaskObj> workflowTasks { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class WorkflowTaskObj
    {
        public int TargetId { get; set; }
        public int OperationId { get; set; }
        public int WorkflowId { get; set; }
        public string StaffEmail { get; set; }
        public string WorkflowToken { get; set; }
    }

    public class IndentityServerApprovalCommand
    {
        public int ApprovalStatus { get; set; }
        public string ApprovalComment { get; set; }
        public int TargetId { get; set; }
        public int ReferredStaffId { get; set; }
        public string WorkflowToken { get; set; }
    }

    public class CustomerTransactionObj
    {
        public string CasaAccountNumber { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string TransactionCode { get; set; }
        public string ReferenceNo { get; set; }
        public string TransactionType { get; set; }
        public decimal? Amount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal? AvailableBalance { get; set; }
        public int CustomerTransactionId { get; set; }
        public string Beneficiary { get; set; }
    }

    public class CustomerTransactionRespObj
    {
        public IEnumerable<CustomerTransactionObj> customerTransaction { get; set; }
        public APIResponseStatus Status { get; set; }
        public byte[] Export { get; set; }
    }

    public class CustomerTransactionSearchObj
    {
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public string AccountNumber { get; set; }
    }
}
