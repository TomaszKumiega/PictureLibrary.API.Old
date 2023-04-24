using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface ITagRepository
    {
        Task<Guid> AddTag(Tag tag);
        Task<IEnumerable<Tag>> GetTags(Guid libraryId);
        Task<Tag?> FindTagById(Guid id);
        Task DeleteTag(Guid id, Guid libraryId);
        Task UpdateTag(Tag tag, IEnumerable<Guid>? libraries);
    }
}