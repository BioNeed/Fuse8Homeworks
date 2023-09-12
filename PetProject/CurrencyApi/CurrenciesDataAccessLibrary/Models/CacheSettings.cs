using System.ComponentModel.DataAnnotations;

namespace CurrenciesDataAccessLibrary.Models
{
    /// <summary>
    /// Настройки кэша
    /// </summary>
    public class CacheSettings
    {
        /// <summary>
        /// Базовая валюта кэша
        /// </summary>
        [StringLength(3, MinimumLength = 3)]
        public string BaseCurrency { get; set; }
    }
}
