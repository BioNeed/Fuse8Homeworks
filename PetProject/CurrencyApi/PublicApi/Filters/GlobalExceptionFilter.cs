using System.Net;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using UserDataAccessLibrary.Exceptions;

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
                case ViolatingDatabaseConstraintsException:
                    {
                        HandleException(context,
                                        context.Exception.Message,
                                        (int)HttpStatusCode.UnprocessableEntity);
                        break;
                    }

                case DatabaseElementNotFoundException:
                    {
                        HandleException(context,
                                        context.Exception.Message,
                                        (int)HttpStatusCode.NotFound);
                        break;
                    }

                case RpcException ex when ex.Status.StatusCode == StatusCode.ResourceExhausted:
                    {
                        HandleException(context, ex.Status.Detail, (int)HttpStatusCode.TooManyRequests);
                        break;
                    }

                case RpcException ex:
                    {
                        HandleException(context, ex.Status.Detail);
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