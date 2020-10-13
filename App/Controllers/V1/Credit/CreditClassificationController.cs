using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.V1;
using Banking.DomainObjects.Credit;
using Banking.Handlers.Auths;
using Banking.Repository.Interface.Credit;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.CreditClassificationObjs;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class CreditClassificationController : Controller
    {
        private readonly ICreditClassification _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreditClassificationController(ICreditClassification repo, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
        }

        [ERPActivity(Action = UserActions.View, Activity = 85)]
        [HttpGet(ApiRoutes.CreditClassification.GET_CLASSIFICATION_BY_ID)]
        public async Task<ActionResult<CreditClassificationRespObj>> GetAccountTypeByIdAsync([FromQuery] CreditClassificationSearchObj search)
        {
            if (search.CreditClassificationId < 1)
            {
                return new CreditClassificationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Credit Classification Id is required" } }
                };
            }

            var response = await _repo.GetCreditClassificationByIdAsync(search.CreditClassificationId);
            var resplist = new List<credit_creditclassification> { response };
            return new CreditClassificationRespObj
            {
                CreditClassification = _mapper.Map<List<CreditClassificationObj>>(resplist),
            };
        }

        
        [HttpGet(ApiRoutes.CreditClassification.GET_ALL_CLASSIFICATION)]
        public async Task<ActionResult<CreditClassificationRespObj>> GetAllAccountTypeAsync()
        {
            try
            {
                var response = await _repo.GetAllCreditClassificationAsync();
                return new CreditClassificationRespObj
                {
                    CreditClassification = _mapper.Map<List<CreditClassificationObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CreditClassificationRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 85)]
        [HttpPost(ApiRoutes.CreditClassification.ADD_CLASSIFICATION)]
        public async Task<ActionResult<CreditClassificationRegRespObj>> AddUpDateAccountType([FromBody] AddUpdateCreditClassification model)
        {
            try
            {
                credit_creditclassification item = null;
                if (model.CreditClassificationId > 0)
                {
                    item = await _repo.GetCreditClassificationByIdAsync(model.CreditClassificationId);
                    if (item == null)
                        return new CreditClassificationRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new credit_creditclassification();
                domainObj.CreditClassificationId = model.CreditClassificationId > 0 ? model.CreditClassificationId : 0;
                domainObj.Description = model.Description;
                domainObj.ProvisioningRequirement = model.ProvisioningRequirement;
                domainObj.UpperLimit = model.UpperLimit;
                domainObj.LowerLimit = model.LowerLimit;
                domainObj.Active = true;
                domainObj.CreatedBy = string.Empty;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.Deleted = false;
                domainObj.UpdatedBy = string.Empty;
                domainObj.UpdatedOn = model.CreditClassificationId > 0 ? DateTime.Today : DateTime.Today;

                var isDone = await _repo.AddUpdateCreditClassificationAsync(domainObj);
                return new CreditClassificationRegRespObj
                {
                    CreditClassificationId = domainObj.CreditClassificationId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CreditClassificationRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 85)]
        [HttpPost(ApiRoutes.CreditClassification.DELETE_CLASSIFICATION)]
        public async Task<IActionResult> DeleteBankClosure([FromBody] DeleteCreditClassificationCommand command)
        {
            var response = false;
            var Ids = command.CreditClassificationIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteCreditClassificationAsync(id);
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


        #region OperatingAccount
        [ERPActivity(Action = UserActions.Add, Activity = 83)]
        [HttpPost(ApiRoutes.CreditClassification.UPDATE_OPERATING_ACCOUNT)]
        public async Task<ActionResult<OperatingAccountRespObj>> AddOrUpdateOperatingAccountAsync([FromBody] OperatingAccountObj model)
        {
            try
            {
                var isDone = await _repo.AddOrUpdateOperatingAccountAsync(model);
                return new OperatingAccountRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "Successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new OperatingAccountRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.CreditClassification.GET_ALL_OPERATING_ACCOUNT)]
        public async Task<ActionResult<OperatingAccountRespObj>> GetOperatingAccount()
        {
            try
            {
                var response = await _repo.GetOperatingAccount();
                return new OperatingAccountRespObj
                {
                    OperatingAccount = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new OperatingAccountRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }
        #endregion
    }
}