using System.Net;
using InternalAPI.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternalAPI.Filters
{
    internal class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ApiRequestLimitException:
                    {
                        HandleException(context,
                                        (int)HttpStatusCode.TooManyRequests);
                        break;
                    }

                default:
                    {
                        HandleException(context);
                        break;
                    }
            }
        }

        private void HandleException(
            ExceptionContext context,
            int responseStatusCode = (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError("Ошибка! {message}", context.Exception.Message);

            context.HttpContext.Response.StatusCode = responseStatusCode;
            context.ExceptionHandled = true;
        }
    }
}