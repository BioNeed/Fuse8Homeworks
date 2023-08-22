using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    public interface IFavouritesService
    {
        Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(CancellationToken cancellationToken);

        Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(string name, CancellationToken cancellationToken);

        Task TryAddFavouriteAsync(string name, string currency, string baseCurrency, CancellationToken cancellationToken);

        Task TryDeleteFavouriteAsync(string name, CancellationToken cancellationToken);

        Task TryUpdateFavouriteAsync(string name, string? newName, string? currency, string? baseCurrency, CancellationToken cancellationToken);
    }
}