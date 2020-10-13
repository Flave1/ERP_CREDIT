using Banking.Contracts.Response.Approvals;
using Banking.Contracts.Response.Common;
using Banking.Contracts.V1;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Banking.Requests
{
    public  class CommonService : ICommonService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int maxRetryTimes = 4;
        private CommonRespObj responseObj  = new CommonRespObj();
        private HttpResponseMessage result = new HttpResponseMessage();
        public CommonService(IHttpClientFactory httpClientFactory, ILoggerService loggerService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _accessor = httpContextAccessor;
            _logger = loggerService;
            _retryPolicy = Policy.Handle<Exception>()

              .WaitAndRetryAsync(maxRetryTimes, times =>

              TimeSpan.FromSeconds(times * 2));
        }
       
        
    }
}
