using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class ApprovalObjs
    {
        public class ApprovalObj : GeneralEntity
        {
            public int OperationId { get; set; }
            public int TargetId { get; set; }
            public string JournalTargetId { get; set; }
            public short ApprovalStatusId { get; set; }
            public string Comment { get; set; }
            public string OperationURL { get; set; }
            public int MyLevelId { get; set; }
            public int NextLevelId { get; set; }
            public decimal Amount { get; set; }
            public bool ExternalInitialization { get; set; }
            public bool KeepPending { get { return true; } }
            public bool DeferredExecution { get; set; }
            public int StaffId { get; set; }
            public int ReferredStaffId { get; set; }
            public int CompanyId { get; set; }
        }

        public class LoanApprovalTrailObj
        {
            public int ApprovalTrailId { get; set; }
            public int TargetId { get; set; }
            public int OperationId { get; set; }
            public int? FromApprovalLevelId { get; set; }
            public int? ToApprovalLevelId { get; set; }
            public int ApprovalStatusId { get; set; }
            public int RequestStaffId { get; set; }
            public int? ResponseStaffId { get; set; }
            public DateTime ArrivalDate { get; set; }
            public DateTime? ActualArrivalDate { get; set; }
            public DateTime? ResponseDate { get; set; }
            public string Comment { get; set; }
            public string FromApprovalLevelName { get; set; }
            public string ToApprovalLevelName { get; set; }
            public string ApprovalStatus { get; set; }
            public string ToStaffName { get; set; }
            public string FromStaffName { get; set; }
        }

        public class UserPrivilegeObj
        {
            public int ApprovalLevelId { get; set; }
            public string ApprovalLevelName { get; set; }
            public int ApprovalGroupId { get; set; }
            public string ApprovalGroupName { get; set; }
            public decimal? MaximumAmount { get; set; }
            public bool CanViewDocument { get; set; }
            public bool CanViewApproval { get; set; }
            public bool CanApprove { get; set; }
            public bool CanUpload { get; set; }
            public bool CanEdit { get; set; }
            public bool CanReceiveEmail { get; set; }
            public bool CanRecieveSMS { get; set; }
            public bool CanEscalate { get; set; }
        }

        public class ApprovalDetailsObj
        {
            public int ApprovalDetailId { get; set; }
            public int StatusId { get; set; }
            public int StaffId { get; set; }
            public string Comment { get; set; }
            public DateTime Date { get; set; }
            public DateTime? ArrivalDate { get; set; }
            public int TargetId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string SupplierName { get; set; }
            public string StatusName { get; set; }
            public string WorkflowToken { get; set; }
        }

        public class ApprovalDetailsRespObj
        {
            public IEnumerable<ApprovalDetailsObj> Details { get; set; }
            public IEnumerable<PreviousStaff> PreviousStaff { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class PreviousStaff
        {
            public int StaffId { get; set; }
            public string Name { get; set; }
        }

        public class ApprovalDetailSearchObj
        {
            public int TargetId { get; set; }
            public string WorkflowToken { get; set; }
        }
    }
}
