namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    public record ApiSettingsModel
    {
        public string BaseAddress { get; init; }

        public string ApiKey { get; init; }
    }
}
