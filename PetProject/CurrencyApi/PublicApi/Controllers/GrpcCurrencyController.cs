using System.Globalization;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// [PublicApi] Методы для работы с Currencyapi API
/// </summary>
[Route("currency")]
public class GrpcCurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;
    private readonly IOptionsSnapshot<CurrencyConfigurationModel> _configuration;

    public GrpcCurrencyController(ICurrencyService currencyService, IOptionsSnapshot<CurrencyConfigurationModel> configuration)
    {
        _currencyService = currencyService;
        _configuration = configuration;
    }

    /// <summary>
    /// Получить курс валют
    /// </summary>
    /// <param name="currencyType">Код валюты, в которой узнать курс базовой валюты. Если не указан, используется RUB</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс валюты
    /// </response>
    /// <response code="429">
    /// Возвращает, если больше не осталось доступных запросов
    /// </response>
    /// <response code="500">
    /// Возвращает в случае других ошибок
    /// </response>
    [HttpGet]
    public async Task<ExchangeRateModel> GetCurrentExchangeRateAsync(
        CurrencyType? currencyType,
        CancellationToken cancellationToken)
    {
        CurrencyType requestCurrencyType = currencyType ??
            Enum.Parse<CurrencyType>(_configuration.Value.DefaultCurrency, true);

        return await _currencyService.GetExchangeRateAsync(
            requestCurrencyType.ToString(), cancellationToken);
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
        if (TryParseDateTime(dateString, out DateTime dateTime) == false)
        {
            throw new InvalidDateFormatException(ApiConstants.ErrorMessages.InvalidDateFormatExceptionMessage);
        }

        ExchangeRateModel exchangeRate = await _currencyService.GetExchangeRateOnDateTimeAsync(
            currencyType.ToString(),
            dateTime,
            cancellationToken);

        return new ExchangeRateHistoricalModel
        {
            Code = exchangeRate.Code,
            Date = dateString,
            Value = exchangeRate.Value,
        };
    }

    /// <summary>
    /// Получить настройки приложения
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">
    /// Возвращает, если удалось получить настройки приложения
    /// </response>
    [Route("/settings")]
    [HttpGet]
    public async Task<CurrencyConfigurationModel> GetConfigSettingsAsync(CancellationToken cancellationToken)
    {
        return await _currencyService.GetSettingsAsync(cancellationToken);
    }

    private bool TryParseDateTime(string dateString, out DateTime dateTime)
    {
        if (DateTime.TryParseExact(dateString,
            ApiConstants.Formats.DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out dateTime) == false)
        {
            return false;
        }

        return true;
    }
}