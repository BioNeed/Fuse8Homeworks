using System.Collections.Specialized;
using System.Web;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// Методы для работы с Currencyapi API
/// </summary>
[Route("currency")]
public class CurrencyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrencyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Получить курс валюты по умолчанию
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс валюты по умолчанию
    /// </response>
    [HttpGet]
    public async Task<ExchangeRateModel> GetDefaultCurrencyExchangeRate(
        [FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig)
    {
        string defaultCurrency = currencyConfig.Value.DefaultCurrency;
        string baseCurrency = currencyConfig.Value.BaseCurrency;

        string requestPath = ApiConstants.Uris.GetCurrency;
        NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
        requestQuery["base_currency"] = baseCurrency;
        requestQuery["currencies"] = defaultCurrency;
        HttpClient httpClient = _httpClientFactory.CreateClient(ApiConstants.HttpClientsNames.CurrencyApi);

        HttpResponseMessage? response = await httpClient.GetAsync(
            requestPath + requestQuery.ToString());

        response.EnsureSuccessStatusCode();
        string responseString = await response.Content.ReadAsStringAsync();

        JObject parsedExchangeRate = JObject.Parse(responseString);
        string currencyCode = parsedExchangeRate["data"][defaultCurrency]["code"].Value<string>();
        decimal currencyExchangeRate = parsedExchangeRate["data"][defaultCurrency]["value"].Value<decimal>();

        int currencyRoundCount = currencyConfig.Value.CurrencyRoundCount;
        decimal roundedExchangeRate = decimal.Round(currencyExchangeRate, currencyRoundCount);

        return new ExchangeRateModel
        {
            Code = currencyCode!,
            Value = roundedExchangeRate,
        };
    }

    /// <summary>
    /// Получить настройки приложения
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <response code="200">
    /// Возвращает, если удалось получить настройки приложения
    /// </response>
    [Route("/settings")]
    [HttpGet]
    public async Task<CurrencyConfigurationModel> GetSettings(
        [FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(ApiConstants.HttpClientsNames.CurrencyApi);
        HttpResponseMessage? response = await httpClient.GetAsync(ApiConstants.Uris.GetStatus);
        response.EnsureSuccessStatusCode();

        string responseString = await response.Content.ReadAsStringAsync();

        JObject status = JObject.Parse(responseString);
        int totalRequests = status["quotas"]["month"]["total"].Value<int>();
        int usedRequests = status["quotas"]["month"]["used"].Value<int>();

        CurrencyConfigurationModel settingsFull = currencyConfig.Value with
        {
            RequestCount = usedRequests,
            RequestLimit = totalRequests,
        };

        return settingsFull;
    }
}