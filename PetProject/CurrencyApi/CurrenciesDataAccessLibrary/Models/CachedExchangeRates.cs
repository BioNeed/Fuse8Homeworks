using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CurrenciesDataAccessLibrary.Models
{
    /// <summary>
    /// Набор курсов валют из кэша на определенную дату
    /// </summary>
    public class CachedExchangeRates
    {
        /// <summary>
        /// Курсы валют
        /// </summary>
        private ExchangeRateDTOModel[]? _exchangeRates;

        /// <summary>
        /// Дата и время актуальности курса
        /// </summary>
        public DateTime RelevantOnDate { get; set; }

        /// <summary>
        /// Базовая валюта, относительно которой заданы курсы валют набора
        /// </summary>
        [StringLength(maximumLength: 3, MinimumLength = 3)]
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Курсы валют в виде json строки
        /// </summary>
        public string ExchangeRatesJson { get; set; }

        /// <summary>
        /// Курсы валют
        /// </summary>
        public ExchangeRateDTOModel[] ExchangeRates
        {
            get
            {
                _exchangeRates ??= JsonSerializer
                            .Deserialize<ExchangeRateDTOModel[]>(ExchangeRatesJson)!;

                return _exchangeRates;
            }

            set
            {
                _exchangeRates = value;
                ExchangeRatesJson = JsonSerializer.Serialize(_exchangeRates);
            }
        }
    }
}
