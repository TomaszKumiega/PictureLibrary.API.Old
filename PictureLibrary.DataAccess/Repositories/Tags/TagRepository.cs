using Microsoft.IdentityModel.Tokens;
using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IDatabaseAccess<Tag> _databaseAccess;

        public TagRepository(IDatabaseAccess<Tag> databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<Guid> AddTag(Tag tag)
        {
            if (tag.Libraries.IsNullOrEmpty())
                throw new ArgumentException(nameof(tag));

            tag.Id = Guid.NewGuid();

            string sql = @"
INSERT INTO Tags (Id, Name, Description, ColorHex)
VALUES (@Id, @Name, @Description, @ColorHex)";

            await _databaseAccess.SaveDataAsync(sql, tag);

            await AddLibraryRelationships(tag.Id, tag.Libraries!);

            return tag.Id;
        }

        private async Task AddLibraryRelationships(Guid tagId, List<Library> libraries)
        {
            string sql = @"
INSERT INTO LibraryTags (Id, TagId, LibraryId)
VALUES (@Id, @TagId, @LibraryId)";

            foreach (Library library in libraries)
            {
                await _databaseAccess.SaveDataAsync(sql, new { Id = Guid.NewGuid(), TagId = tagId, LibraryId = library.Id });
            }
        }
    }
}
