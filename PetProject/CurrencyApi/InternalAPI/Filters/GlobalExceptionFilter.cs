using System.Net;
using InternalAPI.Constants;
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
                                        context.Exception.Message,
                                        (int)HttpStatusCode.TooManyRequests);
                        break;
                    }

                case CacheBaseCurrencyNotFoundException:
                    {
                        HandleException(context, context.Exception.Message);
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
            string message = ApiConstants.ErrorMessages.UnknownExceptionMessage,
            int responseStatusCode = (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError("Ошибка! {message}", message);
            context.HttpContext.Response.StatusCode = responseStatusCode;
            context.ExceptionHandled = true;
        }
    }
}