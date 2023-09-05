using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    /// <summary>
    /// Сервис для работы с Избранными курсами валют
    /// </summary>
    public interface IFavouritesService
    {
        /// <summary>
        /// Получить все Избранные курсы валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Все Избранные курсы валют</returns>
        Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить Избранный курс валют по имени
        /// </summary>
        /// <param name="name">Имя, по которому осуществлять поиск</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Найденный Избранный курс валют</returns>
        Task<FavouriteExchangeRate?> GetFavouriteByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Попробовать добавить Избранное
        /// </summary>
        /// <param name="name">Имя Избранного</param>
        /// <param name="currency">Валюта Избранного</param>
        /// <param name="baseCurrency">Базовая валюта Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task AddFavouriteAsync(string name, string currency, string baseCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Попробовать удалить Избранное
        /// </summary>
        /// <param name="name">Название Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task DeleteFavouriteAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Попробовать обновить Избранное
        /// </summary>
        /// <param name="name">Название Избранного</param>
        /// <param name="newName">Новое имя Избранного</param>
        /// <param name="currency">Новая валюта Избранного</param>
        /// <param name="baseCurrency">Новая базовая валюта Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task UpdateFavouriteAsync(string name, string? newName, string? currency, string? baseCurrency, CancellationToken cancellationToken);
    }
}