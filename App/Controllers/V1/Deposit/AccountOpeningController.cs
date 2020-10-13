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
using System.Net.Http;
using System.Net;
using Banking.Data;
using Banking.Requests;
using Banking.Handlers.Auths;

namespace Banking.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class AccountOpeningController : Controller
    {
        private readonly IAccountOpeningService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _identityServer;


        public AccountOpeningController(IAccountOpeningService repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, DataContext dataContext, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dataContext = dataContext;
            _identityServer = identityServer;
        }

        #region CustomerDetails

        [HttpGet(ApiRoutes.Customer.GET_ALL_CUSTOMER)]
        public async Task<ActionResult<AccountOpeningRespObj>> GetAllCustomerLite()
        {
            try
            {
                var response =  _repo.GetAllCustomerLite();
                return new AccountOpeningRespObj
                {
                    CustomerLite = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AccountOpeningRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Customer.GET_ALL_CUSTOMER_CASA)]
        public async Task<ActionResult<AccountOpeningRespObj>> GetAllCustomerCasaList()
        {
            try
            {
                var response = _repo.GetAllCustomerCasaList();
                return new AccountOpeningRespObj
                {
                    CustomerLite = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AccountOpeningRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Customer.GET_CUSTOMER_BY_ID)]
        public async Task<ActionResult<AccountOpeningRespObj>> GetCustomerDetails([FromQuery] SearchObj search)
        {
            if (search.SearchId < 0)
            {
                return new AccountOpeningRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "CustomerId is required" } }
                };
            }

            var response =  _repo.GetCustomerDetails(search.CustomerId);
            var resplist = new List<DepositAccountOpeningObj> { response };
            return new AccountOpeningRespObj
            {
                AccountOpenings = resplist,
            };
        }


        [HttpPost(ApiRoutes.Customer.ADD_UPDATE_CUSTOMER)]
        public async Task<ActionResult<AccountOpeningRespObj>> AddUpdateCustomerAsync([FromBody] DepositAccountOpeningObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;

                var response = await _repo.AddUpdateCustomerAsync(model);
                return new AccountOpeningRespObj
                {
                    ResponseId = response,
                    Status = new APIResponseStatus { IsSuccessful = response > 0 ? true : false, Message = new APIResponseMessage { FriendlyMessage = response > 0 ? "Successful" : "Unsuccesssful" } }
                };

            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AccountOpeningRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Customer.DELETE_CUSTOMER)]
        public async Task<IActionResult> DeleteCustomerAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteCustomerAsync(id);
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

        #region DocumentUpload
        [HttpGet(ApiRoutes.DocumentUpload.GET_ALL_DOCUMENTUPLOAD)]
        public async Task<ActionResult<KyCustomerDocUploadRespObj>> GetAllKYCustomerDocAsync()
        {
            try
            {
                var response = await _repo.GetAllKYCustomerDocAsync();
                return new KyCustomerDocUploadRespObj
                {
                    KyCustomerDocUploads = _mapper.Map<List<KyCustomerDocUploadObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new KyCustomerDocUploadRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.DocumentUpload.GET_DOCUMENTUPLOAD_BY_ID)]
        public async Task<ActionResult<KyCustomerDocUploadRespObj>> GetKYCustomerDocByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new KyCustomerDocUploadRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "DocumentUpload Id is required" } }
                };
            }

            var response = await _repo.GetKYCustomerDocByIdAsync(search.SearchId);
            var resplist = new List<deposit_customerkycdocumentupload> { response };
            return new KyCustomerDocUploadRespObj
            {
                KyCustomerDocUploads = _mapper.Map<List<KyCustomerDocUploadObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.DocumentUpload.ADD_UPDATE_DOCUMENTUPLOAD)]
        public async Task<ActionResult<KyCustomerDocUploadRegRespObj>> AddUpdateKYCustomerDocAsync([FromBody] AddUpdateKyCustomerDocUploadObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_customerkycdocumentupload item = null;
                if (model.DocumentId > 0)
                {
                    item = await _repo.GetKYCustomerDocByIdAsync(model.DocumentId);
                    if (item == null)
                        return new KyCustomerDocUploadRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_customerkycdocumentupload();
                domainObj.DocumentId = model.DocumentId > 0 ? model.DocumentId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.CustomerId = model.CustomerId;
                domainObj.KycId = model.KycId;
                domainObj.DocumentName = model.DocumentName;
                domainObj.DocumentUpload = model.DocumentUpload;
                domainObj.PhysicalLocation = model.PhysicalLocation;
                domainObj.FileExtension = model.FileExtension;
                domainObj.DocumentType = model.DocumentType;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.DocumentId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateKYCustomerDocAsync(domainObj);
                return new KyCustomerDocUploadRegRespObj
                {
                    DocumentId = domainObj.DocumentId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new KyCustomerDocUploadRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.DocumentUpload.DELETE_DOCUMENTUPLOAD)]
        public async Task<IActionResult> DeleteKYCustomerDocAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteKYCustomerDocAsync(id);
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

        #region Signature Upload
        [HttpGet(ApiRoutes.SignatureUpload.GET_ALL_SIGNATUREUPLOAD)]
        public async Task<ActionResult<CustomerSignatureRespObj>> GetAllSignatureAsync()
        {
            try
            {
                var response = await _repo.GetAllSignatureAsync();
                return new CustomerSignatureRespObj
                {
                    CustomerSignatures = _mapper.Map<List<CustomerSignatureObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerSignatureRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.SignatureUpload.GET_SIGNATUREUPLOAD_BY_ID)]
        public async Task<ActionResult<CustomerSignatureRespObj>> GetSignatureByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CustomerSignatureRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Signature Id is required" } }
                };
            }

            var response = await _repo.GetSignatureByIdAsync(search.SearchId);
            var resplist = new List<deposit_customersignature> { response };
            return new CustomerSignatureRespObj
            {
                CustomerSignatures = _mapper.Map<List<CustomerSignatureObj>>(resplist),
            };
        }

        [HttpGet(ApiRoutes.SignatureUpload.GET_SIGNATUREUPLOAD_BY_IDS)]
        public async Task<ActionResult<CustomerSignatureRespObj>> GetSignatureByIdsAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CustomerSignatureRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Signature Id is required" } }
                };
            }

            var response = await _repo.GetSignaturesByIdsAsync(search.SearchId, search.SearchId);
            var resplist = new List<deposit_customersignature> { response };
            return new CustomerSignatureRespObj
            {
                CustomerSignatures = _mapper.Map<List<CustomerSignatureObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.SignatureUpload.ADD_UPDATE_SIGNATUREUPLOAD)]
        public async Task<ActionResult<CustomerSignatureRegRespObj>> AddUpdateSignatureAsync([FromBody] AddUpdateCustomerSignatureObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_customersignature item = null;
                if (model.SignatureId > 0)
                {
                    item = await _repo.GetSignatureByIdAsync(model.SignatureId);
                    if (item == null)
                        return new CustomerSignatureRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_customersignature();
                domainObj.SignatureId = model.SignatureId > 0 ? model.SignatureId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.CustomerId = model.CustomerId;
                domainObj.Name = model.Name;
                domainObj.SignatureImg = model.SignatureImg;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.SignatureId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateSignatureAsync(domainObj);
                return new CustomerSignatureRegRespObj
                {
                    SignatureId = domainObj.SignatureId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CustomerSignatureRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.SignatureUpload.DELETE_SIGNATUREUPLOAD)]
        public async Task<IActionResult> DeleteSignatureAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteSignatureAsync(id);
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

        #region Signatory

        [HttpPost(ApiRoutes.Signatory.ADD_UPDATE_SIGNATORY)]
        public async Task<ActionResult<CustomerSignatoryRegRespObj>> AddUpdateSignatoryAsync([FromBody] AddUpdateCustomerSignatoryObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_customersignatory item = null;
                if (model.SignatoryId > 0)
                {
                    item = await _repo.GetSignatoryByIdAsync(model.SignatoryId);
                    if (item == null)
                        return new CustomerSignatoryRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_customersignatory();
                domainObj.SignatoryId = model.SignatoryId > 0 ? model.SignatoryId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.CustomerId = model.CustomerId;
                domainObj.Surname = model.Surname;
                domainObj.Firstname = model.Firstname;
                domainObj.Othername = model.Othername;
                domainObj.TitleId = model.TitleId;
                domainObj.MaritalStatusId = model.MaritalStatusId;
                domainObj.GenderId = model.GenderId;
                domainObj.Mobile = model.Mobile;
                domainObj.Email = model.Email;
                domainObj.ClassofSignatory = model.ClassofSignatory;
                domainObj.IdentificationType = model.IdentificationType;
                domainObj.Telephone = model.Telephone;
                domainObj.SignatureUpload = model.SignatureUpload;
                domainObj.Date = model.Date;
                domainObj.DoB = model.DoB;
                domainObj.PlaceOfBirth = model.PlaceOfBirth;
                domainObj.MaidenName = model.MaidenName;
                domainObj.NextofKin = model.NextofKin;
                domainObj.LGA = model.LGA;
                domainObj.StateOfOrigin = model.StateOfOrigin;
                domainObj.TaxIDNumber = model.TaxIDNumber;
                domainObj.MeansOfID = model.MeansOfID;
                domainObj.IDExpiryDate = model.IDExpiryDate;
                domainObj.IDIssueDate = model.IDIssueDate;
                domainObj.Occupation = model.Occupation;
                domainObj.JobTitle = model.JobTitle;
                domainObj.Position = model.Position;
                domainObj.Nationality = model.Nationality;
                domainObj.ResidentPermit = model.ResidentPermit;
                domainObj.PermitIssueDate = model.PermitIssueDate;
                domainObj.PermitExpiryDate = model.PermitExpiryDate;
                domainObj.SocialSecurityNumber = model.SocialSecurityNumber;
                domainObj.Address1 = model.Address1;
                domainObj.City1 = model.City1;
                domainObj.State1 = model.State1;
                domainObj.Country1 = model.Country1;
                domainObj.Address2 = model.Address2;
                domainObj.City2 = model.City2;
                domainObj.State2 = model.State2;
                domainObj.Country2 = model.Country2;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.SignatoryId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateSignatoryAsync(domainObj);
                return new CustomerSignatoryRegRespObj
                {
                    SignatoryId = domainObj.SignatoryId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CustomerSignatoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        #endregion

        #region Signatory SignatureUpload
        [HttpGet(ApiRoutes.Signatory.GET_ALL_SIGNATORY)]
        public async Task<ActionResult<CustomerSignatoryRespObj>> GetAllSignatoryAsync(int cid)
        {
            try
            {
                var response = await _repo.GetAllSignatoryAsync(cid);
                return new CustomerSignatoryRespObj
                {
                    CustomerSignatories = _mapper.Map<List<CustomerSignatoryObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerSignatoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Signatory.GET_SIGNATORY_BY_ID)]
        public async Task<ActionResult<CustomerSignatoryRespObj>> GetSignatoryByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CustomerSignatoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "CustomerSignatory Id is required" } }
                };
            }

            var response = await _repo.GetSignatoryByIdAsync(search.SearchId);
            var resplist = new List<deposit_customersignatory> { response };
            return new CustomerSignatoryRespObj
            {
                CustomerSignatories = _mapper.Map<List<CustomerSignatoryObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.Signatory.UPLOAD_SIGNATORY)]
        /*public async Task<ActionResult<CustomerSignatureRegRespObj>>  SignatoryUpload()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _identityService.UserDataAsync().Result.UserName;

                var isDone =  _repo.SignatoryUpload(image, createdBy);
                return new CustomerSignatureRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerSignatureRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }*/

        [HttpPost(ApiRoutes.Signatory.DELETE_SIGNATORY)]
        public async Task<IActionResult> DeleteSignatoryAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteSignatoryAsync(id);
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

        #region Directors

        [HttpPost(ApiRoutes.Directors.ADD_UPDATE_DIRECTORS)]
        /*  public async Task<ActionResult<AccountTypeRegRespObj>> AddUpdateDirector([FromBody] AddUpdateAccountTypeObj model)
          {
              try
              {
                  var user = await _identityService.UserDataAsync();
                  deposit_accountype item = null;
                  if (model.AccountTypeId > 0)
                  {
                      item = await _repo.GetDirectorByIdAsync(model.AccountTypeId);
                      if (item == null)
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
                      Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                  };
              }
          }*/

        #endregion

        #region Director SignatureUpload
        [HttpGet(ApiRoutes.Directors.GET_ALL_DIRECTORS)]
        public async Task<ActionResult<CustomerDirectorsRespObj>> GetAllDirectorsAsync(int cid)
        {
            try
            {
                var response = await _repo.GetAllDirectorsAsync(cid);
                return new CustomerDirectorsRespObj
                {
                    CustomerDirectors = _mapper.Map<List<CustomerDirectorsObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerDirectorsRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.Directors.GET_DIRECTORS_BY_ID)]
        public async Task<ActionResult<CustomerDirectorsRespObj>> GetDirectorByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CustomerDirectorsRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "CustomerDirectors Id is required" } }
                };
            }

            var response = await _repo.GetDirectorByIdAsync(search.SearchId);
            var resplist = new List<deposit_customerdirectors> { response };
            return new CustomerDirectorsRespObj
            {
                CustomerDirectors = _mapper.Map<List<CustomerDirectorsObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.Directors.DELETE_DIRECTORS)]
        public async Task<IActionResult> DeleteDirectorsAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteDirectorsAsync(id);
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

        [HttpPost(ApiRoutes.Directors.UPLOAD_DIRECTORS)]
        /*public async Task<ActionResult<CustomerDirectorsRegRespObj>> DirectorsignatureUpload()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _identityService.UserDataAsync().Result.UserName;

                //var isDone = await _repo.DirectorsignatureUpload(image, createdBy);
                return new CustomerDirectorsRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerDirectorsRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        } */
        #endregion

        #region KYC
        [HttpGet(ApiRoutes.KYCustomer.GET_ALL_KYCUSTOMER)]
        public async Task<ActionResult<KyCustomersRespObj>> GetAllKYCustomerAsync(int cid)
        {
            try
            {
                var response = await _repo.GetAllKYCustomerAsync(cid);
                return new KyCustomersRespObj
                {
                    KyCustomers = _mapper.Map<List<KyCustomerObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new KyCustomersRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.KYCustomer.GET_KYCUSTOMER_BY_ID)]
        public async Task<ActionResult<KyCustomersRespObj>> GetKYCustomerByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new KyCustomersRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "KyCustomers Id is required" } }
                };
            }

            var response = await _repo.GetKYCustomerByIdAsync(search.SearchId);
            var resplist = new List<deposit_customerkyc> { response };
            return new KyCustomersRespObj
            {
                KyCustomers = _mapper.Map<List<KyCustomerObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.KYCustomer.ADD_UPDATE_KYCUSTOMER)]
        public async Task<ActionResult<KyCustomersRegRespObj>> AddUpdateKYCustomerAsync([FromBody] AddUpdateKyCustomersObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_customerkyc item = null;
                if (model.KycId > 0)
                {
                    item = await _repo.GetKYCustomerByIdAsync(model.KycId);
                    if (item == null)
                        return new KyCustomersRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_customerkyc();
                domainObj.KycId = model.KycId > 0 ? model.KycId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.CustomerId = model.CustomerId;
                domainObj.Financiallydisadvantaged = model.Financiallydisadvantaged;
                domainObj.Bankpolicydocuments = model.Bankpolicydocuments;
                domainObj.TieredKycrequirement = model.TieredKycrequirement;
                domainObj.RiskCategoryId = model.RiskCategoryId;
                domainObj.PoliticallyExposedPerson = model.PoliticallyExposedPerson;
                domainObj.Details = model.Details;
                domainObj.AddressVisited = model.AddressVisited;
                domainObj.CommentOnLocation = model.CommentOnLocation;
                domainObj.LocationColor = model.LocationColor;
                domainObj.LocationDescription = model.LocationDescription;
                domainObj.NameOfVisitingStaff = model.NameOfVisitingStaff;
                domainObj.DateOfVisitation = model.DateOfVisitation;
                domainObj.UtilityBillSubmitted = model.UtilityBillSubmitted;
                domainObj.AccountOpeningCompleted = model.AccountOpeningCompleted;
                domainObj.RecentPassportPhoto = model.RecentPassportPhoto;
                domainObj.ConfirmationName = model.ConfirmationName;
                domainObj.ConfirmationDate = model.ConfirmationDate;
                domainObj.DeferralFullName = model.DeferralFullName;
                domainObj.DeferralDate = model.DeferralDate;
                domainObj.DeferralApproved = model.DeferralApproved;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.KycId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateKYCustomerAsync(domainObj);
                return new KyCustomersRegRespObj
                {
                    KycId = domainObj.KycId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new KyCustomersRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.KYCustomer.DELETE_KYCUSTOMER)]
        public async Task<IActionResult> DeleteKYCustomerAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteKYCustomerAsync(id);
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

        #region Contact Persons
        [HttpGet(ApiRoutes.ContactPersons.GET_ALL_CONTACTPERSONS)]
        public async Task<ActionResult<CustomerContactPersonsRespObj>> GetContactPersonsAsync(int cid)
        {
            try
            {
                var response = await _repo.GetContactPersonsAsync(cid);
                return new CustomerContactPersonsRespObj
                {
                    ContactPersons = _mapper.Map<List<CustomerContactPersonsObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerContactPersonsRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.ContactPersons.GET_CONTACTPERSONS_BY_ID)]
        public async Task<ActionResult<CustomerContactPersonsRespObj>> GetContactPersonsByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CustomerContactPersonsRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Contact Id is required" } }
                };
            }

            var response = await _repo.GetContactPersonsByIdAsync(search.SearchId);
            var resplist = new List<deposit_customercontactpersons> { response };
            return new CustomerContactPersonsRespObj
            {
                ContactPersons = _mapper.Map<List<CustomerContactPersonsObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.ContactPersons.ADD_UPDATE_CONTACTPERSONS)]
        public async Task<ActionResult<CustomerContactPersonsRegRespObj>> AddUpdateContactPersonsAsync([FromBody] AddUpdateCustomerContactPersonsObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_customercontactpersons item = null;
                if (model.ContactPersonId > 0)
                {
                    item = await _repo.GetContactPersonsByIdAsync(model.ContactPersonId);
                    if (item == null)
                        return new CustomerContactPersonsRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_customercontactpersons();
                domainObj.ContactPersonId = model.ContactPersonId > 0 ? model.ContactPersonId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.Title = model.Title;
                domainObj.FirstName = model.FirstName;
                domainObj.OtherName = model.OtherName;
                domainObj.SurName = model.SurName;
                domainObj.Relationship = model.Relationship;
                domainObj.GenderId = model.GenderId;
                domainObj.MobileNumber = model.MobileNumber;
                domainObj.Email = model.Email;
                domainObj.Address = model.Address;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.ContactPersonId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateContactPersonsAsync(domainObj);
                return new CustomerContactPersonsRegRespObj
                {
                    ContactPersonId = domainObj.ContactPersonId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CustomerContactPersonsRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.ContactPersons.DELETE_CONTACTPERSONS)]
        public async Task<IActionResult> DeleteContactPersonsAsync([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteContactPersonsAsync(id);
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

        #region NextOfKin
        [HttpGet(ApiRoutes.NextOfKin.GET_ALL_NEXTOFKIN)]
        public async Task<ActionResult<CustomerNextOfKinRespObj>> GetAllNextOfKinAsync(int cid)
        {
            try
            {
                var response = await _repo.GetAllNextOfKinAsync(cid);
                return new CustomerNextOfKinRespObj
                {
                    customerNextOfKins = _mapper.Map<List<CustomerNextOfKinObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerNextOfKinRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.NextOfKin.GET_NEXTOFKIN_BY_ID)]
        public async Task<ActionResult<CustomerNextOfKinRespObj>> GetNextOfKinByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CustomerNextOfKinRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "NextOfKin Id is required" } }
                };
            }

            var response = await _repo.GetNextOfKinByIdAsync(search.SearchId);
            var resplist = new List<deposit_customernextofkin> { response };
            return new CustomerNextOfKinRespObj
            {
                customerNextOfKins = _mapper.Map<List<CustomerNextOfKinObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.NextOfKin.ADD_UPDATE_NEXTOFKIN)]
        public async Task<ActionResult<CustomerNextOfKinRegRespObj>> AddUpdateNextOfKinAsync([FromBody] AddUpdateCustomerNextOfKinObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_customernextofkin item = null;
                if (model.NextOfKinId > 0)
                {
                    item = await _repo.GetNextOfKinByIdAsync(model.NextOfKinId);
                    if (item == null)
                        return new CustomerNextOfKinRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_customernextofkin();
                domainObj.NextOfKinId = model.NextOfKinId > 0 ? model.NextOfKinId : 0;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.CustomerId = model.CustomerId;
                domainObj.Title = model.Title;
                domainObj.Surname = model.Surname;
                domainObj.FirstName = model.FirstName;
                domainObj.OtherName = model.OtherName;
                domainObj.DOB = model.DOB;
                domainObj.GenderId = model.GenderId;
                domainObj.Relationship = model.Relationship;
                domainObj.MobileNumber = model.MobileNumber;
                domainObj.Email = model.Email;
                domainObj.Address = model.Address;
                domainObj.City = model.City;
                domainObj.State = model.State;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.NextOfKinId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateNextOfKinAsync(domainObj);
                return new CustomerNextOfKinRegRespObj
                {
                    NextOfKinId = domainObj.NextOfKinId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CustomerNextOfKinRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.NextOfKin.DELETE_NEXTOFKIN)]
        public async Task<IActionResult> DeleteNextOfKin([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteNextOfKinAsync(id);
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

        #region IdentityDetails
        [HttpGet(ApiRoutes.IdentityDetails.GET_ALL_IDENTITYDETAILS)]
        public async Task<ActionResult<CustomerIdentificationRespObj>> GetAllIdentityDetailsAsync(int cid)
        {
            try
            {
                var response = await _repo.GetAllIdentityDetailsAsync(cid);
                return new CustomerIdentificationRespObj
                {
                    CustomerIdentifications = _mapper.Map<List<CustomerIdentificationObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CustomerIdentificationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.IdentityDetails.GET_IDENTITYDETAILS_BY_ID)]
        public async Task<ActionResult<CustomerIdentificationRespObj>> GetIdentityDetailsByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CustomerIdentificationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Customer Identification Id is required" } }
                };
            }

            var response = await _repo.GetIdentityDetailsByIdAsync(search.SearchId);
            var resplist = new List<deposit_customeridentification> { response };
            return new CustomerIdentificationRespObj
            {
                CustomerIdentifications = _mapper.Map<List<CustomerIdentificationObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.IdentityDetails.ADD_UPDATE_IDENTITYDETAILS)]
        public async Task<ActionResult<CustomerIdentificationRegRespObj>> AddUpDateIdentityDetails([FromBody] AddUpdateCustomerIdentificationObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_customeridentification item = null;
                if (model.CustomerIdentityId > 0)
                {
                    item = await _repo.GetIdentityDetailsByIdAsync(model.CustomerIdentityId);
                    if (item == null)
                        return new CustomerIdentificationRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_customeridentification();
                domainObj.CustomerIdentityId = model.CustomerIdentityId > 0 ? model.CustomerIdentityId : 0;
                domainObj.CustomerId = model.CustomerId;
                domainObj.Active = true;
                domainObj.CreatedBy = user.UserName;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.MeansOfID = model.MeansOfID;
                domainObj.IDNumber = model.IDNumber;
                domainObj.DateIssued = model.DateIssued;
                domainObj.ExpiryDate = model.ExpiryDate;
                domainObj.UpdatedBy = user.UserName;
                domainObj.UpdatedOn = model.CustomerIdentityId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateIdentityDetailsAsync(domainObj);
                return new CustomerIdentificationRegRespObj
                {
                    CustomerIdentityId = domainObj.CustomerIdentityId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CustomerIdentificationRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.IdentityDetails.DELETE_IDENTITYDETAILS)]
        public async Task<IActionResult> DeleteIdentityDetails([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteIdentityDetailsAsync(id);
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
