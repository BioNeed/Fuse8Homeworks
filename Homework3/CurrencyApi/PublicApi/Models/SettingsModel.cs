namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Текущие настройки приложения
    /// </summary>
    public class SettingsModel
    {
        public SettingsModel()
            : this("RUB", "USD", 2)
        {
        }

        public SettingsModel(string defaultCurrency, string baseCurrency, int currencyRoundCount)
        {
            DefaultCurrency = defaultCurrency;
            BaseCurrency = baseCurrency;
            CurrencyRoundCount = currencyRoundCount;
        }

        public string DefaultCurrency { get; set; }

        public string BaseCurrency { get; set; }

        public int CurrencyRoundCount { get; set; }
    }
}
