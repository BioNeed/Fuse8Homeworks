using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Валюта по умолчанию, в которой узнать курс
        /// </summary>
        public CurrencyType DefaultCurrency { get; set; }

        /// <summary>
        /// Количество знаков после запятой, до которого следует
        /// округлять значение курса валют
        /// </summary>
        [Range(1, 8)]
        public int CurrencyRoundCount { get; set; }
    }
}
