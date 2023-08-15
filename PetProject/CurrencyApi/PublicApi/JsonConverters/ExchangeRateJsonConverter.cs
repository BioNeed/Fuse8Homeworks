using System.Text.Json;
using System.Text.Json.Serialization;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using InternalAPI.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.JsonConverters
{
    public class ExchangeRateJsonConverter : JsonConverter<ExchangeRateModel>
    {
        public override ExchangeRateModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            ExchangeRateModel exchangeRate = new ExchangeRateModel();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                }

                if (reader.TokenType == JsonTokenType.PropertyName
                    && reader.GetString() == "data")
                {
                    break;
                }
            }

            reader.Read();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    continue;
                }

                string propertyName = reader.GetString();

                reader.Read();

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
            }

            while (reader.Read())
            {
            }

            return exchangeRate;
        }

        public override void Write(Utf8JsonWriter writer, ExchangeRateModel value, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }
    }
}
