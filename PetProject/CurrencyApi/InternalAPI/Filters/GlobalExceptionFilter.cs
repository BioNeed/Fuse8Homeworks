using System.Net;
using InternalAPI.Constants;
using InternalAPI.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternalAPI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
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
                        HandleRequestLimitException(context);
                        break;
                    }

                case InvalidDateFormatException:
                    {
                        HandleAnyOtherException(context, context.Exception.Message);
                        break;
                    }

                default:
                    {
                        HandleAnyOtherException(context);
                        break;
                    }
            }
        }

        private void HandleRequestLimitException(ExceptionContext context)
        {
            _logger.LogError("Ошибка! {message}", context.Exception.Message);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.ExceptionHandled = true;
        }

        private void HandleAnyOtherException(
            ExceptionContext context,
            string message = ApiConstants.ErrorMessages.UnknownExceptionMessage)
        {
            _logger.LogError("Ошибка! {message}", message);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }
    }
}