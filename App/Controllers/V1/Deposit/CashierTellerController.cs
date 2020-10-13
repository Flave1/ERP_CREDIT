using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Deposit;
using Banking.Contracts.V1;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Deposit;
using Banking.Requests;
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

namespace Banking.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class CashierTellerController : Controller
    {
        private readonly ICashierTellerService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;

        public CashierTellerController(
            ICashierTellerService repo, 
            IIdentityService identityService, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, ILoggerService logger,
            IIdentityServerRequest serverRequest)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _serverRequest = serverRequest;
        }
        #region CashierTellerSetup
        [HttpGet(ApiRoutes.CashierTeller.GET_ALL_CASHIERTELLERSETUP)]

        public async Task<ActionResult<CashierTellerSetupRespObj>> GetAllCashierTellerSetupAsync()
        {
            try
            {
                var response = await _repo.GetAllCashierTellerSetupAsync();
                return new CashierTellerSetupRespObj
                {
                    DepositCashierTellerSetups = _mapper.Map<List<CashierTellerSetupObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CashierTellerSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CashierTeller.GET_CASHIERTELLERSETUP_BY_ID)]
        public async Task<ActionResult<CashierTellerSetupRespObj>> GetCashierTellerSetupByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CashierTellerSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "CashierTeller Id is required" } }
                };
            }

            var response = await _repo.GetCashierTellerSetupByIdAsync(search.SearchId);
            var resplist = new List<deposit_cashiertellersetup> { response };
            return new CashierTellerSetupRespObj
            {
                DepositCashierTellerSetups = _mapper.Map<List<CashierTellerSetupObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.CashierTeller.DOWNLOAD_CASHIERTELLERSETUP)]
        public async Task<ActionResult<CashierTellerSetupRespObj>> GenerateExportCashierTellerSetup()
        {
            var response = _repo.GenerateExportCashierTellerSetup();

            return new CashierTellerSetupRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.CashierTeller.ADD_UPDATE_CASHIERTELLERSETUP)]
        public async Task<ActionResult<CashierTellerSetupRegRespObj>> AddUpDateCashierTellerSetup([FromBody] AddUpdateCashierTellerSetupObj model)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                deposit_cashiertellersetup item = null;
                if (model.DepositCashierTellerSetupId > 0)
                {
                    item = await _repo.GetCashierTellerSetupByIdAsync(model.DepositCashierTellerSetupId);
                    if (item == null)
                        return new CashierTellerSetupRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_cashiertellersetup();

                domainObj.DepositCashierTellerSetupId = model.DepositCashierTellerSetupId > 0 ? model.DepositCashierTellerSetupId : 0;
                domainObj.Structure = model.Structure;
                domainObj.ProductId = model.ProductId;
                domainObj.PresetChart = model.PresetChart;
                domainObj.Active = true;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.CreatedBy = user.UserName;
                domainObj.Deleted = false;
                domainObj.UpdatedOn = model.DepositCashierTellerSetupId > 0 ? DateTime.Today : DateTime.Today;
                domainObj.UpdatedBy = user.UserName;


                var isDone = await _repo.AddUpdateCashierTellerSetupAsync(domainObj);
                return new CashierTellerSetupRegRespObj
                {
                    DepositCashierTellerSetupId = domainObj.DepositCashierTellerSetupId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CashierTellerSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CashierTeller.UPLOAD_CASHIERTELLERSETUP)]
        public async Task<ActionResult<CashierTellerSetupRegRespObj>> UploadCashierTellerSetupAsync()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _serverRequest.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadCashierTellerSetupAsync(image, createdBy);
                return new CashierTellerSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CashierTellerSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CashierTeller.DELETE_CASHIERTELLERSETUP)]
        public async Task<IActionResult> DeleteCashierTellerSetup([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteCashierTellerSetupAsync(id);
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
