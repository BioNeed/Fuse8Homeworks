using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace InternalAPI.Models
{
    public class CachedExchangeRates
    {
        private ExchangeRateDTOModel[]? _exchangeRates;

        public DateTime RelevantOnDate { get; set; }

        [StringLength(maximumLength: 3, MinimumLength = 3)]
        public string BaseCurrency { get; set; }

        public string ExchangeRatesJson { get; set; }

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
