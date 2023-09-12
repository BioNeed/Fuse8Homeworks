using System.ComponentModel.DataAnnotations;

namespace UserDataAccessLibrary.Models
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Валюта по умолчанию, в которой узнать курс
        /// </summary>
        [StringLength(3, MinimumLength = 3)]
        public string DefaultCurrency { get; set; }

        /// <summary>
        /// Количество знаков после запятой, до которого следует
        /// округлять значение курса валют
        /// </summary>
        [Range(1, 8)]
        public int CurrencyRoundCount { get; set; }
    }
}
