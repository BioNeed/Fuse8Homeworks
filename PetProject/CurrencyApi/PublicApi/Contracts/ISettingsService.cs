using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    /// <summary>
    /// Сервис для работы с репозиторием настроек PublicApi
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Получить настройки PublicApi
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Настройки PublicApi</returns>
        Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Задать новую валюту по умолчанию
        /// </summary>
        /// <param name="currencyType">Новая валюта по умолчанию</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task SetDefaultCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken);

        /// <summary>
        /// Задать новое значение - кол-во знаков после запятой при округлении курсов валют
        /// </summary>
        /// <param name="newRoundCount">Новое кол-во знаков после запятой</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task SetCurrencyRoundCountAsync(int newRoundCount, CancellationToken cancellationToken);
    }
}