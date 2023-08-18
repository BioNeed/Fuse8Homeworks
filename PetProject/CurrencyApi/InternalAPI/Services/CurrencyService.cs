using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Web;
using Grpc.Core;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Exceptions;
using InternalAPI.JsonConverters;
using InternalAPI.Models;
using Microsoft.Extensions.Options;

namespace InternalAPI.Services
{
    public class CurrencyService : IGettingApiConfigService, ICurrencyAPI
    {
        private readonly IOptionsSnapshot<ApiInfoModel> _apiConfig;
        private readonly HttpClient _httpClient;
        private readonly bool _usingByGrpc;

        public CurrencyService(IHttpClientFactory httpClientFactory,
                               IOptionsSnapshot<ApiInfoModel> apiConfig,
                               IHttpContextAccessor httpContextAccessor,
                               IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient(ApiConstants.HttpClientNames.Default);
            _apiConfig = apiConfig;
            _usingByGrpc = httpContextAccessor.HttpContext.Connection.LocalPort ==
                configuration.GetValue<int>(ApiConstants.PortNames.GrpcPort);
        }

        public async Task<ApiInfoModel> GetApiConfigAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                ApiConstants.Uris.GetStatus, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception(ApiConstants.ErrorMessages.UnknownExceptionMessage);
            }

            string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new ApiStatusJsonConverter() },
            };

            ApiStatusModel apiStatusFromJson =
                JsonSerializer.Deserialize<ApiStatusModel>(responseString, options);

            return _apiConfig.Value with
            {
                NewRequestsAvailable = apiStatusFromJson.UsedRequests <
                    apiStatusFromJson.TotalRequests,
            };
        }

        public async Task<ExchangeRateModel[]> GetAllCurrentCurrenciesAsync(
            string baseCurrency,
            CancellationToken cancellationToken)
        {
            ApiInfoModel apiInfo = await GetApiConfigAsync(cancellationToken);

            if (apiInfo.NewRequestsAvailable == false)
            {
                if (_usingByGrpc == true)
                {
                    throw new RpcException(
                        status: new Status(StatusCode.ResourceExhausted,
                            ApiConstants.ErrorMessages.RequestLimitExceptionMessage));
                }

                throw new ApiRequestLimitException(ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
            }

            NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
            requestQuery["base_currency"] = baseCurrency;

            string requestPath = ApiConstants.Uris.GetCurrency + requestQuery.ToString();

            HttpResponseMessage response = await _httpClient.GetAsync(requestPath, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception(ApiConstants.ErrorMessages.UnknownExceptionMessage);
            }

            string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new AllExchangeRatesJsonConverter() },
            };

            ExchangeRateModel[] exchangeRates = JsonSerializer.Deserialize<ExchangeRateModel[]>(
                responseString, options);

            return exchangeRates;
        }

        public async Task<ExchangeRatesHistoricalModel> GetAllCurrenciesOnDateAsync(
            string baseCurrency,
            DateOnly date,
            CancellationToken cancellationToken)
        {
            ApiInfoModel apiInfo = await GetApiConfigAsync(cancellationToken);

            if (apiInfo.NewRequestsAvailable == false)
            {
                if (_usingByGrpc == true)
                {
                    throw new RpcException(
                        status: new Status(StatusCode.ResourceExhausted,
                            ApiConstants.ErrorMessages.RequestLimitExceptionMessage));
                }

                throw new ApiRequestLimitException(ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
            }

            NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
            requestQuery["base_currency"] = baseCurrency;
            requestQuery["date"] = date.ToString(ApiConstants.Formats.DateFormat);

            string requestPath = ApiConstants.Uris.GetCurrencyHistorical + requestQuery.ToString();

            HttpResponseMessage response = await _httpClient.GetAsync(requestPath, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception(ApiConstants.ErrorMessages.UnknownExceptionMessage);
            }

            string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new AllExchangeRatesHistoricalJsonConverter() },
            };

            ExchangeRatesHistoricalModel exchangeRatesOnDate =
                JsonSerializer.Deserialize<ExchangeRatesHistoricalModel>(
                responseString, options);

            return exchangeRatesOnDate;
        }
    }
}