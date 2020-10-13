using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Deposit;
using Banking.Contracts.Response.InvestorFund;
using Banking.Contracts.Response.Mail;
using Banking.Contracts.V1;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.DomainObjects.InvestorFund;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using Banking.Repository.Interface.InvestorFund;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ApprovalObjs;

namespace Banking.Controllers.V1.InvestorsFund
{
    [ERPAuthorize]
    public class InvestorFundController : Controller
    {
        private readonly IInvestorFundService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        ICreditAppraisalRepository _customerTrans;
        public InvestorFundController(IInvestorFundService repo, 
            IMapper mapper, IIdentityService identityService, 
            IHttpContextAccessor httpContextAccessor, 
            ILoggerService logger, DataContext dataContext, ICreditAppraisalRepository customerTrans,
            IIdentityServerRequest serverRequest)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dataContext = dataContext;
            _serverRequest = serverRequest;
            _customerTrans = customerTrans;
        }

        #region InvestorFund

        [HttpPost(ApiRoutes.InvestorFund.ADD_UPDATE_INVESTORFUND)]
        public async Task<ActionResult<InvestorFundRespObj>> AddUpdateInvestorFund([FromBody] InvestorFundObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                entity.CreatedBy = user.UserName;
                entity.UpdatedBy = user.UserName;
                entity.CompanyId = user.CompanyId;
                return await _repo.UpdateInvestorFund(entity);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.InvestorFund.ADD_UPDATE_INVESTORFUND_ROLLOVER)]
        public async Task<ActionResult<InvestorFundRespObj>> UpdateRollOverInvestorFund([FromBody] InvestorFundObj entity)
        {
            try
            {
                return await _repo.UpdateRollOverInvestorFund(entity);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.InvestorFund.ADD_UPDATE_INVESTORFUND_TOPUP)]
        public async Task<ActionResult<InvestorFundRespObj>> UpdateTopUpInvestorFund([FromBody] InvestorFundObj entity)
        {
            try
            {
                var res = await _repo.UpdateTopUpInvestorFund(entity);
                if (res.Status.IsSuccessful)
                {
                    var investment = _dataContext.inf_investorfund.Find(entity.InvestorFundId);
                    var customer = _dataContext.credit_loancustomer.Find(investment.InvestorFundCustomerId);
                    CustomerTransactionObj customerEntry = new CustomerTransactionObj
                    {
                        CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == investment.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                        Description = "Topup Payment",
                        TransactionDate = DateTime.Now,
                        ValueDate = DateTime.Now,
                        TransactionType = "Credit",
                        CreditAmount = entity.ProposedAmount ?? 0,
                        DebitAmount = 0,
                        Beneficiary = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).LastName,
                        ReferenceNo = investment.RefNumber,
                    };
                    _customerTrans.CustomerTransaction(customerEntry);
                    await _serverRequest.SendMail(new MailObj
                    {
                        fromAddresses = new List<FromAddress> { },
                        toAddresses = new List<ToAddress>
                            {
                                new ToAddress{ address = customer.Email, name = customer.FirstName}
                            },
                        subject = "Successful TopUp Application",
                        content = $"Hi {customer.FirstName}, <br> Your topup application process is completed. <br/> Top Up Amount: {entity.ProposedAmount}",
                        sendIt = true,
                    });
                }
                return res;
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORFUND_APPROVAL_LIST)]
        public async Task<ActionResult<InvestorFundRespObj>> GetInvestmentForAppraisalAsync()
        {
            try
            {
                return await _repo.GetInvestmentForAppraisalAsync();
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpPost(ApiRoutes.InvestorFund.ADD_GO_FOR_APPROVAL)]
        public async Task<ActionResult<ApprovalRegRespObj>> GoForApproval([FromBody] ApprovalObj entity)
        {
            try
            {
                using (var _trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await _serverRequest.UserDataAsync();
                        var investment = _dataContext.inf_investorfund.Find(entity.TargetId);

                        var req = new IndentityServerApprovalCommand
                        {
                            ApprovalComment = entity.Comment,
                            ApprovalStatus = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            WorkflowToken = investment.WorkflowToken,
                            ReferredStaffId = entity.ReferredStaffId
                        };

                        var previousDetails = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(investment.WorkflowToken) && x.TargetId == entity.TargetId).ToList();
                        var lastDate = investment.CreatedOn;
                        if (previousDetails.Count() > 0)
                        {
                            lastDate = previousDetails.OrderByDescending(x => x.ApprovalDetailId).FirstOrDefault().Date;
                        }
                        
                        var details = new cor_approvaldetail
                        {
                            Comment = entity.Comment,
                            Date = DateTime.Now,
                            ArrivalDate = previousDetails.Count() > 0 ? lastDate : investment.CreatedOn,
                            StatusId = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            StaffId = user.StaffId,
                            WorkflowToken = investment.WorkflowToken
                        };

                        var result = await _serverRequest.StaffApprovalRequestAsync(req);

                        if (!result.IsSuccessStatusCode)
                        {
                            return new ApprovalRegRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                }
                            };
                        }

                        var stringData = await result.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<ApprovalRegRespObj>(stringData);

                        if (!response.Status.IsSuccessful)
                        {
                            return new ApprovalRegRespObj
                            {
                                Status = response.Status
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Processing)
                        {
                            investment.ApprovalStatus = (int)ApprovalStatus.Processing;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Processing,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }


                        if (response.ResponseId == (int)ApprovalStatus.Revert)
                        {
                            investment.ApprovalStatus = (int)ApprovalStatus.Revert;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Revert,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Approved)
                        {
                            investment.ApprovalStatus = (int)ApprovalStatus.Approved;
                            var res = await _repo.InvestmentApproval(entity.TargetId, entity.ApprovalStatusId);
                            if (res)
                            {
                                CustomerTransactionObj customerEntry = new CustomerTransactionObj
                                {
                                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == investment.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                                    Description = "Investment Payment",
                                    TransactionDate = DateTime.Now,
                                    ValueDate = DateTime.Now,
                                    TransactionType = "Credit",
                                    CreditAmount = investment.ApprovedAmount ?? 0,
                                    DebitAmount = 0,
                                    Beneficiary = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).LastName,
                                    ReferenceNo = investment.RefNumber,
                                };
                                _customerTrans.CustomerTransaction(customerEntry);
                            }                            
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Approved,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Disapproved)
                        {
                            investment.ApprovalStatus = (int)ApprovalStatus.Disapproved;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Disapproved,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }
                        return new ApprovalRegRespObj
                        {
                            Status = response.Status
                        };
                    }
                    catch (SqlException ex)
                    {
                        _trans.Rollback();
                        throw;
                    }
                    finally { _trans.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApprovalRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.DOWNLOAD_INVESTORFUND)]
        public async Task<ActionResult<InvestorFundRespObj>> GenerateExportInvestorFund()
        {
            try
            {
                var response = _repo.GenerateExportInvestorFund();

                return new InvestorFundRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        
        [HttpGet(ApiRoutes.InvestorFund.GET_INVESTORFUND_BY_ID)]
        public async Task<ActionResult<InvestorFundRespObj>> GetInvestorFundAsync([FromQuery] SearchObj search)
        {
            try
            {
                if (search.SearchId < 1)
                {
                    return new InvestorFundRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "InvestorFund Id is required" } }
                    };
                }

                var response = await _repo.GetInvestorFundAsync(search.SearchId);
                var resplist = new List<InvestorFundObj> { response };
                return new InvestorFundRespObj
                {
                    InvestorFunds = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_CURRENT_BAL_BY_INVESTFUND_ID)]
        public async Task<ActionResult<InvestorFundRespObj>> GetCurrentBalance([FromQuery] SearchObj search)
        {
            try
            {
                if (search.SearchId < 1)
                {
                    return new InvestorFundRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "InvestorFund Id is required" } }
                    };
                }

                var response = _repo.GetCurrentBalance(search.SearchDate, search.SearchId);
                return new InvestorFundRespObj
                {
                    CurrentBalance = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.InvestorFund.DELETE_INVESTORFUND)]
        public async Task<IActionResult> DeleteInvestorFund([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteInvestorFund(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }

        [HttpPost(ApiRoutes.InvestorFund.ADD_INVESTORFUND_CUSTOMER)]
        public async Task<ActionResult<InvestorFundRegRespObj>> UpdateInvestorFundByCustomer([FromBody] InvestorFundObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

               return await _repo.UpdateInvestorFundByCustomer(model);

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.InvestorFund.ADD_INVESTORFUND_TOPUP_CUSTOMER)]
        public async Task<ActionResult<InvestorFundRegRespObj>> UpdateInvestorFundByCustomer([FromBody] InvestFundTopUpObj model)
        {
            try
            {
                return await _repo.UpdateInvestorFundTopUpByCustomer(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.InvestorFund.ADD_INVESTORFUND_ROLLOVER_CUSTOMER)]
        public async Task<ActionResult<InvestorFundRegRespObj>> UpdateInvestorFundByCustomer([FromBody] InvestFundRollOverObj model)
        {
            try
            {
                return await _repo.UpdateInvestorFundRollOverByCustomer(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.InvestorFund.ADD_INVESTORFUND_InvestmentRecommendation)]
        public async Task<ActionResult<InvestmentApprovalRecommendationObj>> UpdateInvestmentRecommendation([FromBody] InvestmentApprovalRecommendationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                var response = _repo.UpdateInvestmentRecommendation(model, user);
                var repList = new List<InvestmentApprovalRecommendationObj> { response };
                return new InvestmentApprovalRecommendationObj
                {
                    InvestmentApprovalRecommendations = repList,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved successfully" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestmentApprovalRecommendationObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_INVESTORFUND_RECOMMENDATION)]
        public async Task<ActionResult<InvestorFundRespObj>> GetInvestmentRecommendationLog([FromQuery] SearchObj model)
        {
            try
            {
                var response =  _repo.GetInvestmentRecommendationLog(model.SearchId);
                return new InvestorFundRespObj
                {
                    InvestmentRecommendationLogs = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORFUND_WEBSITE_LIST)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorFundWebsiteList()
        {
            try
            {
                var response = _repo.GetAllInvestorFundWebsiteList();
                return new InvestorFundRespObj
                {
                    InvestorFunds = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORFUND_WEBSITE_ID)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorFundWebsiteById([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllInvestorFundWebsiteById(model.SearchId);
                var resplist = new List<InvestorFundObj> { response };
                return new InvestorFundRespObj
                {
                    InvestorFunds = resplist,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORFUND_WEBSITE_ROLLOVER_LIST)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorFundRollOverWebsiteList()
        {
            try
            {
                var response = _repo.GetAllInvestorFundRollOverWebsiteList();
                return new InvestorFundRespObj
                {
                    RollOver = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORFUND_WEBSITE_ROLLOVER_ID)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorFundRollOverWebsiteById([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllInvestorFundRollOverWebsiteById(model.SearchId);
                var resplist = new List<InvestorFundObj> { response };
                return new InvestorFundRespObj
                {
                    InvestorFunds = resplist,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORFUND_WEBSITE_TOPUP_LIST)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorFundTopUpWebsiteList()
        {
            try
            {
                var response = _repo.GetAllInvestorFundTopUpWebsiteList();
                return new InvestorFundRespObj
                {
                    TopUp = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORFUND_WEBSITE_TOPUP_ID)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorFundTopUpWebsiteById([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllInvestorFundTopUpWebsiteById(model.SearchId);
                var resplist = new List<InvestorFundObj> { response };
                return new InvestorFundRespObj
                {
                    InvestorFunds = resplist,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTORRUNNING_FACILITIES)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorRunningFacilities([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllInvestorRunningFacilities(model.CustomerId);
                return new InvestorFundRespObj
                {
                    InvestorRunningFacilities = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTMENT_BY_CUSTOMER_ID)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestmentByCustomerId([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllInvestmentByCustomerId(model.CustomerId);
                return new InvestorFundRespObj
                {
                    InvestorRunningFacilities = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTMENT_LIST)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestmentList()
        {
            try
            {
                var response = _repo.GetAllInvestmentList();
                return new InvestorFundRespObj
                {
                    InvestmentLists = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTOR_LIST)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestorList()
        {
            try
            {
                var response = _repo.GetAllInvestorList();
                return new InvestorFundRespObj
                {
                    InvestorLists = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_RUNNING_INVESTMENT_BY_CUSTOMER)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllRunningInvestmenByCustomer([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllRunningInvestmenByCustomer(model.CustomerId);
                return new InvestorFundRespObj
                {
                    InvestorFunds = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_INVESTMENT_CERTIFICATES)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllInvestmentCertificates()
        {
            try
            {
                var response = _repo.GetAllInvestmentCertificates();
                return new InvestorFundRespObj
                {
                    InvestorFunds = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_RUNNING_INVESTMENT)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllRunningInvestment()
        {
            try
            {
                var response = _repo.GetAllRunningInvestment();
                return new InvestorFundRespObj
                {
                    InvestorFunds = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.InvestorFund.GET_ALL_RUNNING_INVESTMENT_BY_STATUS)]
        public async Task<ActionResult<InvestorFundRespObj>> GetAllRunningInvestmentByStatus([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetAllRunningInvestmentByStatus(model.SearchId);
                return new InvestorFundRespObj
                {
                    InvestorFunds = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InvestorFundRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion


        #region Collection
        [HttpGet(ApiRoutes.Collection.GET_ALL_COLLECTION)]
        public async Task<ActionResult<CollectionRespObj>> GetAllCollection()
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var response = _repo.GetAllCollection();
                return new CollectionRespObj
                {
                    Collections = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collection.ADD_UPDATE_COLLECTION)]
        public async Task<ActionResult<CollectionRespObj>> AddUpdateCollection([FromBody] CollectionObj entity)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                entity.CreatedBy = user;
                entity.UpdatedBy = user;
                var response = await _repo.AddUpdateCollection(entity);
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response.Status.IsSuccessful, Message = new APIResponseMessage { FriendlyMessage = response.Status.Message.FriendlyMessage } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collection.DOWNLOAD_COLLECTION)]
        public async Task<ActionResult<CollectionRespObj>> GenerateExportCollection()
        {
            try
            {
                var response = _repo.GenerateExportCollection();

                return new CollectionRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collection.GET_COLLECTION_BY_ID)]
        public async Task<ActionResult<CollectionRespObj>> GetCollection([FromQuery] SearchObj search)
        {
            try
            {
                if (search.SearchId < 1)
                {
                    return new CollectionRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Collection Id is required" } }
                    };
                }

                var response = _repo.GetCollection(search.SearchId);
                var resplist = new List<CollectionObj> { response };
                return new CollectionRespObj
                {
                    Collections = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collection.DELETE_COLLECTION)]
        public async Task<IActionResult> DeleteCollection([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCollection(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObjt
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }

        [HttpPost(ApiRoutes.Collection.ADD_COLLECTION_CUSTOMER)]
        public async Task<ActionResult<CollectionRegRespObj>> UpdateCollectionFundByCustomer([FromBody] CollectionObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                return await _repo.UpdateCollectionOperationByCustomer(model);

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collection.ADD_COLLECTION_RECOMMENDATION)]
        public async Task<ActionResult<CollectionApprovalRecommendationObj>> UpdateCollectionRecommendation([FromBody] CollectionApprovalRecommendationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                var response = _repo.UpdateCollectionRecommendation(model, user);
                var repList = new List<CollectionApprovalRecommendationObj> { response };
                return new CollectionApprovalRecommendationObj
                {
                    CollectionApprovalRecommendations = repList,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved successfully" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionApprovalRecommendationObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collection.GET_COLLECTION_RECOMMENDATION)]
        public async Task<ActionResult<CollectionRespObj>> GetCollectionRecommendationLog([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetCollectionRecommendationLog(model.SearchId);
                return new CollectionRespObj
                {
                    CollectionRecommendationLogs = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collection.GET_ALL_COLLECTION_WEBSITE_LIST)]
        public async Task<ActionResult<CollectionRespObj>> GetAllCollectionWebsiteList()
        {
            try
            {
                var response = _repo.GetAllCollectionOperationWebsiteList();
                return new CollectionRespObj
                {
                    Collections = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collection.GET_ALL_COLLECTION_WEBSITE_ID)]
        public async Task<ActionResult<CollectionRespObj>> GetAllCollectionWebsiteById([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetCollectionOperationWebsiteById(model.SearchId);
                var resplist = new List<CollectionObj> { response };
                return new CollectionRespObj
                {
                    Collections = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collection.GET_ALL_COLLECTION_APPROVAL_LIST)]
        public async Task<ActionResult<CollectionRespObj>> GetCollectionForAppraisalAsync()
        {
            try
            {
                return await _repo.GetCollectionForAppraisalAsync();
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollectionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collection.ADD_GO_FOR_COLLECTION_APPROVAL)]
        public async Task<ActionResult<ApprovalRegRespObj>> GoForCollectionApproval([FromBody] ApprovalObj entity)
        {
            try
            {
                using (var _trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await _serverRequest.UserDataAsync();
                        var collection = _dataContext.inf_collection.Find(entity.TargetId);

                        var req = new IndentityServerApprovalCommand
                        {
                            ApprovalComment = entity.Comment,
                            ApprovalStatus = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            WorkflowToken = collection.WorkflowToken,
                            ReferredStaffId = entity.ReferredStaffId
                        };

                        var previousDetails = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(collection.WorkflowToken) && x.TargetId == entity.TargetId).ToList();
                        var lastDate = collection.CreatedOn;
                        if (previousDetails.Count() > 0)
                        {
                            lastDate = previousDetails.OrderByDescending(x => x.ApprovalDetailId).FirstOrDefault().Date;
                        }
                        var details = new cor_approvaldetail
                        {
                            Comment = entity.Comment,
                            Date = DateTime.Now,
                            ArrivalDate = previousDetails.Count() > 0 ? lastDate : collection.CreatedOn,
                            StatusId = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            StaffId = user.StaffId,
                            WorkflowToken = collection.WorkflowToken
                        };

                        var result = await _serverRequest.StaffApprovalRequestAsync(req);

                        if (!result.IsSuccessStatusCode)
                        {
                            return new ApprovalRegRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                }
                            };
                        }

                        var stringData = await result.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<ApprovalRegRespObj>(stringData);

                        if (!response.Status.IsSuccessful)
                        {
                            return new ApprovalRegRespObj
                            {
                                Status = response.Status
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Processing)
                        {
                            collection.ApprovalStatus = (int)ApprovalStatus.Processing;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Processing,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }


                        if (response.ResponseId == (int)ApprovalStatus.Revert)
                        {
                            collection.ApprovalStatus = (int)ApprovalStatus.Revert;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Revert,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Approved)
                        {
                            collection.ApprovalStatus = (int)ApprovalStatus.Approved;
                            var investment = _dataContext.inf_investorfund.Find(collection.InvestorFundId);
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            var res = await _repo.CollectionApproval(entity.TargetId, entity.ApprovalStatusId);
                            if (res)
                            {
                                CustomerTransactionObj customerEntry = new CustomerTransactionObj
                                {
                                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == investment.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                                    Description = "Collection Payment",
                                    TransactionDate = DateTime.Now,
                                    ValueDate = DateTime.Now,
                                    TransactionType = "Credit",
                                    CreditAmount = collection.AmountPayable ?? 0,
                                    DebitAmount = 0,
                                    Beneficiary = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).LastName,
                                    ReferenceNo = investment.RefNumber,
                                };
                                _customerTrans.CustomerTransaction(customerEntry);
                            }

                           
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Approved,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Disapproved)
                        {
                            collection.ApprovalStatus = (int)ApprovalStatus.Disapproved;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Disapproved,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }
                        return new ApprovalRegRespObj
                        {
                            Status = response.Status
                        };
                    }
                    catch (SqlException ex)
                    {
                        _trans.Rollback();
                        throw;
                    }
                    finally { _trans.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApprovalRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        #endregion


        #region Liquidation
        [HttpGet(ApiRoutes.Liquidation.GET_ALL_LIQUIDATION)]
        public async Task<ActionResult<LiquidationRespObj>> GetAllLiquidation()
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var response = _repo.GetAllLiquidation();
                return new LiquidationRespObj
                {
                    Liquidations = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Liquidation.ADD_UPDATE_LIQUIDATION)]
        public async Task<ActionResult<LiquidationRespObj>> AddUpdateLiquidation([FromBody] LiquidationObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                entity.UpdatedBy = user.UserName;
                entity.CreatedBy = user.UserName;
                var response = await _repo.AddUpdateLiquidation(entity);
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response.Status.IsSuccessful, Message = new APIResponseMessage { FriendlyMessage = response.Status.Message.FriendlyMessage } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Liquidation.DOWNLOAD_LIQUIDATION)]
        public async Task<ActionResult<LiquidationRespObj>> GenerateExportLiquidation()
        {
            try
            {
                var response = _repo.GenerateExportLiquidation();

                return new LiquidationRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Liquidation.GET_LIQUIDATION_BY_ID)]
        public async Task<ActionResult<LiquidationRespObj>> GetLiquidation([FromQuery] SearchObj search)
        {
            try
            {
                if (search.SearchId < 1)
                {
                    return new LiquidationRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Liquidation Id is required" } }
                    };
                }

                var response = await _repo.GetLiquidation(search.SearchId);
                var resplist = new List<LiquidationObj> { response };
                return new LiquidationRespObj
                {
                    Liquidations = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Liquidation.DELETE_LIQUIDATION)]
        public async Task<IActionResult> DeleteLiquidation([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLiquidation(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObjt
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }

        [HttpPost(ApiRoutes.Liquidation.ADD_LIQUIDATION_CUSTOMER)]
        public async Task<ActionResult<LiquidationRegRespObj>> UpdateLiquidationByCustomer([FromBody] LiquidationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                return await _repo.UpdateLiquidationOperationByCustomer(model);

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Liquidation.ADD_LIQUIDATION_RECOMMENDATION)]
        public async Task<ActionResult<LiquidationApprovalRecommendationObj>> UpdateLiquidationRecommendation([FromBody] LiquidationApprovalRecommendationObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                var response = _repo.UpdateLiquidationRecommendation(model, user);
                var repList = new List<LiquidationApprovalRecommendationObj> { response };
                return new LiquidationApprovalRecommendationObj
                {
                    LiquidationApprovalRecommendations = repList,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved successfully" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationApprovalRecommendationObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Liquidation.GET_LIQUIDATION_RECOMMENDATION)]
        public async Task<ActionResult<LiquidationRespObj>> GetLiquidationRecommendationLog([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetLiquidationRecommendationLog(model.SearchId);
                return new LiquidationRespObj
                {
                    LiquidationRecommendationLogs = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Liquidation.GET_ALL_LIQUIDATION_WEBSITE_LIST)]
        public async Task<ActionResult<LiquidationRespObj>> GetAllLiquidationWebsiteList()
        {
            try
            {
                var response = _repo.GetAllLiquidationOperationWebsiteList();
                return new LiquidationRespObj
                {
                    Liquidations = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Liquidation.GET_ALL_LIQUIDATION_WEBSITE_ID)]
        public async Task<ActionResult<LiquidationRespObj>> GetAllLiquidationWebsiteById([FromQuery] SearchObj model)
        {
            try
            {
                var response = _repo.GetLiquidationOperationWebsitebyId(model.SearchId);
                var resplist = new List<LiquidationObj> { response };
                return new LiquidationRespObj
                {
                    Liquidations = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Liquidation.GET_ALL_LIQUIDATION_APPROVAL_LIST)]
        public async Task<ActionResult<LiquidationRespObj>> GetLiquidationForAppraisalAsync()
        {
            try
            {
                return await _repo.GetLiquidationForAppraisalAsync();
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LiquidationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Liquidation.ADD_GO_FOR_LIQUIDATION_APPROVAL)]
        public async Task<ActionResult<ApprovalRegRespObj>> GoForLiquidationApproval([FromBody] ApprovalObj entity)
        {
            try
            {
                using (var _trans = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await _serverRequest.UserDataAsync();
                        var liquidation = _dataContext.inf_liquidation.Find(entity.TargetId);

                        var req = new IndentityServerApprovalCommand
                        {
                            ApprovalComment = entity.Comment,
                            ApprovalStatus = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            WorkflowToken = liquidation.WorkflowToken,
                            ReferredStaffId = entity.ReferredStaffId
                        };

                        var previousDetails = _dataContext.cor_approvaldetail.Where(x => x.WorkflowToken.Contains(liquidation.WorkflowToken) && x.TargetId == entity.TargetId).ToList();
                        var lastDate = liquidation.CreatedOn;
                        if (previousDetails.Count() > 0)
                        {
                            lastDate = previousDetails.OrderByDescending(x => x.ApprovalDetailId).FirstOrDefault().Date;
                        }
                        var details = new cor_approvaldetail
                        {
                            Comment = entity.Comment,
                            Date = DateTime.Now,
                            ArrivalDate = previousDetails.Count() > 0 ? lastDate : liquidation.CreatedOn,
                            StatusId = entity.ApprovalStatusId,
                            TargetId = entity.TargetId,
                            StaffId = user.StaffId,
                            WorkflowToken = liquidation.WorkflowToken
                        };

                        var result = await _serverRequest.StaffApprovalRequestAsync(req);

                        if (!result.IsSuccessStatusCode)
                        {
                            return new ApprovalRegRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    Message = new APIResponseMessage { FriendlyMessage = result.ReasonPhrase }
                                }
                            };
                        }

                        var stringData = await result.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<ApprovalRegRespObj>(stringData);

                        if (!response.Status.IsSuccessful)
                        {
                            return new ApprovalRegRespObj
                            {
                                Status = response.Status
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Processing)
                        {
                            liquidation.ApprovalStatus = (int)ApprovalStatus.Processing;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Processing,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }


                        if (response.ResponseId == (int)ApprovalStatus.Revert)
                        {
                            liquidation.ApprovalStatus = (int)ApprovalStatus.Revert;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Revert,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Approved)
                        {
                            var res = await _repo.LiquidationApproval(entity.TargetId, entity.ApprovalStatusId);
                            if (res)
                            {
                                var investment = _dataContext.inf_investorfund.Find(liquidation.InvestorFundId);
                                CustomerTransactionObj customerEntry = new CustomerTransactionObj
                                {
                                    CasaAccountNumber = _dataContext.credit_loancustomer.Where(x => x.CustomerId == investment.InvestorFundCustomerId).FirstOrDefault().CASAAccountNumber,
                                    Description = "Liquidation Payment",
                                    TransactionDate = DateTime.Now,
                                    ValueDate = DateTime.Now,
                                    TransactionType = "Credit",
                                    CreditAmount = liquidation.AmountPayable ?? 0,
                                    DebitAmount = 0,
                                    Beneficiary = _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).FirstName + " " + _dataContext.credit_loancustomer.FirstOrDefault(x => x.CustomerId == investment.InvestorFundCustomerId).LastName,
                                    ReferenceNo = investment.RefNumber,
                                };
                                _customerTrans.CustomerTransaction(customerEntry);
                            }
                            liquidation.ApprovalStatus = (int)ApprovalStatus.Approved;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Approved,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }

                        if (response.ResponseId == (int)ApprovalStatus.Disapproved)
                        {
                            liquidation.ApprovalStatus = (int)ApprovalStatus.Disapproved;
                            await _dataContext.cor_approvaldetail.AddAsync(details);
                            await _dataContext.SaveChangesAsync();
                            _trans.Commit();
                            return new ApprovalRegRespObj
                            {
                                ResponseId = (int)ApprovalStatus.Disapproved,
                                Status = new APIResponseStatus { IsSuccessful = true, Message = response.Status.Message }
                            };
                        }
                        return new ApprovalRegRespObj
                        {
                            Status = response.Status
                        };
                    }
                    catch (SqlException ex)
                    {
                        _trans.Rollback();
                        throw;
                    }
                    finally { _trans.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApprovalRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion
    }
}