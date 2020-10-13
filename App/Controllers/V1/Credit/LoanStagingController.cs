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
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Banking.Contracts.Response.Credit.LoanStagingObjs;

namespace Banking.Controllers.V1.Credit
{
    [ERPAuthorize]
    public class LoanStagingController : Controller
    {
        private readonly ILoanStaging _repo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerRequest _identityServer;

        public LoanStagingController(ILoanStaging repo, IMapper mapper, IIdentityServerRequest identityServer, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _identityServer = identityServer;
            _httpContextAccessor = httpContextAccessor;
        }

        [ERPActivity(Action = UserActions.View, Activity = 84)]
        [HttpGet(ApiRoutes.LoanStaging.GET_LOAN_STAGING_BY_ID)]
        public async Task<ActionResult<LoanStagingRespObj>> GetAccountTypeByIdAsync([FromQuery] LoanStagingSearchObj search)
        {
            if (search.LoanStagingId < 1)
            {
                return new LoanStagingRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Loan Staging Id is required" } }
                };
            }

            var response = await _repo.GetLoanStagingByIdAsync(search.LoanStagingId);
            var resplist = new List<LoanStagingObj> { response };
            return new LoanStagingRespObj
            {
                LoanStagings = resplist,
            };
        }

        [HttpGet(ApiRoutes.LoanStaging.GET_ALL_LOAN_STAGING)]
        public async Task<ActionResult<LoanStagingRespObj>> GetAllAccountTypeAsync()
        {
            try
            {
                var response = await _repo.GetAllLoanStagingAsync();
                return new LoanStagingRespObj
                {
                    LoanStagings = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new LoanStagingRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 84)]
        [HttpPost(ApiRoutes.LoanStaging.ADD_LOAN_STAGING)]
        public async Task<ActionResult<LoanStagingRegRespObj>> AddUpDateAccountType([FromBody] LoanStagingObj model)
        {
            try
            {
                LoanStagingObj item = null;
                if (model.LoanStagingId > 0)
                {
                    item = await _repo.GetLoanStagingByIdAsync(model.LoanStagingId);
                    if (item == null)
                        return new LoanStagingRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }
                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;

                model.CreatedBy = createdBy;
                model.UpdatedBy = createdBy;

                 await _repo.AddOrUpdateLoanStagingAsync(model);
                return new LoanStagingRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new LoanStagingRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 84)]
        [HttpPost(ApiRoutes.LoanStaging.DELETE_LOAN_STAGING)]
        public async Task<IActionResult> DeleteBankClosure([FromBody] DeleteLoanStagingCommand command)
        {
            var response = false;
            var Ids = command.LoanStagingIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteLoanStagingAsync(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObj
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Record did not Delete" } }
                    });
            return Ok(
                    new DeleteRespObj
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Record Deleted Successfully" } }
                    });
        }
    }
}