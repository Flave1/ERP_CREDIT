using Banking.Contracts.Response;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Common;
using Banking.Contracts.Response.Finance;
using Banking.Contracts.Response.FlutterWave;
using Banking.Contracts.Response.IdentityServer;
using Banking.Contracts.Response.Mail;
using Banking.Contracts.V1;
using Banking.DomainObjects.Auth;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using QuestionsRespObj = Banking.Contracts.Response.QuestionsRespObj;

namespace Banking.Requests
{
    public class IdentityServerRequest : IIdentityServerRequest
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private static HttpClient Client;
        private HttpResponseMessage result = new HttpResponseMessage();
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int maxRetryTimes = 10;
        private readonly ILoggerService _logger;
        private GOSLibraries.GOS_Financial_Identity.AuthenticationResult _authResponse = null;
        public IdentityServerRequest(
            IHttpContextAccessor httpContextAccessor, 
            IHttpClientFactory httpClientFactory, IConfiguration configuration,
            ILoggerService loggerService)
        {
            _logger = loggerService;
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy.Handle<HttpRequestException>()

             .WaitAndRetryAsync(maxRetryTimes, times =>

             TimeSpan.FromSeconds(times * 2));
        }

        public async Task<AuthenticationResult> LoginAsync(string userName, string password)
        {
            try
            {

                var loginRquest = new UserLoginReqObj
                {
                    UserName = userName,
                    Password = password,
                };
                //var gosGatewayClient = new HttpClient();
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");

                var jsonContent = JsonConvert.SerializeObject(loginRquest);
                var buffer = Encoding.UTF8.GetBytes(jsonContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //var result = await gosGatewayClient.PostAsync("http://localhost:59798"+ApiRoutes.Identity.LOGIN, byteContent);
                var result = await gosGatewayClient.PostAsync(ApiRoutes.Identity.LOGIN, byteContent);

                var accountInfo = await result.Content.ReadAsStringAsync();
                _authResponse = JsonConvert.DeserializeObject<GOSLibraries.GOS_Financial_Identity.AuthenticationResult>(accountInfo);
                if (_authResponse == null)
                {

                    return new GOSLibraries.GOS_Financial_Identity.AuthenticationResult
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (_authResponse.Token != null)
                {

                    return new GOSLibraries.GOS_Financial_Identity.AuthenticationResult
                    {
                        Token = _authResponse.Token,
                        RefreshToken = _authResponse.RefreshToken
                    };
                }

                return new GOSLibraries.GOS_Financial_Identity.AuthenticationResult
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = _authResponse.Status.IsSuccessful,
                        Message = new APIResponseMessage
                        {
                            TechnicalMessage = _authResponse.Status?.Message?.TechnicalMessage,
                            FriendlyMessage = _authResponse.Status?.Message?.FriendlyMessage
                        }
                    }
                };



            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new GOSLibraries.GOS_Financial_Identity.AuthenticationResult
                {

                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<UserDataResponseObj> UserDataAsync()
        {
            try
            {
                var currentUserId = _accessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                string authorization = _accessor.HttpContext.Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authorization))
                {
                    return new UserDataResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Error Occurred ! Please Contact Systems Administrator"
                            }
                        }
                    };
                }
                gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
                var result = await gosGatewayClient.GetAsync(ApiRoutes.Identity.FETCH_USERDETAILS);
                if (!result.IsSuccessStatusCode)
                {

                }
                var accountInfo = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<UserDataResponseObj>(accountInfo);

            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new UserDataResponseObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<HttpResponseMessage> GotForApprovalAsync(GoForApprovalRequest request)
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);


            var jsonContent = JsonConvert.SerializeObject(request);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var result = await gosGatewayClient.PostAsync(ApiRoutes.IdentitySeverWorkflow.GO_FOR_APPROVAL, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        new ApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }
        public async Task<HttpResponseMessage> GetAnApproverItemsFromIdentityServer()
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.IdentitySeverWorkflow.GET_ALL_STAFF_AWAITING_APPROVALS);
                    if (!result.IsSuccessStatusCode)
                    {
                        new ApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }
        public async Task<HttpResponseMessage> StaffApprovalRequestAsync(IndentityServerApprovalCommand request)
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var jsonContent = JsonConvert.SerializeObject(request);

                    var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    result = await gosGatewayClient.PostAsync(ApiRoutes.IdentitySeverWorkflow.STAFF_APPROVAL_REQUEST, data);

                    if (!result.IsSuccessStatusCode)
                    {
                        new ApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }
        public async Task<HttpResponseMessage> GetCanEditStatusFromIdentityServer()
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.Workflow.STAFF_CAN_EDIT);
                    if (!result.IsSuccessStatusCode)
                    {
                        new ApprovalRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    return result;
                }
                catch (Exception ex) { throw ex; }
            });
        }
        public async Task<CommonRespObj> GetCurrencyAsync()
        {
            try
            {
                CommonRespObj responseObj = new CommonRespObj();
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        result = await gosGatewayClient.GetAsync(ApiRoutes.Currency.GET_ALL_CURRENCY);
                        var data = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<CommonRespObj>(data);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
                if (responseObj == null)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new CommonRespObj
                {
                    commonLookups = responseObj.commonLookups,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new CommonRespObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<CommonRespObj> GetGenderAsync()
        {
            try
            {
                CommonRespObj responseObj = new CommonRespObj();
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        result = await gosGatewayClient.GetAsync(ApiRoutes.Currency.GET_ALL_GENDER);
                        var data = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<CommonRespObj>(data);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
                if (responseObj == null)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new CommonRespObj
                {
                    commonLookups = responseObj.commonLookups,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new CommonRespObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<CommonRespObj> GetMaritalStatusAsync()
        {
            try
            {
                CommonRespObj responseObj = new CommonRespObj();
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        result = await gosGatewayClient.GetAsync(ApiRoutes.Currency.GET_ALL_MARITAL_STATUS);
                        var data = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<CommonRespObj>(data);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
                if (responseObj == null)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new CommonRespObj
                {
                    commonLookups = responseObj.commonLookups,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new CommonRespObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<CommonRespObj> GetEmploymentTypeAsync()
        {
            try
            {
                CommonRespObj responseObj = new CommonRespObj();
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        result = await gosGatewayClient.GetAsync(ApiRoutes.Currency.GET_ALL_EMPLOYMENT_TYPE);
                        var data = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<CommonRespObj>(data);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
                if (responseObj == null)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new CommonRespObj
                {
                    commonLookups = responseObj.commonLookups,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new CommonRespObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<CommonRespObj> GetIdentiticationTypeAsync()
        {
            try
            {
                CommonRespObj responseObj = new CommonRespObj();
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        result = await gosGatewayClient.GetAsync(ApiRoutes.Currency.GET_ALL_IDENTITICATION_TYPE);
                        var data = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<CommonRespObj>(data);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
                if (responseObj == null)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new CommonRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new CommonRespObj
                {
                    commonLookups = responseObj.commonLookups,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new CommonRespObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<StaffRespObj> GetAllStaffAsync()
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                    string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
                    //gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
                    StaffRespObj responseObj = new StaffRespObj();

                    try
                    {
                        result = await gosGatewayClient.GetAsync(ApiRoutes.IdentitySeverWorkflow.GET_ALL_STAFF);
                        if (!result.IsSuccessStatusCode)
                        {
                            new StaffRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                }
                            };
                        }
                        var data = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<StaffRespObj>(data);
                    }
                    catch (Exception ex) { throw ex; }
                    if (responseObj == null)
                    {
                        return new StaffRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                            }
                        };
                    }
                    if (!responseObj.Status.IsSuccessful)
                    {
                        return new StaffRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = responseObj.Status.IsSuccessful,
                                Message = responseObj.Status.Message
                            }
                        };
                    }
                    return new StaffRespObj
                    {
                        staff = responseObj.staff,
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
            });
        }
        public async Task<CompanyStructureRespObj> GetAllCompanyAsync()
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"]; 
            CompanyStructureRespObj responseObj = new CompanyStructureRespObj();
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.CompanyStructure.GET_ALL_COMPANY_STRUCTURE);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<CompanyStructureRespObj>(data1);
                        new CompanyStructureRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<CompanyStructureRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new CompanyStructureRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new CompanyStructureRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new CompanyStructureRespObj
                {
                    companyStructures = responseObj.companyStructures,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            });          
        }
        public async Task<OperationRespObj> GetAllOperationAsync()
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
            OperationRespObj responseObj = new OperationRespObj();
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.Workflow.GET_ALL_OPERATIONS);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<OperationRespObj>(data1);
                        new OperationRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<OperationRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new OperationRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new OperationRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new OperationRespObj
                {
                    Operations = responseObj.Operations,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            });
        }
        public async Task<FinTransacRegRespObj> PassEntryToFinance(TransactionObj request)
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            //string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            //gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
            FinTransacRegRespObj responseObj = new FinTransacRegRespObj();

            var jsonContent = JsonConvert.SerializeObject(request);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.PostAsync(ApiRoutes.Finance.PASS_TO_ENTRY, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<FinTransacRegRespObj>(data1);
                        new FinTransacRegRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<FinTransacRegRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new FinTransacRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new FinTransacRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new FinTransacRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            });
        }
        public async Task<SubGLRespObj> GetAllSubGlAsync()
        {
            try
            {
                SubGLRespObj responseObj = new SubGLRespObj();
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
                gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        result = await gosGatewayClient.GetAsync(ApiRoutes.Finance.GET_ALL_SUBGL);
                        var data = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<SubGLRespObj>(data);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
                if (responseObj == null)
                {
                    return new SubGLRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new SubGLRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new SubGLRespObj
                {
                    subGls = responseObj.subGls,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                return new SubGLRespObj
                {
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Please tyr again later",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : LoginAsync{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
        public async Task<MailRespObj> SendMail(MailObj request)
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            //string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            //gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
            MailRespObj responseObj = new MailRespObj();

            var jsonContent = JsonConvert.SerializeObject(request);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.PostAsync(ApiRoutes.Identity.SEND_MAIL, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<MailRespObj>(data1);
                        new MailRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<MailRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new MailRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new MailRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new MailRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            });
        }
        public async Task<SecretKeysRespObj> GetFlutterWaveKeys()
        {
            var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
            string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
            gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);
            SecretKeysRespObj responseObj = new SecretKeysRespObj();

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await gosGatewayClient.GetAsync(ApiRoutes.Identity.GET_FLUTTERWAVE_KEYS);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<SecretKeysRespObj>(data1);
                        new MailRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                            }
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<SecretKeysRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new SecretKeysRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage { FriendlyMessage = "System Error!! Please contact Administrator" }
                        }
                    };
                }
                if (!responseObj.Status.IsSuccessful)
                {
                    return new SecretKeysRespObj
                    {
                        keys = responseObj.keys,
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = responseObj.Status.IsSuccessful,
                            Message = responseObj.Status.Message
                        }
                    };
                }
                return new SecretKeysRespObj
                {
                    keys = responseObj.keys,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = responseObj.Status.IsSuccessful,
                        Message = responseObj.Status.Message
                    }
                };
            });
        }

        public async Task<LogingFailedRespObj> CheckForFailedTrailsAsync(bool isSuccessful, int module, string userid)
        {
            var response = new LogingFailedRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                var request = new LoginFailed
                {
                    UserId = userid,
                    IsSuccessful = isSuccessful,
                    Module = module
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var buffer = Encoding.UTF8.GetBytes(jsonContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = await gosGatewayClient.PostAsync(ApiRoutes.Identity.FAILED_LOGIN, byteContent);
                var resultString = await result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<LogingFailedRespObj>(resultString);
                return response;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
                #endregion
            }
        }
         
        public async Task<SessionCheckerRespObj> CheckForSessionTrailAsync(string userid, int module)
        {
            var response = new SessionCheckerRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                var request = new SessionTrail
                {
                    UserId = userid,
                    Module = module
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var buffer = Encoding.UTF8.GetBytes(jsonContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = await gosGatewayClient.PostAsync(ApiRoutes.Identity.Session_LOGIN, byteContent);
                var resultString = await result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<SessionCheckerRespObj>(resultString);
                return response;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
                #endregion
            }
        }

        public async Task<SecurityResp> GetSettingsAsync()
        {
            var response = new SecurityResp { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");

                var result = await gosGatewayClient.GetAsync(ApiRoutes.Identity.SECURITY);
                var resultString = await result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<SecurityResp>(resultString);
                return response;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
                #endregion
            }
        }

        public async Task<QuestionsRespObj> GetQuestionsAsync()
        {
            var response = new QuestionsRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");

                var result = await gosGatewayClient.GetAsync(ApiRoutes.IdentitySeverWorkflow.GET_QUESTION);
                var resultString = await result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<QuestionsRespObj>(resultString);
                return response;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
                #endregion
            }
        }


        public class SessionTrail
        {
            public string UserId { get; set; }
            public int Module { get; set; }
        } 
        public class LoginFailed
        {
            public string UserId { get; set; }
            public bool IsSuccessful { get; set; }
            public int Module { get; set; }
        }

        public async Task<ActivityRespObj> GetAllActivityAsync()
        {
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
                gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

                var response = new ActivityRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var result = await gosGatewayClient.GetAsync(ApiRoutes.IdentitySeverWorkflow.ACTIVITES);
                var resultString = await result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ActivityRespObj>(resultString);
                if (!result.IsSuccessStatusCode)
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = $"{result.ReasonPhrase}  {(int)result.StatusCode}  {result.Content}";
                    throw new Exception($"{response}");
                }
                return response;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
                #endregion
            }
        }

        public async Task<UserRoleRespObj> GetUserRolesAsync()
        {
            try
            {
                var gosGatewayClient = _httpClientFactory.CreateClient("GOSDEFAULTGATEWAY");
                string authorization = _accessor.HttpContext.Request.Headers["Authorization"];
                gosGatewayClient.DefaultRequestHeaders.Add("Authorization", authorization);

                var response = new UserRoleRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var result = await gosGatewayClient.GetAsync(ApiRoutes.IdentitySeverWorkflow.GET_THIS_ROLES);
                var resultString = await result.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<UserRoleRespObj>(resultString);
                if (!result.IsSuccessStatusCode)
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = $"{result.ReasonPhrase}  {(int)result.StatusCode}  {result.Content}";
                    throw new Exception($"{response}");
                }
                return response;
            }
            catch (Exception ex)
            {
                #region Log error 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
                #endregion
            }
        }

    }
}
