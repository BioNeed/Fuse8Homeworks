﻿using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Enums;

namespace InternalAPI.Contracts;

/// <summary>
/// Сервис для работы с кэшем
/// </summary>
public interface ICachedCurrencyAPI
{
    /// <summary>
    /// Получает и проверяет базовую валюту кэша
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Корректную базовую валюту кэша</returns>
    Task<string> GetValidBaseCurrencyAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получает текущий курс
    /// </summary>
    /// <param name="currencyType">Валюта, для которой необходимо получить курс</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Текущий курс</returns>
    Task<ExchangeRateDTOModel> GetCurrentExchangeRateAsync(CurrencyType currencyType, CancellationToken cancellationToken);

    /// <summary>
    /// Получает курс валюты, актуальный на <paramref name="date"/>
    /// </summary>
    /// <param name="currencyType">Валюта, для которой необходимо получить курс</param>
    /// <param name="date">Дата, на которую нужно получить курс валют</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Курс на дату</returns>
    Task<ExchangeRateDTOModel> GetExchangeRateOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken);
}