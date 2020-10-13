using Banking.AuthHandler.Interface;
using Banking.Data;
using Banking.DomainObjects.Auth;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Financial_Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Handlers.Auths
{
    public class OTPLoginCommandHandler : IRequestHandler<OTPLoginCommand, AuthResponse>
    {
        private readonly IIdentityService _service; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext; 
        public OTPLoginCommandHandler( 
            UserManager<ApplicationUser> userManager,  
            DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _service = identityService;
        }
        public async Task<AuthResponse> Handle(OTPLoginCommand request, CancellationToken cancellationToken)
        {
            var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

            try
            {  
                var user = await _userManager.FindByEmailAsync(request.Email);
                await _service.RemoveOtpAsync(request.OTP);
                var result = await _service.CustomerLoginAsync(user);
  
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
    }
}

