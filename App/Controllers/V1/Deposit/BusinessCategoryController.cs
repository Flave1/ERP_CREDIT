using AutoMapper;
using Banking.AuthHandler.Interface;
using Banking.Contracts.Response.Deposit;
using Banking.Contracts.V1;
using Banking.Repository.Interface.Deposit;
using GODP.Entities.Models;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Banking.Requests;
using GOSLibraries.GOS_Error_logger.Service;
using Banking.Handlers.Auths;

namespace Banking.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class BusinessCategoryController : Controller
    {
        private readonly IBusinessCategoryService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerRequest _identityServer;
        private readonly ILoggerService _logger;

        public BusinessCategoryController(IBusinessCategoryService repo, IIdentityService identityService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _identityServer = identityServer;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.BusinessCategory.GET_ALL_BUSINESSCATEGORY)]

        public async Task<ActionResult<BusinessCategoryRespObj>> GetAllBusinessCategoryAsync()
        {
            try
            {
                var response = await _repo.GetAllBusinessCategoryAsync();
                return new BusinessCategoryRespObj
                {
                    BusinessCategories = _mapper.Map<List<BusinessCategoryObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new BusinessCategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.BusinessCategory.GET_BUSINESSCATEGORY_BY_ID)]
        public async Task<ActionResult<BusinessCategoryRespObj>> GetBusinessCategoryByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new BusinessCategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "BusinessCategory Id is required" } }
                };
            }

            var response = await _repo.GetBusinessCategoryByIdAsync(search.SearchId);
            var resplist = new List<deposit_businesscategory> { response };
            return new BusinessCategoryRespObj
            {
                BusinessCategories = _mapper.Map<List<BusinessCategoryObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.BusinessCategory.DOWNLOAD_BUSINESSCATEGORY)]
        public async Task<ActionResult<BusinessCategoryRespObj>> GenerateExportBusinessCategory()
        {
            var response = _repo.GenerateExportBusinessCategory();

            return new BusinessCategoryRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.BusinessCategory.ADD_UPDATE_BUSINESSCATEGORY)]
        public async Task<ActionResult<BusinessCategoryRegRespObj>> AddUpDateBusinessCategory([FromBody] AddUpdateBusinessCategoryObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_businesscategory item = null;
                if (model.BusinessCategoryId > 0)
                {
                    item = await _repo.GetBusinessCategoryByIdAsync(model.BusinessCategoryId);
                    if (item == null)
                        return new BusinessCategoryRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_businesscategory();

                domainObj.BusinessCategoryId = model.BusinessCategoryId > 0 ? model.BusinessCategoryId : 0;
                domainObj.Name = model.Name;
                domainObj.Description = model.Description;
                domainObj.Active = true;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.CreatedBy = user.UserName;
                domainObj.Deleted = false;
                domainObj.UpdatedOn = model.BusinessCategoryId > 0 ? DateTime.Today : DateTime.Today;
                domainObj.UpdatedBy = user.UserName;
                

            var isDone = await _repo.AddUpdateBusinessCategoryAsync(domainObj);
                return new BusinessCategoryRegRespObj
                {
                    BusinessCategoryId = domainObj.BusinessCategoryId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new BusinessCategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.BusinessCategory.UPLOAD_BUSINESSCATEGORY)]
        public async Task<ActionResult<BusinessCategoryRegRespObj>> UploadBusinessCategoryAsync()
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

                var isDone = await _repo.UploadBusinessCategoryAsync(byteList, createdBy);
                return new BusinessCategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new BusinessCategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.BusinessCategory.DELETE_BUSINESSCATEGORY)]
        public async Task<IActionResult> DeleteBusinessCategory([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteBusinessCategoryAsync(id);
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

    }
}

