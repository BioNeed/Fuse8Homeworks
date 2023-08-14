using System.ComponentModel.DataAnnotations;
using System.Globalization;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Exceptions;
using InternalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InternalAPI.Controllers;

/// <summary>
/// [InternalApi] Методы для работы с Currencyapi API
/// </summary>
[Route("currency")]
public class CurrencyController : ControllerBase
{
    private readonly IGettingApiConfigService _gettingApiConfigService;
    private readonly ICachedCurrencyAPI _cachedCurrencyService;

    public CurrencyController(IGettingApiConfigService gettingApiConfigService, ICachedCurrencyAPI cachedCurrencyService)
    {
        _gettingApiConfigService = gettingApiConfigService;
        _cachedCurrencyService = cachedCurrencyService;
    }

    /// <summary>
    /// Получить курс валют
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <param name="currencyType">(Необязателен) Код валюты, в которой узнать курс базовой валюты. Если не указан, используется RUB</param>
    /// <param name="cancellationToken">Токен отмены</param>
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
        [FromQuery] CurrencyType? currencyType,
        CancellationToken cancellationToken)
    {
        CurrencyType requestCurrencyType = currencyType ??
            Enum.Parse<CurrencyType>(currencyConfig.Value.DefaultCurrency, true);

        ExchangeRateDTOModel exchangeRateDTO = await _cachedCurrencyService
            .GetCurrentExchangeRateAsync(requestCurrencyType, cancellationToken);

        return new ExchangeRateModel
        {
            Code = exchangeRateDTO.CurrencyType.ToString(),
            Value = exchangeRateDTO.Value,
        };
    }

    /// <summary>
    /// Получить курс валют на выбранную дату
    /// </summary>
    /// <param name="currencyType">Код валюты, в которой узнать курс базовой валюты</param>
    /// <param name="dateString">Выбранная дата в формате yyyy-MM-dd</param>
    /// <param name="cancellationToken">Токен отмены</param>
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
        [FromRoute(Name = "currencyCode")] CurrencyType currencyType,
        [FromRoute(Name = "date")] string dateString,
        CancellationToken cancellationToken)
    {
        if (TryParseDate(dateString, out DateOnly date) == false)
        {
            throw new InvalidDateFormatException(ApiConstants.ErrorMessages.InvalidDateFormatExceptionMessage);
        }

        ExchangeRateDTOModel exchangeRateDTO = await _cachedCurrencyService
            .GetExchangeRateOnDateAsync(currencyType, date, cancellationToken);

        return new ExchangeRateHistoricalModel
        {
            Code = exchangeRateDTO.CurrencyType.ToString(),
            Date = dateString,
            Value = exchangeRateDTO.Value,
        };
    }

    /// <summary>
    /// Получить настройки приложения
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">
    /// Возвращает, если удалось получить настройки приложения
    /// </response>
    [Route("/settings")]
    [HttpGet]
    public async Task<CurrencyConfigurationModel> GetConfigSettingsAsync(
        [FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig,
        CancellationToken cancellationToken)
    {
        return await _gettingApiConfigService.GetApiConfigAsync(currencyConfig.Value, cancellationToken);
    }

    private bool TryParseDate(string dateString, out DateOnly date)
    {
        date = DateOnly.MinValue;
        if (DateTime.TryParseExact(dateString,
            ApiConstants.Formats.DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime dateTime) == false)
        {
            return false;
        }

        date = DateOnly.FromDateTime(dateTime);
        return true;
    }
}