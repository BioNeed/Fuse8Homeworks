namespace InternalAPI.Models
{
    /// <summary>
    /// Курс валюты относительно базовой валюты
    /// </summary>
    public class ExchangeRateModel
    {
        /// <summary>
        /// Код валюты, в которой нужно узнать курс
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Курс валюты относительно базовой валюты
        /// </summary>
        public decimal Value { get; set; }
    }
}
