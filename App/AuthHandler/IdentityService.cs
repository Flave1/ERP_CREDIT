using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Polly.Retry;
using Polly;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.Options;
using GOSLibraries.GOS_API_Response;
using GOSLibraries;
using GOSLibraries.GOS_Financial_Identity;
using Banking.AuthHandler.Interface;
using Banking.Data;
using Banking.Contracts.V1;
using Banking.Contracts.Response;
using System.Security.Claims;
using Banking.DomainObjects.Auth;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Banking.Contracts.GeneralExtension;
using System.Collections.Generic;
using Banking.Contracts.Response.Mail;
using Banking.Requests;
using Banking.DomainObjects.Credit;
using Banking.Helpers;
using Microsoft.Extensions.Configuration;
using Banking.Contracts.Response.IdentityServer;
using MimeKit;
using GOSLibraries.GOS_MAIL_BOX;

namespace Banking.AuthHandler.Inplimentation
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _logger; 
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int maxRetryTimes = 4;
        private readonly IHttpClientFactory _httpClientFactory;
        private AuthenticationResult _authResponse = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerRequest _identityServer;        
        public IdentityService(
            IHttpContextAccessor httpContextAccessor, IIdentityServerRequest identityServer, IConfiguration configuration,
        JwtSettings jwtSettings, 
            TokenValidationParameters tokenValidationParameters,
            DataContext dataContext, 
            RoleManager<IdentityRole> roleManager, 
            ILoggerService loggerService, 
            IHttpClientFactory httpClientFactory,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _httpClientFactory = httpClientFactory;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
            _roleManager = roleManager;
            _logger = loggerService;
            _identityServer = identityServer;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _retryPolicy = Policy.Handle<HttpRequestException>()

                .WaitAndRetryAsync(maxRetryTimes, times =>

                TimeSpan.FromSeconds(times * 2));
        }

        public async Task<bool> OTPDateExpiredAsync(string otp)
        {
            var otpitem = await _dataContext.OTPTracker.FirstOrDefaultAsync(q => q.OTP.Trim().ToLower() == otp.Trim().ToLower());
            if (otpitem != null)
            {
                if (DateTime.Now > otpitem.ExpiryDate)
                {
                    await this.RemoveOtpAsync(otpitem.OTP);
                    return await Task.Run(() => true);
                }
            }
            return await Task.Run(() => false);
        }

        public async Task<bool> UnlockUserAsync(string userid)
        {
            var lockedAccount = await _userManager.FindByNameAsync(userid) ?? null;
            if (lockedAccount != null)
            {
                lockedAccount.IsItQuestionTime = false;
                lockedAccount.EnableAtThisTime = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
                await _userManager.UpdateAsync(lockedAccount);
            }
            return true;
        }

        public async Task<bool> PerformLockFunction(string user, DateTime unlockat, bool isQuestionTime)
        {
            var useraccount = await _userManager.FindByNameAsync(user) ?? null;
            if (useraccount == null)
            {
                return true;
            }
            useraccount.IsItQuestionTime = isQuestionTime ? true : false;
            useraccount.EnableAtThisTime = unlockat;
            await _userManager.UpdateAsync(useraccount);
            return true;
        }
      
        public async Task<OTPTracker> GetSingleOtpTrackAsync(string otp)
        {
            return await _dataContext.OTPTracker.FirstOrDefaultAsync(q => q.OTP.Trim().ToLower() == otp.Trim().ToLower()) ?? null;
        }

        public async Task<AuthenticationResult> CustomerLoginAsync(ApplicationUser user)
        {
            try
            {
                return await CustomerGenerateAuthenticationResultForUserAsync(user);

            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AuthenticationResult
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }

        public async Task<AuthenticationResult> CustomerRefreshTokenAsync(string refreshToken, string token)
        {
            try
            {
                var validatedToken = GetClaimsPrincipalFromToken(token);
                if (validatedToken == null)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Invalid Token"
                            }
                        }
                    };
                }


                var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                     .AddDays(expiryDateUnix);

                if (expiryDateTimeUtc > DateTime.UtcNow)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This Token Hasn't Expired Yet"
                            }
                        }
                    };
                }


                var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;

                var storedRefreshToken = _dataContext.RefreshToken.SingleOrDefault(x => x.Token == refreshToken);

                if (storedRefreshToken == null)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This Refresh Token does not Exist"
                            }
                        }
                    };
                }

                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This Refresh Token has Expired"
                            }
                        }
                    };
                }

                if (storedRefreshToken.Invalidated)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This Refresh Token has been Invalidated"
                            }
                        }
                    };
                }

                if (storedRefreshToken.Used)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This Refresh Token has been Used"
                            }
                        }
                    };
                }

                if (storedRefreshToken.JwtId != jti)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This Refresh Token Does not match this JWT"
                            }
                        }
                    };
                }

                storedRefreshToken.Used = true;
                _dataContext.Update(storedRefreshToken);
                await _dataContext.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(validatedToken.Claims.SingleOrDefault(x => x.Type == "id").Value);

                return await CustomerGenerateAuthenticationResultForUserAsync(user);
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID :  {errorCode} Ex: {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AuthenticationResult
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }


        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;
                else
                    return principal;
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID :  {errorCode} <br>Ex: {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken &&
                            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                            StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResult> CustomerRegisterAsync(CustomUserRegistrationReqObj userRegistration)
        {
            try
            {
                var accountNumber = GeneralHelpers.GenerateRandomDigitCode(10);
                var existingUser = await _userManager.FindByEmailAsync(userRegistration.Email);

                if (existingUser != null)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "User with this email address already exist"
                            }
                        }
                    };
                }


                var user = new ApplicationUser
                {
                    Email = userRegistration.Email,
                    UserName = userRegistration.Email,
                    PhoneNumber = userRegistration.PhoneNo,
                    FirstName = userRegistration.FirstName,
                    LastName = userRegistration.LastName,
                    Address = userRegistration.Address,
                    CustomerTypeId = userRegistration.CustomerTypeId,
                    ApprovalStatusId = 1,
                    SecurityAnswered = userRegistration.SecurityAnswered,
                    QuestionId = userRegistration.QuestionId
                };

                var createdUser = await _userManager.CreateAsync(user, userRegistration.Password);
                var customer = new credit_loancustomer
                {
                    Email = userRegistration.Email,
                    UserIdentity = user.Id,
                    PhoneNo = userRegistration.PhoneNo,
                    FirstName = userRegistration.FirstName,
                    LastName = userRegistration.LastName,
                    Address = userRegistration.Address,
                    CustomerTypeId = userRegistration.CustomerTypeId,
                    CASAAccountNumber = accountNumber,
                    ApprovalStatusId = 1,
                    ProfileStatus = 0,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.Email,
                    CreatedOn = DateTime.Now,
                    RegistrationSource = "Website"
                };
                _dataContext.credit_loancustomer.Add(customer);
                _dataContext.SaveChanges();

                if (!createdUser.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = createdUser.Errors.Select(x => x.Description).FirstOrDefault(),
                            }
                        }
                    };
                }

                var custCode = ConfirmationCode.Generate();
                var successful = await CustomerSendAndStoreConfirmationCode(custCode, user.Email);

                var accountId = user.Id;
                var name = user.FirstName;
                var baseUrl = _configuration.GetValue<string>("FrontEndUrl:webUrl");

                var url = baseUrl + "/#/auth/activate-account/" + accountId + "";
                var content1 = "Welcome to GOS Credit! There's just one step before you get to complete your customer account registration. Verify you have the right email address by clicking on the button below.";
                var content2 = "Once your account creation is completed, your can explore our services and have a seamless experience.";
                var body = GeneralHelpers.MailBody(name, url, content1, content2);

                var addresses = new ToAddress
                {
                    name = user.FirstName + " " + user.LastName,
                    address = user.Email
                };
                var addressList = new List<ToAddress> { addresses };
                var mailObj = new MailObj
                {
                    subject = "Email Verification",
                    content = body,
                    toAddresses = addressList,
                    fromAddresses = new List<FromAddress> { },
                    sendIt = true,
                    saveIt = false
                };

                var res = _identityServer.SendMail(mailObj);

                return new AuthenticationResult
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        CustomToken = custCode,
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AuthenticationResult
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }

        public async Task<bool> verifyCustomerEmailAccount(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            var customer = _dataContext.credit_loancustomer.Where(x => x.UserIdentity == userId).FirstOrDefault();

            if (existingUser == null || customer == null)
            {
                return false;
            }          
            
            customer.ApprovalStatusId = 2;
            existingUser.ApprovalStatusId = 2;
            return _dataContext.SaveChanges() > 0;
        }

        private async Task<AuthenticationResult> CustomerGenerateAuthenticationResultForUserAsync(ApplicationUser user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("userId", user.Id),
                    new Claim("securedERPuser", "clientCustomer")
                };

                var userClaims = await _userManager.GetClaimsAsync(user);

                claims.AddRange(userClaims);

                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole) ?? null);

                    var role = await _roleManager.FindByNameAsync(userRole);

                    if (role == null)
                    {
                        continue;
                    }
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    foreach (var roleClaim in roleClaims)
                    {
                        if (claims.Contains(roleClaim)) continue;
                        claims.Add(roleClaim);
                    }
                }

                var tokenDecriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeSpan),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };


                var token = tokenHandler.CreateToken(tokenDecriptor);

                var refreshToken = new RefreshToken
                {
                    JwtId = token.Id,
                    UserId = user.Id,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddSeconds(6),
                };

                try
                {
                    await _dataContext.RefreshToken.AddAsync(refreshToken);
                    await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = ex.InnerException.Message,
                            }
                        }
                    };
                }

                return new AuthenticationResult
                {
                    Token = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken.Token,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AuthenticationResult
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID :{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }

        public async Task<AuthenticationResult> CustomerChangePasswsord(ChangePassword pass)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(pass.Email);

                var userPassword = await _userManager.CheckPasswordAsync(user, pass.OldPassword);

                if (!userPassword)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This password is not associated to this account",
                            }
                        }
                    };
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var changepassword = await _userManager.ResetPasswordAsync(user, token, pass.NewPassword);

                if (!changepassword.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = changepassword.Errors.Select(x => x.Description).FirstOrDefault(),
                            }
                        }
                    };
                }

                return await CustomerGenerateAuthenticationResultForUserAsync(user);
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AuthenticationResult
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }

        public async Task<bool> CustomerCheckUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) return true;
            return false;
        }

        public async Task<bool> CustomerCheckUserAsyncByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null) return true;
            return false;
        }
        public async Task<UserDataResponseObj> CustomerFetchLoggedInUserDetailsAsync(string userId)
        {
            try
            {
                UserDataResponseObj profile = null;
                var currentUser = await _userManager.FindByIdAsync(userId);

                //var supplierData = await _supRepo.GetSupplierByEmailAsync(currentUser.Email); 

                profile = new UserDataResponseObj
                {
                    Email = currentUser.Email,
                    UserId = currentUser.Id,
                    UserName = currentUser.UserName,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    PhoneNumber = currentUser.PhoneNumber,
                    Status = new APIResponseStatus { IsSuccessful = true }
                };


                if (profile == null)
                {
                    return new UserDataResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Unable to fetch user details"
                            }
                        }
                    };
                }
                return profile;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new UserDataResponseObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }

        public async Task<ConfirnmationResponse> CustomerConfirmEmailAsync(ConfirnmationRequest request)
        {
            try
            {
                var confirmCode = ConfirmationCode.Generate();
                var sent = await CustomerSendAndStoreConfirmationCode(confirmCode, request.Email);
                if (!sent)
                    return new ConfirnmationResponse
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Unable to send mail!! please contact systems administrator"
                            }
                        }
                    };

                return new ConfirnmationResponse
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Please Check your email for email for confirnmation"
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ConfirnmationResponse
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }

        public async Task<ConfirnmationResponse> CustomerVerifyAsync(string email, string code)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ConfirnmationResponse
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "User not Identified"
                            }
                        }
                    };
                }

                var verificationCodeFrmRepo = await _dataContext.ConfirmEmailCode
                    .FirstOrDefaultAsync(x => x.ConfirnamationTokenCode.Trim().ToLower() == code.Trim().ToLower()
                && user.Id.ToLower() == x.UserId);

                if (verificationCodeFrmRepo == null)
                {
                    return new ConfirnmationResponse
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Verification Code Unidentified"
                            }
                        }
                    };
                }
                if (verificationCodeFrmRepo.ExpiryDate < DateTime.Now)
                {
                    return new ConfirnmationResponse
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Verification Code has Expired"
                            }
                        }
                    };
                }
                return new ConfirnmationResponse
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ConfirnmationResponse
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please try again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID :  {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }

        }

        public async Task<bool> CustomerSendAndStoreConfirmationCode(string code, string email)
        {
            try
            {
                var thisUser = await _userManager.FindByEmailAsync(email);
                //await _emailService.Send(new EmailMessage
                //{
                //    FromAddresses = new List<EmailAddress>
                //        {
                //            new EmailAddress{ Address = "favouremmanuel433@gmail.com", Name = "Flave Techs"}
                //        },
                //    ToAddresses = new List<EmailAddress>
                //        {
                //            new EmailAddress{ Address = email, Name = thisUser.UserName}
                //        },
                //    Subject = "Account Confirmation",
                //    Content = $"Dear {thisUser.UserName}, <br> Copy and paste this code {code} on the confirmation field to change your password",
                //});

                var userConfirmationCode = new ConfirmEmailCode
                {
                    ConfirnamationTokenCode = code,
                    ExpiryDate = DateTime.Now.AddHours(1),
                    IssuedDate = DateTime.Now,
                    UserId = thisUser.Id
                };
                await _dataContext.ConfirmEmailCode.AddAsync(userConfirmationCode);
                var saved = await _dataContext.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {
                var errorId = ErrorID.Generate(4);
                _logger.Error($"{errorId}   Error Message{ ex?.Message ?? ex?.InnerException?.Message}");
                return false;
            }


        }


        private async Task<OTPTracker> ProduceOtpAsync(ApplicationUser user)
        {
            Random rnd = new Random();
            var otp = (rnd.Next(100000, 999999)).ToString();

            var newOtp = new OTPTracker
            {
                DateIssued = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMinutes(2),
                OTP = otp,
                UserId = user.Id,
                Email = user.Email,
            };
            _logger.Information("I am in ProduceOtpAsync "); 
            await _dataContext.OTPTracker.AddAsync(newOtp);
            await _dataContext.SaveChangesAsync();
            return newOtp;
        }

        public async Task<bool> SendOTPToEmailAsync(ApplicationUser user)
        { 
            var newOtp = await ProduceOtpAsync(user);
            await SendOTPEmailAsync(newOtp);
            return await _dataContext.SaveChangesAsync() > 0;
        }
       

        public async Task<bool> RemoveOtpAsync(string otp)
        {
            var otpitem = await _dataContext.OTPTracker.FirstOrDefaultAsync(q => q.OTP.Trim().ToLower() == otp.Trim().ToLower());
            if (otpitem != null)
            {
                _dataContext.OTPTracker.Remove(otpitem);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return await Task.Run(() => false);
        }

        private async Task<bool> SendOTPEmailAsync(OTPTracker person)
        {
            try
            {   
                var builder = new BodyBuilder();
               
                var emailMessage = new MailObj();
                emailMessage.toAddresses = new List<ToAddress>();
                emailMessage.subject = "OTP Verification";
                emailMessage.content = person.OTP.Insert(2, "-").Insert(5, "-");
                emailMessage.sendIt = true;
                emailMessage.fromAddresses = new List<FromAddress>();
                emailMessage.toAddresses.Add(new ToAddress { address = person.Email });
                emailMessage.template = 5;
                await _identityServer.SendMail(emailMessage);
                return await Task.Run(() => true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return true;
            }

        }
        public async Task<bool> ReturnStatusAsync(string userid)
        {

            var lockedAccount = await _userManager.FindByNameAsync(userid) ?? null;
            if (lockedAccount != null)
            {
                if (lockedAccount.IsItQuestionTime)
                    return false;
                if (lockedAccount.EnableAtThisTime > DateTime.UtcNow)
                    return false;
            }
            return await Task.Run(() => true);
        }

    }
}
