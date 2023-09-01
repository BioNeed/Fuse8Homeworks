using CurrenciesDataAccessLibrary.Models;

namespace CurrenciesDataAccessLibrary.Contracts
{
    /// <summary>
    /// Репозиторий для работы с кэшированными курсами валют
    /// </summary>
    public interface IExchangeRatesRepository
    {
        /// <summary>
        /// Получить последние курсы валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Последние курсы валют</returns>
        Task<CachedExchangeRates?> GetLastCachedExchangeRatesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить курсы валют на определенную дату
        /// </summary>
        /// <param name="date">Дата, на которую получить курсы валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курсы валют на выбранную дату</returns>
        Task<CachedExchangeRates?> GetHistoricalCachedExchangeRatesAsync(
            DateOnly date,
            CancellationToken cancellationToken);

        /// <summary>
        /// Сохранить курсы валют в таблицу
        /// </summary>
        /// <param name="baseCurrency">Базовая валюта, относительно которой заданы все курсы валют в наборе</param>
        /// <param name="exchangeRates">Курсы валют</param>
        /// <param name="relevantOnDate">Дата актуальности курсов</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task SaveCacheDataAsync(string baseCurrency,
                                ExchangeRateDTOModel[] exchangeRates,
                                DateTime relevantOnDate,
                                CancellationToken cancellationToken);

        /// <summary>
        /// Получить все наборы кешированных курсов валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Все наборы кешированных курсов валют</returns>
        Task<CachedExchangeRates[]> GetAllCachedExchangeRatesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Обновить набор кешированных курсов валют на определенную дату и время
        /// </summary>
        /// <param name="relevantOnDate">Дата и время актуальности курса</param>
        /// <param name="newBaseCurrency">Новая базовая валюта набора</param>
        /// <param name="newExchangeRates">Новые курсы валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task UpdateCachedExchangeRatesAsync(DateTime relevantOnDate, string newBaseCurrency, ExchangeRateDTOModel[] newExchangeRates, CancellationToken cancellationToken);
    }
}