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
using Microsoft.AspNetCore.Http;
using GOSLibraries.GOS_Error_logger.Service;
using Banking.Requests;
using Banking.Handlers.Auths;

namespace Banking.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class TransactionChargeController : Controller
    {
        private readonly ITransactionChargeService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        public TransactionChargeController(
            ITransactionChargeService transactionChargeervice, 
            IMapper mapper, 
            IIdentityService identityService, 
            IHttpContextAccessor httpContextAccessor, 
            ILoggerService logger,
            IIdentityServerRequest serverRequest)
        {
            _mapper = mapper;
            _serverRequest = serverRequest;
            _repo = transactionChargeervice;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.TransactionCharge.GET_ALL_TRANSACTIONCHARGE)]

        public async Task<ActionResult<TransactionChargeRespObj>> GetAllTransactionChargeAsync()
        {
            try
            {
                var response = await _repo.GetAllTransactionChargeAsync();
                return new TransactionChargeRespObj
                {
                    TransactionCharges = _mapper.Map<List<TransactionChargeObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransactionChargeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.TransactionCharge.GET_TRANSACTIONCHARGE_BY_ID)]
        public async Task<ActionResult<TransactionChargeRespObj>> GetTransactionChargeByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new TransactionChargeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "TransactionCharge Id is required" } }
                };
            }

            var response = await _repo.GetTransactionChargeByIdAsync(search.SearchId);
            var resplist = new List<deposit_transactioncharge> { response };
            return new TransactionChargeRespObj
            {
                TransactionCharges = _mapper.Map<List<TransactionChargeObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.TransactionCharge.ADD_UPDATE_TRANSACTIONCHARGE)]
        public async Task<ActionResult<TransactionChargeRegRespObj>> AddUpDateTransactionCharge([FromBody] AddUpdateTransactionChargeObj model)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                deposit_transactioncharge item = null;
                if (model.TransactionChargeId > 0)
                {
                    item = await _repo.GetTransactionChargeByIdAsync(model.TransactionChargeId);
                    if (item == null)
                        return new TransactionChargeRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_transactioncharge();

                domainObj.TransactionChargeId = model.TransactionChargeId > 0 ? model.TransactionChargeId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.Description = model.Description;
                domainObj.FixedOrPercentage = model.FixedOrPercentage;
                domainObj.Amount_Percentage = model.Amount_Percentage;
                domainObj.Name = model.Name;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.TransactionChargeId > 0 ? DateTime.Today : DateTime.Today;
                

                var isDone = await _repo.AddUpdateTransactionChargeAsync(domainObj);
                return new TransactionChargeRegRespObj
                {
                    TransactionChargeId = domainObj.TransactionChargeId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new TransactionChargeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.TransactionCharge.DELETE_TRANSACTIONCHARGE)]

        public async Task<IActionResult> DeleteTransactionCharge([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteTransactionChargeAsync(id);
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

        [HttpGet(ApiRoutes.TransactionCharge.DOWNLOAD_TRANSACTIONCHARGE)]
        public async Task<ActionResult<TransactionChargeRespObj>> GenerateExportTransactionCharge()
        {
            var response = _repo.GenerateExportTransactionCharge();

            return new TransactionChargeRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.TransactionCharge.UPLOAD_TRANSACTIONCHARGE)]
        public async Task<ActionResult<TransactionChargeRegRespObj>> UploadTransactionChargeAsync()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _serverRequest.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadTransactionChargeAsync(image, createdBy);
                return new TransactionChargeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransactionChargeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

    }
}
