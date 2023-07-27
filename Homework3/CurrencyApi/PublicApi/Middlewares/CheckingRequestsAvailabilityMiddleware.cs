namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Middlewares
{
    public class CheckingRequestsAvailabilityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CheckingRequestsAvailabilityMiddleware> _logger;

        public CheckingRequestsAvailabilityMiddleware(
            RequestDelegate next, ILogger<CheckingRequestsAvailabilityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogWarning("Checking request availability!");
            await _next(context);
        }
    }
}
