﻿using System.ComponentModel.DataAnnotations;
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
        public async Task AddFavourite(
            string newFavouriteName,
            [Required] CurrencyType currency,
            [Required] CurrencyType baseCurrency,
            CancellationToken cancellationToken)
        {
            await _favouritesService.TryAddFavouriteAsync(
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
        /// Возвращает, если не существует Избранного с таким именем
        /// </response>
        /// <response code="422">
        /// Возвращает, если
        /// 1. Валюта совпадает с базовой валютой.
        /// 2. Уже существует Избранное с таким именем
        /// 3. Уже существует Избранное с таким набором (Валюта, Базовая валюта)
        /// </response>
        /// <response code="500">
        /// Возвращает в случае других ошибок
        /// </response>
        [HttpPost("{favouriteName}")]
        public async Task UpdateFavourite(
            string favouriteName,
            string? newFavouriteName,
            CurrencyType? currency,
            CurrencyType? baseCurrency,
            CancellationToken cancellationToken)
        {
            string? currencyString = currency != null
                                    ? currency.ToString()
                                    : null;

            string baseCurrencyString = baseCurrency != null
                                        ? baseCurrency.ToString()
                                        : null;

            await _favouritesService.TryUpdateFavouriteAsync(
                name: favouriteName,
                newName: newFavouriteName,
                currency: currencyString,
                baseCurrency: baseCurrencyString,
                cancellationToken);
        }
    }
}
