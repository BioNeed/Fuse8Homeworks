using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Web;
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
        private readonly IOptionsSnapshot<ApiInfoModel> _configuration;
        private readonly HttpClient _httpClient;

        public CurrencyService(IHttpClientFactory httpClientFactory, IOptionsSnapshot<ApiInfoModel> configuration)
        {
            _httpClient = httpClientFactory.CreateClient(ApiConstants.HttpClientNames.Default);
            _configuration = configuration;
        }

        public async Task<ApiInfoModel> GetApiConfigAsync(
            CancellationToken cancellationToken)
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

            return _configuration.Value with
            {
                NewRequestsAvailable = apiStatusFromJson.UsedRequests <
                    apiStatusFromJson.TotalRequests,
            };
        }

        public async Task<ExchangeRateModel[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken)
        {
            ApiInfoModel config = await GetApiConfigAsync(cancellationToken);

            if (config.NewRequestsAvailable == false)
            {
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

        public async Task<ExchangeRatesHistoricalModel> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken)
        {
            ApiInfoModel config = await GetApiConfigAsync(cancellationToken);

            if (config.NewRequestsAvailable == false)
            {
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