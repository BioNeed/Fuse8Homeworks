using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// Методы для работы с Currencyapi API
/// </summary>
[Route("currency")]
public class CurrencyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrencyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Получить настройки приложения
    /// </summary>
    /// <param name="currencyConfig">Конфигурационные настройки для работы с валютами</param>
    /// <response code="200">
    /// Возвращает, если удалось получить настройки приложения
    /// </response>
    [Route("/settings")]
    [HttpGet]
    public async Task<CurrencyConfigurationModel> GetSettings([FromServices] IOptionsSnapshot<CurrencyConfigurationModel> currencyConfig)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(Constants.HttpClientsNames.CurrencyApi);
        HttpResponseMessage? response = await httpClient.GetAsync(Constants.Uris.GetStatus);
        response.EnsureSuccessStatusCode();

        string responseString = await response.Content.ReadAsStringAsync();

        JObject status = JObject.Parse(responseString);
        int totalRequests = status["quotas"]["month"]["total"].Value<int>();
        int usedRequests = status["quotas"]["month"]["used"].Value<int>();

        CurrencyConfigurationModel settingsFull = currencyConfig.Value with
        {
            RequestCount = usedRequests,
            RequestLimit = totalRequests,
        };

        return settingsFull;
    }
}