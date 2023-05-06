using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient.Libraries
{
    public class LibraryClient : ClientBase
    {
        public async Task<IEnumerable<Library>> GetAllLibrariesAsync(AuthorizationData authorizationData, Guid userId)
        {
            var result =  await SendRequestAndDeserializeResponseAsync<GetAllLibrariesResponse>(HttpMethod.Get, $"library/all?userId={userId}", authorizationData: authorizationData);
            return result?.Libraries ?? Enumerable.Empty<Library>();
        }

        public async Task<Guid?> AddLibraryAsync(AuthorizationData authorizationData, Library library)
        {
            var response = await SendRequestAndDeserializeResponseAsync<AddLibraryResponse>(HttpMethod.Post, $"library", library, authorizationData);
            return response?.LibraryId;
        }

        public async Task UpdateLibraryAsync(AuthorizationData authorizationData, Guid libraryId, Library library)
        {
            await SendRequest(HttpMethod.Put, $"library/{libraryId}", library, authorizationData);
        }
    }
}
