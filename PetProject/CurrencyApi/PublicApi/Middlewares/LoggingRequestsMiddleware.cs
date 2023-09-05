namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Middlewares
{
    internal class LoggingRequestsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingRequestsMiddleware> _logger;

        public LoggingRequestsMiddleware(RequestDelegate next, ILogger<LoggingRequestsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Method: {Method}, path: {Path}, query: {Query}",
                                   context.Request.Method,
                                   context.Request.Path,
                                   context.Request.Query);

            return _next(context);
        }
    }
}