using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Web;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.JsonConverters;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRateModel> GetExchangeRateAsync(
            CurrencyConfigurationModel currencyConfig,
            string? currencyCode)
        {
            if (await IsRequestAvailableAsync() == false)
            {
                throw new ApiRequestLimitException(ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
            }

            string requestDefaultCurrency = currencyCode ?? currencyConfig.DefaultCurrency;
            string requestBaseCurrency = currencyConfig.BaseCurrency;

            NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
            requestQuery["base_currency"] = requestBaseCurrency;
            requestQuery["currencies"] = requestDefaultCurrency;

            string requestPath = ApiConstants.Uris.GetCurrency + requestQuery.ToString();

            HttpResponseMessage response = await _httpClient.GetAsync(requestPath);

            if (response.IsSuccessStatusCode == false)
            {
                await RaiseExceptionAsync(response);
            }

            string responseString = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new ExchangeRateJsonConverter() },
            };

            var exchangeRate = JsonSerializer.Deserialize<ExchangeRateModel>(
                responseString, options);

            int currencyRoundCount = currencyConfig.CurrencyRoundCount;
            decimal roundedExchangeRate = decimal.Round(exchangeRate.Value, currencyRoundCount);

            return new ExchangeRateModel
            {
                Code = exchangeRate.Code,
                Value = roundedExchangeRate,
            };
        }

        public async Task<ExchangeRateHistoricalModel> GetExchangeRateByDateAsync(
            CurrencyConfigurationModel currencyConfig,
            string currencyCode,
            string dateString)
        {
            if (await IsRequestAvailableAsync() == false)
            {
                throw new ApiRequestLimitException(ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
            }

            string requestDefaultCurrency = currencyCode;
            string requestBaseCurrency = currencyConfig.BaseCurrency;

            NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
            requestQuery["base_currency"] = requestBaseCurrency;
            requestQuery["currencies"] = requestDefaultCurrency;
            requestQuery["date"] = dateString;

            string requestPath = ApiConstants.Uris.GetCurrencyHistorical + requestQuery.ToString();

            HttpResponseMessage response = await _httpClient.GetAsync(requestPath);

            if (response.IsSuccessStatusCode == false)
            {
                await RaiseExceptionAsync(response);
            }

            string responseString = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new ExchangeRateJsonConverter() },
            };

            var exchangeRate = JsonSerializer.Deserialize<ExchangeRateModel>(
                responseString, options);

            int currencyRoundCount = currencyConfig.CurrencyRoundCount;
            decimal roundedExchangeRate = decimal.Round(exchangeRate.Value, currencyRoundCount);

            return new ExchangeRateHistoricalModel
            {
                Code = exchangeRate.Code,
                Value = roundedExchangeRate,
                Date = dateString,
            };
        }

        public async Task<CurrencyConfigurationModel> GetSettingsAsync(
            CurrencyConfigurationModel currencyConfig)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(ApiConstants.Uris.GetStatus);

            if (response.IsSuccessStatusCode == false)
            {
                await RaiseExceptionAsync(response);
            }

            string responseString = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new ApiStatusJsonConverter() },
            };

            ApiStatusModel apiStatusFromJson =
                JsonSerializer.Deserialize<ApiStatusModel>(responseString, options);

            return currencyConfig with
            {
                RequestCount = apiStatusFromJson.UsedRequests,
                RequestLimit = apiStatusFromJson.TotalRequests,
            };
        }

        private async Task RaiseExceptionAsync(HttpResponseMessage response)
        {
            await HandleIfUnknownCurrencyAsync(response);

            throw new Exception(ApiConstants.ErrorMessages.UnknownExceptionMessage);
        }

        private async Task HandleIfUnknownCurrencyAsync(HttpResponseMessage badResponse)
        {
            if (badResponse.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                string responseString = await badResponse.Content.ReadAsStringAsync();

                if (responseString.Contains(ApiConstants.ErrorMessages.InvalidCurrencyMessage))
                {
                    throw new CurrencyNotFoundException(ApiConstants.ErrorMessages.InvalidCurrencyMessage);
                }
            }
        }

        private async Task<bool> IsRequestAvailableAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                ApiConstants.Uris.GetStatus);

            string responseString = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Converters = { new ApiStatusJsonConverter() },
            };

            ApiStatusModel apiStatusFromJson =
                JsonSerializer.Deserialize<ApiStatusModel>(responseString, options);

            return apiStatusFromJson.UsedRequests < apiStatusFromJson.TotalRequests;
        }
    }
}
