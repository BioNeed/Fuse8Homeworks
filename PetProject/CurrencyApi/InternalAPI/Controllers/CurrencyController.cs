using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternalAPI.Controllers;

/// <summary>
/// Методы для работы с Currencyapi API
/// </summary>
[Route("currency")]
public class CurrencyController : ControllerBase
{
    private readonly ICachedCurrencyAPI _cachedCurrencyService;

    public CurrencyController(ICachedCurrencyAPI cachedCurrencyService)
    {
        _cachedCurrencyService = cachedCurrencyService;
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
    public async Task<ExchangeRateModel> GetExchangeRateAsync(
        [Required] CurrencyType currencyType,
        CancellationToken cancellationToken)
    {
        ExchangeRateDTOModel exchangeRateDTO = await _cachedCurrencyService
            .GetCurrentExchangeRateAsync(currencyType, cancellationToken);

        return new ExchangeRateModel
        {
            Code = exchangeRateDTO.Code.ToString(),
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
        if (TryParseDate(dateString, out DateOnly date) == false
            || date > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            return null;
        }

        ExchangeRateDTOModel exchangeRateDTO = await _cachedCurrencyService
            .GetExchangeRateOnDateAsync(currencyType, date, cancellationToken);

        return new ExchangeRateHistoricalModel
        {
            Code = exchangeRateDTO.Code.ToString(),
            Date = dateString,
            Value = exchangeRateDTO.Value,
        };
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