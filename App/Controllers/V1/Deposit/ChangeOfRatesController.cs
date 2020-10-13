using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Deposit;
using Banking.Contracts.V1;
using Banking.Repository.Interface.Deposit;
using GODP.Entities.Models;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Banking.Requests;
using Banking.Handlers.Auths;

namespace Banking.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class ChangeOfRatesController : Controller
    {
        private readonly IChangeOfRatesService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;


        public ChangeOfRatesController(IChangeOfRatesService repo, IIdentityService identityService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
        }

        #region ChangeOfRatesSetup
        [HttpGet(ApiRoutes.ChangeOfRates.GET_ALL_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRespObj>> GetAllChangeOfRatesSetupAsync()
        {
            try
            {
                var response = await _repo.GetAllChangeOfRatesSetupAsync();
                return new ChangeOfRateSetupRespObj
                {
                    ChangeOfRateSetups = _mapper.Map<List<ChangeOfRateSetupObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new ChangeOfRateSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ChangeOfRates.GET_CHANGEOFRATES_SETUP_BY_ID)]
        public async Task<ActionResult<ChangeOfRateSetupRespObj>> GetChangeOfRatesSetupByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new ChangeOfRateSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "ChangeOfRateSetup Id is required" } }
                };
            }

            var response = await _repo.GetChangeOfRatesSetupByIdAsync(search.SearchId);
            var resplist = new List<deposit_changeofratesetup> { response };
            return new ChangeOfRateSetupRespObj
            {
                ChangeOfRateSetups = _mapper.Map<List<ChangeOfRateSetupObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.ChangeOfRates.DOWNLOAD_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRespObj>> GenerateExportChangeOfRatesSetup()
        {
            var response = _repo.GenerateExportChangeOfRatesSetup();

            return new ChangeOfRateSetupRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.ChangeOfRates.ADD_UPDATE_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRegRespObj>> AddUpdateChangeOfRatesSetupAsync([FromBody] AddUpdateChangeOfRateSetupObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_changeofratesetup item = null;
                if (model.ChangeOfRateSetupId > 0)
                {
                    item = await _repo.GetChangeOfRatesSetupByIdAsync(model.ChangeOfRateSetupId);
                    if (item == null)
                        return new ChangeOfRateSetupRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_changeofratesetup();

                domainObj.ChangeOfRateSetupId = model.ChangeOfRateSetupId > 0 ? model.ChangeOfRateSetupId : 0;
                domainObj.Structure = model.Structure;
                domainObj.ProductId = model.ProductId;
                domainObj.CanApply = model.CanApply;
                domainObj.Active = true;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.CreatedBy = user.UserName;
                domainObj.Deleted = false;
                domainObj.UpdatedOn = model.ChangeOfRateSetupId > 0 ? DateTime.Today : DateTime.Today;
                domainObj.UpdatedBy = user.UserName;

                var isDone = await _repo.AddUpdateChangeOfRatesSetupAsync(domainObj);
                return new ChangeOfRateSetupRegRespObj
                {
                    ChangeOfRateSetupId = domainObj.ChangeOfRateSetupId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ChangeOfRateSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.ChangeOfRates.UPLOAD_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRegRespObj>> UploadChangeOfRatesSetupAsync()
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

                var isDone = await _repo.UploadChangeOfRatesSetupAsync(byteList, createdBy);
                return new ChangeOfRateSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ChangeOfRateSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.ChangeOfRates.DELETE_CHANGEOFRATES_SETUP)]
        public async Task<IActionResult> DeleteChangeOfRatesSetupAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteChangeOfRatesSetupAsync(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObjt
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region TransferForm
        [HttpGet(ApiRoutes.ChangeOfRates.GET_ALL_CHANGEOFRATES)]
        public async Task<ActionResult<ChangeOfRatesRespObj>> GetAllChangeOfRatesAsync()
        {
            try
            {
                var response = await _repo.GetAllChangeOfRatesAsync();
                return new ChangeOfRatesRespObj
                {
                    ChangeOfRates = _mapper.Map<List<ChangeOfRatesObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new ChangeOfRatesRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ChangeOfRates.GET_CHANGEOFRATES_BY_ID)]
        public async Task<ActionResult<ChangeOfRatesRespObj>> GetChangeOfRatesByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new ChangeOfRatesRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "ChangeOfRates Id is required" } }
                };
            }

            var response = await _repo.GetChangeOfRatesByIdAsync(search.SearchId);
            var resplist = new List<deposit_changeofrates> { response };
            return new ChangeOfRatesRespObj
            {
                ChangeOfRates = _mapper.Map<List<ChangeOfRatesObj>>(resplist),
            };

        }

        [HttpPost(ApiRoutes.ChangeOfRates.ADD_UPDATE_CHANGEOFRATES)]
        public async Task<ActionResult<ChangeOfRatesRegRespObj>> AddUpDateChangeOfRates([FromBody] AddUpdateChangeOfRatesObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_changeofrates item = null;
                if (model.ChangeOfRateId > 0)
                {
                    item = await _repo.GetChangeOfRatesByIdAsync(model.ChangeOfRateId);
                    if (item == null)
                        return new ChangeOfRatesRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_changeofrates();

                domainObj.ChangeOfRateId = model.ChangeOfRateId > 0 ? model.ChangeOfRateId : 0;
                domainObj.Structure = model.Structure;
                domainObj.Product = model.Product;
                domainObj.CurrentRate = model.CurrentRate;
                domainObj.ProposedRate = model.ProposedRate;
                domainObj.Reasons = model.Reasons;
                domainObj.ApproverName = model.ApproverName;
                domainObj.ApproverComment = model.ApproverComment;
                domainObj.Active = true;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.CreatedBy = user.UserName;
                domainObj.Deleted = false;
                domainObj.UpdatedOn = model.ChangeOfRateId > 0 ? DateTime.Today : DateTime.Today;
                domainObj.UpdatedBy = user.UserName;


                var isDone = await _repo.AddUpdateChangeOfRatesAsync(domainObj);
                return new ChangeOfRatesRegRespObj
                {
                    ChangeOfRateId = domainObj.ChangeOfRateId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ChangeOfRatesRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.ChangeOfRates.DELETE_CHANGEOFRATES)]
        public async Task<IActionResult> DeleteChangeOfRatesAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteChangeOfRatesAsync(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObjt
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion
    }


}
