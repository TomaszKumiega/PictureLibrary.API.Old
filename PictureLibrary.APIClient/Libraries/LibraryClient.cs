using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Responses;

namespace PictureLibrary.APIClient.Libraries
{
    public class LibraryClient : ClientBase
    {
        public async Task<IEnumerable<Library>> GetAllLibraries(AuthorizationData authorizationData, Guid userId)
        {
            var result =  await SendRequestAndDeserializeResponseAsync<GetAllLibrariesResponse>(HttpMethod.Get, $"library/all?userId={userId}", authorizationData: authorizationData);
            return result?.Libraries ?? Enumerable.Empty<Library>();
        }
    }
}
