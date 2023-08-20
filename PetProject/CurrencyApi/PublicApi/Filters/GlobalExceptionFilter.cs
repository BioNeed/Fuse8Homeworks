using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Grpc.Core;
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
                case InvalidDateFormatException:
                    {
                        HandleAnyOtherException(context, context.Exception.Message);
                        break;
                    }

                case RpcException ex when ex.Status.StatusCode == StatusCode.ResourceExhausted:
                    {
                        HandleRequestLimitException(context, ex.Status.Detail);
                        break;
                    }

                case RpcException ex:
                    {
                        HandleAnyOtherException(context, ex.Status.Detail);
                        break;
                    }

                default:
                    {
                        HandleAnyOtherException(context);
                        break;
                    }
            }
        }

        private void HandleRequestLimitException(ExceptionContext context, string message)
        {
            _logger.LogError("Ошибка! {message}", message);
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