using System.Text.Json.Serialization;

namespace InternalAPI.Enums
{
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CurrencyType
    {
        Usd,
        Rub,
        Kzt,
    }
}
