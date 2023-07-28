using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services;
using Newtonsoft.Json.Linq;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Middlewares
{
    public class CheckingRequestsAvailabilityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRequestSender _requestSender;

        public CheckingRequestsAvailabilityMiddleware(
            RequestDelegate next,
            IRequestSender requestSender)
        {
            _next = next;
            _requestSender = requestSender;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path != ApiConstants.Uris.GetSettings)
            {
                if (await IsRequestAvailableAsync() == false)
                {
                    throw new ApiRequestLimitException(
                        ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
                }
            }

            await _next(context);
        }

        private async Task<bool> IsRequestAvailableAsync()
        {
            string responseString = await _requestSender.SendRequestAsync(
                ApiConstants.HttpClientsNames.CurrencyApi,
                ApiConstants.Uris.GetStatus);

            JObject status = JObject.Parse(responseString);
            int totalRequests = status["quotas"]["month"]["total"].Value<int>();
            int usedRequests = status["quotas"]["month"]["used"].Value<int>();

            return usedRequests < totalRequests;
        }
    }
}
