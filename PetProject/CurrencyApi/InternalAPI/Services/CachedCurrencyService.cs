using Fuse8_ByteMinds.SummerSchool.InternalApi.Contracts;
using InternalAPI.Enums;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    public class CachedCurrencyService : ICachedCurrencyAPI
    {
        private readonly ICurrencyAPI _currencyAPI;

        public CachedCurrencyService(ICurrencyAPI currencyAPI)
        {
            _currencyAPI = currencyAPI;
        }

        public Task<CurrencyDTOModel> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyDTOModel> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
