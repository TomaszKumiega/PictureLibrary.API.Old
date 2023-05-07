using PictureLibrary.APIClient.Model;
using PictureLibrary.APIClient.Model.Authorization;
using PictureLibrary.APIClient.Model.Requests;

namespace PictureLibrary.APIClient.Tags
{
    public interface ITagClient
    {
        Task<Guid?> AddTagAsync(AuthorizationData authorizationData, AddTagRequest addTagRequest);
        Task DeleteTagAsync(AuthorizationData authorizationData, Guid libraryId, Guid tagId);
        Task<IEnumerable<Tag>> GetAllTagsAsync(AuthorizationData authorizationData, Guid libraryId);
        Task UpdateTagAsync(AuthorizationData authorizationData, Guid tagId, UpdateTagRequest request);
    }
}