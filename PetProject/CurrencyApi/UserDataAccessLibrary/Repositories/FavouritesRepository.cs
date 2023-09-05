using Microsoft.EntityFrameworkCore;
using UserDataAccessLibrary.Constants;
using UserDataAccessLibrary.Contracts;
using UserDataAccessLibrary.Database;
using UserDataAccessLibrary.Exceptions;
using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Repositories
{
    /// <inheritdoc cref="IFavouritesRepository"/>
    public class FavouritesRepository : IFavouritesRepository
    {
        private readonly UserDbContext _userDbContext;

        public FavouritesRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(
            CancellationToken cancellationToken)
        {
            return _userDbContext.Favourites.AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(
            string name, CancellationToken cancellationToken)
        {
            return _userDbContext.Favourites.AsNoTracking()
                .FirstOrDefaultAsync(
                    predicate: f => f.Name == name,
                    cancellationToken: cancellationToken);
        }

        public async Task AddFavouriteAsync(FavouriteExchangeRate favouriteToAdd,
                                            CancellationToken cancellationToken)
        {
            await _userDbContext.Favourites.AddAsync(favouriteToAdd, cancellationToken);

            await _userDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateFavouriteAsync(
            string name,
            FavouriteExchangeRate newFavourite,
            CancellationToken cancellationToken)
        {
            FavouriteExchangeRate favouriteToUpdate =
                await _userDbContext.Favourites.FirstAsync(
                                    predicate: f => f.Name == name,
                                    cancellationToken: cancellationToken);

            favouriteToUpdate.Name = newFavourite.Name;
            favouriteToUpdate.Currency = newFavourite.Currency;
            favouriteToUpdate.BaseCurrency = newFavourite.BaseCurrency;

            await _userDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteFavouriteAsync(
            string name,
            CancellationToken cancellationToken)
        {
            FavouriteExchangeRate? favouriteToDelete = await _userDbContext
                .Favourites.FirstOrDefaultAsync(
                    predicate: f => f.Name == name,
                    cancellationToken: cancellationToken);

            if (favouriteToDelete == null)
            {
                throw new DatabaseElementNotFoundException(
                    LibraryConstants.ErrorMessages.FavouriteNotFoundByNameExceptionMessage);
            }

            _userDbContext.Favourites.Remove(favouriteToDelete);

            await _userDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}