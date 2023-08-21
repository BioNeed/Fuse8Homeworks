using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.DataAccess;
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
    }
}
