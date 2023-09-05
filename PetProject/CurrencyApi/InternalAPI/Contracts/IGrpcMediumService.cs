using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Contracts.GrpcContracts;

namespace InternalAPI.Contracts
{
    /// <summary>
    /// Сервис для получения курсов валют для Grpc-сервиса
    /// </summary>
    public interface IGrpcMediumService
    {
        /// <summary>
        /// Получить текущий курс валют
        /// </summary>
        /// <param name="currency">Валюта, в которой узнать курс валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Текущий курс валют</returns>
        Task<ExchangeRateDTOModel> GetCurrentExchangeRateDtoAsync(string currency, CancellationToken cancellationToken);

        /// <summary>
        /// Получить текущий курс валют Избранного
        /// </summary>
        /// <param name="favouriteInfo">Избранное, для которого узнать курс валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение курса валют Избранного на данный момент</returns>
        Task<decimal> GetCurrentFavouriteExchangeRateAsync(FavouriteInfo favouriteInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Получить курс валют на определенную дату
        /// </summary>
        /// <param name="currency">Валюта, в которой узнать курс валют</param>
        /// <param name="dateTime">Дата, на которую узнать курс валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Курс валют на выбранную дату</returns>
        Task<ExchangeRateDTOModel> GetExchangeRateDtoOnDateAsync(string currency, DateTime dateTime, CancellationToken cancellationToken);

        /// <summary>
        /// Получить курс валют Избранного на определенную дату
        /// </summary>
        /// <param name="request">Избранное и дата, для которых узнать курс</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение курса валют Избранного на выбранную дату</returns>
        Task<decimal> GetFavouriteExchangeRateOnDateAsync(FavouriteOnDateRequest request, CancellationToken cancellationToken);
    }
}