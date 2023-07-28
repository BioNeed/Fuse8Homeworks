using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GlobalExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilterAttribute> _logger;

        public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger)
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

        private void HandleAnyOtherException(ExceptionContext context)
        {
            _logger.LogError("Ошибка! {message}", context.Exception.Message);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }
    }
}