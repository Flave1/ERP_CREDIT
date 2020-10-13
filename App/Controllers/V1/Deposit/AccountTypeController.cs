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
    public class AccountTypeController : Controller
    {
        private readonly IDepositAccountypeService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;


        public AccountTypeController(IDepositAccountypeService repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
        }

        [HttpGet(ApiRoutes.AccountType.GET_ALL_ACCOUNT_TYPE)]
        
        public async Task<ActionResult<AccountTypeRespObj>> GetAllAccountTypeAsync()
        {
            try
            {
                var response = await _repo.GetAllAccountTypeAsync();
                return new AccountTypeRespObj
                {
                    AccountTypes = _mapper.Map<List<AccountTypeObj>>(response),
                }; 
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AccountTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.AccountType.GET_ACCOUNT_TYPE_BY_ID)]
        public async Task<ActionResult<AccountTypeRespObj>> GetAccountTypeByIdAsync([FromQuery] AccountTypeSearchObj search)
        {
            if(search.AccountTypeId < 1)
            {
                return new AccountTypeRespObj
                {
                    Status = new APIResponseStatus{ IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "AccountType Id is required" } }
                };
            }

            var response = await _repo.GetAccountTypeByIdAsync(search.AccountTypeId);
            var resplist = new List<deposit_accountype> { response };
            return new AccountTypeRespObj
            {
                AccountTypes = _mapper.Map<List<AccountTypeObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.AccountType.ADD_UPDATE_ACCOUNT_TYPE)]
        public async Task<ActionResult<AccountTypeRegRespObj>> AddUpDateAccountType([FromBody] AddUpdateAccountTypeObj model)
        {
            try 
            {
                var user = await _identityServer.UserDataAsync();
                deposit_accountype item = null;
                if (model.AccountTypeId > 0)
                {
                     item = await _repo.GetAccountTypeByIdAsync(model.AccountTypeId);
                    if (item ==null)
                        return new AccountTypeRegRespObj  
                        {  
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_accountype();
                domainObj.AccountTypeId = model.AccountTypeId > 0 ? model.AccountTypeId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.Description = model.Description;
                domainObj.Name = model.Name;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.AccountTypeId > 0 ? DateTime.Today : DateTime.Today;
                 
                var isDone = await _repo.AddUpdateAccountTypeAsync(domainObj);
                return new AccountTypeRegRespObj
                {
                    AccountTypeId = domainObj.AccountTypeId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AccountTypeRegRespObj 
                {
                    Status = new APIResponseStatus { IsSuccessful =  false, Message = new APIResponseMessage { FriendlyMessage =  "Error Occurred", TechnicalMessage =ex?.Message , MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.AccountType.DELETE_ACCOUNT_TYPE)]
        public async Task<IActionResult> DeleteAccountType([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteAccountTypeAsync(id);
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

        [HttpGet(ApiRoutes.AccountType.DOWNLOAD_ACCOUNT_TYPE)]
        public async Task<ActionResult<AccountTypeRespObj>> GenerateExportAccountType()
        {
            var response = _repo.GenerateExportAccountType();

            return new AccountTypeRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.AccountType.UPLOAD_ACCOUNT_TYPE)]
        public async Task<ActionResult<AccountTypeRegRespObj>> UploadAccountTypeAsync()
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

                var isDone = await _repo.UploadAccountTypeAsync(byteList, createdBy);
                return new AccountTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AccountTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

    }
}
