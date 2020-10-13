using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Credit;
using Banking.Contracts.V1;
using Banking.Data;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.CollateralCustomerObjs;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class IFRSController : Controller
    {
        private readonly IIFRSRepository _repo;
        private readonly IIdentityService _identityService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly IIdentityServerRequest _identityServer;

         
        #region IFRS_SETUP_DATA

        public IFRSController(IIFRSRepository repo, IIdentityService identityService, ILoggerService logger, IHttpContextAccessor httpContextAccessor, DataContext context, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _identityServer = identityServer;
        }

        [HttpGet(ApiRoutes.Ifrs.GET_ALL_SETUP_DATA)]
        public async Task<ActionResult<IFRSRespObj>> GetAllIFRSSetupData()
        {
            try
            {
                var response = _repo.GetAllIFRSSetupData();
                return new IFRSRespObj
                {
                    SetUpData = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.GET_SETUP_DATA_ID)]
        public async Task<ActionResult<IFRSRespObj>> GetIFRSSetupData([FromQuery] IFRSSearchObj model)
        {
            try
            {
                var response = _repo.GetIFRSSetupData(model.SetUpId);
                var respList = new List<IFRSSetupDataObj> { response };
                return new IFRSRespObj
                {
                    SetUpData = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        [HttpPost(ApiRoutes.Ifrs.ADD_SETUP_DATA)]
        public async Task<ActionResult<IFRSRespObj>> AddUpdateIFRSSetupData([FromBody] IFRSSetupDataObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response =  _repo.AddUpdateIFRSSetupData(model);
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Record saved Successfully" : "Unsuccessful" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Ifrs.DELETE_SETUP_DATA)]
        public ActionResult<IFRSRespObj> DeleteIFRSSetupData([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteIFRSSetupData(id);
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

        #region IFRS_SCENARIO_SETUP

        [HttpGet(ApiRoutes.Ifrs.GET_ALL_SCENARIO_SETUP)]
        public async Task<ActionResult<IFRSRespObj>> GetAllIfrsScenarioSetup()
        {
            try
            {
                var response = _repo.GetAllIfrsScenarioSetup();
                return new IFRSRespObj
                {
                    IfrsScenarioSetup = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.GET_SCENARIO_SETUP_ID)]
        public async Task<ActionResult<IFRSRespObj>> GetIfrsScenarioSetup([FromQuery] IFRSSearchObj model)
        {
            try
            {
                var response = _repo.GetIfrsScenarioSetup(model.ScenarioId);
                var respList = new List<IfrsScenarioSetupObj> { response };
                return new IFRSRespObj
                {
                    IfrsScenarioSetup = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        [HttpPost(ApiRoutes.Ifrs.ADD_SCENARIO_SETUP)]
        public async Task<ActionResult<IFRSRespObj>> UpdateIfrsScenarioSetup([FromBody] IfrsScenarioSetupObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = _repo.UpdateIfrsScenarioSetup(model);
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Record saved Successfully" : "Unsuccessful" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Ifrs.DELETE_SCENARIO_SETUP)]
        public ActionResult<IFRSRespObj> DeleteIfrsScenarioSetup([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteIfrsScenarioSetup(id);
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

        #region IFRS_MACRO_ECONOMIC_VARIABLE

        [HttpGet(ApiRoutes.Ifrs.GET_ALL_MACRO_VARIABLE)]
        public async Task<ActionResult<IFRSRespObj>> GetAllMacroEconomicVariable()
        {
            try
            {
                var response = _repo.GetAllMacroEconomicVariable();
                return new IFRSRespObj
                {
                    MacroVariables = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.GET_MACRO_VARIABLE_ID)]
        public async Task<ActionResult<IFRSRespObj>> GetMacroEconomicVariable([FromQuery] IFRSSearchObj model)
        {
            try
            {
                var response = _repo.GetMacroEconomicVariable(model.MacroEconomicVariableId);
                var respList = new List<MacroEconomicVariableObj> { response };
                return new IFRSRespObj
                {
                    MacroVariables = respList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        [HttpPost(ApiRoutes.Ifrs.ADD_MACRO_VARIABLE)]
        public async Task<ActionResult<IFRSRespObj>> AddUpdateMacroEconomicVariable([FromBody] MacroEconomicVariableObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = _repo.AddUpdateMacroEconomicVariable(model);
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Record saved Successfully" : "Unsuccessful" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Ifrs.DELETE_MACRO_VARIABLE)]
        public ActionResult<IFRSRespObj> DeleteMacroEconomicVariable([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteMacroEconomicVariable(id);
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

        [HttpGet(ApiRoutes.Ifrs.DOWNLOAD_MACRO_VARIABLE)]
        public async Task<ActionResult<IFRSRespObj>> GenerateExportMacroEconomicViriable()
        {
            try
            {
                var response = _repo.GenerateExportMacroEconomicViriable();

                return new IFRSRespObj
                {
                    Export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Ifrs.UPLOAD_MACRO_VARIABLE)]
        public async Task<ActionResult<IFRSRespObj>> UploadCreditRiskAttribute()
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

                var isDone = _repo.UploadMacroEconomicViriable(byteList, createdBy);
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region IFRS_SCORE_CARD

        [HttpGet(ApiRoutes.Ifrs.GET_ALL_SCORE_CARD)]
        public async Task<ActionResult<IFRSRespObj>> GetAllScoreCardHistory()
        {
            try
            {
                var response = _repo.GetAllScoreCardHistory();
                return new IFRSRespObj
                {
                    ScoreCardHistory = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Ifrs.DELETE_SCORE_CARD)]
        public ActionResult<IFRSRespObj> DeleteScoreCardHistory([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteScoreCardHistory(id);
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


        [HttpPost(ApiRoutes.Ifrs.UPLOAD_SCORE_CARD)]
        public async Task<ActionResult<IFRSRespObj>> UploadScoreCardHistory()
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

                var response = _repo.UploadScoreCardHistory(byteList, createdBy);

                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Record saved Successfully" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = ex.ToString(), TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.DOWNLOAD_SCORE_CARD)]
        public async Task<ActionResult<IFRSRespObj>> GenerateExportScoreCardHistory()
        {
            try
            {
                var isDone = _repo.GenerateExportScoreCardHistory();
                return new IFRSRespObj
                {
                    Export = isDone,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.DOWNLOAD_HISTORICAL_PD)]
        public async Task<ActionResult<IFRSRespObj>> GenerateExportScoreCardHistoryByProduct([FromQuery] IFRSSearchObj model)
        {
            try
            {
                var isDone = _repo.GenerateExportScoreCardHistoryByProduct(model.ProductId, model.CustomerTypeId);
                return new IFRSRespObj
                {
                    Export = isDone,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region IFRS_LGD

        [HttpGet(ApiRoutes.Ifrs.GET_ALL_LGD)]
        public async Task<ActionResult<IFRSRespObj>> GetAllLoanLGDHistory()
        {
            try
            {
                var response = _repo.GetAllLoanLGDHistory();
                return new IFRSRespObj
                {
                    LGDHistory = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Ifrs.DELETE_LGD)]
        public ActionResult<IFRSRespObj> DeleteLGDHistory([FromBody] DeleteCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLGDHistory(id);
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


        [HttpPost(ApiRoutes.Ifrs.UPLOAD_LGD)]
        public async Task<ActionResult<IFRSRespObj>> UploadLGDHistory()
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

                var response = _repo.UploadLGDHistory(byteList, createdBy);

                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Record saved Successfully" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.DOWNLOAD_LGD)]
        public async Task<ActionResult<IFRSRespObj>> GenerateExportLGDHistory()
        {
            try
            {
                var isDone = _repo.GenerateExportLGDHistory();
                return new IFRSRespObj
                {
                    Export = isDone,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region IMPAIRMENT
        [HttpPost(ApiRoutes.Ifrs.RUN_IMPAIRMENT)]
        public async Task<ActionResult<IFRSRespObj>> RunImpairment()
        {
            try
            {
                var response = _repo.RunImpairment();
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = response ? true : false, Message = new APIResponseMessage { FriendlyMessage = response ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.GET_IMPAIRMENT)]
        public async Task<ActionResult<IFRSRespObj>> GetImpairmentFromSP()
        {
            try
            {
                var response = _repo.GetImpairmentFromSP();
                return new IFRSRespObj
                {
                    Impairment = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Ifrs.DOWNLOAD_IMPAIRMENT)]
        public async Task<ActionResult<IFRSRespObj>> GenerateExportImpairment()
        {
            try
            {
                var response = _repo.GenerateExportImpairment();
                return new IFRSRespObj
                {
                    Export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new IFRSRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion
    }
}