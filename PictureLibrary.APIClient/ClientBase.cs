using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;
using PictureLibrary.APIClient.Model.Responses;
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

        private HttpRequestMessage CreateRequest(HttpMethod method, string requestUrl, object? content = null)
        {
            EnsureInitialized();

            HttpRequestMessage httpRequest = new(method, requestUrl);
            if (content != null)
            {
                httpRequest.Content = JsonContent.Create(content, content.GetType());
            }

            return httpRequest;
        }

        #region SendRequest
        protected async Task<TResponse?> SendRequestAndDeserializeResponseAsync<TResponse>(HttpMethod method, string requestUrl, IRequest? content = null, AuthorizationData? authorizationData = null)
            where TResponse : class
        {
            var request = CreateRequest(method, requestUrl, content);

            HttpClient client = new()
            {
                BaseAddress = new Uri(BaseUrl!),
            };

            if (authorizationData != null)
            {
                client = await AddAuthorizationHeaderAsync(client, authorizationData);
            }
            
            HttpResponseMessage? httpResponse = await client.SendAsync(request);

            if (httpResponse == null)
                return null;

            _ = httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadFromJsonAsync<TResponse>();
        }

        protected async Task SendRequestAsync(HttpMethod method, string requestUrl, IRequest? content = null, AuthorizationData? authorizationData = null)
        {
            var request = CreateRequest(method, requestUrl, content);

            HttpClient client = new()
            {
                BaseAddress = new Uri(BaseUrl!),
            };


            if (authorizationData != null)
            {
                client = await AddAuthorizationHeaderAsync(client, authorizationData);
            }

            HttpResponseMessage? httpResponse = await client.SendAsync(request);

            _ = httpResponse?.EnsureSuccessStatusCode();
        }
        #endregion

        #region Authorization
        private async Task<HttpClient> AddAuthorizationHeaderAsync(HttpClient client, AuthorizationData authorizationData)
        {
            if (authorizationData.ExpiryDate < DateTime.UtcNow.AddMinutes(5))
            {
                authorizationData = await RefreshTokenAsync(authorizationData);
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationData.AccessToken);
        
            return client;
        }

        private async Task<AuthorizationData> RefreshTokenAsync(AuthorizationData oldAuthorizationData)
        {
            var request = new RefreshTokenRequest()
            {
                AccessToken = oldAuthorizationData.AccessToken,
                RefreshToken = oldAuthorizationData.RefreshToken,
            };

            var response = await SendRequestAndDeserializeResponseAsync<RefreshTokensResponse>(HttpMethod.Post, "authorization/refresh", request);
            
            return new AuthorizationData()
            { 
                AccessToken = response!.AccessToken,
                RefreshToken = response.RefreshToken,
                ExpiryDate = response.ExpiryDate,
            };
        }
        #endregion
    }
}
