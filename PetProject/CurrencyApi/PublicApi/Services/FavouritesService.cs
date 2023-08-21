using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class FavouritesService : IFavouritesService
    {
        private const string ViolatingUniqueNameExceptionMessage =
            "Избранное с таким именем уже существует";

        private const string ViolatingUniqueCurrencyAndBaseCurrencyExceptionMessage =
            "Избранное с таким набором (currency, base_currency) уже существует";

        private const string ViolatingNotEqualCurrencyAndBaseCurrencyExceptionMessage =
            "У Избранного валюта не должна совпадать с базовой валютой";

        private const string FavouriteWithSpecifiedNameNotFoundExceptionMessage =
            "Избранное с указанным именем не найдено";

        private readonly IFavouritesRepository _favouritesRepository;

        public FavouritesService(IFavouritesRepository favouritesRepository)
        {
            _favouritesRepository = favouritesRepository;
        }

        public async Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(
            CancellationToken cancellationToken)
        {
            return await _favouritesRepository.GetAllFavouritesAsync(cancellationToken);
        }

        public async Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(
            string name, CancellationToken cancellationToken)
        {
            return await _favouritesRepository.GetFavouriteByNameAsync(name, cancellationToken);
        }

        public async Task TryAddFavouriteAsync(string name,
                                            string currency,
                                            string baseCurrency,
                                            CancellationToken cancellationToken)
        {
            FavouriteExchangeRate favouriteToAdd = new FavouriteExchangeRate
            {
                Name = name,
                Currency = currency,
                BaseCurrency = baseCurrency,
            };

            ThrowIfEqualCurrencyAndBaseCurrency(favouriteToAdd);

            FavouriteExchangeRate[]? favourites =
                await GetAllFavouritesAsync(cancellationToken);

            ThrowIfNotUniqueCurrencyAndBaseCurrency(favouriteToAdd, favourites);
            ThrowIfNotUniqueFavouriteName(favouriteToAdd, favourites);

            await _favouritesRepository.AddFavouriteAsync(
                favouriteToAdd, cancellationToken);
        }

        public async Task TryUpdateFavouriteAsync(string name,
                                                  string? newName,
                                                  string? currency,
                                                  string? baseCurrency,
                                                  CancellationToken cancellationToken)
        {
            FavouriteExchangeRate? oldFavourite = await GetFavouriteByNameAsync(name, cancellationToken);

            if (oldFavourite == null)
            {
                throw new DatabaseElementNotFoundException(
                    FavouriteWithSpecifiedNameNotFoundExceptionMessage);
            }

            bool changedName = newName != null &&
                newName.Equals(
                    oldFavourite.Name,
                    StringComparison.OrdinalIgnoreCase) == false;

            bool changedCurrency = currency != null &&
                currency.Equals(
                    oldFavourite.Currency,
                    StringComparison.OrdinalIgnoreCase) == false;

            bool changedBaseCurrency = baseCurrency != null &&
                baseCurrency.Equals(
                    oldFavourite.BaseCurrency,
                    StringComparison.OrdinalIgnoreCase) == false;

            if (changedName == false &&
                changedCurrency == false &&
                changedBaseCurrency == false)
            {
                return;
            }

            FavouriteExchangeRate newFavourite = new FavouriteExchangeRate
            {
                Name = newName ?? oldFavourite.Name,
                Currency = changedCurrency
                    ? currency!
                    : oldFavourite!.Currency,
                BaseCurrency = changedBaseCurrency
                    ? baseCurrency!
                    : oldFavourite!.BaseCurrency,
            };

            FavouriteExchangeRate[]? favourites =
                await GetAllFavouritesAsync(cancellationToken);

            if (changedName == true)
            {
                ThrowIfNotUniqueFavouriteName(newFavourite, favourites);
            }

            if (changedCurrency == true ||
                changedBaseCurrency == true)
            {
                ThrowIfEqualCurrencyAndBaseCurrency(newFavourite);
                ThrowIfNotUniqueCurrencyAndBaseCurrency(newFavourite, favourites);
            }

            await _favouritesRepository.UpdateFavouriteAsync(
                name,
                newFavourite,
                cancellationToken);
        }

        private void ThrowIfEqualCurrencyAndBaseCurrency(FavouriteExchangeRate favourite)
        {
            if (favourite.Currency.Equals(favourite.BaseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                throw new ViolatingDatabaseConstraintsException(
                        ViolatingNotEqualCurrencyAndBaseCurrencyExceptionMessage);
            }
        }

        private void ThrowIfNotUniqueCurrencyAndBaseCurrency(
            FavouriteExchangeRate favouriteToValidate,
            FavouriteExchangeRate[] favourites)
        {
            foreach (FavouriteExchangeRate favourite in favourites)
            {
                if (favouriteToValidate.Currency.Equals(favourite.Currency, StringComparison.OrdinalIgnoreCase) &&
                    favouriteToValidate.BaseCurrency.Equals(favourite.BaseCurrency, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ViolatingDatabaseConstraintsException(
                        ViolatingUniqueCurrencyAndBaseCurrencyExceptionMessage);
                }
            }
        }

        private void ThrowIfNotUniqueFavouriteName(
            FavouriteExchangeRate favouriteToValidate,
            FavouriteExchangeRate[] favourites)
        {
            foreach (FavouriteExchangeRate favourite in favourites)
            {
                if (favouriteToValidate.Name.Equals(favourite.Name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ViolatingDatabaseConstraintsException(
                        ViolatingUniqueNameExceptionMessage);
                }
            }
        }
    }
}
