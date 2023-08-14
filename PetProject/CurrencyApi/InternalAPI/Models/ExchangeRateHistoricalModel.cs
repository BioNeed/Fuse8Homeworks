namespace InternalAPI.Models
{
    /// <summary>
    /// Курс валюты относительно базовой валюты
    /// </summary>
    public record ExchangeRateHistoricalModel
    {
        /// <summary>
        /// Код валюты, в которой нужно узнать курс
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// Курс валюты относительно базовой валюты
        /// </summary>
        public decimal Value { get; init; }

        /// <summary>
        /// Дата акутуальности курса
        /// </summary>
        public string Date { get; init; }
    }
}
