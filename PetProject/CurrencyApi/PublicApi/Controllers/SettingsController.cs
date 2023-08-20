using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для редактирования настроек приложения
    /// </summary>
    public class SettingsController : Controller
    {
        private readonly IChangingSettingsService _changingSettingsService;

        public SettingsController(IChangingSettingsService changingSettingsService)
        {
            _changingSettingsService = changingSettingsService;
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
            await _changingSettingsService.SetDefaultCurrencyAsync(
                newDefaultCurrency, cancellationToken);
        }
    }
}
