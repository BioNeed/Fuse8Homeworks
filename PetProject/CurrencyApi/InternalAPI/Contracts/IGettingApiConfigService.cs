using InternalAPI.Models;

namespace InternalAPI.Contracts;

public interface IGettingApiConfigService
{
    Task<CurrencyConfigurationModel> GetApiConfigAsync(
        CurrencyConfigurationModel currencyConfig,
        CancellationToken cancellationToken);
}
