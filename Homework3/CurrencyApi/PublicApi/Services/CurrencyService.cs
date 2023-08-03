using System.Collections.Specialized;
using System.Net;
using System.Web;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Newtonsoft.Json.Linq;

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

            JObject parsedExchangeRate = JObject.Parse(responseString);
            string responseCurrencyCode = parsedExchangeRate["data"][requestDefaultCurrency]["code"].Value<string>();
            decimal responseCurrencyExchangeRate = parsedExchangeRate["data"][requestDefaultCurrency]["value"].Value<decimal>();

            int currencyRoundCount = currencyConfig.CurrencyRoundCount;
            decimal roundedExchangeRate = decimal.Round(responseCurrencyExchangeRate, currencyRoundCount);

            return new ExchangeRateModel
            {
                Code = responseCurrencyCode!,
                Value = roundedExchangeRate,
            };
        }

        public async Task<ExchangeRateHistoricalModel> GetExchangeRateByDateAsync(
            CurrencyConfigurationModel currencyConfig,
            string? currencyCode,
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

            JObject parsedExchangeRate = JObject.Parse(responseString);
            string responseCurrencyCode = parsedExchangeRate["data"][requestDefaultCurrency]["code"].Value<string>();
            decimal responseCurrencyExchangeRate = parsedExchangeRate["data"][requestDefaultCurrency]["value"].Value<decimal>();

            int currencyRoundCount = currencyConfig.CurrencyRoundCount;
            decimal roundedExchangeRate = decimal.Round(responseCurrencyExchangeRate, currencyRoundCount);

            return new ExchangeRateHistoricalModel
            {
                Code = responseCurrencyCode!,
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

            JObject status = JObject.Parse(responseString);
            int totalRequests = status["quotas"]["month"]["total"].Value<int>();
            int usedRequests = status["quotas"]["month"]["used"].Value<int>();

            CurrencyConfigurationModel settingsFull = currencyConfig with
            {
                RequestCount = usedRequests,
                RequestLimit = totalRequests,
            };

            return settingsFull;
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

                JObject parsedBadResponse = JObject.Parse(responseString);
                IEnumerable<JToken> errorDescriptions = parsedBadResponse["errors"]["currencies"].Values();

                foreach (JToken errorDescription in errorDescriptions)
                {
                    if (errorDescription?.Value<string>() == ApiConstants.ErrorMessages.InvalidCurrencyMessage)
                    {
                        throw new CurrencyNotFoundException(ApiConstants.ErrorMessages.InvalidCurrencyMessage);
                    }
                }
            }
        }

        private async Task<bool> IsRequestAvailableAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                ApiConstants.Uris.GetStatus);

            string responseString = await response.Content.ReadAsStringAsync();

            JObject status = JObject.Parse(responseString);
            int totalRequests = status["quotas"]["month"]["total"].Value<int>();
            int usedRequests = status["quotas"]["month"]["used"].Value<int>();

            return usedRequests < totalRequests;
        }
    }
}
