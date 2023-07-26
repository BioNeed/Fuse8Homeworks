namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Настройки приложения и доступные запросы
    /// </summary>
    public record SettingsFullModel
    {
        public SettingsModel ConfigurationSettings { get; init; }

        public int RequestLimit { get; init; }

        public int RequestCount { get; init; }
    }
}
