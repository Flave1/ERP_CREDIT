using Banking.Contracts.Response;
using Banking.Contracts.Response.IdentityServer;
using Banking.DomainObjects.Auth;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Banking.AuthHandler.Interface
{
    public interface IIdentityService
    {

        Task<bool> OTPDateExpiredAsync(string otp);
        Task<OTPTracker> GetSingleOtpTrackAsync(string otp);
        Task<AuthenticationResult> CustomerRegisterAsync(CustomUserRegistrationReqObj userRegistration);
        Task<AuthenticationResult> CustomerChangePasswsord(ChangePassword pass);
        Task<AuthenticationResult> CustomerLoginAsync(ApplicationUser user);
        Task<AuthenticationResult> CustomerRefreshTokenAsync(string refreshToken, string token);
        Task<bool> CustomerCheckUserAsync(string email);
        Task<bool> CustomerCheckUserAsyncByUserId(string userId);
        Task<UserDataResponseObj> CustomerFetchLoggedInUserDetailsAsync(string userId);
        Task<ConfirnmationResponse> CustomerConfirmEmailAsync(ConfirnmationRequest request);
        Task<ConfirnmationResponse> CustomerVerifyAsync(string email, string code);
        Task<bool> verifyCustomerEmailAccount(string userId);
        Task<bool> SendOTPToEmailAsync(ApplicationUser user);
        Task<bool> RemoveOtpAsync(string otp);
        Task<bool> PerformLockFunction(string user, DateTime unlockat, bool isQuestionTime);
        Task<bool> UnlockUserAsync(string userid);
        Task<bool> ReturnStatusAsync(string userid);


    }
}
