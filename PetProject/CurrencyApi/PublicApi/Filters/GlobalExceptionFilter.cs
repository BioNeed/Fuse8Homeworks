using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using InternalAPI.Constants;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Filters
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

                case CurrencyNotFoundException:
                    {
                        HandleCurrencyNotFoundException(context);
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

        private void HandleCurrencyNotFoundException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
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