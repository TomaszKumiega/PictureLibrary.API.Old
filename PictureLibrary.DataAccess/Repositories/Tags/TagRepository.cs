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

        public async Task DeleteTag(Guid id, Guid libraryId)
        {
            string deleteLibaryTagSql = @"
DELETE FROM LibraryTags
WHERE TagId = @TagId AND LibraryId = @LibraryId";

            await _databaseAccess.SaveDataAsync(deleteLibaryTagSql, new { TagId = id, LibraryId = libraryId });

            string countLibraryTagsAssociatedWithTag = @"
SELECT Count(*) FROM LibraryTags
WHERE TagId = @TagId";

            IEnumerable<int> libraryTagsCount = await _databaseAccess.LoadDataAsync<int>(countLibraryTagsAssociatedWithTag, new { TagId = id });
            
            if (libraryTagsCount.First() == 0)
            {
                string deleteTagSql = @"
DELETE FROM Tags
WHERE Id = @Id";

                await _databaseAccess.SaveDataAsync(deleteTagSql, new { Id = id });
            }
        }

        public async Task<Tag?> FindTagById(Guid id)
        {
            string sql = @"
SELECT * FROM Tags
WHERE Id = @Id";

            var tags = await _databaseAccess.LoadDataAsync(sql, new { Id = id });
            return tags.FirstOrDefault();
        }

        public async Task<IEnumerable<Tag>> GetTags(Guid libraryId)
        {
            string sql = @"
SELECT _tag.* FROM Tags _tag
INNER JOIN LibraryTags _libraryTag
ON _tag.Id = _libraryTag.TagId
WHERE _libraryTag.LibraryId = @LibraryId";

            return await _databaseAccess.LoadDataAsync(sql, new { Libraryid = libraryId });
        }

        public async Task UpdateTag(Tag tag, IEnumerable<Guid>? libraries)
        {
            string updateTagSql = @"
UPDATE Tags
SET Name = @Name, 
    Description = @Description
    ColorHex = @ColorHex
WHERE Id = @Id";

            await _databaseAccess.SaveDataAsync(updateTagSql, tag);

            await UpdateLibraries(tag, libraries);
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

        private async Task UpdateLibraries(Tag tag, IEnumerable<Guid>? libraries)
        {
            string sql = @"
SELECT LibraryId FROM LibraryTags
WHERE TagId = @Id";

            var libraryIds = await _databaseAccess.LoadDataAsync<Guid>(sql, new { tag.Id });
            var updatedTagLibraryIds = libraries ?? Enumerable.Empty<Guid>();
            var newLibraries = updatedTagLibraryIds.Except(libraryIds);
            var removedLibraries = libraryIds.Except(updatedTagLibraryIds);

            foreach (var libraryId in newLibraries)
            {
                await AddLibraryTag(tag.Id, libraryId);
            }

            foreach (var libraryId in removedLibraries)
            {
                await RemoveLibraryTag(tag.Id, libraryId);
            }
        }

        private async Task RemoveLibraryTag(Guid tagId, Guid libraryId)
        {
            string sql = @"
DELETE FROM LibraryTags
WHERE TagId = @TagId AND LibraryId = @LibraryId";

            await _databaseAccess.SaveDataAsync(sql, new { TagId = tagId, LibraryId = libraryId });
        }

        private async Task AddLibraryTag(Guid tagId, Guid libraryId)
        {
            string sql = @"
INSERT INTO LibraryTags(Id, TagId, LibraryId)
VALUES (@Id, @TagId, @LibraryId";

            await _databaseAccess.SaveDataAsync(sql, new { Id = Guid.NewGuid(), TagId = tagId, LibraryId = libraryId });
        }
    }
}
