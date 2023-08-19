using InternalAPI.DataAccess;

namespace InternalAPI.Services
{
    public class ExchangeRatesRepository
    {
        private readonly CurrenciesDbContext _currenciesDbContext;

        public ExchangeRatesRepository(CurrenciesDbContext currenciesDbContext)
        {
            _currenciesDbContext = currenciesDbContext;
        }



    }
}
