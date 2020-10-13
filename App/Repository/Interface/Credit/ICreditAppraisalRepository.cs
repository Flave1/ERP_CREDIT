using Banking.Contracts.Response.Approvals;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ApprovalObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ICreditAppraisalRepository
    {
        Task<ActionResult<LoanApplicationRespObj>> GetLoanApplicationForAppraisalAsync();
        UserPrivilegeObj GetUserPrivilege(int operationId, string userName);
        IEnumerable<ApprovalDetailsObj> GetApprovalTrail(int targetId, string workflowToken);
        ApprovalRegRespObj LoanApplicationApproval(int targetId, int approvalStatusId);
        IEnumerable<PreviousStaff> GetApprovalTrailStaff(int targetId, string workflowToken);




        ApprovalRegRespObj CustomerTransaction(CustomerTransactionObj item);
        ApprovalRegRespObj CustomerTransactionEoD(CustomerTransactionObj item);
        IEnumerable<CustomerTransactionObj> GetAllCustomerTransactionBySearch(string acctNumber, DateTime? date1, DateTime? date2);
        byte[] GenerateExportCustomerTransaction(string acctNumber, DateTime? date1, DateTime? date2);
        CustomerTransactionObj GetCustomerTransaction(int TransactionId);

    }
}
