using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.V1;
using Banking.DomainObjects.Credit;
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
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;
using static Banking.Contracts.Response.Credit.ProductObjs;

namespace Banking.Controllers.V1.Credit
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProduct _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IAllowableCollateralRepository _allowableCollateralRepository;
        private readonly IIdentityServerRequest _identityServer;
         
        #region PRODUCT

        public ProductController(IProduct repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IAllowableCollateralRepository allowableCollateralRepository, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _allowableCollateralRepository = allowableCollateralRepository;
            _identityServer = identityServer;
        }

        [HttpGet(ApiRoutes.CreditProducts.GET_ALL_PRODUCT)]
        public async Task<ActionResult<ProductRespObj>> GetAllProduct()
        {
            try
            {
                var response = _repo.GetAllProduct();
                return new ProductRespObj
                {
                    Products = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 88)]
        [HttpPost(ApiRoutes.CreditProducts.ADD_PRODUCT)]
        public async Task<ActionResult<ProductRegRespObj>> AddUpdateProductAsync([FromBody] ProductObj entity)
        {
            try
            {
                var isDone = false;
                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;
                entity.CreatedBy = createdBy;
                entity.UpdatedBy = createdBy;

                var product =  _repo.AddUpdateProduct(entity);

                if (product != null)
                {
                    isDone = true;
                    var allAllowableCollaterals = await _allowableCollateralRepository.GetAllowableCollateralByProductIdAsync(product.ProductId);

                    if (allAllowableCollaterals != null)
                    {
                        await _allowableCollateralRepository.DeleteListAllowableCollateralByProductIdAsync(product.ProductId);
                    }
                    var listOfAallowableCollateral = new List<AllowableCollateralObj>();
                    if (entity.AllowableCollaterals != null)
                    {
                        foreach (var id in entity.AllowableCollaterals)
                        {
                            listOfAallowableCollateral.Add(new AllowableCollateralObj { CollateralTypeId = id, ProductId = product.ProductId });
                        }

                        await _allowableCollateralRepository.AddAllowableCollateral(listOfAallowableCollateral);
                    }                 
                }
                return new ProductRegRespObj
                {
                    Products = product,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 88)]
        [HttpGet(ApiRoutes.CreditProducts.DOWNLOAD_PRODUCT)]
        public async Task<ActionResult<ProductRespObj>> GenerateExportProduct()
        {
            try
            {
                var response = _repo.GenerateExportProduct();

                return new ProductRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 88)]
        [HttpPost(ApiRoutes.CreditProducts.UPLOAD_PRODUCT)]
        public async Task<ActionResult<ProductRegRespObj>> UploadProduct()
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

                return _repo.UploadProduct(byteList, createdBy);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 88)]
        [HttpGet(ApiRoutes.CreditProducts.GET_PRODUCT_BY_ID)]
        public async Task<ActionResult<ProductRespObj>> GetProductById([FromQuery] ProductSearchObj search)
        {
            try
            {
                if (search.ProductId < 1)
                {
                    return new ProductRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Product Id is required" } }
                    };
                }

                var response = _repo.GetProduct(search.ProductId);
                var allowableCollaterals = await _allowableCollateralRepository.GetAllowableCollateralByProductIdAsync(search.ProductId);
                var collateralTypeIds = new List<int>();

                foreach (var item in allowableCollaterals)
                {
                    collateralTypeIds.Add(item.CollateralTypeId);
                }

                response.AllowableCollaterals = collateralTypeIds.ToArray();

                var resplist = new List<ProductObj> { response };
                return new ProductRespObj
                {
                    Products = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 88)]
        [HttpPost(ApiRoutes.CreditProducts.DELETE_PRODUCT)]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommand command)
        {
            var response = false;
            var Ids = command.ProductIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteProduct(id);
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


        #region PRODUCT_TYPE
        [HttpGet(ApiRoutes.CreditProducts.GET_PRODUCT_TYPE)]
        public async Task<ActionResult<ProductTypeRespObj>> GetAllProductTypeAsync()
        {
            try
            {
                var response = await _repo.GetAllProductTypeAsync();
                return new ProductTypeRespObj
                {
                    ProductType = _mapper.Map<List<ProductTypeObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 87)]
        [HttpPost(ApiRoutes.CreditProducts.ADD_PRODUCT_TYPE)]
        public async Task<ActionResult<ProductTypeRegRespObj>> AddUpdateProductAsync([FromBody] ProductTypeObj entity)
        {
            try
            {
                credit_producttype item = null;
                if (entity.ProductTypeId > 0)
                {
                    item = await _repo.GetProductTypeByIdAsync(entity.ProductTypeId);
                    if (item == null)
                        return new ProductTypeRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var isDone =  _repo.AddUpdateProductType(entity);
                return new ProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 87)]
        [HttpGet(ApiRoutes.CreditProducts.DOWNLOAD_PRODUCT_TYPE)]
        public async Task<ActionResult<ProductRespObj>> GenerateExportProductType()
        {
            try
            {
                var response = _repo.GenerateExportProductType();

                return new ProductRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 87)]
        [HttpPost(ApiRoutes.CreditProducts.UPLOAD_PRODUCT_TYPE)]
        public async Task<ActionResult<ProductTypeRegRespObj>> UploadProductTypeAsync()
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

                var isDone = await _repo.UploadProductTypeAsync(byteList, createdBy);
                return new ProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 87)]
        [HttpGet(ApiRoutes.CreditProducts.GET_PRODUCT_TYPE_ID)]
        public async Task<ActionResult<ProductTypeRespObj>> GetProductTypeByIdAsync([FromQuery] ProductTypeSearchObj search)
        {
            try
            {
                if (search.ProductTypeId < 1)
                {
                    return new ProductTypeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "ProductType Id is required" } }
                    };
                }

                var response = await _repo.GetProductTypeByIdAsync(search.ProductTypeId);
                var resplist = new List<credit_producttype> { response };
                return new ProductTypeRespObj
                {
                    ProductType = _mapper.Map<List<ProductTypeObj>>(resplist),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 87)]
        [HttpPost(ApiRoutes.CreditProducts.DELETE_PRODUCT_TYPE)]
        public async Task<IActionResult> DeleteProductTypeAsync([FromBody] DeleteProductTypeCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteProductTypeAsync(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion


        #region PRODUCT_FEE
        [HttpGet(ApiRoutes.CreditProducts.GET_ALL_PRODUCT_FEE)]
        public async Task<ActionResult<ProductFeeRespObj>> GetAllProductFee()
        {
            try
            {
                var response = _repo.GetAllProductFee();
                var resList = new List<ProductFeeObj>(response);
                return new ProductFeeRespObj
                {
                    ProductFee = resList,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductFeeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditProducts.ADD_PRODUCT_FEE)]
        public async Task<ActionResult<ProductFeeRegRespObj>> AddUpdateProductAsync([FromBody] AddUpdateProductFeeObj entity)
        {
            try
            {
                ProductFeeObj item = null;
                if (entity.ProductFeeId > 0)
                {
                    item =  _repo.GetProductFee(entity.ProductFeeId);
                    if (item == null)
                        return new ProductFeeRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new credit_productfee();
                domainObj.ProductFeeId = entity.ProductFeeId > 0 ? entity.ProductFeeId : 0;
                domainObj.ProductFeeType = entity.ProductFeeType;
                domainObj.FeeId = entity.FeeId;
                domainObj.ProductPaymentType = entity.ProductPaymentType;
                domainObj.ProductAmount = entity.ProductAmount;
                domainObj.ProductId = entity.ProductId;
                domainObj.Active = true;
                domainObj.CreatedBy = string.Empty;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.UpdatedBy = string.Empty;
                domainObj.UpdatedOn = entity.ProductFeeId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.UpdateProductFee(domainObj);
                return new ProductFeeRegRespObj
                {
                    ProductFeeId = domainObj.ProductFeeId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductFeeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditProducts.DOWNLOAD_PRODUCT_FEE)]
        public async Task<ActionResult<ProductFeeRespObj>> GenerateExportProductFee(int ProductId)
        {
            try
            {
                var response = _repo.GenerateExportProductFee(ProductId);

                return new ProductFeeRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductFeeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditProducts.UPLOAD_PRODUCT_FEE)]
        public async Task<ActionResult<ProductFeeRegRespObj>> UploadProductFee()
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

                return await _repo.UploadProductFee(byteList, createdBy);
              
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductFeeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditProducts.GET_PRODUCT_FEE_BY_ID)]
        public async Task<ActionResult<ProductFeeRespObj>> GetProductFee([FromQuery] ProductFeeSearchObj search)
        {
            try
            {
                if (search.ProductFeeId < 1)
                {
                    return new ProductFeeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "ProductFee Id is required" } }
                    };
                }

                var response = _repo.GetProductFee(search.ProductFeeId);
                var resplist = new List<ProductFeeObj> { response };
                return new ProductFeeRespObj
                {
                    ProductFee = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductFeeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditProducts.GET_PRODUCT_FEE_BY_PRODUCT_ID)]
        public async Task<ActionResult<ProductFeeRespObj>> GetProductFeeByProduct([FromQuery] ProductFeeSearchObj search)
        {
            try
            {
                if (search.ProductId < 1)
                {
                    return new ProductFeeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Product Id is required" } }
                    };
                }

                var response = _repo.GetProductFeeByProduct(search.ProductId);
                //var resplist = new List<ProductFeeObj> { response };
                return new ProductFeeRespObj
                {
                    ProductFee = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductFeeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditProducts.GET_PRODUCT_FEE_BY_LOANAPPLICATION_ID)]
        public async Task<ActionResult<ProductFeeRespObj>> GetProductFeeByLoanApplicationId([FromQuery] ProductFeeSearchObj search)
        {
            try
            {
                if (search.LoanApplicationId < 1)
                {
                    return new ProductFeeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "ProductFee Id is required" } }
                    };
                }

                var response = _repo.GetProductFeeByLoanApplicationId(search.LoanApplicationId);
                //var resplist = new List<ProductFeeObj> { response };
                return new ProductFeeRespObj
                {
                    ProductFee = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new ProductFeeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.CreditProducts.DELETE_PRODUCT_FEE)]
        public async Task<IActionResult> DeleteProductTypeAsync([FromBody] DeleteProductFeeCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteProductFeeAsync(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion
    }
}