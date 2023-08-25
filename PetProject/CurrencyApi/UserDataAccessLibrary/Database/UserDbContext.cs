using Microsoft.EntityFrameworkCore;
using UserDataAccessLibrary.Constants;
using UserDataAccessLibrary.Database.Configurations;
using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<Settings> Settings { get; set; }

        public DbSet<FavouriteExchangeRate> Favourites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(LibraryConstants.SchemaNames.User);
            modelBuilder.ApplyConfigurationsFromAssembly(
                assembly: GetType().Assembly,
                predicate: p => p.Namespace == typeof(SettingsConfiguration).Namespace);
        }
    }
}
