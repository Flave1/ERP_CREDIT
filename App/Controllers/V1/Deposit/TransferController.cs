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
    public class TransferController : Controller
    {
        private readonly ITransferService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;


        public TransferController(ITransferService repo, IIdentityService identityService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
        }

        #region TransferSetup
        [HttpGet(ApiRoutes.Transfer.GET_ALL_TRANSFER_SETUP)]
        public async Task<ActionResult<TransferSetupRespObj>> GetAllTransferSetupAsync()
        {
            try
            {
                var response = await _repo.GetAllTransferSetupAsync();
                return new TransferSetupRespObj
                {
                    TransferSetups = _mapper.Map<List<TransferSetupObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransferSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Transfer.GET_TRANSFER_SETUP_BY_ID)]
        public async Task<ActionResult<TransferSetupRespObj>> GetTransferSetupByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new TransferSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "TransferSetup Id is required" } }
                };
            }

            var response = await _repo.GetTransferSetupByIdAsync(search.SearchId);
            var resplist = new List<deposit_transfersetup> { response };
            return new TransferSetupRespObj
            {
                TransferSetups = _mapper.Map<List<TransferSetupObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.Transfer.DOWNLOAD_TRANSFER_SETUP)]
        public async Task<ActionResult<TransferSetupRespObj>> GenerateExportTransferSetup()
        {
            var response = _repo.GenerateExportTransferSetup();

            return new TransferSetupRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.Transfer.ADD_UPDATE_TRANSFER_SETUP)]
        public async Task<ActionResult<TransferSetupRegRespObj>> AddUpdateTransferSetupAsync([FromBody] AddUpdateTransferSetupObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_transfersetup item = null;
                if (model.TransferSetupId > 0)
                {
                    item = await _repo.GetTransferSetupByIdAsync(model.TransferSetupId);
                    if (item == null)
                        return new TransferSetupRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_transfersetup();

                domainObj.TransferSetupId = model.TransferSetupId > 0 ? model.TransferSetupId : 0;
                domainObj.Structure = model.Structure;
                domainObj.PresetChart = model.PresetChart;
                domainObj.AccountType = model.AccountType;
                domainObj.DailyWithdrawalLimit = model.DailyWithdrawalLimit;
                domainObj.ChargesApplicable = model.ChargesApplicable;
                domainObj.Charges = model.Charges;
                domainObj.ChargeType = model.ChargeType;
                domainObj.Active = true;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.CreatedBy = user.UserName;
                domainObj.Deleted = false;
                domainObj.UpdatedOn = model.TransferSetupId > 0 ? DateTime.Today : DateTime.Today;
                domainObj.UpdatedBy = user.UserName;

                var isDone = await _repo.AddUpdateTransferSetupAsync(domainObj);
                return new TransferSetupRegRespObj
                {
                    TransferSetupId = domainObj.TransferSetupId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new TransferSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Transfer.UPLOAD_TRANSFER_SETUP)]
        public async Task<ActionResult<TransferSetupRegRespObj>> UploadTransferSetupAsync()
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

                var isDone = await _repo.UploadTransferSetupAsync(byteList, createdBy);
                return new TransferSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new TransferSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Transfer.DELETE_TRANSFER_SETUP)]
        public async Task<IActionResult> DeleteTransferSetupAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteTransferSetupAsync(id);
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
        [HttpGet(ApiRoutes.Transfer.GET_ALL_TRANSFER)]
        public async Task<ActionResult<TransferFormRespObj>> GetAllTransferAsync()
        {
            try
            {
                var response = await _repo.GetAllTransferAsync();
                return new TransferFormRespObj
                {
                    TransferForms = _mapper.Map<List<TransferFormObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransferFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Transfer.GET_TRANSFER_BY_ID)]
        public async Task<ActionResult<TransferFormRespObj>> GetTransferByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new TransferFormRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "TransferForm Id is required" } }
                };
            }

            var response = await _repo.GetTransferByIdAsync(search.SearchId);
            var resplist = new List<deposit_transferform> { response };
            return new TransferFormRespObj
            {
                TransferForms = _mapper.Map<List<TransferFormObj>>(resplist),
            };

        }

        [HttpPost(ApiRoutes.Transfer.ADD_UPDATE_TRANSFER)]
        public async Task<ActionResult<TransferFormRegRespObj>> AddUpDateTransfer([FromBody] AddUpdateTransferFormObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_transferform item = null;
                if (model.TransferFormId > 0)
                {
                    item = await _repo.GetTransferByIdAsync(model.TransferFormId);
                    if (item == null)
                        return new TransferFormRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_transferform();

                domainObj.TransferFormId = model.TransferFormId > 0 ? model.TransferFormId : 0;
                domainObj.Structure = model.Structure;
                domainObj.Product = model.Product;
                domainObj.TransactionDate = model.TransactionDate;
                domainObj.ValueDate = model.ValueDate;
                domainObj.ExternalReference = model.ExternalReference;
                domainObj.TransactionReference = model.TransactionReference;
                domainObj.PayingAccountNumber = model.PayingAccountNumber;
                domainObj.PayingAccountName = model.PayingAccountName;
                domainObj.PayingAccountCurrency = model.PayingAccountCurrency;
                domainObj.Amount = model.Amount;
                domainObj.BeneficiaryAccountNumber = model.BeneficiaryAccountNumber;
                domainObj.BeneficiaryAccountName = model.BeneficiaryAccountName;
                domainObj.BeneficiaryAccountCurrency = model.BeneficiaryAccountCurrency;
                domainObj.TransactionNarration = model.TransactionNarration;
                domainObj.ExchangeRate = model.ExchangeRate;
                domainObj.TotalCharge = model.TotalCharge;
                domainObj.Active = true;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.CreatedBy = user.UserName;
                domainObj.Deleted = false;
                domainObj.UpdatedOn = model.TransferFormId > 0 ? DateTime.Today : DateTime.Today;
                domainObj.UpdatedBy = user.UserName;


                var isDone = await _repo.AddUpdateTransferAsync(domainObj);
                return new TransferFormRegRespObj
                {
                    TransferFormId = domainObj.TransferFormId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new TransferFormRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Transfer.DELETE_TRANSFER)]
        public async Task<IActionResult> DeleteTransferAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteTransferAsync(id);
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
