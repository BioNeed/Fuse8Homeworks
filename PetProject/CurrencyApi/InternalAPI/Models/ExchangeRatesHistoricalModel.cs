using InternalAPI.Models;

namespace InternalAPI.Models
{
    /// <summary>
    /// Курсы валют на конкретную дату
    /// </summary>
    public class ExchangeRatesHistoricalModel
    {
        /// <summary>
        /// Дата обновления данных
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Список курсов валют
        /// </summary>
        public List<ExchangeRateModel> Currencies { get; set; } = new List<ExchangeRateModel>();
    }
}
