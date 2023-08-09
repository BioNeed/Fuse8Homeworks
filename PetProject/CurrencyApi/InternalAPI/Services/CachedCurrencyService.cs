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

        private readonly ICurrencyAPI _currencyAPI;
        private readonly TimeSpan _cacheExpirationTime;
        private readonly string _cachedCurrenciesDirectoryPath =
            Directory.GetParent(Environment.CurrentDirectory)
                .Parent.Parent.FullName
            + ApiConstants.RelativePaths.CachedCurrenciesOnDates;

        public CachedCurrencyService(ICurrencyAPI currencyAPI,
            IOptionsSnapshot<ApiSettingsModel> apiSettings)
        {
            _currencyAPI = currencyAPI;
            _cacheExpirationTime = TimeSpan.FromHours(apiSettings.Value.CacheExpirationTimeInHours);
        }

        public async Task<ExchangeRateDTOModel> GetCurrentCurrencyAsync(
            CurrencyType currencyType, CancellationToken cancellationToken)
        {
            IEnumerable<string> relativeFileNames = Directory
                .GetFiles(_cachedCurrenciesDirectoryPath)
                .Select(f => Path.GetFileName(f));

            DateTime currentDateTime = DateTime.UtcNow;

            if (relativeFileNames.Any() == true)
            {
                (DateTime latestDateTime, string latestRelativeFileName) =
                    FindLatestFileAndDateTime(relativeFileNames);

                if (currentDateTime - latestDateTime < _cacheExpirationTime)
                {
                    ExchangeRateModel[] exchangeRatesFromFile = ReadCachedExchangeRatesFromFile(latestRelativeFileName);

                    return FindExchangeRateDTOByType(currencyType, exchangeRatesFromFile);
                }
            }

            ExchangeRateModel[] currentExchangeRates = await _currencyAPI.
                GetAllCurrentCurrenciesAsync(currencyType.ToString(), cancellationToken);

            CacheExchangeRatesToFile(currentDateTime, currentExchangeRates);

            return FindExchangeRateDTOByType(currencyType, currentExchangeRates);
        }

        public async Task<ExchangeRateDTOModel> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private ExchangeRateDTOModel FindExchangeRateDTOByType(CurrencyType currencyType, ExchangeRateModel[] exchangeRates)
        {
            foreach (ExchangeRateModel exchangeRate in exchangeRates)
            {
                if (currencyType.ToString() == exchangeRate.Code)
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

        private (DateTime, string) FindLatestFileAndDateTime(IEnumerable<string> relativeFileNames)
        {
            DateTime latestDateTime = DateTime.MinValue;
            string latestRelativeFileName = string.Empty;

            foreach (string relativeFileName in relativeFileNames)
            {
                string dateString = relativeFileName[..^JsonExtension.Length];

                DateTime fileDateTime = DateTime.ParseExact(dateString,
                    ApiConstants.Formats.FileNameDateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                if (fileDateTime > latestDateTime)
                {
                    latestDateTime = fileDateTime;
                    latestRelativeFileName = relativeFileName;
                }
            }

            return (latestDateTime, latestRelativeFileName);
        }

        private ExchangeRateModel[] ReadCachedExchangeRatesFromFile(string relativeFileName)
        {
            string fileFullName = Path.Combine(_cachedCurrenciesDirectoryPath, relativeFileName);

            string jsonStringFromFile = File.ReadAllText(fileFullName);
            ExchangeRateModel[] exchangeRatesFromFile = JsonSerializer
                .Deserialize<ExchangeRateModel[]>(jsonStringFromFile)!;

            return exchangeRatesFromFile;
        }

        private void CacheExchangeRatesToFile(DateTime currentDateTime, ExchangeRateModel[] currentExchangeRates)
        {
            string relativeFileName = currentDateTime.ToString(ApiConstants.Formats.FileNameDateTimeFormat) +
                    JsonExtension;
            string newFileFullName = Path.Combine(_cachedCurrenciesDirectoryPath,
                relativeFileName);

            string jsonStringToWrite = JsonSerializer.Serialize(currentExchangeRates);
            File.WriteAllText(newFileFullName, jsonStringToWrite);
        }
    }
}
