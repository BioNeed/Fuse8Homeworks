using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Newtonsoft.Json.Linq;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services.FindingExceptions
{
    public class BadResponseHandler : IResponseHandler
    {
        private const string InvalidCurrencyMessage = "The selected currencies is invalid.";

        public async Task RaiseExceptionAsync(HttpResponseMessage response)
        {
            await HandleIfUnknownCurrencyAsync(response);

            throw new Exception(ApiConstants.ErrorMessages.UnknownExceptionMessage);
        }

        private async Task HandleIfUnknownCurrencyAsync(HttpResponseMessage badResponse)
        {
            if (badResponse.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                string responseString = await badResponse.Content.ReadAsStringAsync();

                JObject parsedBadResponse = JObject.Parse(responseString);
                IEnumerable<JToken> errorDescriptions = parsedBadResponse["errors"]["currencies"].Values();

                foreach (JToken errorDescription in errorDescriptions)
                {
                    if (errorDescription?.Value<string>() == InvalidCurrencyMessage)
                    {
                        throw new CurrencyNotFoundException(InvalidCurrencyMessage);
                    }
                }
            }
        }
    }
}
