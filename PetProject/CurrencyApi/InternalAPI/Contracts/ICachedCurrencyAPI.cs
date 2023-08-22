using InternalAPI.Enums;
using InternalAPI.Models;

namespace InternalAPI.Contracts;

public interface ICachedCurrencyAPI
{
    /// <summary>
    /// Базовая валюта, относительно которой строится кэш
    /// </summary>
    public string BaseCurrency { get; }

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