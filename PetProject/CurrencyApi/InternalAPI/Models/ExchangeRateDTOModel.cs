using InternalAPI.Enums;

namespace InternalAPI.Models
{
    /// <summary>
    /// Курс валюты
    /// </summary>
    public class ExchangeRateDTOModel
    {
        /// <summary>
        /// Валюта
        /// </summary>
        public CurrencyType CurrencyType { get; set; }

        /// <summary>
        /// Значение курса
        /// </summary>
        public decimal Value { get; set; }
    }
}
