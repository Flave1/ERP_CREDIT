using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Deposit;
using Banking.Contracts.Response.InvestorFund;
using Banking.Contracts.V1;
using Banking.DomainObjects.InvestorFund;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.InvestorFund;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers.V1.InvestorsFund
{
    [ERPAuthorize]
    public class InvestorFundProductController : Controller
    {

        private readonly IProductService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        public InvestorFundProductController(
            IProductService repo, 
            IMapper mapper, 
            IIdentityService identityService,
            IHttpContextAccessor httpContextAccessor, 
            ILoggerService logger,
            IIdentityServerRequest serverRequest)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _serverRequest = serverRequest;
        }

        #region Product
        [HttpGet(ApiRoutes.Product.GET_ALL_PRODUCT)]
        public async Task<ActionResult<InfProductRespObj>> GetAllProduct()
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var response = _repo.GetAllProduct();
                return new InfProductRespObj
                {
                    InfProducts = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Product.ADD_UPDATE_PRODUCT)]
        public async Task<ActionResult<InfProductRegRespObj>> AddUpdateProduct([FromBody] AddUpdateInfProductObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                InfProductObj item = null;
                if (entity.ProductId > 0)
                {
                    item = _repo.GetProduct(entity.ProductId);
                    if (item == null)
                        return new InfProductRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new inf_product();
                domainObj.ProductId = entity.ProductId > 0 ? entity.ProductId : 0;
                domainObj.ProductCode = entity.ProductCode;
                domainObj.ProductName = entity.ProductName;
                domainObj.Rate = entity.Rate;
                domainObj.ProductTypeId = entity.ProductTypeId;
                domainObj.ProductLimit = entity.ProductLimit;
                domainObj.ProductLimitId = entity.ProductLimitId;
                domainObj.InterestRateMax = entity.InterestRateMax;
                domainObj.InterestRepaymentTypeId = entity.InterestRepaymentTypeId;
                domainObj.ScheduleMethodId = entity.ScheduleMethodId;
                domainObj.FrequencyId = entity.FrequencyId;
                domainObj.MaximumPeriod = entity.MaximumPeriod;
                domainObj.InterestRateAnnual = entity.InterestRateAnnual;
                domainObj.InterestRateFrequency = entity.InterestRateFrequency;
                domainObj.ProductPrincipalGl = entity.ProductPrincipalGl;
                domainObj.ReceiverPrincipalGl = entity.ReceiverPrincipalGl;
                domainObj.InterstExpenseGl = entity.InterstExpenseGl;
                domainObj.InterestPayableGl = entity.InterestPayableGl;
                domainObj.EarlyTerminationCharge = entity.EarlyTerminationCharge;
                domainObj.Active = true;
                domainObj.Createdby = user.UserName;
                domainObj.Createdon = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.Updatedby = user.UserName;
                domainObj.Updatedon = entity.ProductId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = _repo.AddUpdateProduct(domainObj);
                return new InfProductRegRespObj
                {
                    ProductId = domainObj.ProductId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Product.DOWNLOAD_PRODUCT)]
        public async Task<ActionResult<InfProductRespObj>> GenerateExportProduct()
        {
            try
            {
                var response = _repo.GenerateExportProduct();

                return new InfProductRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Product.UPLOAD_PRODUCT)]
        public async Task<ActionResult<InfProductRegRespObj>> UploadProduct()
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

                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;

                return _repo.UploadProduct(byteList, createdBy);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Product.GET_PRODUCT_BY_ID)]
        public async Task<ActionResult<InfProductRespObj>> GetProduct([FromQuery] SearchObj search)
        {
            try
            {
                if (search.SearchId < 1)
                {
                    return new InfProductRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Product Id is required" } }
                    };
                }

                var response = _repo.GetProduct(search.SearchId);
                var resplist = new List<InfProductObj> { response };
                return new InfProductRespObj
                {
                    InfProducts = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Product.DELETE_PRODUCT)]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteProduct(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObjt
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region ProductType
        [HttpGet(ApiRoutes.ProductType.GET_ALL_PRODUCTTYPE)]
        public async Task<ActionResult<InfProductTypeRespObj>> GetAllProductType()
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var response = _repo.GetAllProductType();
                return new InfProductTypeRespObj
                {
                    InfProductTypes = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.ProductType.ADD_UPDATE_PRODUCTTYPE)]
        public async Task<ActionResult<InfProductTypeRegRespObj>> AddUpdateProductType([FromBody] AddUpdateInfProductTypeObj entity)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                InfProductTypeObj item = null;
                if (entity.ProductTypeId > 0)
                {
                    item = _repo.GetProductType(entity.ProductTypeId);
                    if (item == null)
                        return new InfProductTypeRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new inf_producttype();
                domainObj.ProductTypeId = entity.ProductTypeId > 0 ? entity.ProductTypeId : 0;
                domainObj.Name = entity.Name;
                domainObj.Description = entity.Description;
                domainObj.Active = true;
                domainObj.Createdby = user.UserName;
                domainObj.Createdon = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.Updatedby = user.UserName;
                domainObj.Updatedon = entity.ProductTypeId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = _repo.AddUpdateProductType(domainObj);
                return new InfProductTypeRegRespObj
                {
                    ProductTypeId = domainObj.ProductTypeId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ProductType.DOWNLOAD_PRODUCTTYPE)]
        public async Task<ActionResult<InfProductTypeRespObj>> GenerateExportProductType()
        {
            try
            {
                var response = _repo.GenerateExportProductType();

                return new InfProductTypeRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.ProductType.UPLOAD_PRODUCTTYPE)]
        public async Task<ActionResult<InfProductTypeRegRespObj>> UploadProductType()
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

                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;

                return _repo.UploadProductType(byteList, createdBy);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ProductType.GET_PRODUCTTYPE_BY_ID)]
        public async Task<ActionResult<InfProductTypeRespObj>> GetProductType([FromQuery] SearchObj search)
        {
            try
            {
                if (search.SearchId < 1)
                {
                    return new InfProductTypeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "CollateralType Id is required" } }
                    };
                }

                var response = _repo.GetProductType(search.SearchId);
                var resplist = new List<InfProductTypeObj> { response };
                return new InfProductTypeRespObj
                {
                    InfProductTypes = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new InfProductTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.ProductType.DELETE_PRODUCTTYPE)]
        public async Task<IActionResult> DeleteProductType([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteProductType(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
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