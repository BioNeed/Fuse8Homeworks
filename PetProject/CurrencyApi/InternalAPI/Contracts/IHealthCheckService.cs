using InternalAPI.Models;

namespace InternalAPI.Contracts
{
    public interface IHealthCheckService
    {
        Task<HealthCheckResult> CheckExternalApiAsync(CancellationToken cancellationToken);
    }
}