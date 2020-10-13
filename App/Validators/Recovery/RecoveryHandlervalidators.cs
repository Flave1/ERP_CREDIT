using APIGateway.AuthGrid.Recovery;
using Banking.DomainObjects.Auth;
using FluentValidation; 
using Microsoft.AspNetCore.Identity; 
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Validations.Recovery
{
    public class RecoverAccountByEmailCommandvalidator : AbstractValidator<RecoverAccountByEmailCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RecoverAccountByEmailCommandvalidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            RuleFor(r => r.Email).NotEmpty().WithMessage("Email is required for password recovery")
                .MustAsync(MustExistAsync).WithMessage("Unable to identify email");
        }
        private async Task<bool> MustExistAsync(string Email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return false;
            return true;
        }
    }

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ChangePasswordCommandValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            RuleFor(r => r.Email).NotEmpty().WithMessage("Email is required for password recovery");
            RuleFor(r => r.NewPassword).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).MaximumLength(16).MustAsync(IsPasswordCharactersValid).WithMessage("Invalid Password");
            RuleFor(e => e.Token).NotEmpty();
            RuleFor(r => r).MustAsync(MustNotBeSamePasswordAsync).WithMessage("New password must be differrent from old password");
        }
        private async Task<bool> MustNotBeSamePasswordAsync(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);
                if (checkPassword)
                    return false;
            }
            return true;
        }
        private async Task<bool> IsPasswordCharactersValid(string password, CancellationToken cancellationToken)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return await Task.Run(() => hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password));
        }
    }
}
