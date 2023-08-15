using System.Net;
using InternalAPI.Contracts;
using InternalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static InternalAPI.Models.HealthCheckResult;

namespace InternalAPI.Controllers;

/// <summary>
/// Методы для проверки работоспособности InternalAPI
/// </summary>
[Route("healthcheck")]
public class HealthCheckController : ControllerBase
{
    private readonly IHealthCheck _healthCheck;

    public HealthCheckController(IHealthCheck healthCheck)
    {
        _healthCheck = healthCheck;
    }

    /// <summary>
    /// Проверить что API работает
    /// </summary>
    /// <param name="checkExternalApi">Необходимо проверить работоспособность внешнего API. Если FALSE или NULL - проверяется работоспособность только текущего API</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">
    /// Возвращает если удалось получить доступ к API
    /// </response>
    /// <response code="400">
    /// Возвращает если не удалось получить доступ к API
    /// </response>
    [HttpGet]
    public async Task<HealthCheckResult> CheckAsync(bool? checkExternalApi, CancellationToken cancellationToken)
    {
        if (checkExternalApi == null || checkExternalApi == false)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return new HealthCheckResult
            {
                Status = CheckStatus.Ok,
                CheckedOn = DateTimeOffset.Now,
            };
        }

        HealthCheckResult healthCheckResult = await _healthCheck
            .CheckExternalApiAsync(cancellationToken);

        if (healthCheckResult.Status == CheckStatus.Ok)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        }
        else
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        return healthCheckResult;
    }
}