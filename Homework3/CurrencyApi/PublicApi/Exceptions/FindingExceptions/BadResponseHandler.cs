using System.Net;
using Newtonsoft.Json.Linq;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions.FindingExceptions
{
    public class BadResponseHandler : IResponseHandler
    {
        private const string InvalidCurrencyMessage = "The selected currencies is invalid.";

        public async Task TryRaiseSpecificExceptionsAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == true)
            {
                return;
            }

            await HandleIfUnknownCurrencyAsync(response);
        }

        private async Task HandleIfUnknownCurrencyAsync(HttpResponseMessage badResponse)
        {
            if (badResponse.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                string responseString = await badResponse.Content.ReadAsStringAsync();
                JObject parsedBadResponse = JObject.Parse(responseString);
                IJEnumerable<JToken> errorDescriptions = parsedBadResponse["errors"]["currencies"].Values();

                if (errorDescriptions.Any(e =>
                    e.Value<string>() == InvalidCurrencyMessage) == true)
                {
                    throw new CurrencyNotFoundException(InvalidCurrencyMessage);
                }
            }
        }
    }
}
