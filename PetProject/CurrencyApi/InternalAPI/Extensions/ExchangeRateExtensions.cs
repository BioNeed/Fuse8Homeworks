using InternalAPI.Models;

namespace InternalAPI.Extensions
{
    public static class ExchangeRateExtensions
    {
        public static ExchangeRateDTOModel[] MapExchangeRatesToDTOs(
            this IEnumerable<ExchangeRateModel> exchangeRatesDTO)
        {
            int count = exchangeRatesDTO.Count();

            ExchangeRateDTOModel[] result = new ExchangeRateDTOModel[count];

            int i = 0;
            foreach (ExchangeRateModel exchangeRateDTO in exchangeRatesDTO)
            {
                result[i] = new ExchangeRateDTOModel
                {
                    Code = exchangeRateDTO.Code,
                    Value = exchangeRateDTO.Value,
                };

                i++;
            }

            return result;
        }
    }
}
