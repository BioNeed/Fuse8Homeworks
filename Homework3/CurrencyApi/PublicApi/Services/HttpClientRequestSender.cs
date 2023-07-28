namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class HttpClientRequestSender : IRequestSender
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientRequestSender(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> SendRequestAsync(string clientName, string requestPath)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(clientName);
            return await httpClient.GetAsync(requestPath);
        }
    }
}
