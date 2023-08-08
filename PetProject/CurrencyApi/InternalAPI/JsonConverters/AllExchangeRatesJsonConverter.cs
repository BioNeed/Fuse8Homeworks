using System.Text.Json;
using System.Text.Json.Serialization;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace InternalAPI.JsonConverters
{
    public class AllExchangeRatesJsonConverter : JsonConverter<ExchangeRateModel[]>
    {
        public override ExchangeRateModel[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            List<ExchangeRateModel> exchangeRates = new List<ExchangeRateModel>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName
                    && reader.GetString() == "data")
                {
                    break;
                }
            }

            reader.Read();

            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    continue;
                }

                string propertyName = reader.GetString();

                reader.Read();

                ExchangeRateModel exchangeRate = new ExchangeRateModel();

                switch (propertyName)
                {
                    case "code":
                        string currencyCode = reader.GetString();
                        exchangeRate.Code = currencyCode!;
                        break;
                    case "value":
                        decimal value = reader.GetDecimal();
                        exchangeRate.Value = value;
                        break;
                }

                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, ExchangeRateModel[] value, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }
    }
}
