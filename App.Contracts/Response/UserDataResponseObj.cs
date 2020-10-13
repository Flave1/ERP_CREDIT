using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response
{
    public class UserDataResponseObj
    {
        public int CompanyId { get; set; }
        public int StaffId { get; set; }
        public int CustomerId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string StaffName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? BranchId { get; set; }
        public int? CountryId { get; set; }
        public decimal? ProfileStatus { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CustomUserRegistrationReqObj
    {
        public string UserName { get; set; }
        public int CustomerTypeId { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int QuestionId { get; set; }
        public string SecurityAnswered { get; set; }
    }
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class QuestionsRegRespObj
    {
        public int QuestionId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class QuestionsRespObj
    {
        public List<QuestionsObj> Questions { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class SingleQuestionsRespObj
    {
        public QuestionsObj Question { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class QuestionsObj
    {
        public int QuestionId { get; set; }
        public string Qiestion { get; set; }
    }

    public class ActivityRespObj
    {
        public List<ActivityObj> Activities { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class ActivityObj
    {
        public int ActivityId { get; set; }
        public int ActivityParentId { get; set; }
        public string ActivityName { get; set; }
        //.......................
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public string ActivityParentName { get; set; }
        public bool CanAdd { get; set; }
        public bool CanApprove { get; set; }
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        //........................ 
    }



    public class UserRoleObj
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }

    }


    public class UserRoleRespObj
    {
        public List<UserRoleObj> UserRoles { get; set; }
        public List<ActivityObj> UserRoleActivities { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
