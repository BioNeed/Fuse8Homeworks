using CurrenciesDataAccessLibrary.Models;

namespace CurrenciesDataAccessLibrary.Contracts
{
    /// <summary>
    /// Репозиторий для работы с таблицей настроек кэша
    /// </summary>
    public interface ICacheSettingsRepository
    {
        /// <summary>
        /// Получить настройки кэша
        /// </summary>
        /// <returns>Возвращает единственную запись в таблице</returns>
        CacheSettings GetCacheSettings();

        /// <inheritdoc cref="GetCacheSettings"/>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<CacheSettings> GetCacheSettingsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Записать новую базовую валюту кэша в таблицу
        /// </summary>
        /// <param name="newBaseCurrency">Новая базовая валюта кэша</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task SetBaseCurrencyAsync(string newBaseCurrency, CancellationToken cancellationToken);
    }
}