using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Extensions
{
    public static class FavouriteExtensions
    {
        public static (bool isNameChanged,
            bool isCurrencyChanged,
            bool isBaseCurrencyChanged) CheckIfFavouriteChanged(
                this FavouriteExchangeRate favourite,
                string newName,
                string newCurrency,
                string newBaseCurrency)
        {
            bool isNameChanged = newName != null &&
                newName.Equals(
                    favourite.Name,
                    StringComparison.OrdinalIgnoreCase) == false;

            bool isCurrencyChanged = newCurrency != null &&
                newCurrency.Equals(
                    favourite.Currency,
                    StringComparison.OrdinalIgnoreCase) == false;

            bool isBaseCurrencyChanged = newBaseCurrency != null &&
                newBaseCurrency.Equals(
                    favourite.BaseCurrency,
                    StringComparison.OrdinalIgnoreCase) == false;

            return (isNameChanged, isCurrencyChanged, isBaseCurrencyChanged);
        }
    }
}
