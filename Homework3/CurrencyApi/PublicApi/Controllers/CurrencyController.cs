using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services;
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
    private readonly IRequestSender _sender;

    public CurrencyController(IRequestSender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Получить курс валют
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <param name="currencyCode">(Необязателен) Код валюты, в которой узнать курс базовой валюты. Если не указан, используется RUB</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс валюты
    /// </response>
    [HttpGet]
    public async Task<ExchangeRateModel> GetExchangeRate(
        [FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig,
        [FromQuery] string? currencyCode)
    {
        string requestDefaultCurrency = currencyCode ?? currencyConfig.Value.DefaultCurrency;
        string requestBaseCurrency = currencyConfig.Value.BaseCurrency;

        NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
        requestQuery["base_currency"] = requestBaseCurrency;
        requestQuery["currencies"] = requestDefaultCurrency;

        string requestPath = ApiConstants.Uris.GetCurrency + requestQuery.ToString();

        HttpResponseMessage response = await _sender.SendRequestAsync(
            ApiConstants.HttpClientsNames.CurrencyApi,
            requestPath);

        string responseString = await response.Content.ReadAsStringAsync();

        JObject parsedExchangeRate = JObject.Parse(responseString);
        string responseCurrencyCode = parsedExchangeRate["data"][requestDefaultCurrency]["code"].Value<string>();
        decimal responseCurrencyExchangeRate = parsedExchangeRate["data"][requestDefaultCurrency]["value"].Value<decimal>();

        int currencyRoundCount = currencyConfig.Value.CurrencyRoundCount;
        decimal roundedExchangeRate = decimal.Round(responseCurrencyExchangeRate, currencyRoundCount);

        return new ExchangeRateModel
        {
            Code = responseCurrencyCode!,
            Value = roundedExchangeRate,
        };
    }

    /// <summary>
    /// Получить курс валют на выбранную дату
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <param name="dateValidator">Валидатор даты в формате yyyy-MM-dd</param>
    /// <param name="currencyCode">Код валюты, в которой узнать курс базовой валюты</param>
    /// <param name="dateString">Выбранная дата в формате yyyy-MM-dd</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс валюты на выбранную дату
    /// </response>
    [HttpGet("{currencyCode}/{date}")]
    public async Task<ExchangeRateHistoricalModel> GetExchangeRateByDate(
        [FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig,
        [FromServices] DateValidator dateValidator,
        [FromRoute] string? currencyCode,
        [FromRoute(Name = "date")] string dateString)
    {
        if (dateValidator.IsDateValid(dateString) == false)
        {
            throw new InvalidDateFormatException("Неверно указана дата. Ожидался формат: yyyy-MM-dd");
        }

        string requestDefaultCurrency = currencyCode;
        string requestBaseCurrency = currencyConfig.Value.BaseCurrency;

        NameValueCollection requestQuery = HttpUtility.ParseQueryString(string.Empty);
        requestQuery["base_currency"] = requestBaseCurrency;
        requestQuery["currencies"] = requestDefaultCurrency;
        requestQuery["date"] = dateString;

        string requestPath = ApiConstants.Uris.GetCurrencyHistorical + requestQuery.ToString();

        HttpResponseMessage response = await _sender.SendRequestAsync(
            ApiConstants.HttpClientsNames.CurrencyApi,
            requestPath);

        string responseString = await response.Content.ReadAsStringAsync();

        JObject parsedExchangeRate = JObject.Parse(responseString);
        string responseCurrencyCode = parsedExchangeRate["data"][requestDefaultCurrency]["code"].Value<string>();
        decimal responseCurrencyExchangeRate = parsedExchangeRate["data"][requestDefaultCurrency]["value"].Value<decimal>();

        int currencyRoundCount = currencyConfig.Value.CurrencyRoundCount;
        decimal roundedExchangeRate = decimal.Round(responseCurrencyExchangeRate, currencyRoundCount);

        return new ExchangeRateHistoricalModel
        {
            Code = responseCurrencyCode!,
            Value = roundedExchangeRate,
            Date = dateString,
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
        HttpResponseMessage response = await _sender.SendRequestAsync(
            ApiConstants.HttpClientsNames.CurrencyApi,
            ApiConstants.Uris.GetStatus);

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