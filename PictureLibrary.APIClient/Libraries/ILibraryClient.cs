using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;

namespace PictureLibrary.APIClient.Libraries
{
    public interface ILibraryClient
    {
        Task<Guid?> AddLibraryAsync(AuthorizationData authorizationData, Library library);
        Task DeleteLibraryAsync(AuthorizationData authorizationData, Guid libraryId);
        Task<IEnumerable<Library>> GetAllLibrariesAsync(AuthorizationData authorizationData, Guid userId);
        Task UpdateLibraryAsync(AuthorizationData authorizationData, Guid libraryId, Library library);
    }
}