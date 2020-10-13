using Banking.Contracts.Response.Deposit;
using Banking.Contracts.V1; 
using Banking.Repository.Interface.Deposit;
using AutoMapper;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GOSLibraries;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Banking.AuthHandler.Interface;
using Banking.Requests;
using Microsoft.AspNetCore.Http;
using GOSLibraries.GOS_Error_logger.Service;
using Banking.Handlers.Auths;

namespace Banking.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class AccountSetupController : Controller
    {
        private readonly IAccountSetupService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;

        public AccountSetupController(IAccountSetupService repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
        }

        #region DEPOSIT_ACCOUNT_SETUP
        [HttpGet(ApiRoutes.AccountSetup.GET_ALL_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRespObj>> GetAllAccountSetupAsync()
        {
            try
            {
                var response = await _repo.GetAllAccountSetupAsync();
                return new AccountSetupRespObj
                {
                    DepositAccounts = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AccountSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.AccountSetup.GET_ACCOUNTSETUP_BY_ID)]
        public async Task<ActionResult<AccountSetupRespObj>> GetAccountSetupByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new AccountSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "AccountSetup Id is required" } }
                };
            }

            var response = await _repo.GetAccountSetupByIdAsync(search.SearchId);
            var resplist = new List<DepositAccountObj> { response };
            return new AccountSetupRespObj
            {
                DepositAccounts = resplist,
            };
            
        }

        [HttpPost(ApiRoutes.AccountSetup.ADD_UPDATE_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRegRespObj>> AddUpDateAccountSetup([FromBody] AddUpdateAccountSetupObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                DepositAccountObj item = null;
                if (model.DepositAccountId > 0)
                {
                    item = await _repo.GetAccountSetupByIdAsync(model.DepositAccountId);
                    if (item == null)
                        return new AccountSetupRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_accountsetup();

                domainObj.DepositAccountId = model.DepositAccountId > 0 ? model.DepositAccountId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.Description = model.Description;
                domainObj.AccountName = model.AccountName;
                domainObj.AccountTypeId = model.AccountTypeId;
                domainObj.DormancyDays = model.DormancyDays;
                domainObj.InitialDeposit = model.InitialDeposit;
                domainObj.CategoryId = model.CategoryId;
                domainObj.BusinessCategoryId = model.BusinessCategoryId;
                domainObj.GLMapping = model.GLMapping;
                domainObj.CurrencyId = model.CurrencyId;
                domainObj.BankGl = model.BankGl;
                domainObj.InterestRate = model.InterestRate;
                domainObj.InterestType = model.InterestType;
                domainObj.CheckCollecting = model.CheckCollecting;
                domainObj.MaturityType = model.MaturityType;
                domainObj.PreTerminationLiquidationCharge = model.PreTerminationLiquidationCharge;
                domainObj.InterestAccrual = model.InterestAccrual;
                domainObj.Status = model.Status;
                domainObj.OperatedByAnother = model.OperatedByAnother;
                domainObj.CanNominateBenefactor = model.CanNominateBenefactor;
                domainObj.UsePresetChartofAccount = model.UsePresetChartofAccount;
                domainObj.TransactionPrefix = model.TransactionPrefix;
                domainObj.CancelPrefix = model.CancelPrefix;
                domainObj.RefundPrefix = model.RefundPrefix;
                domainObj.Useworkflow = model.Useworkflow;
                domainObj.CanPlaceOnLien = model.CanPlaceOnLien;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.DepositAccountId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateAccountSetupAsync(domainObj);
                return new AccountSetupRegRespObj
                {
                    DepositAccountId = domainObj.DepositAccountId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AccountSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.AccountSetup.DELETE_ACCOUNTSETUP)]

        public async Task<IActionResult> DeleteAccountSetup([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteAccountSetupAsync(id);
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

        [HttpGet(ApiRoutes.AccountSetup.DOWNLOAD_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRespObj>> GenerateExportAccountSetup()
        {
            var response = _repo.GenerateExportAccountSetup();

            return new AccountSetupRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.AccountSetup.UPLOAD_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRegRespObj>> UploadAccountSetupAsync()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _identityServer.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadAccountSetupAsync(image, createdBy);
                return new AccountSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AccountSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region DEPOSIT_FORM
        [HttpGet(ApiRoutes.AccountSetup.GET_ALL_DEPOSITFORM)]
        public async Task<ActionResult<DepositFormRespObj>> GetAllDepositForm()
        {
            try
            {
                var response = _repo.GetAllDepositForm();
                return new DepositFormRespObj
                {
                    DepositForm = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new DepositFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.AccountSetup.GET_DEPOSITFORM_BY_ID)]
        public async Task<ActionResult<DepositFormRespObj>> GetDepositForm([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new DepositFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Depositform Id is required" } }
                };
            }

            var response = _repo.GetDepositForm(search.SearchId);
            var resplist = new List<DepositformObj> { response };
            return new DepositFormRespObj
            {
                DepositForm = resplist,
            };

        }

        [HttpPost(ApiRoutes.AccountSetup.ADD_UPDATE_DEPOSITFORM)]
        public async Task<ActionResult<DepositFormRespObj>> AddUpdateDepositForm([FromBody] DepositformObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                model.CreatedBy = user.UserName;
                model.UpdatedBy = user.UserName;

                var isDone =  _repo.AddUpdateDepositForm(model);
                return new DepositFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DepositFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.AccountSetup.DELETE_DEPOSITFORM)]

        public async Task<IActionResult> DeleteDepositionForm([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteDepositionForm(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful= false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                new DeleteRespObjt
                {
                    Deleted = true,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                });

        }
        #endregion
    }
}
