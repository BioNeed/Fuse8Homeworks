using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для управления настройками приложения
    /// </summary>
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Изменить валюту по умолчанию
        /// </summary>
        /// <param name="newDefaultCurrency">Новая валюта по умолчанию</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если удалось изменить валюту по умолчанию
        /// </response>
        [HttpPost("/settings/default_currency/{newDefaultCurrency}")]
        public async Task SetDefaultCurrencyAsync(
            CurrencyType newDefaultCurrency,
            CancellationToken cancellationToken)
        {
            await _settingsService.SetDefaultCurrencyAsync(
                newDefaultCurrency, cancellationToken);
        }

        /// <summary>
        /// Изменить количество знаков после запятой для округления
        /// </summary>
        /// <param name="newRoundCount">Новое значение - количество знаков после запятой у курса валют</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает, если удалось изменить количество знаков после запятой
        /// </response>
        [HttpPost("/settings/round_count/{newRoundCount}")]
        public async Task SetCurrencyRoundCountAsync(
            [Range(1, 8)] int newRoundCount,
            CancellationToken cancellationToken)
        {
            await _settingsService.SetCurrencyRoundCountAsync(
                newRoundCount, cancellationToken);
        }
    }
}
