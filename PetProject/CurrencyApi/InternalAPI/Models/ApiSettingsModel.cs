namespace InternalAPI.Models
{
    public record ApiSettingsModel
    {
        public string BaseAddress { get; init; }

        public string ApiKey { get; init; }
    }
}
