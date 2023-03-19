using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface ITagRepository
    {
        Task<Guid> AddTag(Tag tag);
        Task<IEnumerable<Tag>> GetTags(Guid libraryId);
    }
}