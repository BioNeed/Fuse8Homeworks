using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Contracts
{
    public interface IFavouritesRepository
    {
        Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(CancellationToken cancellationToken);

        Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(string name, CancellationToken cancellationToken);

        Task AddFavouriteAsync(FavouriteExchangeRate favouriteToAdd, CancellationToken cancellationToken);

        Task UpdateFavouriteAsync(string name, FavouriteExchangeRate newFavouriteInfo, CancellationToken cancellationToken);

        Task TryDeleteFavouriteAsync(string name, CancellationToken cancellationToken);
    }
}