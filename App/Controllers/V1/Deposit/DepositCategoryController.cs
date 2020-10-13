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
    public class DepositCategoryController : Controller
    {
        private readonly IDepositCategoryService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;


        public DepositCategoryController(IDepositCategoryService repo, IIdentityService identityService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
        }

        [HttpGet(ApiRoutes.DepositCategory.GET_ALL_DEPOSITCATEGORY)]

        public async Task<ActionResult<CategoryRespObj>> GetAllCategoryAsync()
        {
            try
            {
                var response = await _repo.GetAllCategoryAsync();
                return new CategoryRespObj
                {
                    Categories = _mapper.Map<List<CategoryObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.DepositCategory.GET_DEPOSITCATEGORY_BY_ID)]
        public async Task<ActionResult<CategoryRespObj>> GetCategoryByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "DepositCategory Id is required" } }
                };
            }

            var response = await _repo.GetCategoryByIdAsync(search.SearchId);
            var resplist = new List<deposit_category> { response };
            return new CategoryRespObj
            {
                Categories = _mapper.Map<List<CategoryObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.DepositCategory.DOWNLOAD_DEPOSITCATEGORY)]
        public async Task<ActionResult<CategoryRespObj>> GenerateExportCategory()
        {
            var response = _repo.GenerateExportCategory();

            return new CategoryRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.DepositCategory.ADD_UPDATE_DEPOSITCATEGORY)]
        public async Task<ActionResult<CategoryRegRespObj>> AddUpDateCategory([FromBody] AddUpdateCategoryObj model)
        {
            try
            {
                var user = await _identityServer.UserDataAsync();
                deposit_category item = null;
                if (model.CategoryId > 0)
                {
                    item = await _repo.GetCategoryByIdAsync(model.CategoryId);
                    if (item == null)
                        return new CategoryRegRespObj
                        {
                            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
                        };
                }

                var domainObj = new deposit_category();

                domainObj.CategoryId = model.CategoryId > 0 ? model.CategoryId : 0;
                domainObj.Name = model.Name;
                domainObj.Description = model.Description;
                domainObj.Active = true;
                domainObj.CreatedOn = DateTime.Today;
                domainObj.CreatedBy = user.UserName;
                domainObj.Deleted = false;
                domainObj.UpdatedOn = model.CategoryId > 0 ? DateTime.Today : DateTime.Today;
                domainObj.UpdatedBy = user.UserName;


                var isDone = await _repo.AddUpdateCategoryAsync(domainObj);
                return new CategoryRegRespObj
                {
                    CategoryId = domainObj.CategoryId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.DepositCategory.UPLOAD_DEPOSITCATEGORY)]
        public async Task<ActionResult<CategoryRegRespObj>> UploadCategoryAsync()
        {
            try
            {
                var postedFile = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var fileName = _httpContextAccessor.HttpContext.Request.Form.Files["Image"].FileName;
                var fileExtention = Path.GetExtension(fileName);
                var image = new byte[postedFile.Length];
                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;

                var createdBy = _identityServer.UserDataAsync().Result.UserName;

                var isDone = await _repo.UploadCategoryAsync(image, createdBy);
                return new CategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.DepositCategory.DELETE_DEPOSITCATEGORY)]
        public async Task<IActionResult> DeleteCategory([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteCategoryAsync(id);
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
