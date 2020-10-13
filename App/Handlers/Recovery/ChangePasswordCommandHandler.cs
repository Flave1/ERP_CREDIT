using Banking.Contracts.Response.IdentityServer;
using Banking.Contracts.Response.Mail;
using Banking.DomainObjects.Auth;
using Banking.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.URI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace APIGateway.AuthGrid.Recovery
{
    public class ChangePasswordCommand : IRequest<RecoveryResp>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, RecoveryResp>
        {
            private async Task RecoveryMail(string email)
            { 

                var path = $"{_uRIs.MainClient}#/auth/login";
                var sm = new MailObj();
                sm.subject = $"Account Recovery";
                sm.content = $"Account recovery was successful. <br> click <a href='{path}'> here </a> to login into your account";
                sm.sendIt = true;
                sm.saveIt = true;
                sm.template = (int)EmailTemplate.LoginDetails;
                sm.toAddresses = new List<ToAddress>();
                sm.fromAddresses = new List<FromAddress>();
                sm.toAddresses.Add(new ToAddress { address = email, name = email });
                await _identityServer.SendMail(sm);
            }

            private readonly IBaseURIs _uRIs;
            private readonly IIdentityServerRequest _identityServer; 
            private readonly UserManager<ApplicationUser> _userManager;
            public ChangePasswordCommandHandler(IBaseURIs uRIs, IIdentityServerRequest identityServerRequest,
                  UserManager<ApplicationUser> userManager)
            {
                _uRIs = uRIs;
                _identityServer = identityServerRequest;
                _userManager = userManager; 
            }
            public async Task<RecoveryResp> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var response = new RecoveryResp { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);
                    if(user != null)
                    {
                        var passChanged = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
                        if (!passChanged.Succeeded)
                        {
                            response.Status.Message.FriendlyMessage = passChanged.Errors.FirstOrDefault().Description;
                            return response;
                        }
                    }
                    await RecoveryMail(request.Email);
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = "Password has successfully been changed";
                    return response;
                }
                catch (Exception ex)
                {
                    response.Status.Message.FriendlyMessage = "Unable to process request";
                    response.Status.Message.TechnicalMessage = ex.ToString();
                    return response;
                }
            }
        }
    }
   
}
