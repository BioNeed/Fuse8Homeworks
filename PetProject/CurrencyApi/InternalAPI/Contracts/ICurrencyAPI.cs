using InternalAPI.Models;

namespace InternalAPI.Contracts;

/// <summary>
/// Сервис для работы с внешним API (CurrencyApi)
/// </summary>
public interface ICurrencyAPI
{
    /// <summary>
    /// Получает текущий курс для всех валют
    /// </summary>
    /// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список курсов валют</returns>
    Task<ExchangeRateModel[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken);

    /// <summary>
    /// Получает курс для всех валют, актуальный на <paramref name="date"/>
    /// </summary>
    /// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
    /// <param name="date">Дата, на которую нужно получить курс валют</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список курсов валют на дату</returns>
    Task<ExchangeRatesHistoricalModel> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken);
}
