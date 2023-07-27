namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class HttpClientRequestSender : IRequestSender
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientRequestSender(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SendRequestAsync(string clientName, string requestPath)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(clientName);
            HttpResponseMessage? response = await httpClient.GetAsync(requestPath);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
