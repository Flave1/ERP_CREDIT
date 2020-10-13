using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Credit;
using Banking.Contracts.V1;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class LoanCustomerController : Controller
    {
        private readonly ILoanCustomerRepository _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;
       

        public LoanCustomerController(ILoanCustomerRepository repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
        }

        #region LOAN CUSTOMER

        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER)]
        public async Task<ActionResult<LoanCustomerRespObj>> GetAllLoanCustomer()
        {
            try
            {
                var response = _repo.GetAllLoanCustomer();
                return new LoanCustomerRespObj
                {
                    Customers = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_CASA)]
        public async Task<ActionResult<LoanCustomerRespObj>> GetLoanCustomerCASA([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerCASA(model.CustomerId);
                return new LoanCustomerRespObj
                {
                    Customers = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER_LITE)]
        public async Task<ActionResult<LoanCustomerRespObj>> GetAllLoanCustomerLite()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerLite();
                return new LoanCustomerRespObj
                {
                    CustomerLites = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_BY_ID)]
        public async Task<ActionResult<LoanCustomerRespObj>> GetAllLoanCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomer(model.CustomerId);
                var resplist = new List<LoanCustomerObj> { response };
                return new LoanCustomerRespObj
                {
                    Customers = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER)]
        public ActionResult<LoanCustomerRespObj> UpdateLoanCustomer([FromBody] LoanCustomerObj entity)
        {
            try
            {
                var response =  _repo.UpdateLoanCustomer(entity);
                return new LoanCustomerRespObj
                {
                    CustomerId = response.CustomerId,
                    Status = response.Status
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.DOWNLOAD_LOANCUSTOMER)]
        public async Task<ActionResult<LoanCustomerRespObj>> GenerateExportLoanCustomer()
        {
            try
            {
                var response = _repo.GenerateExportLoanCustomer();

                return new LoanCustomerRespObj
                {
                    export = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.UPLOAD_LOANCUSTOMER)]
        public async Task<ActionResult<LoanCustomerRespObj>> UploadProduct()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];

                var user = await 
                    _identityServer.UserDataAsync();
                var createdBy = user.UserName;

                var isDone = _repo.UploadLoanCustomer(image, createdBy);
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }


        [HttpPost(ApiRoutes.LoanCustomer.DELETE_LOANCUSTOMER)]
        public IActionResult DeleteLoanCustomer([FromBody] DeleteCustomerCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomer(id);
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

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_BY_CUSTOMER)]
        public ActionResult<LoanCustomerRespObj> UpdateLoanCustomerByCustomer([FromBody] LoanCustomerObj entity)
        {
            try
            {

                var response = _repo.UpdateLoanCustomerByCustomer(entity);

                return new LoanCustomerRespObj
                {
                    CustomerId = response.CustomerId,
                    Status = response.Status
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_BY_CUSTOMER_WEBSITE)]
        public ActionResult<LoanCustomerRespObj> UpdateLoanCustomerFromWebsite([FromBody] LoanCustomerObj entity)
        {
            try
            {

                var isDone = _repo.UpdateLoanCustomerFromWebsite(entity);

                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.VERIFY_EMAIL)]
        public async Task<ActionResult<LoanCustomerRespObj>> verifyEmailAccount([FromBody] LoanCustomerSearchObj model)
        {
            try
            {
                var isDone = _repo.verifyEmailAccount(model.CustomerId);
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.START_LOAN_CUSTOMER)]
        public async Task<ActionResult<StartLoanCustomerRespObj>> GetStartLoanApplicationCustomer()
        {
            try
            {
                var response = _repo.GetStartLoanApplicationCustomer();
                return new StartLoanCustomerRespObj
                {
                    Customers = response, 
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new StartLoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.START_LOAN_CUSTOMER_ID)]
        public async Task<ActionResult<StartLoanCustomerRespObj>> GetStartLoanApplicationCustomer([FromQuery]LoanCustomerSearchObj search)
        {
            try
            {
                var response = _repo.GetStartLoanApplicationCustomerById(search.CustomerId);
                var resplist = new List<StartLoanApplicationCustomerObj> { response };
                return new StartLoanCustomerRespObj
                {
                    Customers = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new StartLoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DASHBOARD)]
        public async Task<ActionResult<LoanCustomerRespObj>> GetWebLoanCustomerCASA([FromQuery]LoanCustomerSearchObj search)
        {
            try
            {
                var response = _repo.GetWebLoanCustomerCASA(search.CustomerId);
                var resplist = new List<LoanCustomerObj> { response };
                return new LoanCustomerRespObj
                {
                    Customers = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_TRANSACTION)]
        public async Task<ActionResult<GLTransactionRespObj>> GetWebCustomerTransactionDetails([FromQuery]LoanCustomerSearchObj search)
        {
            try
            {
                var response = _repo.GetWebCustomerTransactionDetails(search.AccountNumber);
                return new GLTransactionRespObj
                {
                    CustomerTransactions = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new GLTransactionRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion

        #region LOAN CUSTOMER DIRECTOR
        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER_DIRECTOR)]
        public async Task<ActionResult<LoanCustomerDirectorRespObj>> GetAllLoanCustomerDirector()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerDirector();
                return new LoanCustomerDirectorRespObj
                {
                    CustomerDirectors = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DIRECTOR_BY_ID)]
        public async Task<ActionResult<LoanCustomerDirectorRespObj>> GetLoanCustomerDirector([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerDirector(model.CustomerDirectorId);
                var resplist = new List<LoanCustomerDirectorObj> { response };
                return new LoanCustomerDirectorRespObj
                {
                    CustomerDirectors = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DIRECTOR_BY_CUSTOMER)]
        public async Task<ActionResult<LoanCustomerDirectorRespObj>> GetLoanCustomerDirectorByCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerDirectorByLoanCustomer(model.CustomerId);
                //var resplist = new List<LoanCustomerDirectorObj> { response };
                return new LoanCustomerDirectorRespObj
                {
                    CustomerDirectors = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DIRECTOR_SIGNATURE)]
        public async Task<ActionResult<LoanCustomerDirectorRespObj>> GetDirectorSignature([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetDirectorSignature(model.CustomerDirectorId, model.CustomerId);
                var resplist = new List<LoanCustomerDirectorObj> { response };
                return new LoanCustomerDirectorRespObj
                {
                    CustomerDirectors = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_DIRECTOR)]
        public async Task<ActionResult<LoanCustomerRespObj>> UpdateLoanCustomerDirector()
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files;
                //var fileName = httpRequest.Files["file"].FileName;
                var customerDirectorId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["customerDirectorId"]);
                var directorTypeId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["directorTypeId"]);
                var directorType = _httpContextAccessor.HttpContext.Request.Form["directorType"];
                var name = _httpContextAccessor.HttpContext.Request.Form["name"];
                var position = _httpContextAccessor.HttpContext.Request.Form["position"];
                var address = _httpContextAccessor.HttpContext.Request.Form["address"];
                var email = _httpContextAccessor.HttpContext.Request.Form["email"];
                var customerId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["customerId"]);
                var phoneNo = _httpContextAccessor.HttpContext.Request.Form["phoneNo"];
                var percentageShare = (_httpContextAccessor.HttpContext.Request.Form["percentageShare"]);
                var dob = _httpContextAccessor.HttpContext.Request.Form["dob"];
                var politicallyPosition = Convert.ToBoolean(_httpContextAccessor.HttpContext.Request.Form["politicallyPosition"]);
                var relativePoliticallyPosition = Convert.ToBoolean(_httpContextAccessor.HttpContext.Request.Form["relativePoliticallyPosition"]);

                var byteArray = new byte[0];
                foreach (var fileBit in postedFile)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteArray = ms.ToArray();
                        }
                    }
                }

                var model = new LoanCustomerDirectorObj
                {
                    CustomerDirectorId = customerDirectorId,
                    DirectorTypeId = directorTypeId,
                    DirectorType = directorType.ToString(),
                    Name = name.ToString(),
                    Position = position.ToString(),
                    Address = address.ToString(),
                    Email = email.ToString(),
                    CustomerId = customerId,
                    PhoneNo = phoneNo.ToString(),
                    PercentageShare = Convert.ToDecimal(percentageShare),
                    Dob = Convert.ToDateTime(dob),
                    Signature = byteArray,
                    PoliticallyPosition = politicallyPosition,
                    RelativePoliticallyPosition = relativePoliticallyPosition
                };
                model.CreatedBy = user;
                model.UpdatedBy = user;
                var isDone = _repo.UpdateLoanCustomerDirector(model);

                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.DELETE_LOANCUSTOMER_DIRECTOR)]
        public async Task<ActionResult<LoanCustomerRespObj>> DeleteLoanCustomerDirector([FromBody] DeleteCustomerCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerDirector(id);
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

        #region LOAN_CUSTOMER_DOCUMENT
        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER_DOCUMENT)]
        public async Task<ActionResult<LoanCustomerDocumentRespObj>> GetAllLoanCustomerDocument()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerDocument();
                return new LoanCustomerDocumentRespObj
                {
                    CustomerDocuments = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDocumentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DOCUMENT_BY_ID)]
        public async Task<ActionResult<LoanCustomerDocumentRespObj>> GetLoanCustomerDocument([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerDocument(model.DocumentTypeId);
                var resplist = new List<LoanCustomerDocumentObj> { response };
                return new LoanCustomerDocumentRespObj
                {
                    CustomerDocuments = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDocumentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DOCUMENT_BY_CUSTOMER)]
        public async Task<ActionResult<LoanCustomerDocumentRespObj>> GetLoanCustomerDocumentByCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerDocumentByLoanCustomer(model.CustomerId);
                //var resplist = new List<LoanCustomerDocumentObj> { response };
                return new LoanCustomerDocumentRespObj
                {
                    CustomerDocuments = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDocumentRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_DOCUMENT)]
        public async Task<ActionResult<LoanCustomerRespObj>> UpdateLoanCustomerDocument()
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                var customerDirectorId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["customerDirectorId"]);
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files;
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["file"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var customerId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["customerId"]);
                var documentTypeId = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["documentTypeId"]);
                var physicalLocation = _httpContextAccessor.HttpContext.Request.Form["physicalLocation"];
                var documentName = _httpContextAccessor.HttpContext.Request.Form["documentName"];

                var byteArray = new byte[0];
                foreach (var fileBit in postedFile)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteArray = ms.ToArray();
                        }
                    }
                }
                //postedFile.InputStream.Read(image, 0, postedFile.ContentLength);

                var model = new LoanCustomerDocumentObj
                {
                    DocumentTypeId = documentTypeId,
                    DocumentExtension = fileExtention,
                    DocumentName = documentName.ToString(),
                    PhysicalLocation = physicalLocation.ToString(),
                    DocumentFile = byteArray,
                    CustomerDocumentId = 0,
                    CustomerId = customerId,
                    CreatedBy = user
                };
                model.CreatedBy = user;
                model.UpdatedBy = user;
                var isDone = _repo.UpdateLoanCustomerDocument(model);

                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.DELETE_DOCUMENT)]
        public async Task<ActionResult<LoanCustomerRespObj>> DeleteLoanCustomerDocument(int Ids)
        {
            var response = false;
            response = _repo.DeleteLoanCustomerDocument(Ids);
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

        #region LOAN CUSTOMER IDENTITY
        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER_IDENTITY)]
        public async Task<ActionResult<LoanCustomerIdentityRespObj>> GetAllLoanCustomerIdentityDetails()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerIdentityDetails();
                return new LoanCustomerIdentityRespObj
                {
                    CustomerIdentity = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerIdentityRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_IDENTITY_BY_ID)]
        public async Task<ActionResult<LoanCustomerIdentityRespObj>> GetLoanCustomerIdentityDetails([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerIdentityDetails(model.CustomerId);
                var resplist = new List<LoanCustomerIdentityDetailsObj> { response };
                return new LoanCustomerIdentityRespObj
                {
                    CustomerIdentity = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerIdentityRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_IDENTITY_BY_CUSTOMER)]
        public async Task<ActionResult<LoanCustomerIdentityRespObj>> GetLoanCustomerIdentityDetailsByCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerIdentityDetailsByLoanCustomer(model.CustomerId);
                //var resplist = new List<LoanCustomerIdentityDetailsObj> { response };
                return new LoanCustomerIdentityRespObj
                {
                    CustomerIdentity = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerIdentityRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_IDENTITY)]
        public async Task<ActionResult<LoanCustomerIdentityRespObj>> UpdateLoanCustomerIdentityDetails([FromBody]LoanCustomerIdentityDetailsObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;
               
                model.CreatedBy = user;
                model.UpdatedBy = user;
                var isDone = _repo.UpdateLoanCustomerIdentityDetails(model);

                return new LoanCustomerIdentityRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerIdentityRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.DELETE_IDENTITY)]
        public async Task<ActionResult<LoanCustomerRespObj>> DeleteLoanCustomerIdentityDetails([FromBody] DeleteCustomerCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerIdentityDetails(id);
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

        #region LOAN CUSTOMER BANK DETAILS
        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER_BANKDETAILS)]
        public async Task<ActionResult<LoanCustomerBankDetailRespObj>> GetAllLoanCustomerBankDetails()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerBankDetails();
                return new LoanCustomerBankDetailRespObj
                {
                    CustomerBankDetails = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerBankDetailRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_BANKDETAILS_BY_ID)]
        public async Task<ActionResult<LoanCustomerBankDetailRespObj>> GetLoanCustomerBankDetails([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerBankDetails(model.CustomerBankDetailsId);
                var resplist = new List<LoanCustomerBankDetailsObj> { response };
                return new LoanCustomerBankDetailRespObj
                {
                    CustomerBankDetails = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerBankDetailRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_BANKDETAILS_BY_CUSTOMER)]
        public async Task<ActionResult<LoanCustomerBankDetailRespObj>> GetLoanCustomerBankDetailsByCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerBankDetailsByLoanCustomer(model.CustomerId);
                return new LoanCustomerBankDetailRespObj
                {
                    CustomerBankDetails = response,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerBankDetailRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_CARDDETAILS_BY_CUSTOMER)]
        public async Task<ActionResult<LoanCustomerBankDetailRespObj>> GetLoanCustomerCardDetailsByLoanCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerCardDetailsByLoanCustomer(model.CustomerId);
                return new LoanCustomerBankDetailRespObj
                {
                    CustomerCardDetails = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerBankDetailRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_BANKDETAILS)]
        public async Task<ActionResult<LoanCustomerIdentityRespObj>> UpdateLoanCustomerBankDetails([FromBody]LoanCustomerBankDetailsObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;
                return _repo.UpdateLoanCustomerBankDetails(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerIdentityRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_CARDDETAILS)]
        public async Task<ActionResult<LoanCustomerIdentityRespObj>> UpdateLoanCustomerCardDetails([FromBody]LoanCustomerCardDetailsObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;
                return _repo.UpdateLoanCustomerCardDetails(model);
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerIdentityRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.DELETE_BANKDETAILS)]
        public async Task<ActionResult<LoanCustomerRespObj>> DeleteLoanCustomerBankDetails([FromBody] DeleteCustomerCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerBankDetails(id);
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

        #region LOAN CUSTOMER NEXT OF KIN
        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER_NEXTOFKIN)]
        public async Task<ActionResult<LoanCustomerNextOfKinRespObj>> GetAllLoanCustomerNextOfKin()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerNextOfKin();
                return new LoanCustomerNextOfKinRespObj
                {
                    CustomerNextOfKin = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerNextOfKinRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_NEXTOFKIN)]
        public async Task<ActionResult<LoanCustomerNextOfKinRespObj>> GetLoanCustomerNextOfKin([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerNextOfKin(model.CustomerNextOfKinId);
                var resplist = new List<LoanCustomerNextOfKinObj> { response };
                return new LoanCustomerNextOfKinRespObj
                {
                    CustomerNextOfKin = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerNextOfKinRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_NEXTOFKIN_BY_CUSTOMER)]
        public async Task<ActionResult<LoanCustomerNextOfKinRespObj>> GetLoanCustomerNextOfKinByCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerNextOfKinByLoanCustomer(model.CustomerId);
                //var resplist = new List<LoanCustomerNextOfKinObj> { response };
                return new LoanCustomerNextOfKinRespObj
                {
                    CustomerNextOfKin = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerNextOfKinRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_NEXTOFKIN)]
        public async Task<ActionResult<LoanCustomerNextOfKinRespObj>> UpdateLoanCustomerBankDetails([FromBody]LoanCustomerNextOfKinObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;
                var isDone = _repo.UpdateLoanCustomerNextOfKin(model);

                return new LoanCustomerNextOfKinRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerNextOfKinRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.DELETE_NEXTOFKIN)]
        public async Task<ActionResult<LoanCustomerRespObj>> DeleteLoanCustomerNextOfKin([FromBody] DeleteCustomerCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerNextOfKin(id);
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

        #region LOAN CUSTOMER DIRECTOR SHAREHOLDER
        [HttpGet(ApiRoutes.LoanCustomer.GET_ALL_LOANCUSTOMER_DIRECTORSHAREHOLDER)]
        public async Task<ActionResult<LoanCustomerDirectorShareHolderRespObj>> GetAllLoanCustomerDirectorShareHolder()
        {
            try
            {
                var response = _repo.GetAllLoanCustomerDirectorShareHolder();
                return new LoanCustomerDirectorShareHolderRespObj
                {
                    DirectorShareHolder = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorShareHolderRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DIRECTORSHAREHOLDER)]
        public async Task<ActionResult<LoanCustomerDirectorShareHolderRespObj>> GetLoanCustomerDirectorShareHolder([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerDirectorShareHolder(model.DirectorShareHolderId);
                var resplist = new List<LoanCustomerDirectorShareHolderObj> { response };
                return new LoanCustomerDirectorShareHolderRespObj
                {
                    DirectorShareHolder = resplist,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorShareHolderRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.LoanCustomer.GET_LOANCUSTOMER_DIRECTORSHAREHOLDER_BY_CUSTOMER)]
        public async Task<ActionResult<LoanCustomerDirectorShareHolderRespObj>> GetLoanCustomerDirectorShareHolderByCustomer([FromQuery] LoanCustomerSearchObj model)
        {
            try
            {
                var response = _repo.GetLoanCustomerDirectorShareHolderByLoanCustomer(model.CustomerId);
                //var resplist = new List<LoanCustomerDirectorShareHolderObj> { response };
                return new LoanCustomerDirectorShareHolderRespObj
                {
                    DirectorShareHolder = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorShareHolderRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.ADD_LOANCUSTOMER_DIRECTORSHAREHOLDER)]
        public async Task<ActionResult<LoanCustomerDirectorShareHolderRespObj>> UpdateLoanCustomerDirectorShareHolder([FromBody]LoanCustomerDirectorShareHolderObj model)
        {
            try
            {
                var identity = await _identityServer.UserDataAsync();
                var user = identity.UserName;

                model.CreatedBy = user;
                model.UpdatedBy = user;
                var isDone = _repo.UpdateLoanCustomerDirectorShareHolder(model);

                return new LoanCustomerDirectorShareHolderRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new LoanCustomerDirectorShareHolderRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.LoanCustomer.DELETE_DIRECTORSHAREHOLDER)]
        public async Task<ActionResult<LoanCustomerRespObj>> DeleteLoanCustomerDirectorShareHolder([FromBody] DeleteCustomerCommand command)
        {
            var response = false;
            var Ids = command.Ids;
            foreach (var id in Ids)
            {
                response = _repo.DeleteLoanCustomerDirectorShareHolder(id);
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

    }
}