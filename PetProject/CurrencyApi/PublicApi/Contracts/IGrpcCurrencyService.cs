using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    /// <summary>
    /// Сервис, посылащий запросы Grpc-серверу через Grpc-клиента
    /// </summary>
    public interface IGrpcCurrencyService
    {
        /// <summary>
        /// Получить курс определенной валюты
        /// </summary>
        /// <param name="currencyCode">Валюта, в которой узнать курс</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курс выбранной валюты</returns>
        Task<ExchangeRateModel> GetExchangeRateAsync(CurrencyType? currencyCode, CancellationToken cancellationToken);

        /// <summary>
        /// Получить курс определенной валюты на определенную дату
        /// </summary>
        /// <param name="currencyCode">Валюта, в которой узнать курс</param>
        /// <param name="dateTime">Дата актуальности курса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курс заданной валюты на заданную дату</returns>
        Task<ExchangeRateModel> GetExchangeRateOnDateAsync(string currencyCode, DateTime dateTime, CancellationToken cancellationToken);

        /// <summary>
        /// Получить курс валют Избранного
        /// </summary>
        /// <param name="favouriteName">Название Избранного курса валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курс валют Избранного с заданным названием</returns>
        Task<ExchangeRateWithBaseModel?> GetFavouriteExchangeRateAsync(string favouriteName, CancellationToken cancellationToken);

        /// <summary>
        /// Получить курс валют Избранного на определенную дату
        /// </summary>
        /// <param name="favouriteName">Название Избранного курса валют</param>
        /// <param name="dateTime">Дата актуальности курса Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курс валют Избранного на определенную дату</returns>
        Task<ExchangeRateWithBaseModel?> GetFavouriteExchangeRateOnDateAsync(string favouriteName, DateTime dateTime, CancellationToken cancellationToken);

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Настройки приложения</returns>
        Task<CurrencyConfigurationModel> GetSettingsAsync(CancellationToken cancellationToken);
    }
}