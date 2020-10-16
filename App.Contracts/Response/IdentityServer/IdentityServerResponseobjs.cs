using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.IdentityServer
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse); 
    public class CompanyStructure
    {
        public int companyStructureId { get; set; }
        public string name { get; set; }
        public string parentCompanyName { get; set; }
        public int structureLevel { get; set; }
        public int structureTypeId { get; set; }
        public int countryId { get; set; }
        public string address { get; set; }
        public int headStaffId { get; set; }
        public int? parentCompanyID { get; set; }
    }

    
    public class CompanyStructureRespObj
    {
        public List<CompanyStructure> companyStructures { get; set; }
        public APIResponseStatus Status { get; set; }

    }

    public class RecoveryResp
    {
        public APIResponseStatus Status { get; set; }
    }

    public class StaffObj
    {
        public int staffId { get; set; }
        public string staffCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public int jobTitle { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string gender { get; set; }
    }

    public class StaffRespObj
    {
        public List<StaffObj> staff { get; set; }
        public APIResponseStatus Status { get; set; }

    }

    public class OperationObj
    {
        public int OperationId { get; set; }
        public string OperationName { get; set; }

        public int OperationTypeId { get; set; }
        public string OperationTypeName { get; set; }

        public bool EnableWorkflow { get; set; }

        public bool? Active { get; set; }
    }

    public class OperationRespObj
    {
        public List<OperationObj> Operations { get; set; }
        public APIResponseStatus Status { get; set; }

    }
    public class SessionCheckerRespObj
    {
        public int StatusCode { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LogingFailedRespObj
    {
        public bool IsSecurityQuestion { get; set; }
        public DateTime UnLockAt { get; set; }
        public APIResponseStatus Status { get; set; }

    }

    public class SecurityResp
    {
        public List<Security> authSettups { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class Security
    { 
        public int ScrewIdentifierGridId { get; set; }
        public bool ShouldAthenticate { get; set; }
        public int Media { get; set; }
        public int Module { get; set; }
        public bool ActiveOnMobileApp { get; set; }
        public bool ActiveOnWebApp { get; set; }
        public bool UseActiveDirectory { get; set; }
        public string ActiveDirectory { get; set; }
        public bool EnableLoginFailedLockout { get; set; }
        public int NumberOfFailedLoginBeforeLockout { get; set; }
        public int NumberOfFailedAttemptsBeforeSecurityQuestions { get; set; }
        //public TimeSpan RetryTimeInMinutes { get; set; }
        public bool EnableRetryOnMobileApp { get; set; }
        public bool EnableRetryOnWebApp { get; set; }
        public bool ShouldRetryAfterLockoutEnabled { get; set; }
        //public TimeSpan InActiveSessionTimeout { get; set; }
        public int PasswordUpdateCycle { get; set; }
        public bool SecuritySettingActiveOnMobileApp { get; set; }
        public bool SecuritySettingsActiveOnWebApp { get; set; }
        public bool EnableLoadBalance { get; set; }
        public int LoadBalanceInHours { get; set; }
    }

    
 
}
