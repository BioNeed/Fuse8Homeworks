namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Настройки API
    /// </summary>
    public record ApiSettingsModel
    {
        /// <summary>
        /// Базовый адрес для работы с InternalApi
        /// </summary>
        public string BaseAddress { get; init; }
    }
}
