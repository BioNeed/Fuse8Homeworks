using System.Text.Json;
using System.Text.Json.Serialization;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.JsonConverters
{
    public class ApiStatusJsonConverter : JsonConverter<ApiStatusModel>
    {
        public override ApiStatusModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            ApiStatusModel apiStatus = new ApiStatusModel();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName
                    && reader.GetString() == "month")
                {
                    break;
                }
            }

            reader.Read();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string propertyName = reader.GetString();

                reader.Read();

                switch (propertyName)
                {
                    case "total":
                        int totalRequests = reader.GetInt32();
                        apiStatus.TotalRequests = totalRequests;
                        break;
                    case "used":
                        int usedRequests = reader.GetInt32();
                        apiStatus.UsedRequests = usedRequests;
                        break;
                }
            }

            while (reader.Read())
            {
            }

            return apiStatus;
        }

        public override void Write(Utf8JsonWriter writer, ApiStatusModel value, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }
    }
}
