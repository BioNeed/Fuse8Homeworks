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
    /// <returns></returns>
    [Route("/settings")]
    [HttpGet]
    public async Task<SettingsFullModel> GetSettings([FromServices] IOptionsSnapshot<SettingsModel> settingsOptions)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(Constants.HttpClientsNames.CurrencyApi);
        HttpResponseMessage? response = await httpClient.GetAsync(Constants.Uris.GetStatus);
        response.EnsureSuccessStatusCode();

        string responseString = await response.Content.ReadAsStringAsync();

        JObject status = JObject.Parse(responseString);
        int totalRequests = status["quotas"]["month"]["total"].Value<int>();
        int usedRequests = status["quotas"]["month"]["used"].Value<int>();

        SettingsFullModel settingsFull = new SettingsFullModel()
        {
            ConfigurationSettings = settingsOptions.Value,
            RequestCount = usedRequests,
            RequestLimit = totalRequests,
        };

        return settingsFull;
    }
}