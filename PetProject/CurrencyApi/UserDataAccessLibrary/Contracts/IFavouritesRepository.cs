using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Contracts
{
    /// <summary>
    /// Репозиторий для работы с таблицей Избранных курсов валют
    /// </summary>
    public interface IFavouritesRepository
    {
        /// <summary>
        /// Получить массив всех Избранных
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Массив всех Избранных</returns>
        Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить Избранное по названию
        /// </summary>
        /// <param name="name">Название для поиска</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Найденное Избранное (null, если не найдено)</returns>
        Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Добавить Избранное в таблицу
        /// </summary>
        /// <param name="favouriteToAdd">Избранное для добавления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task AddFavouriteAsync(FavouriteExchangeRate favouriteToAdd, CancellationToken cancellationToken);

        /// <summary>
        /// Обновить существующее Избранное
        /// </summary>
        /// <param name="name">Название избранного</param>
        /// <param name="newFavouriteInfo">Новые параметры Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task UpdateFavouriteAsync(string name, FavouriteExchangeRate newFavouriteInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Попробовать удалить Избранное
        /// </summary>
        /// <param name="name">Название Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task DeleteFavouriteAsync(string name, CancellationToken cancellationToken);
    }
}