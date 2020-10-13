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
    public class TransactionTaxController : Controller
    {
        private readonly ITransactionTaxService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        public TransactionTaxController(ITransactionTaxService transactionTaxService, IIdentityServerRequest serverRequest, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger)
        {
            _mapper = mapper;
            _serverRequest = serverRequest;
            _repo = transactionTaxService;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.TransactionTax.GET_ALL_TRANSACTIONTAX)]

        public async Task<ActionResult<TransactionTaxRespObj>> GetAllTransactionTaxAsync()
        {
            try
            {
                var response = await _repo.GetAllTransactionTaxAsync();
                return new TransactionTaxRespObj
                {
                    TransactionTaxes = _mapper.Map<List<TransactionTaxObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransactionTaxRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.TransactionTax.GET_TRANSACTIONTAX_BY_ID)]
        public async Task<ActionResult<TransactionTaxRespObj>> GetTransactionTaxByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new TransactionTaxRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "TransactionCharge Id is required" } }
                };
            }

            var response = await _repo.GetTransactionTaxByIdAsync(search.SearchId);
            var resplist = new List<deposit_transactiontax> { response };
            return new TransactionTaxRespObj
            {
                TransactionTaxes = _mapper.Map<List<TransactionTaxObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.TransactionTax.ADD_UPDATE_TRANSACTIONTAX)]
        public async Task<ActionResult<TransactionTaxRegRespObj>> AddUpDateTransactionTax([FromBody] AddUpdateTransactionTaxObj model)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                deposit_transactiontax item = null;
                if (model.TransactionTaxId > 0)
                {
                    item = await _repo.GetTransactionTaxByIdAsync(model.TransactionTaxId);
                    if (item == null)
                        return new TransactionTaxRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_transactiontax();

                domainObj.TransactionTaxId = model.TransactionTaxId > 0 ? model.TransactionTaxId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.Description = model.Description;
                domainObj.Name = model.Name;
                domainObj.FixedOrPercentage = model.FixedOrPercentage;
                domainObj.Amount_Percentage = model.Amount_Percentage;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.TransactionTaxId > 0 ? DateTime.Today : DateTime.Today;
                

                var isDone = await _repo.AddUpdateTransactionTaxAsync(domainObj);
                return new TransactionTaxRegRespObj
                {
                    TransactionTaxId = domainObj.TransactionTaxId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new TransactionTaxRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.TransactionTax.DELETE_TRANSACTIONTAX)]

        public async Task<IActionResult> DeleteTransactionTax([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteTransactionTaxAsync(id);
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

        [HttpGet(ApiRoutes.TransactionTax.DOWNLOAD_TRANSACTIONTAX)]
        public async Task<ActionResult<TransactionTaxRespObj>> GenerateExportTransactionTax()
        {
            var response = _repo.GenerateExportTransactionTax();

            return new TransactionTaxRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.TransactionTax.UPLOAD_TRANSACTIONTAX)]
        public async Task<ActionResult<TransactionTaxRegRespObj>> UploadTransactionTaxAsync()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _serverRequest.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadTransactionTaxAsync(image, createdBy);
                return new TransactionTaxRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransactionTaxRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

    }
}
