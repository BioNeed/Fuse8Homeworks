using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Contracts.GrpcContracts;

namespace InternalAPI.Contracts
{
    public interface IGrpcMediumService
    {
        Task<ExchangeRateDTOModel> GetCurrentExchangeRateDtoAsync(string currency, CancellationToken cancellationToken);

        Task<decimal> GetCurrentFavouriteExchangeRateAsync(FavouriteInfo favouriteInfo, CancellationToken cancellationToken);

        Task<ExchangeRateDTOModel> GetExchangeRateDtoOnDateAsync(string currency, DateTime dateTime, CancellationToken cancellationToken);

        Task<decimal> GetFavouriteExchangeRateOnDateAsync(FavouriteOnDateRequest request, CancellationToken cancellationToken);
    }
}