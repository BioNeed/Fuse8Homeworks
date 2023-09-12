using System.Collections.Specialized;
using System.Text.Json;
using System.Web;
using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Models;
using Grpc.Core;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Exceptions;
using InternalAPI.JsonConverters;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    /// <summary>
    /// <inheritdoc cref="ICurrencyAPI"/>
    /// <inheritdoc cref="IGettingApiInfoService"/>
    /// </summary>
    public class CurrencyService : IGettingApiInfoService, ICurrencyAPI
    {
        private readonly ICacheSettingsRepository _cacheSettingsRepository;
        private readonly HttpClient _httpClient;

        public CurrencyService(IHttpClientFactory httpClientFactory,
                               ICacheSettingsRepository cacheSettingsRepository)
        {
            _httpClient = httpClientFactory.CreateClient(ApiConstants.HttpClientNames.CurrencyApi);
            _cacheSettingsRepository = cacheSettingsRepository;
        }

        public async Task<ApiInfoModel> GetApiInfoAsync(CancellationToken cancellationToken)
        {
            ApiStatusModel apiStatusFromJson = await GetApiStatusAsync(cancellationToken);

            CacheSettings cacheSettings = await _cacheSettingsRepository.GetCacheSettingsAsync(cancellationToken);

            return new ApiInfoModel
            {
                BaseCurrency = cacheSettings.BaseCurrency,
                NewRequestsAvailable = apiStatusFromJson.UsedRequests
                                        < apiStatusFromJson.TotalRequests,
            };
        }

        public async Task<ExchangeRateModel[]> GetAllCurrentCurrenciesAsync(
            string baseCurrency,
            CancellationToken cancellationToken)
        {
            await ThrowIfNoRequestsAvailableAsync(cancellationToken);

            NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
            requestQuery["base_currency"] = baseCurrency;

            string responseString = await GetAsStringFromClientAsync(
                ApiConstants.Uris.GetCurrency + requestQuery.ToString(),
                cancellationToken);

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
            await ThrowIfNoRequestsAvailableAsync(cancellationToken);

            NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
            requestQuery["base_currency"] = baseCurrency;
            requestQuery["date"] = date.ToString(ApiConstants.Formats.DateFormat);

            string responseString = await GetAsStringFromClientAsync(
                    ApiConstants.Uris.GetCurrencyHistorical + requestQuery.ToString(),
                    cancellationToken);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new AllExchangeRatesHistoricalJsonConverter() },
            };

            ExchangeRatesHistoricalModel exchangeRatesOnDate =
                JsonSerializer.Deserialize<ExchangeRatesHistoricalModel>(
                responseString, options);

            return exchangeRatesOnDate;
        }

        private async Task ThrowIfNoRequestsAvailableAsync(CancellationToken cancellationToken)
        {
            ApiStatusModel apiStatus = await GetApiStatusAsync(cancellationToken);

            if (apiStatus.UsedRequests > apiStatus.TotalRequests)
            {
                throw new ApiRequestLimitException(ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
            }
        }

        private async Task<string> GetAsStringFromClientAsync(string requestPath, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(requestPath, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception(ApiConstants.ErrorMessages.UnknownExceptionMessage);
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        private async Task<ApiStatusModel> GetApiStatusAsync(CancellationToken cancellationToken)
        {
            string responseString = await GetAsStringFromClientAsync(ApiConstants.Uris.GetStatus, cancellationToken);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new ApiStatusJsonConverter() },
            };

            return JsonSerializer.Deserialize<ApiStatusModel>(responseString, options);
        }
    }
}