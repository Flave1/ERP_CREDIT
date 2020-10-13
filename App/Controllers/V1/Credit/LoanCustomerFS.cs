using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Credit;
using Banking.Contracts.V1;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.CollateralCustomerObjs;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class LoanCustomerFS : Controller
    {
        private readonly ILoanCustomerFSRepository _repo;
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerRequest _identityServer;

        #region LOAN_CUSTOMER_FS_CAPTION_GROUP

        public LoanCustomerFS(ILoanCustomerFSRepository repo, IIdentityService identityService, ILoggerService logger, IHttpContextAccessor httpContextAccessor, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _identityServer = identityServer;
        }

        [ERPActivity(Action = UserActions.Add, Activity = 94)]
        [HttpPost(ApiRoutes.LoanCustomerFS.ADD_FS_CAPTION_GROUP)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> AddUpdateLoanCustomerFSCaptionGroup([FromBody] LoanCustomerFSCaptionGroupObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = _repo.AddUpdateLoanCustomerFSCaptionGroup(model);
                if (response)
                {
                    return new LoanCustomerFSCaptionGroupRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = $"Record saved successfully." } }
                    };
                }
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION_GROUP)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetAllLoanCustomerFSCaptionGroup()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerFSCaptionGroup();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSGroup = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [ERPActivity(Action = UserActions.View, Activity = 94)]
        [HttpGet(ApiRoutes.LoanCustomerFS.GET_FS_CAPTION_GROUP_BY_GROUP_ID)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSCaptionGroupByGroupId([FromQuery] LoanCustomerFSSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSCaptionGroupByGroupId(model.FSCaptionGroupId);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaption = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 94)]
        [HttpGet(ApiRoutes.LoanCustomerFS.GET_FS_CAPTION_GROUP_BY_ID)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSCaptionGroup([FromQuery] LoanCustomerFSSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSCaptionGroup(model.FSCaptionGroupId);
                var respList = new List<LoanCustomerFSCaptionGroupObj> { response };
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSGroup = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.FS_ALL_CAPTION_GROUP)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSCaptionGroup()
        {
            try
            {
                var response = _repo.GetLoanCustomerFSCaptionGroup();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSGroup = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 94)]
        [HttpPost(ApiRoutes.LoanCustomerFS.DELETE_FS_CAPTION_GROUP)]
        public ActionResult<LoanCustomerFSCaptionGroupRespObj> DeleteLoanApplication([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerFSCaptionGroup(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region LOAN_CUSTOMER_FS_CAPTION
        [ERPActivity(Action = UserActions.Add, Activity = 94)]
        [HttpPost(ApiRoutes.LoanCustomerFS.ADD_FS_CAPTION)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> AddUpdateLoanCustomerFSCaption([FromBody] LoanCustomerFSCaptionObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                if (model.FSCaptionGroupId == 0)
                {
                    if (_repo.ValidateFSCaption(model.FSCaptionName))
                    {
                        return new LoanCustomerFSCaptionGroupRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = $"Record with Caption {model.FSCaptionName} already exist" } }
                        };
                    }
                }
                var response = _repo.AddUpdateLoanCustomerFSCaption(model);
                if (response)
                {
                    return new LoanCustomerFSCaptionGroupRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = $"Record saved successfully." } }
                    };
                }
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetAllLoanCustomerFSCaption()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerFSCaption();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaption = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 94)]
        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION_BY_ID)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSCaptionByCaptionId([FromQuery] LoanCustomerFSSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSCaptionByCaptionId(model.FSCaptionId);
                var respList = new List<LoanCustomerFSCaptionObj> { response };
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaption = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION_BY_GROUP_ID)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSCaptionByCaptionGroupId([FromQuery] LoanCustomerFSSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSCaptionByCaptionGroupId(model.FSCaptionGroupId);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaption = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomerFS.GET_FS_CAPTION_UNMAPPED)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetUnmappedLoanCustomerFSCaption([FromBody]FSQueryObj model)
        {
            try
            {
                var response = _repo.GetUnmappedLoanCustomerFSCaption(model.FsCaptionGroupId, model.CustomerId, model.FsDate);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaption = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomerFS.DELETE_FS_CAPTION)]
        public ActionResult<LoanCustomerFSCaptionGroupRespObj> DeleteLoanCustomerFSCaptionGroup([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerFSCaption(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region LOAN_CUSTOMER_FS_CAPTION_DETAIL
        [HttpPost(ApiRoutes.LoanCustomerFS.ADD_FS_CAPTION_DETAIL)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> AddUpdateLoanCustomerFSCaptionDetail([FromBody] LoanCustomerFSCaptionDetailObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = _repo.AddUpdateLoanCustomerFSCaptionDetail(model);
                if (response)
                {
                    return new LoanCustomerFSCaptionGroupRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = $"Record saved successfully." } }
                    };
                }
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION_DETAIL)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetAllLoanCustomerFSCaptionDetail()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerFSCaptionDetail();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaptionDetail = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION_DETAIL_BY_ID)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSCaptionDetailById([FromQuery] LoanCustomerFSSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSCaptionDetailById(model.FSDetailId);
                var respList = new List<LoanCustomerFSCaptionDetailObj> { response };
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaptionDetail = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION_MAPPED)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSCaptionByCaptionGroupId([FromQuery] FSQueryObj model)
        {
            try
            {
                var response = _repo.GetMappedCustomerFsCaptions(model.CustomerId);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaptionDetail = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomerFS.GET_ALL_FS_CAPTION_DETAIL_MAPPED)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetMappedCustomerFsCaptionDetail([FromBody] FSQueryObj model)
        {
            try
            {
                var response = _repo.GetMappedCustomerFsCaptionDetail(model.CustomerId, model.FsCaptionGroupId, model.FsDate);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSCaptionDetail = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomerFS.DELETE_FS_CAPTION_DETAIL)]
        public ActionResult<LoanCustomerFSCaptionGroupRespObj> DeleteLoanCustomerFSCaptionDetail(int fSDetailId)
        {
            var response = false;
                response = _repo.DeleteLoanCustomerFSCaptionDetail(fSDetailId);
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region LOAN_CUSTOMER_FS_RATIO_DETAIL
        [HttpPost(ApiRoutes.LoanCustomerFS.ADD_FS_RATIO_DETAIL)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> AddUpdateLoanCustomerFSRatioDetail([FromBody] LoanCustomerFSRatioDetailObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = _repo.AddUpdateLoanCustomerFSRatioDetail(model);
                if (response)
                {
                    return new LoanCustomerFSCaptionGroupRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = $"Record saved successfully." } }
                    };
                }
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_RATIO_DETAIL)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetAllLoanCustomerFSRatioDetail()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerFSRatioDetail();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSRatioDetail = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_RATIO_CAPTION)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetFSRatioCaption()
        {
            try
            {
                var response = _repo.GetFSRatioCaption();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSRatioCaption = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_ALL_FS_RATIO_DETAIL_BY_ID)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSRatioDetails([FromQuery] LoanCustomerFSSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSRatioDetail(model.RatioDetailId);
                var respList = new List<LoanCustomerFSRatioDetailObj> { response };
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSRatioDetail = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_FS_RATIO_DETAIL)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSRatioDetail([FromQuery] LoanCustomerFSSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSRatioDetail(model.RatioDetailId, model.FSCaptionGroupId);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSRatioDetail = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_FS_RATIO_CALCULATION)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSRatiosCalculation([FromQuery] FSQueryObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSRatiosCalculation(model.CustomerId);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSRatioCalculation = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.GET_FS_RATIO_VALUES)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetLoanCustomerFSRatioValues([FromQuery] FSQueryObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerFSRatioValues(model.CustomerId);
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    LoanCustomerFSRatioCaptionReport = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomerFS.DIVISOR_TYPES)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetAllLoanCustomerFSDivisorType([FromQuery] FSQueryObj model)
        {
            try
            {
                var response = _repo.GetAllLoanCustomerFSDivisorType();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Types = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomerFS.VALUE_TYPES)]
        public async Task<ActionResult<LoanCustomerFSCaptionGroupRespObj>> GetAllLoanCustomerFSValueType([FromBody] FSQueryObj model)
        {
            try
            {
                var response = _repo.GetAllLoanCustomerFSValueType();
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Types = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerFSCaptionGroupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomerFS.DELETE_FS_RATIO_DETAIL)]
        public ActionResult<LoanCustomerFSCaptionGroupRespObj> DeleteLoanCustomerFSRatioDetail([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerFSRatioDetail(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion
    }
}
