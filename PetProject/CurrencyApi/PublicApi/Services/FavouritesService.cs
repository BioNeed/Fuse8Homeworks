using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Extensions;
using UserDataAccessLibrary.Contracts;
using UserDataAccessLibrary.Exceptions;
using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    /// <inheritdoc cref="IFavouritesService"/>
    public class FavouritesService : IFavouritesService
    {
        private const string ViolatingUniqueNameExceptionMessage =
            "Избранное с таким именем уже существует";

        private const string ViolatingUniqueCurrencyAndBaseCurrencyExceptionMessage =
            "Избранное с таким набором (currency, base_currency) уже существует";

        private const string ViolatingNotEqualCurrencyAndBaseCurrencyExceptionMessage =
            "У Избранного валюта не должна совпадать с базовой валютой";

        private readonly IFavouritesRepository _favouritesRepository;

        public FavouritesService(IFavouritesRepository favouritesRepository)
        {
            _favouritesRepository = favouritesRepository;
        }

        public Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(
            CancellationToken cancellationToken)
        {
            return _favouritesRepository.GetAllFavouritesAsync(cancellationToken);
        }

        public Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(
            string name, CancellationToken cancellationToken)
        {
            return _favouritesRepository.GetFavouriteByNameAsync(name, cancellationToken);
        }

        public async Task AddFavouriteAsync(string name,
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

            FavouriteExchangeRate[] favourites =
                await GetAllFavouritesAsync(cancellationToken);

            ThrowIfNotUniqueCurrencyAndBaseCurrency(favouriteToAdd, favourites);
            ThrowIfNotUniqueFavouriteName(favouriteToAdd, favourites);

            await _favouritesRepository.AddFavouriteAsync(
                favouriteToAdd, cancellationToken);
        }

        public async Task UpdateFavouriteAsync(string name,
                                               string? newName,
                                               string? currency,
                                               string? baseCurrency,
                                               CancellationToken cancellationToken)
        {
            FavouriteExchangeRate? oldFavourite = await GetFavouriteByNameAsync(name, cancellationToken);

            if (oldFavourite == null)
            {
                throw new DatabaseElementNotFoundException(
                    ApiConstants.ErrorMessages.FavouriteNotFoundByNameExceptionMessage);
            }

            (bool isNameChanged, bool isCurrencySetChanged) =
                oldFavourite.CheckIfFavouriteChanged(newName, currency, baseCurrency);

            if (isNameChanged == false &&
                isCurrencySetChanged == false)
            {
                return;
            }

            FavouriteExchangeRate updatedFavourite = new FavouriteExchangeRate
            {
                Name = newName ?? oldFavourite.Name,
                Currency = currency ?? oldFavourite.Currency,
                BaseCurrency = baseCurrency ?? oldFavourite.BaseCurrency,
            };

            await ThrowIfViolatesDbConstraints(isNameChanged,
                                               isCurrencySetChanged,
                                               updatedFavourite,
                                               cancellationToken);

            await _favouritesRepository.UpdateFavouriteAsync(name,
                                                             updatedFavourite,
                                                             cancellationToken);
        }

        public Task DeleteFavouriteAsync(string name, CancellationToken cancellationToken)
        {
            return _favouritesRepository.DeleteFavouriteAsync(name, cancellationToken);
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

        private async Task ThrowIfViolatesDbConstraints(bool isNameChanged,
                                                        bool isCurrencySetChanged,
                                                        FavouriteExchangeRate newFavourite,
                                                        CancellationToken cancellationToken)
        {
            FavouriteExchangeRate[]? favourites =
                await GetAllFavouritesAsync(cancellationToken);

            if (isNameChanged == true)
            {
                ThrowIfNotUniqueFavouriteName(newFavourite, favourites);
            }

            if (isCurrencySetChanged == true)
            {
                ThrowIfEqualCurrencyAndBaseCurrency(newFavourite);
                ThrowIfNotUniqueCurrencyAndBaseCurrency(newFavourite, favourites);
            }
        }
    }
}
