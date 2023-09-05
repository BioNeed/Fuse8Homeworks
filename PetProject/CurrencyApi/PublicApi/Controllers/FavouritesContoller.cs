using System.ComponentModel.DataAnnotations;
using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Microsoft.AspNetCore.Mvc;
using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для управления Избранными курсами валют
    /// </summary>
    [Route("favourites")]
    public class FavouritesContoller : ControllerBase
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
        /// Возвращает курс, если удалось его получить.
        /// Ничего не возвращает, если не существует Избранного с указанным именем.
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpGet]
        public Task<FavouriteExchangeRate[]> GetAllFavouritesAsync(
            CancellationToken cancellationToken)
        {
            return _favouritesService
                    .GetAllFavouritesAsync(cancellationToken);
        }

        /// <summary>
        /// Получить Избранное по названию
        /// </summary>
        /// <param name="favouriteName">Название Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает курс, если удалось его получить.
        /// </response>
        /// <response code="404">
        /// Возвращает, если не найдено Избранное с указанным именем
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
        /// <param name="currency">Валюта Избранного (должны отличаться с базовой валютой)</param>
        /// <param name="baseCurrency">Базовая валюта Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если удалось добавить новое Избранное
        /// </response>
        /// <response code="422">
        /// Возвращает, если
        /// 1. Валюта совпадает с базовой валютой.
        /// 2. Уже существует Избранное с таким новым именем.
        /// 3. Уже существует Избранное с таким набором (Валюта, Базовая валюта).
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpPut("{newFavouriteName}")]
        public Task AddFavouriteAsync(
            string newFavouriteName,
            [Required] CurrencyType currency,
            [Required] CurrencyType baseCurrency,
            CancellationToken cancellationToken)
        {
            return _favouritesService.AddFavouriteAsync(
                name: newFavouriteName,
                currency: currency.ToString(),
                baseCurrency: baseCurrency.ToString(),
                cancellationToken);
        }

        /// <summary>
        /// Изменить существующее Избранное
        /// </summary>
        /// <param name="favouriteName">Название Избранного</param>
        /// <param name="newFavouriteName">(Необязательно) Новое название Избранного</param>
        /// <param name="currency">(Необязательно) Новая валюта Избранного (должны отличаться с базовой валютой)</param>
        /// <param name="baseCurrency">(Необязательно) Новая базовая валюта Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если
        /// 1. Удалось изменить Избранное
        /// 2. Пользователь не ввел данные для изменения
        /// 3. Пользователь ввел данные для изменения, которые совпадают с уже существующими данными
        /// </response>
        /// <response code="404">
        /// Возвращает, если не найдено Избранного с таким именем
        /// </response>
        /// <response code="422">
        /// Возвращает, если
        /// 1. Валюта совпадает с базовой валютой.
        /// 2. Уже существует Избранное с указанным новым именем
        /// 3. Уже существует Избранное с таким набором (Валюта, Базовая валюта)
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpPost("{favouriteName}")]
        public Task UpdateFavouriteAsync(
            string favouriteName,
            string? newFavouriteName,
            CurrencyType? currency,
            CurrencyType? baseCurrency,
            CancellationToken cancellationToken)
        {
            string? currencyString = currency?.ToString();
            string? baseCurrencyString = baseCurrency?.ToString();

            return _favouritesService.UpdateFavouriteAsync(
                                        name: favouriteName,
                                        newName: newFavouriteName,
                                        currency: currencyString,
                                        baseCurrency: baseCurrencyString,
                                        cancellationToken);
        }

        /// <summary>
        /// Удалить существующее Избранное
        /// </summary>
        /// <param name="favouriteName">Название Избранного</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если удалось удалить Избранное
        /// </response>
        /// <response code="404">
        /// Возвращает, если не существует Избранного с таким именем
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpDelete("{favouriteName}")]
        public Task DeleteFavouriteAsync(
            string favouriteName,
            CancellationToken cancellationToken)
        {
            return _favouritesService.DeleteFavouriteAsync(favouriteName, cancellationToken);
        }
    }
}
