namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Курс валюты относительно валюты по умолчанию
    /// </summary>
    public record ExchangeRate
    {
        /// <summary>
        /// Код валюты
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// Курс валюты относительно валюты по умолчанию
        /// </summary>
        public decimal Value { get; init; }
    }
}
