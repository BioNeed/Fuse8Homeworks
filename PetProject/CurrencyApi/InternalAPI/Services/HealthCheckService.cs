using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Models;
using static InternalAPI.Models.HealthCheckResult;

namespace InternalAPI.Services
{
    public class HealthCheckService : IHealthCheck
    {
        private readonly HttpClient _httpClient;

        public HealthCheckService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(ApiConstants.HttpClientNames.Default);
        }

        public async Task<HealthCheckResult> CheckExternalApiAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                ApiConstants.Uris.GetStatus, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return new HealthCheckResult
                {
                    Status = CheckStatus.Ok,
                    CheckedOn = DateTimeOffset.Now,
                };
            }

            return new HealthCheckResult
            {
                Status = CheckStatus.Failed,
                CheckedOn = DateTimeOffset.Now,
            };
        }
    }
}
