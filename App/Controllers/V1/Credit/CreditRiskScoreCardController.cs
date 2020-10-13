using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
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
using static Banking.Contracts.Response.Credit.CreditBureauObjs;
using static Banking.Contracts.Response.Credit.CreditRiskAttributeObjs;
using static Banking.Contracts.Response.Credit.CreditRiskRatingObjs;
using static Banking.Contracts.Response.Credit.CreditRiskScoreCardObjs;
using static Banking.Contracts.Response.Credit.CreditWeightedRiskScoreCardObjs;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class CreditRiskScoreCardController : Controller
    {
        private readonly ICreditRiskScoreCardRepository _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;

        public CreditRiskScoreCardController(ICreditRiskScoreCardRepository repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
        }
 
        #region CREDIT_RISK_SCORE_CARD
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_CREDITRISK_SCORECARD)]
        public async Task<ActionResult<CreditRiskScoreCardObjRespObj>> GetAllCreditRiskScoreCard()
        {
            try
            {
                var response = _repo.GetAllCreditRiskScoreCard();
                return new CreditRiskScoreCardObjRespObj
                {
                    CreditRiskScoreCard = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskScoreCardObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITRISK_GROUP_ATTRIBUTE)]
        public async Task<ActionResult<GroupedCreditRiskAttibuteObjRespObj>> GetGroupedAttribute()
        {
            try
            {
                var response = _repo.GetGroupedAttribute();
                return new GroupedCreditRiskAttibuteObjRespObj
                {
                    GroupedCreditRiskAttibute = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new GroupedCreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 93)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITRISK_SCORECARD_BY_ID)]
        public async Task<ActionResult<CreditRiskScoreCardObjRespObj>> GetCreditRiskScoreCard([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditRiskScoreCard(model.CreditRiskAttributeId);
                //var resplist = new List<CreditRiskScoreCardObj> { response };
                return new CreditRiskScoreCardObjRespObj
                {
                    CreditRiskScoreCard = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskScoreCardObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITRISK_APPLICATION_ATTRIBUTE)]
        public async Task<ActionResult<ApplicationCreditRiskAttibuteObjRespObj>> GetApplicationCreditRiskAttribute([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetApplicationCreditRiskAttribute(model.LoanApplicationId);
                //var resplist = new List<CreditRiskScoreCardObj> { response };
                return new ApplicationCreditRiskAttibuteObjRespObj
                {
                    ApplicationCreditRiskAttibute = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ApplicationCreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 93)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDITRISK_APPLICATION_SCORECARD)]
        public async Task<ActionResult<CreditRiskScoreCardObjRespObj>> AddUpdateCreditRiskScoreCard([FromBody] LoanApplicationScoreCardObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;
                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = _repo.AddUpdateAppicationScoreCard(model);
                if (isDone == 1)
                {
                    return new CreditRiskScoreCardObjRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved successfully" } }
                    };
                }
                else if(isDone == 2)
                {
                    return new CreditRiskScoreCardObjRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Fill application scord card to continue" } }
                    };
                }else if(isDone == 3)
                {
                    return new CreditRiskScoreCardObjRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Upload Credit Bureua to continue" } }
                    };
                }
                return new CreditRiskScoreCardObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskScoreCardObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 93)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDITRISK_SCORECARD)]
        public async Task<ActionResult<CreditRiskScoreCardObjRespObj>> AddUpdateCreditRiskScoreCard([FromBody] List<CreditRiskScoreCardObj> model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;
                foreach(var item in model)
                {
                    item.CreatedBy = user;
                    item.UpdatedBy = user;
                }
              
                var isDone = _repo.AddUpdateCreditRiskScoreCard(model);

                return new CreditRiskScoreCardObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskScoreCardObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 93)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_CREDITRISK_SCORECARD)]
        public ActionResult<CreditRiskScoreCardObjRespObj> DeleteCreditRiskScoreCard([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.TargetIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditRiskScoreCard(Convert.ToInt32(id));
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

        #region CREDIT_RISK_ATTRIBUTE
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_CREDITRISK_ATTRIBUTE)]
        public async Task<ActionResult<CreditRiskAttibuteObjRespObj>> GetAllCreditRiskAttribute()
        {
            try
            {
                var response = _repo.GetAllCreditRiskAttribute();
                return new CreditRiskAttibuteObjRespObj
                {
                    CreditRiskAttibutes = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_SYSTEM_ATTRIBUTE)]
        public async Task<ActionResult<CreditRiskAttibuteObjRespObj>> GetAllSystemCreditRiskAttribute()
        {
            try
            {
                var response = _repo.GetAllSystemCreditRiskAttribute();
                return new CreditRiskAttibuteObjRespObj
                {
                    SystemAttibutes = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_MAPPED_RISKED_ATTRIBUTE)]
        public async Task<ActionResult<CreditRiskAttibuteObjRespObj>> GetAllMappedCreditRiskAttribute()
        {
            try
            {
                var response = _repo.GetAllMappedCreditRiskAttribute();
                return new CreditRiskAttibuteObjRespObj
                {
                    MappedAttibutes = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 90)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITRISK_ATTRIBUTE_BY_ID)]
        public async Task<ActionResult<CreditRiskAttibuteObjRespObj>> GetCreditRiskAttribute([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditRiskAttribute(model.CreditRiskAttributeId);
                var resplist = new List<CreditRiskAttibuteObj> { response };
                return new CreditRiskAttibuteObjRespObj
                {
                    CreditRiskAttibutes = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 90)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDITRISK_ATTRIBUTE)]
        public async Task<ActionResult<CreditRiskAttibuteObjRespObj>> AddUpdateCreditAttribute([FromBody] CreditRiskAttibuteObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = _repo.AddUpdateCreditAttribute(model);

                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 90)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_CREDITRISK_ATTRIBUTE)]
        public ActionResult<CreditRiskAttibuteObjRespObj> DeleteCreditRiskAttribute([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditRiskAttribute(id);
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

        [ERPActivity(Action = UserActions.View, Activity = 90)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.DOWNLOAD_CREDITRISK_ATTRIBUTE)]
        public async Task<ActionResult<CreditRiskAttibuteObjRespObj>> GenerateExportCreditRiskAttribute()
        {
            try
            {
                var response = _repo.GenerateExportCreditRiskAttribute();

                return new CreditRiskAttibuteObjRespObj
                {
                    Export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 90)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.UPLOAD_CREDITRISK_ATTRIBUTE)]
        public async Task<ActionResult<CreditRiskAttibuteObjRespObj>> UploadCreditRiskAttribute()
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;
                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

                var byteList = new List<byte[]>();
                foreach (var fileBit in files)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteList.Add(ms.ToArray());
                        }
                    }

                }
                var isDone = _repo.UploadCreditRiskAttribute(byteList, createdBy);
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskAttibuteObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region CREDIT_RISK_CATEGORY
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_CREDIT_CATEGORY)]
        public async Task<ActionResult<CreditRiskCategoryObjRespObj>> GetAllCreditRiskCategory()
        {
            try
            {
                var response = _repo.GetAllCreditRiskCategory();
                return new CreditRiskCategoryObjRespObj
                {
                    CreditRiskCategory = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskCategoryObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 89)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDIT_CATEGORY_BY_ID)]
        public async Task<ActionResult<CreditRiskCategoryObjRespObj>> GetCreditRiskCategory([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditRiskCategory(model.CreditRiskCategoryId);
                var resplist = new List<CreditRiskCategoryObj> { response };
                return new CreditRiskCategoryObjRespObj
                {
                    CreditRiskCategory = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskCategoryObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 89)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDIT_CATEGORY)]
        public async Task<ActionResult<CreditRiskCategoryObjRespObj>> AddUpdateCreditCategory([FromBody] CreditRiskCategoryObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;

                var isDone = _repo.AddUpdateCreditCategory(model);

                return new CreditRiskCategoryObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskCategoryObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 89)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_CREDIT_CATEGORY)]
        public ActionResult<CreditRiskScoreCardObjRespObj> DeleteCreditRiskCategory([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditRiskCategory(id);
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

        #region CREDIT_RISK_RATING
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_CREDITRISK_RATING)]
        public async Task<ActionResult<CreditRiskRatingObjRespObj>> GetAllCreditRiskRating()
        {
            try
            {
                var response = _repo.GetAllCreditRiskRating();
                return new CreditRiskRatingObjRespObj
                {
                    CreditRiskRating = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 92)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITRISK_RATING_DETAIL)]
        public async Task<ActionResult<CreditRiskRatingObjRespObj>> GetAllSystemCreditRiskAttribute([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditRiskRatingDetail(model.LoanApplicationId);
                return new CreditRiskRatingObjRespObj
                {
                    CreditRiskRatingDetails = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 92)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITRISK_RATING_BY_ID)]
        public async Task<ActionResult<CreditRiskRatingObjRespObj>> GetCreditRiskRating([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditRiskRating(model.CreditRiskRatingId);
                var resplist = new List<CreditRiskRatingObj> { response };
                return new CreditRiskRatingObjRespObj
                {
                    CreditRiskRating = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 92)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDITRISK_RATING)]
        public async Task<ActionResult<CreditRiskRatingObjRespObj>> AddUpdateCreditRiskRating([FromBody] CreditRiskRatingObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = _repo.AddUpdateCreditRiskRating(model);

                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 92)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_CREDITRISK_RATING)]
        public ActionResult<CreditRiskAttibuteObjRespObj> DeleteCreditRiskRating([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditRiskRating(id);
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

        [ERPActivity(Action = UserActions.View, Activity = 92)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.DOWNLOAD_CREDITRISK_RATING)]
        public async Task<ActionResult<CreditRiskRatingObjRespObj>> GenerateExportCreditRiskRate()
        {
            try
            {
                var response = _repo.GenerateExportCreditRiskRate();

                return new CreditRiskRatingObjRespObj
                {
                    Export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 92)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.UPLOAD_CREDITRISK_RATING)]
        public async Task<ActionResult<CreditRiskRatingObjRespObj>> UploadCreditRiskRate()
        {
            try
            {
                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

                var byteList = new List<byte[]>();
                foreach (var fileBit in files)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteList.Add(ms.ToArray());
                        }
                    }

                }

                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;

                var isDone = _repo.UploadCreditRiskRate(byteList, createdBy);
                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region CREDIT_RISK_WEIGHTED_SCORE
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_CREDIT_WEIGHTED_RISKSCORE)]
        public async Task<ActionResult<CreditWeightedRiskScoreObjRespObj>> GetAllCreditWeightedRiskScore()
        {
            try
            {
                var response = _repo.GetAllCreditWeightedRiskScore();
                return new CreditWeightedRiskScoreObjRespObj
                {
                    CreditWeightedRiskScore = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditWeightedRiskScoreObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDIT_WEIGHTED_RISKSCORE_BY_ID)]
        public async Task<ActionResult<CreditWeightedRiskScoreObjRespObj>> GetCreditWeightedRiskScore([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditWeightedRiskScore(model.ProductId);
                //var resplist = new List<CreditWeightedRiskScoreObj> { response };
                return new CreditWeightedRiskScoreObjRespObj
                {
                    CreditWeightedRiskScore = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditWeightedRiskScoreObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDIT_WEIGHTED_RISKSCORE_BY_CUSTOMERTYPE)]
        public async Task<ActionResult<CreditWeightedRiskScoreObjRespObj>> GetCreditWeightedRiskScoreByCustomerType([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditWeightedRiskScoreByCustomerType(model.ProductId, model.CustomerTypeId);
                //var resplist = new List<CreditWeightedRiskScoreObj> { response };
                return new CreditWeightedRiskScoreObjRespObj
                {
                    CreditWeightedRiskScore = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditWeightedRiskScoreObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDIT_WEIGHTED_RISKSCORE)]
        public async Task<ActionResult<CreditWeightedRiskScoreObjRespObj>> AddUpdateCreditWeightedRiskScore([FromBody] List<CreditWeightedRiskScoreObj> model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;
                foreach(var item in model)
                {
                    item.CreatedBy = user;
                    item.UpdatedBy = user;
                }               

                var isDone = _repo.AddUpdateCreditWeightedRiskScore(model);

                return new CreditWeightedRiskScoreObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditWeightedRiskScoreObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_CREDIT_WEIGHTED_RISKSCORE)]
        public ActionResult<CreditRiskScoreCardObjRespObj> DeleteCreditWeightedRiskScore([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditWeightedRiskScore(id);
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

        #region CREDIT_BUREAU_SETUP
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_CREDITBUREAU)]
        public async Task<ActionResult<CreditBureauObjRespObj>> GetAllCreditBureau()
        {
            try
            {
                var response = _repo.GetAllCreditBureau();
                return new CreditBureauObjRespObj
                {
                    CreditBureau = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 82)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITBUREAU_BY_ID)]
        public async Task<ActionResult<CreditBureauObjRespObj>> GetCreditBureau([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditBureau(model.CreditBureauId);
                var resplist = new List<CreditBureauObj> { response };
                return new CreditBureauObjRespObj
                {
                    CreditBureau = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 82)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDITBUREAU)]
        public async Task<ActionResult<CreditBureauObjRespObj>> AddUpdateCreditBureau([FromBody] CreditBureauObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = _repo.AddUpdateCreditBureau(model);

                return new CreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 82)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_CREDITBUREAU)]
        public ActionResult<CreditBureauObjRespObj> DeleteCreditBureau([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditBureau(id);
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

        [ERPActivity(Action = UserActions.View, Activity = 82)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.DOWNLOAD_CREDITBUREAU)]
        public async Task<ActionResult<CreditBureauObjRespObj>> GenerateExportCreditBureau()
        {
            try
            {
                var response = _repo.GenerateExportCreditBureau();

                return new CreditBureauObjRespObj
                {
                    Export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 82)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.UPLOAD_CREDITBUREAU)]
        public async Task<ActionResult<CreditBureauObjRespObj>> UploadCreditBureau()
        {
            try
            {
                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

                var byteList = new List<byte[]>();
                foreach (var fileBit in files)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteList.Add(ms.ToArray());
                        }
                    }

                }

                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;

                var isDone = _repo.UploadCreditBureau(byteList, createdBy);
                return new CreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region LOAN_CREDIT_BUREAU
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_LOAN_CREDITBUREAU)]
        public async Task<ActionResult<LoanCreditBureauObjRespObj>> GetAllLoanCreditBureau()
        {
            try
            {
                var response = _repo.GetAllLoanCreditBureau();
                return new LoanCreditBureauObjRespObj
                {
                    LoanCreditBureau = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_LOAN_CREDITBUREAU_BY_ID)]
        public async Task<ActionResult<LoanCreditBureauObjRespObj>> GetLoanCreditBureau([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCreditBureau(model.CreditBureauId);
                var resplist = new List<LoanCreditBureauObj> { response };
                return new LoanCreditBureauObjRespObj
                {
                    LoanCreditBureau = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_APPLICATION_CREDITBUREAU)]
        public async Task<ActionResult<LoanCreditBureauObjRespObj>> GetLoanApplicationCreditBureau([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanApplicationCreditBureau(model.LoanApplicationId);
                var resplist = new List<LoanCreditBureauObj> { response };
                return new LoanCreditBureauObjRespObj
                {
                    LoanCreditBureau = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_LOAN_CREDITBUREAU)]
        public ActionResult<LoanCreditBureauObjRespObj> DeleteLoanCreditBureau([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCreditBureau(id);
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

        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_LOAN_CREDITBUREAU)]
        public async Task<ActionResult<LoanCreditBureauObjRespObj>> AddUpdateLoanCreditBureau()
        {
            try
            {            
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files;
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["supportDocument"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var creditBureauId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["creditBureauId"]);
                var loanApplicationId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["loanApplicationId"]);
                var chargeAmount = Convert.ToDecimal(_httpContextAccessor.HttpContext.Request.Form["chargeAmount"]);
                var reportStatus = Convert.ToBoolean(_httpContextAccessor.HttpContext.Request.Form["reportStatus"]);

                var byteArray = new byte[0];
                foreach (var fileBit in postedFile)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteArray = ms.ToArray();
                        }
                    }
                }

                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;

                var model = new LoanCreditBureauObj
                {
                    LoanApplicationId = loanApplicationId,
                    CreditBureauId = creditBureauId,
                    ReportStatus = reportStatus,
                    ChargeAmount = chargeAmount,
                    SupportDocument = byteArray,
                    CreatedBy = createdBy
                };

                var isDone = await _repo.AddUpdateLoanCreditBureau(model);
                return new LoanCreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCreditBureauObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region CREDIT_RISK_RATING_PD
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_ALL_CREDITRISK_RATING_PD)]
        public async Task<ActionResult<CreditRiskRatingPDObjRespObj>> GetAllCreditRiskRatingPD()
        {
            try
            {
                var response = _repo.GetAllCreditRiskRatingPD();
                return new CreditRiskRatingPDObjRespObj
                {
                    CreditRiskRatingPD = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_GROUPED_CREDITRISK_RATING_PD)]
        public async Task<ActionResult<CreditRiskRatingPDObjRespObj>> GetGroupedCreditRiskRatingPD()
        {
            try
            {
                var response = _repo.GetGroupedCreditRiskRatingPD();
                return new CreditRiskRatingPDObjRespObj
                {
                    GroupedCreditRiskRatingPD = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 91)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.GET_CREDITRISK_RATING_PD_BY_ID)]
        public async Task<ActionResult<CreditRiskRatingPDObjRespObj>> GetCreditRiskRatingPD([FromQuery] CreditRiskScoreCardSearchObj model)
        {
            try
            {
                var response = _repo.GetCreditRiskRatingPD(model.CreditRiskRatingPDId);
                var resplist = new List<CreditRiskRatingPDObj> { response };
                return new CreditRiskRatingPDObjRespObj
                {
                    CreditRiskRatingPD = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 91)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.ADD_CREDITRISK_RATING_PD)]
        public async Task<ActionResult<CreditRiskRatingPDObjRespObj>> AddUpdateCreditRiskRatingPD([FromBody] CreditRiskRatingPDObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = _repo.AddUpdateCreditRiskRatingPD(model);

                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 91)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.DELETE_CREDITRISK_RATING_PD)]
        public ActionResult<CreditRiskRatingPDObjRespObj> DeleteCreditRiskRatingPD([FromBody] DeleteCreditRiskScoreCommand command)
        {
            var response = false;
            var Ids = command.TargetIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCreditRiskRatingPD(Convert.ToInt32(id));
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

        [ERPActivity(Action = UserActions.View, Activity = 91)]
        [HttpGet(ApiRoutes.CreditRiskScoreCard.DOWNLOAD_CREDITRISK_RATING_PD)]
        public async Task<ActionResult<CreditRiskRatingPDObjRespObj>> GenerateExportCreditRiskRatingPD()
        {
            try
            {
                var response = _repo.GenerateExportCreditRiskRatingPD();

                return new CreditRiskRatingPDObjRespObj
                {
                    Export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 91)]
        [HttpPost(ApiRoutes.CreditRiskScoreCard.UPLOAD_CREDITRISK_RATING_PD)]
        public async Task<ActionResult<CreditRiskRatingPDObjRespObj>> UploadCreditRiskRatingPD()
        {
            try
            {
                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

                var byteList = new List<byte[]>();
                foreach (var fileBit in files)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteList.Add(ms.ToArray());
                        }
                    }

                }

                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;

                var isDone = _repo.UploadCreditRiskRatingPD(byteList, createdBy);
                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CreditRiskRatingPDObjRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion
    }
}