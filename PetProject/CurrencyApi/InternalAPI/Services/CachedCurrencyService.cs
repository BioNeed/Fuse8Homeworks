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

        public async Task<ExchangeRateDTOModel> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken)
        {
            string projectDirectoryPath = Directory
                .GetParent(Environment.CurrentDirectory)
                .Parent.Parent.FullName;





            // ЭТО МОЖНО ВЫНЕСТИ В readonly поле
            string _cachedCurrenciesDirectoryPath = projectDirectoryPath
                    + ApiConstants.RelativePaths.CachedCurrenciesOnDates;

            IEnumerable<string> relativeFileNames = Directory
                .GetFiles(_cachedCurrenciesDirectoryPath)
                .Select(f => Path.GetFileName(f));

            DateTime currentDateTime = DateTime.UtcNow;

            DateTime maxDateTime = DateTime.MinValue;
            string latestRelativeFileName = string.Empty;

            foreach (string relativeFileName in relativeFileNames)
            {
                string dateString = relativeFileName[..^JsonExtension.Length];

                DateTime fileDateTime = DateTime.ParseExact(dateString,
                    ApiConstants.Formats.FileNameDateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                if (fileDateTime > maxDateTime)
                {
                    maxDateTime = fileDateTime;
                    latestRelativeFileName = relativeFileName;
                }
            }

            if (currentDateTime - maxDateTime < _cacheExpirationTime)
            {
                // найти нужное значение
                string fileFullName = _cachedCurrenciesDirectoryPath + "/" + 
                    latestRelativeFileName;

                string jsonStringFromFile = File.ReadAllText(fileFullName);
                ExchangeRateModel[] result = JsonSerializer.Deserialize<ExchangeRateModel[]>(jsonStringFromFile)!;

                foreach (ExchangeRateModel exchangeRate in result)
                {

                }

                throw new NotImplementedException();
            }

            ExchangeRateModel[] exchangeRates = await _currencyAPI.
                GetAllCurrentCurrenciesAsync(currencyType.ToString(), cancellationToken);

            string newFileFullName = _cachedCurrenciesDirectoryPath + "/" +
                currentDateTime.ToString(ApiConstants.Formats.FileNameDateTimeFormat) + 
                JsonExtension;
            string jsonStringToWrite = JsonSerializer.Serialize(exchangeRates);
            File.WriteAllText(newFileFullName, jsonStringToWrite);

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

        public async Task<ExchangeRateDTOModel> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
