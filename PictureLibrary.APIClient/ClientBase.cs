using PictureLibrary.APIClient.Model.Requests;
using System.Net.Http.Json;

namespace PictureLibrary.APIClient
{
    public class ClientBase
    {
        public string? BaseUrl { get; set; }

        public void Initialize(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        protected void EnsureInitialized()
        {
            if (BaseUrl == null)
                throw new InvalidOperationException($"{nameof(BaseUrl)} needs to be initialized first.");
        }

        private HttpRequestMessage CreateRequest(string requestUrl, object? content = null)
        {
            EnsureInitialized();

            HttpRequestMessage httpRequest = new(HttpMethod.Post, requestUrl);
            if (content != null)
            {
                httpRequest.Content = JsonContent.Create(content, content.GetType());
            }

            return httpRequest;
        }

        protected async Task<TResponse?> SendRequestAndDeserializeResponseAsync<TResponse>(string requestUrl, IRequest? content = null)
            where TResponse : class
        {
            var request = CreateRequest(requestUrl, content);

            HttpClient client = new()
            {
                BaseAddress = new Uri(BaseUrl!),
            };

            HttpResponseMessage? httpResponse = await client.SendAsync(request);

            if (httpResponse == null)
                return null;

            _ = httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}
