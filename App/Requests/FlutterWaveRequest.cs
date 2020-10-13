using Banking.Contracts.Response.FlutterWave;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Requests
{
    public class FlutterWaveRequest : IFlutterWaveRequest
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IIdentityServerRequest _serverRequest;
        private static HttpClient Client;
        private HttpResponseMessage result = new HttpResponseMessage();
        private readonly AsyncRetryPolicy _retryPolicy;
        private const int maxRetryTimes = 10;
        private readonly ILoggerService _logger;
        private GOSLibraries.GOS_Financial_Identity.AuthenticationResult _authResponse = null;
        public FlutterWaveRequest(
            IHttpContextAccessor httpContextAccessor, IIdentityServerRequest serverRequest,
            IHttpClientFactory httpClientFactory, IConfiguration configuration,
            ILoggerService loggerService)
        {
            _logger = loggerService;
            _configuration = configuration;
            _accessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _serverRequest = serverRequest;
            _retryPolicy = Policy.Handle<HttpRequestException>()

             .WaitAndRetryAsync(maxRetryTimes, times =>

             TimeSpan.FromSeconds(times * 2));
        }

        public async Task<CardDetailsRespObj> validateCardDetails(string url)
        {
            CardDetailsRespObj responseObj = new CardDetailsRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.GetAsync(url);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<CardDetailsRespObj>(data1);
                        new CardDetailsRespObj
                        {
                            status = "Not Successful"
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<CardDetailsRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new CardDetailsRespObj
                    {
                        status = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new CardDetailsRespObj
                    {
                        status = responseObj.status,
                        data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new CardDetailsRespObj
                {
                    status = "Not Successful"
                };
            });
        }
        public async Task<BvnDetailsRespObj> validateBvnDetails(string url)
        {
            BvnDetailsRespObj responseObj = new BvnDetailsRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.GetAsync(url);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<BvnDetailsRespObj>(data1);
                        return new BvnDetailsRespObj
                        {
                            status = "BVN: "+ responseObj.message
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<BvnDetailsRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new BvnDetailsRespObj
                    {
                        status = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new BvnDetailsRespObj
                    {
                        status = responseObj.status,
                        data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new BvnDetailsRespObj
                {
                    status = "BVN: " + responseObj.message
                };
            });
        }
        public async Task<AccountDetailsRespObj> validateAccountDetails(AccountObj account)
        {
            var url = "accounts/resolve";
            var jsonContent = JsonConvert.SerializeObject(account);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            AccountDetailsRespObj responseObj = new AccountDetailsRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.PostAsync(url, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<AccountDetailsRespObj>(data1);
                        return new AccountDetailsRespObj
                        {
                            message = responseObj.message
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<AccountDetailsRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new AccountDetailsRespObj
                    {
                        status = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new AccountDetailsRespObj
                    {
                        status = responseObj.status,
                        data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new AccountDetailsRespObj
                {
                    message = "Not Successful"
                };
            });
        }
        public async Task<TransferRespObj> createTransfer(TransferObj transfer)
        {
            var url = "transfers";
            var jsonContent = JsonConvert.SerializeObject(transfer);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            TransferRespObj responseObj = new TransferRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.PostAsync(url, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<TransferRespObj>(data1);
                        return new TransferRespObj
                        {
                            message = responseObj.message
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<TransferRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new TransferRespObj
                    {
                        status = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new TransferRespObj
                    {
                        status = responseObj.status,
                        //data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new TransferRespObj
                {
                    message = "Not Successful"
                };
            });
        }
        public async Task<TransferRespObj> createBulkTransfer(BulkTransferObj transfer)
        {
            var url = "bulk-transfers";
            var jsonContent = JsonConvert.SerializeObject(transfer);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            TransferRespObj responseObj = new TransferRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.PostAsync(url, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<TransferRespObj>(data1);
                        return new TransferRespObj
                        {
                            message = responseObj.message,
                            status = responseObj.status
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<TransferRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new TransferRespObj
                    {
                        status = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new TransferRespObj
                    {
                        status = responseObj.status,
                        //data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new TransferRespObj
                {
                    message = "Not Successful",
                    status = "Not Successful"
                };
            });
        }
        public async Task<PaymentRespObj> makePayment(PaymentObj payment)
        {
            var url = "payments";
            var jsonContent = JsonConvert.SerializeObject(payment);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            PaymentRespObj responseObj = new PaymentRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.PostAsync(url, byteContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<PaymentRespObj>(data1);
                        return new PaymentRespObj
                        {
                            message = responseObj.message
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<PaymentRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new PaymentRespObj
                    {
                        message = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new PaymentRespObj
                    {
                        status = responseObj.status,
                        data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new PaymentRespObj
                {
                    message = "Not Successful"
                };
            });
        }
        public async Task<TransactionVerificationRespObj> transactionVerification(string url)
        {
            TransactionVerificationRespObj responseObj = new TransactionVerificationRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.GetAsync(url);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<TransactionVerificationRespObj>(data1);
                        return new TransactionVerificationRespObj
                        {
                            message =  responseObj.message,
                            status = responseObj.status
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<TransactionVerificationRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new TransactionVerificationRespObj
                    {
                        status = "Not Successful",
                        message = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new TransactionVerificationRespObj
                    {
                        status = responseObj.status,
                        data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new TransactionVerificationRespObj
                {
                    status = "Not Successful",
                    message = "Not Successful"
                };
            });
        }
        public async Task<GetTransferRespObj> getAllTransfer()
        {
            var url = "transfers";
            GetTransferRespObj responseObj = new GetTransferRespObj();
            var flutterWaveClient = _httpClientFactory.CreateClient("FLUTTERWAVE");
            var keys = await _serverRequest.GetFlutterWaveKeys();
            flutterWaveClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + keys.keys.secret_keys);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    result = await flutterWaveClient.GetAsync(url);
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        responseObj = JsonConvert.DeserializeObject<GetTransferRespObj>(data1);
                        return new GetTransferRespObj
                        {
                            message = responseObj.message,
                            status = responseObj.status
                        };
                    }
                    var data = await result.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<GetTransferRespObj>(data);
                }
                catch (Exception ex) { throw ex; }
                if (responseObj == null)
                {
                    return new GetTransferRespObj
                    {
                        status = "Not Successful"
                    };
                }
                if (responseObj.status == "success")
                {
                    return new GetTransferRespObj
                    {
                        status = responseObj.status,
                        data = responseObj.data,
                        message = responseObj.message
                    };
                }
                return new GetTransferRespObj
                {
                    message = responseObj.message,
                    status = responseObj.status
                };
            });
        }
    }
}
