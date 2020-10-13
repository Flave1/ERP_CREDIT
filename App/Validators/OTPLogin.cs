using Banking.AuthHandler.Inplimentation;
using Banking.AuthHandler.Interface;
using Banking.DomainObjects.Auth;
using FluentValidation;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Validators
{
    public class OTPLoginCommandValidator : AbstractValidator<OTPLoginCommand>
    {
        private readonly IIdentityService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public OTPLoginCommandValidator(IIdentityService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;

            RuleFor(a => a.OTP).NotEmpty();
            RuleFor(a => a.Email).NotEmpty();
            RuleFor(a => a).MustAsync(OTPExistAsync).WithMessage("OTP Error Occurred!! <br/> UnIdentified OTP");
            RuleFor(a => a).MustAsync(IsValidOtpAsync).WithMessage("OTP Error Occurred!! <br/> OTP Has Expired");
            RuleFor(a => a).MustAsync(UserOwnCurrentOTPAsyync).WithMessage("OTP Error Occurred!! <br/> OTP not associated to this user account");
        }

        private async Task<bool> OTPExistAsync(OTPLoginCommand request, CancellationToken cancellation)
        {
            var otp = await _service.GetSingleOtpTrackAsync(request.OTP);
            if (otp == null)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> IsValidOtpAsync(OTPLoginCommand request, CancellationToken cancellation)
        {
            var isNotValidOTP = await _service.OTPDateExpiredAsync(request.OTP);
            if (isNotValidOTP)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }

        private async Task<bool> UserOwnCurrentOTPAsyync(OTPLoginCommand request, CancellationToken cancellation)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return await Task.Run(() => false);
            }

            if (user.Email.Trim().ToLower() != request.Email.Trim().ToLower())
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
