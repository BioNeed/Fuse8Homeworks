using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.DataAccess;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class FavouritesRepository : IFavouritesRepository
    {
        private readonly UserDbContext _userDbContext;

        public FavouritesRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(
            CancellationToken cancellationToken)
        {
            return await _userDbContext.Favourites.AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public async Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(
            string name, CancellationToken cancellationToken)
        {
            return await _userDbContext.Favourites.AsNoTracking()
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

        public async Task TryDeleteFavouriteAsync(
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
                    ApiConstants.ErrorMessages.FavouriteNotFoundByNameExceptionMessage);
            }

            _userDbContext.Favourites.Remove(favouriteToDelete);

            await _userDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}