using System.Globalization;
using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts.GrpcContracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// [PublicApi] Методы для работы с Internal API
/// </summary>
[Route("currency")]
public class GrpcCurrencyController : ControllerBase
{
    private readonly IGrpcCurrencyService _grpcCurrencyService;
    private readonly ISettingsService _settingsService;

    public GrpcCurrencyController(IGrpcCurrencyService currencyService,
                                  ISettingsService settingsService)
    {
        _grpcCurrencyService = currencyService;
        _settingsService = settingsService;
    }

    /// <summary>
    /// Получить курс валют
    /// </summary>
    /// <param name="currencyType">Код валюты, в которой узнать курс базовой валюты</param>
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
            (await _settingsService.GetApplicationSettingsAsync(cancellationToken))
            .DefaultCurrency;

        return await _grpcCurrencyService.GetExchangeRateAsync(
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
    /// <response code="422">
    /// Возвращает, если неверно введена дата
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
            Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            return null;
        }

        ExchangeRateModel exchangeRate = await _grpcCurrencyService.GetExchangeRateOnDateTimeAsync(
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
        return await _grpcCurrencyService.GetSettingsAsync(cancellationToken);
    }

    /// <summary>
    /// Получить курс Избранного
    /// </summary>
    /// <param name="favouriteName">Название Избранного, для которого узнать курс</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс Избранного
    /// </response>
    /// <response code="404">
    /// Возвращает, если нет Избранного с указанным именем
    /// </response>
    /// <response code="429">
    /// Возвращает, если больше не осталось доступных запросов
    /// </response>
    /// <response code="500">
    /// Возвращает в случае других ошибок
    /// </response>
    [HttpGet("favourite/{favouriteName}")]
    public async Task<ExchangeRateWithBaseModel> GetFavouriteExchangeRateAsync(
        string favouriteName,
        CancellationToken cancellationToken)
    {
        ExchangeRateWithBaseModel? exchangeRateWithBase = await _grpcCurrencyService
            .GetFavouriteExchangeRateAsync(favouriteName, cancellationToken);

        if (exchangeRateWithBase == null)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        return exchangeRateWithBase;
    }

    /// <summary>
    /// Получить курс Избранного на выбранную дату
    /// </summary>
    /// <param name="favouriteName">Название Избранного, для которого узнать курс</param>
    /// <param name="dateString">Дата, на которую узнать курс избранного</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">
    /// Возвращает, если удалось получить курс Избранного
    /// </response>
    /// <response code="404">
    /// Возвращает, если нет Избранного с указанным именем
    /// </response>
    /// <response code="422">
    /// Возвращает, если неверно введена дата
    /// </response>
    /// <response code="429">
    /// Возвращает, если больше не осталось доступных запросов
    /// </response>
    /// <response code="500">
    /// Возвращает в случае других ошибок
    /// </response>
    [HttpGet("favourite/{favouriteName}/{date}")]
    public async Task<ExchangeRateWithBaseHistoricalModel> GetFavouriteExchangeRateOnDateAsync(
        string favouriteName,
        [FromRoute(Name = "date")] string dateString,
        CancellationToken cancellationToken)
    {
        if (TryParseDateTime(dateString, out DateTime dateTime) == false)
        {
            Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            return null;
        }

        ExchangeRateWithBaseModel? exchangeRateWithBase = await _grpcCurrencyService
            .GetFavouriteExchangeRateOnDateAsync(favouriteName, dateTime, cancellationToken);

        if (exchangeRateWithBase == null)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return null;
        }

        return new ExchangeRateWithBaseHistoricalModel
        {
            BaseCurrency = exchangeRateWithBase.BaseCurrency,
            Currency = exchangeRateWithBase.Currency,
            Value = exchangeRateWithBase.Value,
            Date = dateString,
        };
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