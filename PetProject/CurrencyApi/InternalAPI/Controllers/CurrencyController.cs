using System.Globalization;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Exceptions;
using InternalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternalAPI.Controllers;

/// <summary>
/// Методы для работы с Currencyapi API
/// </summary>
[Route("currency")]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    /// <summary>
    /// Получить курс валют
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <param name="currencyCode">(Необязателен) Код валюты, в которой узнать курс базовой валюты. Если не указан, используется RUB</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс валюты
    /// </response>
    /// <response code="404">
    /// Возвращает, если не найдена указанная валюта
    /// </response>
    /// <response code="429">
    /// Возвращает, если больше не осталось доступных запросов
    /// </response>
    /// <response code="500">
    /// Возвращает в случае других ошибок
    /// </response>
    [HttpGet]
    public async Task<ExchangeRateModel> GetExchangeRateAsync(
        [FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig,
        [FromQuery] string? currencyCode)
    {
        return await _currencyService.GetExchangeRateAsync(currencyConfig.Value, currencyCode);
    }

    /// <summary>
    /// Получить курс валют на выбранную дату
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <param name="currencyCode">Код валюты, в которой узнать курс базовой валюты</param>
    /// <param name="dateString">Выбранная дата в формате yyyy-MM-dd</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс валюты на выбранную дату
    /// </response>
    /// <response code="404">
    /// Возвращает, если не найдена указанная валюта
    /// </response>
    /// <response code="429">
    /// Возвращает, если больше не осталось доступных запросов
    /// </response>
    /// <response code="500">
    /// Возвращает в случае других ошибок
    /// </response>
    [HttpGet("{currencyCode}/{date}")]
    public async Task<ExchangeRateHistoricalModel> GetExchangeRateByDateAsync(
        [FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig,
        [FromRoute] string currencyCode,
        [FromRoute(Name = "date")] string dateString)
    {
        if (IsDateValid(dateString) == false)
        {
            throw new InvalidDateFormatException(ApiConstants.ErrorMessages.InvalidDateFormatExceptionMessage);
        }

        return await _currencyService.GetExchangeRateByDateAsync(currencyConfig.Value, currencyCode, dateString);
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
    public async Task<CurrencyConfigurationModel> GetConfigSettingsAsync([FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig)
    {
        return await _currencyService.GetSettingsAsync(currencyConfig.Value);
    }

    private bool IsDateValid(string dateString)
    {
        if (DateTime.TryParseExact(dateString,
            ApiConstants.Formats.DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _) == false)
        {
            return false;
        }

        return true;
    }
}