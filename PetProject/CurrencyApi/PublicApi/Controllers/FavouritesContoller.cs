using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для управления Избранными курсами валют
    /// </summary>
    [Route("favourites")]
    public class FavouritesContoller : Controller
    {
        private readonly IFavouritesService _favouritesService;

        public FavouritesContoller(IFavouritesService favouritesService)
        {
            _favouritesService = favouritesService;
        }

        /// <summary>
        /// Получить все Избранные
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если удалось получить курс валюты
        /// </response>
        /// <response code="404">
        /// Возвращает, если нет ни одного Избранного
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpGet]
        public async Task<FavouriteExchangeRate[]> GetFavouriteByNameAsync(
            CancellationToken cancellationToken)
        {
            FavouriteExchangeRate[] allFavourites = await _favouritesService
                .GetAllFavouritesAsync(cancellationToken);

            if (allFavourites.Any() == false)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            return allFavourites;
        }

        /// <summary>
        /// Получить Избранное по названию
        /// </summary>
        /// <param name="favouriteName">Название Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если удалось получить курс валюты
        /// </response>
        /// <response code="404">
        /// Возвращает, если не найдено Избранное (Избранного с таким именем не существует)
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpGet("{favouriteName}")]
        public async Task<FavouriteExchangeRate> GetFavouriteByNameAsync(
            string favouriteName,
            CancellationToken cancellationToken)
        {
            FavouriteExchangeRate? favourite = await _favouritesService
                .GetFavouriteByNameAsync(favouriteName, cancellationToken);

            if (favourite == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            return favourite;
        }

        /// <summary>
        /// Добавить новое Избранное
        /// </summary>
        /// <param name="newFavouriteName">Название нового Избранного</param>
        /// <param name="currency">Валюта Избранного</param>
        /// <param name="baseCurrency">Базовая валюта Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если удалось добавить новое Избранное
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpPut("{newFavouriteName}")]
        public async Task AddFavourite(
            string newFavouriteName,
            CurrencyType currency,
            CurrencyType baseCurrency,
            CancellationToken cancellationToken)
        {
            await _favouritesService
                .GetFavouriteByNameAsync(favouriteName, cancellationToken);
        }

    }
}
