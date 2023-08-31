using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Extensions
{
    public static class FavouriteExtensions
    {
        public static (bool isNameChanged, bool isCurrencyChanged, bool isBaseCurrencyChanged)
            CheckIfFavouriteChanged(this FavouriteExchangeRate favourite,
                                    string newName,
                                    string newCurrency,
                                    string newBaseCurrency)
        {
            bool isNameChanged = newName.IsNotNullAndNotEquals(favourite.Name);

            bool isCurrencyChanged = newCurrency
                                    .IsNotNullAndNotEquals(favourite.Currency);

            bool isBaseCurrencyChanged = newBaseCurrency
                                        .IsNotNullAndNotEquals(favourite.BaseCurrency);

            return (isNameChanged, isCurrencyChanged, isBaseCurrencyChanged);
        }
    }
}
