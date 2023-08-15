using InternalAPI.Models;

namespace InternalAPI.Contracts
{
    public interface IHealthCheck
    {
        Task<HealthCheckResult> CheckExternalApiAsync(CancellationToken cancellationToken);
    }
}