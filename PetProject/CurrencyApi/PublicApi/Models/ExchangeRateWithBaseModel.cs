namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Курс валюты с базовой валютой
    /// </summary>
    public class ExchangeRateWithBaseModel
    {
        /// <summary>
        /// Код валюты, в которой вычисляется курс
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Код базовой валюты, относительно которой вычисляется курс
        /// </summary>
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Курс валюты относительно базовой валюты
        /// </summary>
        public decimal Value { get; set; }
    }
}
