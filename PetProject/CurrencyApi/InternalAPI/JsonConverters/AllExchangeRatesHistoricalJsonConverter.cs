using System.Text.Json;
using System.Text.Json.Serialization;
using InternalAPI.Models;

namespace InternalAPI.JsonConverters
{
    public class AllExchangeRatesHistoricalJsonConverter : JsonConverter<ExchangeRatesHistoricalModel>
    {
        public override ExchangeRatesHistoricalModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            ExchangeRatesHistoricalModel result = new ExchangeRatesHistoricalModel();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName
                    && reader.GetString() == "last_updated_at")
                {
                    reader.Read();
                    result.LastUpdatedAt = reader.GetDateTime();

                    break;
                }
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

            while (reader.Read())
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    continue;
                }

                ExchangeRateModel exchangeRate = new ExchangeRateModel();

                while (reader.TokenType != JsonTokenType.EndObject)
                {
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

                    reader.Read();
                }

                exchangeRates.Add(exchangeRate);
            }

            result.Currencies = exchangeRates.ToArray();
            return result;
        }

        public override void Write(Utf8JsonWriter writer, ExchangeRatesHistoricalModel value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
