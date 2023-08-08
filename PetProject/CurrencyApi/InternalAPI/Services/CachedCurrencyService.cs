using System.Globalization;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    public class CachedCurrencyService : ICachedCurrencyAPI
    {
        private readonly ICurrencyAPI _currencyAPI;
        private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromHours(2);

        public CachedCurrencyService(ICurrencyAPI currencyAPI)
        {
            _currencyAPI = currencyAPI;
        }

        public async Task<ExchangeRateDTOModel> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken)
        {
            string projectDirectoryPath = Directory
                .GetParent(Environment.CurrentDirectory)
                .Parent.Parent.FullName;

            IEnumerable<string> relativeFileNames = Directory
                .GetFiles(projectDirectoryPath
                    + ApiConstants.RelativePaths.CachedCurrenciesOnDates)
                .Select(f => Path.GetFileName(f));

            DateTime currentDateTime = DateTime.UtcNow;

            DateTime maxDateTime = DateTime.MinValue;
            string lastFileName = string.Empty;

            foreach (string relativeFileName in relativeFileNames)
            {
                DateTime fileDateTime = DateTime.ParseExact(relativeFileName,
                    ApiConstants.Formats.FileNameDateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                if (fileDateTime > maxDateTime)
                {
                    maxDateTime = fileDateTime;
                    lastFileName = relativeFileName;
                }
            }

            if (currentDateTime - maxDateTime < _cacheExpirationTime)
            {
                // найти нужное значение
                // вернуть значение
            }

            ExchangeRateModel[] exchangeRates = await _currencyAPI.
                GetAllCurrentCurrenciesAsync(currencyType.ToString(), cancellationToken);


            throw new NotImplementedException();
        }

        public async Task<ExchangeRateDTOModel> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
