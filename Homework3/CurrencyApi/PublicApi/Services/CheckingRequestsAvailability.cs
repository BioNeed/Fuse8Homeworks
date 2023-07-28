using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Newtonsoft.Json.Linq;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class CheckingRequestsAvailability : ICheckingBeforeRequests
    {
        private readonly IRequestSender _requestSender;

        public CheckingRequestsAvailability(IRequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        public async Task<bool> IsRequestAvailableAsync()
        {
            HttpResponseMessage response = await _requestSender.SendRequestAsync(
                ApiConstants.HttpClientsNames.CurrencyApi,
                ApiConstants.Uris.GetStatus);

            string responseString = await response.Content.ReadAsStringAsync();

            JObject status = JObject.Parse(responseString);
            int totalRequests = status["quotas"]["month"]["total"].Value<int>();
            int usedRequests = status["quotas"]["month"]["used"].Value<int>();

            return usedRequests < totalRequests;
        }
    }
}
