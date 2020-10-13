using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.IdentityServer;
using Banking.Data;
using Banking.DomainObjects.Auth;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_Financial_Identity;
using GOSLibraries.Options;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Banking.Handlers.Auths
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ERPAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var response = new MiddlewareResponse { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            string userId = context.HttpContext.User?.FindFirst("userId")?.Value ?? string.Empty;
            StringValues authHeader = context.HttpContext.Request.Headers["Authorization"];

            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
            if (context == null || hasAllowAnonymous)
            {
                await next();
                return;
            }
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(authHeader))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnauthorizedObjectResult(response);
                return;
            }
            string token = authHeader.ToString().Replace("Bearer ", "").Trim();
            var handler = new JwtSecurityTokenHandler();
            var tokena = handler.ReadJwtToken(token);
            var FromDate = tokena.IssuedAt.AddHours(1);
            var EndDate = tokena.ValidTo.AddHours(1);

            var expieryMatch = DateTime.UtcNow.AddHours(1);
            if (expieryMatch > EndDate)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnauthorizedObjectResult(response);
                return;
            } 
            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                try
                {
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    IIdentityServerRequest _measureService = scopedServices.GetRequiredService<IIdentityServerRequest>();
                    //if (_measureService.CheckForSessionTrailAsync(userId, (int)Modules.CREDIT).Result.StatusCode == 401)
                    //{
                    //    context.HttpContext.Response.StatusCode = 401;
                    //    context.Result = new UnauthorizedObjectResult(response);
                    //    return;
                    //}
                    await next();
                    return;
                }
                catch (Exception ex)
                {
                    context.HttpContext.Response.StatusCode = 500;
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = ex.Message;
                    response.Status.Message.TechnicalMessage = ex.ToString();
                    context.Result = new InternalServerErrorObjectResult(response);
                    return;
                }
            }
        }
    }


    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class ERPLoginAttribute : Attribute, IAsyncActionFilter
    //{
    //    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        var response = new MiddlewareResponse { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
          
    //        using (var scope = context.HttpContext.RequestServices.CreateScope())
    //        {
    //            try
    //            {
                    
    //                IServiceProvider scopedServices = scope.ServiceProvider;
    //                IIdentityServerRequest serverRequest = scopedServices.GetRequiredService<IIdentityServerRequest>();
    //                IDetectionService detection = scopedServices.GetRequiredService<IDetectionService>();
    //                IIdentityService _service = scopedServices.GetRequiredService<IIdentityService>();
    //                UserManager<ApplicationUser> UseManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
    //                DataContext Data = scopedServices.GetRequiredService<DataContext>();
    //                var resexecutedContextult = await next();
    //                bool ResponseStatus = true;  
    //                var userid = detection.UserAgent.ToLower().ToLower();
    //                if (string.IsNullOrEmpty(userid))
    //                {
    //                    context.HttpContext.Response.StatusCode = 401;
    //                    response.Status.Message.FriendlyMessage = "Un-Authorized Access -- User Agent not found";
    //                    context.Result = new UnauthorizedObjectResult(response);
    //                    return;
    //                }
    //                if (resexecutedContextult.Result is BadRequestObjectResult badRequestObjectResult)
    //                {
    //                    ResponseStatus = false;
    //                }
    //                var lockedAccount = Data.AccountsLocked.FirstOrDefault(r => r.Userid == userid) ?? null;
    //                if (lockedAccount != null)
    //                {
    //                    if (lockedAccount.UnlockAt > DateTime.UtcNow)
    //                    {
    //                        ResponseStatus = false;
    //                    }
    //                }
    //                var res = await serverRequest.CheckForFailedTrailsAsync(ResponseStatus, (int)Modules.CREDIT, detection.UserAgent.ToString());
                    
    //                if (!res.Status.IsSuccessful)
    //                {
    //                    lockedAccount = new AccountsLocked();
    //                    lockedAccount.Userid = userid;
    //                    lockedAccount.UnlockAt = res.UnLockAt;
    //                    await _service.PerformLockFunction(lockedAccount);
    //                    context.HttpContext.Response.StatusCode = 400;
    //                    response.Status.Message.FriendlyMessage = res.Status.Message.FriendlyMessage;

    //                    var objectresut = JsonConvert.SerializeObject(res);
    //                    var contentResult = new ContentResult
    //                    {
    //                        Content = objectresut,
    //                        ContentType = "application/json",
    //                        StatusCode = 400
    //                    }; 
    //                    context.Result = new BadRequestObjectResult(contentResult);
    //                    return;

    //                    //context.Result = new BadRequestObjectResult("Invalid");
    //                  //  return;
    //                } 
    //                return;
    //            }
    //            catch (Exception ex)
    //            {
    //                context.HttpContext.Response.StatusCode = 500;
    //                response.Status.IsSuccessful = false;
    //                response.Status.Message.FriendlyMessage = ex.Message;
    //                response.Status.Message.TechnicalMessage = ex.ToString();
    //                context.Result = new InternalServerErrorObjectResult(response);
    //                return;
    //            }
    //        }
    //    }
         
    //}
     
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentityServerRequest _service; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDetectionService _detectionService;
        private readonly DataContext _securityContext;
        private readonly ILoggerService _logger;
        private readonly IIdentityService _identityService;
        public LoginCommandHandler(
            IIdentityServerRequest identityRepoService,
            UserManager<ApplicationUser> userManager, 
            DataContext dataContext,
            IIdentityService identityService,
            IDetectionService detectionService,
            ILoggerService loggerService)
        {
            _userManager = userManager; 
            _service = identityRepoService; 
            _securityContext = dataContext;
            _logger = loggerService;
            _detectionService = detectionService;
            _identityService = identityService;
        }

        public async Task<AuthResponse> OTPOptionsAsync(string userid)
        {
            try
            { 
                var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var settings = await _service.GetSettingsAsync()??new SecurityResp { authSettups = new List<Security>()};
                if(settings.authSettups.Count() > 0)
                {
                    var multiplefFA = settings.authSettups.Where(a => a.Module == (int)Modules.CREDIT).ToList();
                    var user = await _userManager.FindByIdAsync(userid);
                    if (multiplefFA.Count() > 0)
                    {
                        if (_detectionService.Device.Type.ToString().ToLower() == Device.Desktop.ToString().ToLower())
                        {
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL).ActiveOnWebApp)
                            {
                                await _identityService.SendOTPToEmailAsync(user);
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your email";
                                return response;
                            }
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS) != null && multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS).ActiveOnWebApp)
                            {
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your number";
                                return response;
                            }
                        }
                        if (_detectionService.Device.Type.ToString().ToLower() == Device.Mobile.ToString().ToLower())
                        {
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL).ActiveOnMobileApp)
                            {
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your email";
                                return response;
                            }
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS).ActiveOnMobileApp)
                            {
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your number";
                                return response;
                            }
                        }
                    }
                }
                
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                if (!await _identityService.ReturnStatusAsync(request.UserName))
                {
                    response.Status.IsSuccessful = false;
                    return response;
                }
                if (!await IsPasswordCharactersValid(request.Password))
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "Invalid Password";
                    return response;
                }
                if (!await UserExist(request))
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "User does not exist";
                    return response;
                }
                if (!await IsValidPassword(request))
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "User/Password Combination is wrong";
                    return response;
                }

                var user = await _userManager.FindByNameAsync(request.UserName);


                var otp = await OTPOptionsAsync(user.Id);
                if (otp.Status.IsSuccessful)
                {
                    otp.Status.Message.MessageId = user.Email;
                    return otp;
                }

                var result = await _identityService.CustomerLoginAsync(user);
              
                response.Token = result.Token;
                response.RefreshToken = result.RefreshToken;
                return response;
            }
            catch (Exception ex)
            {
                response.Status.Message.FriendlyMessage = ex?.Message ?? ex?.InnerException?.Message;
                response.Status.Message.TechnicalMessage = ex.ToString();
                return response;
            }
        }

        private async Task<bool> IsValidPassword(LoginCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isValidPass = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPass)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> UserExist(LoginCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> IsPasswordCharactersValid(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return await Task.Run(() => hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password));
        }

    }

}
