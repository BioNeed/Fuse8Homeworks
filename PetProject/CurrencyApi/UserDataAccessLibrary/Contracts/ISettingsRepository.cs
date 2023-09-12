using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Contracts
{
    /// <summary>
    /// Репозиторий для работы с таблицей настроек PublicApi
    /// </summary>
    public interface ISettingsRepository
    {
        /// <summary>
        /// Получить настройки PublicApi (единственная запись в таблице)
        /// </summary>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Настройки PublicApi</returns>
        Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Задать новую валюту по умолчанию
        /// </summary>
        /// <param name="newDefaultCurrency">Новая валюта по умолчанию</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task SetDefaultCurrencyAsync(string newDefaultCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Задать новое значение - кол-во знаков после запятой для округления курсов валют
        /// </summary>
        /// <param name="newRoundCount">Новое кол-во знаков после запятой</param>
        /// <param name="cancellationToken">Токен отмены</param>ъ
        Task SetCurrencyRoundCountAsync(int newRoundCount, CancellationToken cancellationToken);
    }
}