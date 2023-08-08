using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

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
        public ExchangeRateModel[] Currencies { get; set; }
    }
}
