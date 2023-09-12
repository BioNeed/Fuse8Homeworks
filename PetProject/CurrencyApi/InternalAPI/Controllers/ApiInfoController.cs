using InternalAPI.Contracts;
using InternalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternalAPI.Controllers;

/// <summary>
/// Методы для получения информации об API
/// </summary>
[Route("settings")]
public class ApiInfoController : ControllerBase
{
    private readonly IGettingApiInfoService _gettingApiInfoService;

    public ApiInfoController(IGettingApiInfoService gettingApiInfoService)
    {
        _gettingApiInfoService = gettingApiInfoService;
    }

    /// <summary>
    /// Получить настройки приложения
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <response code="200">
    /// Возвращает, если удалось получить настройки приложения
    /// </response>
    [Route("/settings")]
    [HttpGet]
    public Task<ApiInfoModel> GetConfigSettingsAsync(
        CancellationToken cancellationToken)
    {
        return _gettingApiInfoService.GetApiInfoAsync(cancellationToken);
    }
}