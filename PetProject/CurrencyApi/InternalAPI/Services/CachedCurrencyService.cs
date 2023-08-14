using System.Globalization;
using System.Text.Json;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Models;
using Microsoft.Extensions.Options;

namespace InternalAPI.Services
{
    public class CachedCurrencyService : ICachedCurrencyAPI
    {
        private const string JsonExtension = ".json";

        private readonly string _baseCurrency;
        private readonly ICurrencyAPI _currencyAPI;
        private readonly TimeSpan _cacheExpirationTime;
        private readonly string _cachedCurrenciesDirectoryPath =
            Environment.CurrentDirectory
                + ApiConstants.RelativePaths.CachedCurrenciesOnDates;

        public CachedCurrencyService(ICurrencyAPI currencyAPI,
            IOptionsSnapshot<ApiSettingsModel> apiSettings,
            IOptionsSnapshot<CurrencyConfigurationModel> apiConfig)
        {
            _currencyAPI = currencyAPI;
            _cacheExpirationTime = TimeSpan.FromHours(apiSettings.Value.CacheExpirationTimeInHours);
            _baseCurrency = apiConfig.Value.BaseCurrency;
        }

        public async Task<ExchangeRateDTOModel> GetCurrentExchangeRateAsync(
            CurrencyType currencyType, CancellationToken cancellationToken)
        {
            IEnumerable<string> cacheDateTimeStrings = Directory
                .GetFiles(_cachedCurrenciesDirectoryPath)
                .Select(f => Path.GetFileNameWithoutExtension(f));

            DateTime currentDateTime = DateTime.UtcNow;

            if (cacheDateTimeStrings.Any() == true)
            {
                DateTime latestDateTime = FindLatestCacheDateTime(cacheDateTimeStrings);

                if (currentDateTime - latestDateTime < _cacheExpirationTime)
                {
                    string relativeFileName = latestDateTime
                        .ToString(ApiConstants.Formats.FileNameDateTimeFormat) + JsonExtension;

                    ExchangeRateModel[] exchangeRatesFromFile =
                        ReadCachedExchangeRatesFromFile(relativeFileName);

                    return FindExchangeRateDTOByType(currencyType, exchangeRatesFromFile);
                }
            }

            ExchangeRateModel[] currentExchangeRates = await _currencyAPI.
                GetAllCurrentCurrenciesAsync(_baseCurrency, cancellationToken);

            CacheExchangeRatesToFile(currentDateTime, currentExchangeRates);

            return FindExchangeRateDTOByType(currencyType, currentExchangeRates);
        }

        public async Task<ExchangeRateDTOModel> GetExchangeRateOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
        {
            string dateString = date.ToString(ApiConstants.Formats.DateFormat);

            IEnumerable<string> cacheDateTimeStringsOnDate = Directory
                 .GetFiles(_cachedCurrenciesDirectoryPath)
                 .Select(f => Path.GetFileNameWithoutExtension(f))
                 .Where(f => f.StartsWith(dateString));

            if (cacheDateTimeStringsOnDate.Any() == true)
            {
                DateTime latestDateTime =
                    FindLatestCacheDateTime(cacheDateTimeStringsOnDate);

                string relativeFileName = latestDateTime
                        .ToString(ApiConstants.Formats.FileNameDateTimeFormat) + JsonExtension;

                ExchangeRateModel[] exchangeRatesFromFile =
                    ReadCachedExchangeRatesFromFile(relativeFileName);

                return FindExchangeRateDTOByType(currencyType, exchangeRatesFromFile);
            }

            ExchangeRatesHistoricalModel exchangeRatesHistorical = await _currencyAPI
                .GetAllCurrenciesOnDateAsync(_baseCurrency, date, cancellationToken);

            DateTime dtForFileName = exchangeRatesHistorical.LastUpdatedAt;
            CacheExchangeRatesToFile(dtForFileName, exchangeRatesHistorical.Currencies);

            return FindExchangeRateDTOByType(currencyType, exchangeRatesHistorical.Currencies);
        }

        private ExchangeRateDTOModel FindExchangeRateDTOByType(CurrencyType currencyType, ExchangeRateModel[] exchangeRates)
        {
            foreach (ExchangeRateModel exchangeRate in exchangeRates)
            {
                if (exchangeRate.Code.Equals(currencyType.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ExchangeRateDTOModel
                    {
                        CurrencyType = currencyType,
                        Value = exchangeRate.Value,
                    };
                }
            }

            throw new InvalidOperationException();
        }

        private DateTime FindLatestCacheDateTime(IEnumerable<string> cacheDateTimeStrings)
        {
            DateTime latestDateTime = DateTime.MinValue;

            foreach (string cacheDateTimeString in cacheDateTimeStrings)
            {
                DateTime cacheDateTime = DateTime.ParseExact(cacheDateTimeString,
                    ApiConstants.Formats.FileNameDateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                if (cacheDateTime > latestDateTime)
                {
                    latestDateTime = cacheDateTime;
                }
            }

            return latestDateTime;
        }

        private ExchangeRateModel[] ReadCachedExchangeRatesFromFile(string relativeFileName)
        {
            string fileFullName = Path.Combine(_cachedCurrenciesDirectoryPath, relativeFileName);

            string jsonStringFromFile = File.ReadAllText(fileFullName);
            ExchangeRateModel[] exchangeRatesFromFile = JsonSerializer
                .Deserialize<ExchangeRateModel[]>(jsonStringFromFile)!;

            return exchangeRatesFromFile;
        }

        private void CacheExchangeRatesToFile(DateTime dtForFileName, ExchangeRateModel[] currentExchangeRates)
        {
            string relativeFileName = dtForFileName.ToString(ApiConstants.Formats.FileNameDateTimeFormat) +
                    JsonExtension;
            string newFileFullName = Path.Combine(_cachedCurrenciesDirectoryPath,
                relativeFileName);

            string jsonStringToWrite = JsonSerializer.Serialize(currentExchangeRates);
            File.WriteAllText(newFileFullName, jsonStringToWrite);
        }
    }
}
