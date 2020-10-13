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
using Banking.Repository.Implement.Credit.Collateral;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.CollateralCustomerObjs;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;
using static Banking.Contracts.Response.Credit.LoanApplicationCollateralObjs;
using DeleteRespObj = Banking.Contracts.Response.Credit.CollateralCustomerObjs.DeleteRespObj;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class CollateralController : Controller
    {
        private readonly ICollateralRepository _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly ILoanApplicationCollateralDocumentRespository _loanApplicationCollateralDocumentRespository;
        private readonly ICollateralCustomerConsumptionRepository _collateralCustomerConsumptionRepository;
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly IAllowableCollateralRepository _allowableCollateralRepository;
        public CollateralController(ICollateralRepository repo, IMapper mapper, IIdentityService identityService, 
            IHttpContextAccessor httpContextAccessor, ILoggerService logger,
             ILoanApplicationCollateralDocumentRespository loanApplicationCollateralDocumentRespository,
            ICollateralCustomerConsumptionRepository collateralCustomerConsumptionRepository,
            ILoanApplicationRepository loanApplicationRepository,
            IAllowableCollateralRepository allowableCollateralRepository,
            IIdentityServerRequest serverRequest
            )
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _serverRequest = serverRequest;
            _logger = logger;
            _loanApplicationCollateralDocumentRespository = loanApplicationCollateralDocumentRespository;
            _collateralCustomerConsumptionRepository = collateralCustomerConsumptionRepository;
            _loanApplicationRepository = loanApplicationRepository;
            _allowableCollateralRepository = allowableCollateralRepository;
        }

        #region COLLATER_TYPE
        [ERPActivity(Action = UserActions.View, Activity = 95)]
        [HttpGet(ApiRoutes.Collateral.GET_ALL_COLLATERAL_TYPE)]
        public async Task<ActionResult<CollateralTypeRespObj>> GetAllCollateralType()
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                var response = _repo.GetAllCollateralType();
                return new CollateralTypeRespObj
                {
                    CollateralTypes = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 95)]
        [HttpPost(ApiRoutes.Collateral.ADD_COLLATERAL_TYPE)]
        public async Task<ActionResult<CollateralTypeRegRespObj>> AddUpdateCollateralType([FromBody] CollateralTypeObj entity)
        {
            try
            {
                CollateralTypeObj item = null;
                if (entity.CollateralTypeId > 0)
                {
                    item = _repo.GetCollateralType(entity.CollateralTypeId);
                    if (item == null)
                        return new CollateralTypeRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var isDone =  _repo.AddUpdateCollateralType(entity);
                return new CollateralTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 95)]
        [HttpGet(ApiRoutes.Collateral.DOWNLOAD_COLLATERAL_TYPE)]
        public async Task<ActionResult<CollateralTypeRespObj>> GenerateExportCollateralType()
        {
            try
            {
                var response = _repo.GenerateExportCollateralType();

                return new CollateralTypeRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 95)]
        [HttpPost(ApiRoutes.Collateral.UPLOAD_COLLATERAL_TYPE)]
        public async Task<ActionResult<CollateralTypeRegRespObj>> UploadCollateralType()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;

                var isDone = _repo.UploadCollateralType(image, createdBy);
                return new CollateralTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.View, Activity = 95)]
        [HttpGet(ApiRoutes.Collateral.GET_COLLATERAL_TYPE_BY_ID)]
        public async Task<ActionResult<CollateralTypeRespObj>> GetCollateralType([FromQuery] CollateralTypeSearchObj search)
        {
            try
            {
                if (search.CollateralTypeId < 1)
                {
                    return new CollateralTypeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "CollateralType Id is required" } }
                    };
                }

                var response = _repo.GetCollateralType(search.CollateralTypeId);
                var resplist = new List<CollateralTypeObj> { response };
                return new CollateralTypeRespObj
                {
                    CollateralTypes = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpGet(ApiRoutes.Collateral.GET_COLLATERAL_TYPE_BY__LOANAPPLICATION_ID)]
        public async Task<ActionResult<CollateralTypeRespObj>> GetAllowableCollateralByLoanApplicationId([FromQuery] CollateralTypeSearchObj search)
        {
            try
            {
                if (search.LoanApplicationId < 1)
                {
                    return new CollateralTypeRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "LoanApplication Id is required" } }
                    };
                }

                var response = await _repo.GetAllowableCollateralByLoanApplicationId(search.LoanApplicationId);
                //var resplist = new List<CollateralTypeObj> { response };
                return new CollateralTypeRespObj
                {
                    CollateralTypes = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 95)]
        [HttpPost(ApiRoutes.Collateral.DELETE_COLLATERAL_TYPE)]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteCollateralTypeCommand command)
        {
            var response = false;
            var Ids = command.CollateralTypeIds;
            foreach (var id in Ids)
            {
                response =  _repo.DeleteCollateralType(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful=false, Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus {IsSuccessful=true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
        }
        #endregion

        #region COLLATERAL_CUSTOMER
        [HttpGet(ApiRoutes.Collateral.GET_COLLATERAL_CUSTOMER)]
        public async Task<ActionResult<CollateralCustomerRespObj>> GetAllCollateralCustomer()
        {
            try
            {
                var response = _repo.GetAllCollateralCustomer();
                return new CollateralCustomerRespObj
                {
                    CollateralCustomers = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collateral.CUSTOMER_LOAN_COLLATERAL_CONSUMPTION)]
        public async Task<ActionResult<CollateralCustomerRespObj>> AddOrUpdateCollateralCustomerConsumptionAsync([FromBody] CollateralCustomerConsumptionObj model)
        {
            try
            {
                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;
                model.CreatedBy = createdBy;
                model.UpdatedBy = createdBy;
                var isDone = await _collateralCustomerConsumptionRepository.AddOrUpdateCollateralCustomerConsumptionAsync(model);
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful =  true, Message = new APIResponseMessage { FriendlyMessage = "Successful"  } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collateral.ADD_COLLATERAL_CUSTOMER)]
        public async Task<ActionResult<CollateralCustomerRegRespObj>> AddUpdateCustomerCollateral([FromBody] AddUpdateCollateralCustomerObj entity)
        {
            try
            {
                var isDone = false;
                CollateralCustomerObj item = null;
                if (entity.CollateralCustomerId > 0)
                {
                    item = _repo.GetCollateralCustomer(entity.CollateralCustomerId);
                    if (item == null)
                        return new CollateralCustomerRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                var domainObj = new credit_collateralcustomer();
                domainObj.CollateralCustomerId = entity.CollateralCustomerId > 0 ? entity.CollateralCustomerId : 0;
                domainObj.CustomerId = entity.CustomerId;
                domainObj.CollateralTypeId = entity.CollateralTypeId;
                domainObj.CurrencyId = entity.CurrencyId;
                domainObj.CollateralValue = entity.CollateralValue;
                domainObj.CollateralVerificationStatus = entity.CollateralVerificationStatus;
                domainObj.CollateralCode = "";
                domainObj.Location = entity.Location;
                domainObj.Active = true;
                domainObj.CreatedBy = user;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.UpdatedBy = user;
                domainObj.UpdatedOn = entity.CollateralTypeId > 0 ? DateTime.Today : DateTime.Today;

                var result = await _repo.AddUpdateCustomerCollateral(domainObj);
                if (result > 0)
                    isDone = true;
                return new CollateralCustomerRegRespObj
                {
                    CollateralCustomerId = result,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collateral.UPDATE_COLLATERAL_CUSTOMER)]
        public async Task<ActionResult<CollateralCustomerRegRespObj>> AddOrUpdateLoanApplicationCollateralDocument()
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;
                var file = _httpContextAccessor.HttpContext.Request.Form.Files["document"];
                var isDone = false;
                var collateralCustomerId = _httpContextAccessor.HttpContext.Request.Form.Files["collateralCustomerId"];
                var customerId = _httpContextAccessor.HttpContext.Request.Form["customerId"];
                var collateralTypeId = _httpContextAccessor.HttpContext.Request.Form["collateralTypeId"];
                var currencyId = _httpContextAccessor.HttpContext.Request.Form["currencyId"];
                var collateralValue = _httpContextAccessor.HttpContext.Request.Form["collateralValue"];
                var collateralVerificationStatus = _httpContextAccessor.HttpContext.Request.Form["collateralVerificationStatus"];
                var loanApplicationId = _httpContextAccessor.HttpContext.Request.Form["loanApplicationId"];
                var collateralCode = _httpContextAccessor.HttpContext.Request.Form["collateralCode"];
                var location = _httpContextAccessor.HttpContext.Request.Form["location"]; 


                 //var file = _httpContextAccessor.HttpContext.Request.Form.Files.Count > 0 ? _httpContextAccessor.HttpContext.Request.Form.Files[0] : null;               
                byte[] fileData = null;

                if (file != null && file.Length > 0)
                {
                    using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes(((int)file.Length));
                    }
                }
                    var model = new credit_collateralcustomer
                    {
                        CollateralCode = collateralCode.ToString(),
                        Location = location.ToString(),
                        CollateralTypeId = Convert.ToInt32(collateralTypeId),
                        CollateralCustomerId = Convert.ToInt32(collateralCustomerId),
                        CollateralValue = Convert.ToDecimal(collateralValue),
                        CollateralVerificationStatus = Convert.ToBoolean(collateralVerificationStatus),
                        CustomerId = Convert.ToInt32(customerId),
                        //LoanApplicationId = Convert.ToInt32(loanApplicationId),
                        CurrencyId = Convert.ToInt32(currencyId),
                        Active = true,
                        CreatedBy = user,
                        CreatedOn = DateTime.Today,
                        Deleted = false,
                        UpdatedBy = user,
                        UpdatedOn = DateTime.Today,
                    };
                    var response = await _repo.AddUpdateCustomerCollateral(model);

                    if (response > 0)
                    {
                        isDone = true;
                            var loanApplicationCollateralDocumentViewModel = new LoanApplicationCollateralDocumentObj
                            {
                                CollateralCustomerId = model.CollateralCustomerId,
                                CollateralTypeId = Convert.ToInt32(collateralTypeId),
                                Document = fileData,
                                LoanApplicationId = Convert.ToInt32(loanApplicationId),
                                DocumentName = file.FileName,
                            };

                            await _loanApplicationCollateralDocumentRespository.AddOrUpdateLoanApplicationCollateralDocumentAsync(loanApplicationCollateralDocumentViewModel);                       
                    }
                return new CollateralCustomerRegRespObj
                {
                    CollateralCustomerId = model.CollateralTypeId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };



            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.DOWNLOAD_COLLATERAL_CUSTOMER)]
        public async Task<ActionResult<CollateralCustomerRespObj>> GenerateExportCustomerCollateral()
        {
            try
            {
                var response = _repo.GenerateExportCustomerCollateral();

                return new CollateralCustomerRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collateral.UPLOAD_COLLATERAL_CUSTOMER)]
        public async Task<ActionResult<CollateralCustomerRegRespObj>> UploadCustomerCollateral()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var user = await _serverRequest.UserDataAsync();
                var createdBy = user.UserName;

                var isDone = _repo.UploadCustomerCollateral(image, createdBy);
                return new CollateralCustomerRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.GET_COLLATERAL_CUSTOMER_ID)]
        public async Task<ActionResult<CollateralCustomerRespObj>> GetCollateralCustomer([FromQuery] CollateralCustomerSearchObj search)
        {
            try
            {
                if (search.CollateralCustomerId < 1)
                {
                    return new CollateralCustomerRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "CollateralCustomer Id is required" } }
                    };
                }

                var response = _repo.GetCollateralCustomer(search.CollateralCustomerId);
                var resplist = new List<CollateralCustomerObj> { response };
                return new CollateralCustomerRespObj
                {
                    CollateralCustomers = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.REQUIRED_COLLATERAL_VALUE)]
        public async Task<ActionResult<CollateralCustomerRespObj>> GetLoanApplicationRequireAmount([FromQuery] CollateralCustomerSearchObj search)
        {
            try
            {
                if (search.LoanApplicationId < 1)
                {
                    return new CollateralCustomerRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "LoanApplication Id is required" } }
                    };
                }

                var response = _repo.GetLoanApplicationRequireAmount(search.LoanApplicationId);

                if (response != 0)
                {
                    return new CollateralCustomerRespObj
                    {
                        amount = response,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    };
                }
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.COLLATERAL_SINGLE_CUSTOMER)]
        public async Task<ActionResult<CollateralCustomerRespObj>> GetCollateralSingleCustomer([FromQuery] CollateralCustomerSearchObj search)
        {
            try
            {
                if (search.CustomerId < 1)
                {
                    return new CollateralCustomerRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer Id is required" } }
                    };
                }

                var response = _repo.GetCollateralSingleCustomer(search.CustomerId);

                return new CollateralCustomerRespObj
                {
                    CollateralCustomers = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.GET_COLLATERAL_CUSTOMER_BY_IDS)]
        public async Task<ActionResult<CollateralCustomerRespObj>> GetAllCustomerCollateralByLoanApplication([FromQuery] CollateralCustomerSearchObj search)
        {
            try
            {
                var response = _repo.GetAllCustomerCollateralByLoanApplication(search.LoanApplicationId, search.IncludeNotAllowSharing);

                //Check for collateral verification status
                if (search.VerificationStatus)
                {
                    response = response.Where(x => x.CollateralVerificationStatus == search.VerificationStatus);
                }

                return new CollateralCustomerRespObj
                {
                    CollateralCustomers = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.CURRENT_CUSTOMER_COLLATERAL_VALUE)]
        public async Task<ActionResult<CollateralCustomerRespObj>> CalculateCurrentCustomerCollateralValue([FromQuery] CollateralCustomerSearchObj search)
        {
            try
            {

                var customerCollateral = _repo.GetCollateralCustomer(search.CollateralCustomerId);
                var collateral = _repo.GetCollateralType(customerCollateral.CollateralTypeId);
                var loanApplication = _loanApplicationRepository.GetLoanApplication(search.LoanApplicationId);
                var allCollateralConsumptions = await _collateralCustomerConsumptionRepository
                    .GetCollateralCustomerConsumptionByCollateralCustomerIdAsync(search.CollateralCustomerId);
                decimal totalConsumptionOnCollateral = 0;

                foreach (var coll in allCollateralConsumptions)
                {
                    totalConsumptionOnCollateral += coll.Amount;
                }

                var collateralValueWithHairCutConsidered = customerCollateral.CollateralValue;

                if (collateral.HairCut != 0 && collateral.HairCut != null)
                {
                    collateralValueWithHairCutConsidered = (customerCollateral.CollateralValue * (100 - collateral.HairCut.Value)) / 100;
                }

                var response = new CollateralCustomerConsumptionObj
                {
                    //ActualCollateralValue = customerCollateral.collateralValue,
                    Amount = 0,
                    CollateralCurrentAmount = collateralValueWithHairCutConsidered - totalConsumptionOnCollateral,
                    CollateralCustomerId = customerCollateral.CollateralCustomerId,
                    CollateralCustomerConsumptionId = search.collateralCustomerConsumptionId
                };

                if (search.collateralCustomerConsumptionId != 0)
                {
                    var consumption = await _collateralCustomerConsumptionRepository.GetCollateralCustomerConsumptionById(search.collateralCustomerConsumptionId);
                    response.ActualCollateralValue = consumption.Amount;
                    response.CollateralCurrentAmount += consumption.Amount;
                }

                if (response != null)
                {
                    return new CollateralCustomerRespObj
                    {
                        CollateralCustomerConsumption = response,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    };
                }
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CollateralCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collateral.DELETE_COLLATERAL_CUSTOMER)]
        public async Task<IActionResult> DeleteCollateralCutomer([FromBody] DeleteCollateralCustomerCommand command)
        {
            var response = false;
            var Ids = command.CollateralCustomerIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteCollateralCutomer(id);
                if (response)
                {
                    await _repo.DeleteLoanApplicationCollateralDocumentAsync(id);
                }
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

        #region LOAN_APPLICATION_COLLATERAL
        [HttpPost(ApiRoutes.Collateral.ADD_LOAN_APPLICATION_COLLATERAL)]
        public async Task<ActionResult<LoanApplicationCollateralRespObj>> AddUpdateLoanApplicationCollateral([FromBody] LoanApplicationCollateralObj model)
        {
            try
            {
                var identity = await _serverRequest.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var isDone = _repo.AddUpdateLoanApplicationCollateral(model);
                if (isDone)
                {
                    return new LoanApplicationCollateralRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record saved Successfully" } }
                    };
                }
                return new LoanApplicationCollateralRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record not saved" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanApplicationCollateralRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.GET_ALL_LOAN_APPLICATION_COLLATERAL)]
        public async Task<ActionResult<LoanApplicationCollateralRespObj>> GetAllLoanApplicationCollateral()
        {
            try
            {
                var response = _repo.GetAllLoanApplicationCollateral();
                return new LoanApplicationCollateralRespObj
                {
                    LoanApplicationCollaterals = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanApplicationCollateralRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.GET_LOAN_APPLICATION_COLLATERAL_BY_LOANAPPLICATION_ID)]
        public async Task<ActionResult<LoanApplicationCollateralRespObj>> GetLoanApplicationCollateralForLoanApplicationId([FromQuery] CollateralTypeSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanApplicationCollateralForLoanApplicationId(model.LoanApplicationId);
                //var resList = new List<LoanApplicationCollateralObj> { response };
                return new LoanApplicationCollateralRespObj
                {
                    LoanApplicationCollaterals = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanApplicationCollateralRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Collateral.GET_ALL_COLLATERAL_MANAGEMENT)]
        public async Task<ActionResult<LoanApplicationCollateralRespObj>> GetCollateralManagementAsync()
        {
            try
            {
                var response = _loanApplicationRepository.GetCollateralManagementAsync();
                return new LoanApplicationCollateralRespObj
                {
                    CollateralManagement = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanApplicationCollateralRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Collateral.DELETE_LOAN_APPLICATION_COLLATERAL)]
        public async Task<IActionResult> DeleteLoanApplicationCollateral([FromBody] DeleteLoanApplicationCollateralCommand command)
        {
            var response = false;
            var Ids = command.LoanApplicationCollateralIds;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanApplicationCollateral(id);
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

        [HttpPost(ApiRoutes.Collateral.DELETE_LOAN_CUSTOMER_COLLATERAL)]
        public async Task<IActionResult> DeleteListCollateralCustomerConsumptionByCollateralCustomerConsumptionIdAsync([FromBody] DeleteLoanApplicationCollateralCommand command)
        {
            var response = false;
            var Ids = command.collateralCustomerConsumptionIds;
            foreach (var id in Ids)
            {
                 await _collateralCustomerConsumptionRepository.DeleteListCollateralCustomerConsumptionByCollateralCustomerConsumptionIdAsync(id);
                response = true;
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