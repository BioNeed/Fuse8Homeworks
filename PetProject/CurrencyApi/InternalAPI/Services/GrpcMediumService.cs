using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Contracts;
using InternalAPI.Contracts.GrpcContracts;
using InternalAPI.Enums;

namespace InternalAPI.Services
{
    /// <summary>
    /// <inheritdoc cref="IGrpcMediumService"/>
    /// </summary>
    public class GrpcMediumService : IGrpcMediumService
    {
        private readonly ICachedCurrencyAPI _cachedCurrencyAPI;

        public GrpcMediumService(ICachedCurrencyAPI cachedCurrencyAPI)
        {
            _cachedCurrencyAPI = cachedCurrencyAPI;
        }

        public async Task<ExchangeRateDTOModel> GetCurrentExchangeRateDtoAsync(
            string currency,
            CancellationToken cancellationToken)
        {
            CurrencyType currencyType = Enum.Parse<CurrencyType>(
                currency, ignoreCase: true);

            return await _cachedCurrencyAPI.GetCurrentExchangeRateAsync(
                currencyType,
                cancellationToken);
        }

        public Task<ExchangeRateDTOModel> GetExchangeRateDtoOnDateAsync(
            string currency,
            DateTime dateTime,
            CancellationToken cancellationToken)
        {
            CurrencyType currencyType = Enum.Parse<CurrencyType>(
                currency, ignoreCase: true);

            DateOnly date = DateOnly.FromDateTime(dateTime);

            return _cachedCurrencyAPI.GetExchangeRateOnDateAsync(
                currencyType,
                date,
                cancellationToken);
        }

        public async Task<decimal> GetCurrentFavouriteExchangeRateAsync(
            FavouriteInfo favouriteInfo, CancellationToken cancellationToken)
        {
            ExchangeRateDTOModel favouriteExchangeRate = await
                GetCurrentExchangeRateDtoAsync(favouriteInfo.Currency,
                                               cancellationToken);

            string baseCurrency = await _cachedCurrencyAPI.GetValidBaseCurrencyAsync(cancellationToken);
            if (baseCurrency.Equals(favouriteInfo.BaseCurrency,
                                    StringComparison.OrdinalIgnoreCase))
            {
                return favouriteExchangeRate.Value;
            }

            ExchangeRateDTOModel favouriteBaseExchangeRate = await
               GetCurrentExchangeRateDtoAsync(favouriteInfo.BaseCurrency,
                                              cancellationToken);

            return favouriteExchangeRate.Value / favouriteBaseExchangeRate.Value;
        }

        public async Task<decimal> GetFavouriteExchangeRateOnDateAsync(
            FavouriteOnDateRequest request, CancellationToken cancellationToken)
        {
            DateTime requestDateTime = request.Date.ToDateTime();
            ExchangeRateDTOModel favouriteExchangeRate = await
                GetExchangeRateDtoOnDateAsync(request.FavouriteInfo.Currency,
                                              requestDateTime,
                                              cancellationToken);

            string baseCurrency = await _cachedCurrencyAPI.GetValidBaseCurrencyAsync(cancellationToken);
            if (baseCurrency.Equals(request.FavouriteInfo.BaseCurrency,
                                    StringComparison.OrdinalIgnoreCase))
            {
                return favouriteExchangeRate.Value;
            }

            ExchangeRateDTOModel favouriteBaseExchangeRate = await
                GetExchangeRateDtoOnDateAsync(request.FavouriteInfo.BaseCurrency,
                                              requestDateTime,
                                              cancellationToken);

            return favouriteExchangeRate.Value / favouriteBaseExchangeRate.Value;
        }
    }
}
