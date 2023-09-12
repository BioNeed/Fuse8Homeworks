using System.ComponentModel.DataAnnotations;

namespace UserDataAccessLibrary.Models
{
    /// <summary>
    /// Избранный курс валют
    /// </summary>
    public class FavouriteExchangeRate
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Валюта, в которой узнать курс
        /// </summary>
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        /// <summary>
        /// Валюта, относительно которой узнать курс
        /// </summary>
        [StringLength(3, MinimumLength = 3)]
        public string BaseCurrency { get; set; }
    }
}
