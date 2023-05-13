using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public class ImageFileRepository : IImageFileRepository
    {
        private readonly IDatabaseAccess<ImageFile> _databaseAccess;

        public ImageFileRepository(IDatabaseAccess<ImageFile> databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<IEnumerable<ImageFile>> GetAll(Guid libraryId)
        {
            string sql = @"
SELECT _imageFile.*, _library.* FROM ImageFiles _imageFile
LEFT JOIN LibraryImageFiles _libraryImageFile
ON _imageFile.Id = _libraryImageFile.ImageFileId
LEFT JOIN Libraries _library
ON _library.Id = _libraryImageFile.Id
WHERE _libraryImageFile.LibraryId = @LibraryId";

            IEnumerable<(ImageFile ImageFile, Library Library)> result = await _databaseAccess.LoadDataAsync<ImageFile, Library>(sql, (imageFile, library) => (imageFile, library), new { LibraryId = libraryId });

            return result.GroupBy(x => x.ImageFile.Id)
                .Select(g =>
                {
                    var imageFile = g.First().ImageFile;
                    imageFile.Libraries = g.Select(x => x.Library)
                    .Where(x => x != null)
                    .ToList();

                    return imageFile;
                });
        }

        public async Task<Guid> AddImageFile(ImageFile imageFile)
        {
            imageFile.Id = Guid.NewGuid();

            string sql = @"
INSERT INTO ImageFiles(Id, Name, FilePath, Extension, Size)
VALUES (@Id, @Name, @FilePath, @Extension, @Size)";

            await _databaseAccess.SaveDataAsync(sql, imageFile);

            await UpdateLibraryImages(imageFile, true);

            return imageFile.Id;
        }

        public async Task<ImageFile?> FindImageFileById(Guid id)
        {
            string sql = @"
SELECT _imageFile.*, _library.* FROM ImageFiles _imageFile
LEFT JOIN LibraryImageFiles _libraryImageFile
ON _imageFile.Id = _libraryImageFIle.ImageFileId
LEFT JOIN Libraries _library
ON _library.Id = _libraryImageFile.Id
WHERE _libraryImageFile.ImageFileId = @Id";

            IEnumerable<(ImageFile ImageFile, Library Library)> result = await _databaseAccess.LoadDataAsync<ImageFile, Library>(sql, (imageFile, library) => (imageFile, library), new { Id = id });

            return result.GroupBy(x => x.ImageFile.Id)
                .Select(g =>
                {
                    var imageFile = g.First().ImageFile;
                    imageFile.Libraries = g.Select(x => x.Library)
                    .Where(x => x != null)
                    .ToList();

                    return imageFile;
                })
                .FirstOrDefault();
        }

        public async Task DeleteImageFile(Guid imageFileId)
        {
            await DeleteLibraryImageFile(imageFileId);

            string sql = @"
DELETE FROM ImageFiles
WHERE Id = @Id";

            await _databaseAccess.SaveDataAsync(sql, new { Id = imageFileId });
                
        }

        public async Task UpdateImageFile(ImageFile imageFile)
        {
            string sql = @"
UPDATE ImageFiles
SET Name = @Name,
    FilePath = @FilePath
WHERE Id = @Id";

            await _databaseAccess.SaveDataAsync(sql, imageFile);
        }

        #region Private methods
        private async Task UpdateLibraryImages(ImageFile imageFile, bool isNewImageFile = false)
        {
            IEnumerable<Guid> updatedLibraries = imageFile.Libraries?.Select(x => x.Id) ?? Enumerable.Empty<Guid>();
            IEnumerable<Guid> newLibraries = updatedLibraries;

            if (!isNewImageFile)
            {
                string sql = @"
SELECT LibraryId FROM LibraryImageFiles
WHERE ImageFileId = @ImageFileId";

                var libraryIds = await _databaseAccess.LoadDataAsync<Guid>(sql, new { ImageFileId = imageFile.Id.ToString() });
                newLibraries = updatedLibraries.Except(libraryIds);
                var removedLibraries = libraryIds.Except(updatedLibraries);

                foreach (var removedLibraryId in removedLibraries)
                {
                    await RemoveLibraryImageFile(imageFile.Id, removedLibraryId);
                }
            }

            foreach (var newLibraryId in newLibraries)
            {
                await AddLibraryImageFile(imageFile.Id, newLibraryId);
            }
        }

        private async Task RemoveLibraryImageFile(Guid imageFileId, Guid libraryId)
        {
            string sql = @"
DELETE FROM LibraryImageFiles
WHERE ImageFileId = @ImageFileId 
AND LibraryId = @LibraryId";

            await _databaseAccess.SaveDataAsync(sql, new 
            { 
                ImageFileId = imageFileId.ToString(), 
                LibraryId = libraryId.ToString() 
            });
        }

        private async Task AddLibraryImageFile(Guid imageFileId, Guid libraryId)
        {
            string sql = @"
INSERT INTO LibraryImageFiles(Id, ImageFileId, LibraryId)
VALUES (@Id, @ImageFileId, @LibraryId)";

            await _databaseAccess.SaveDataAsync(sql, new
            {
                Id = Guid.NewGuid(),
                ImageFileId = imageFileId.ToString(),
                LibraryId = libraryId.ToString()
            });
        }

        private async Task DeleteLibraryImageFile(Guid imageFileId)
        {
            string sql = @"
DELETE FROM LibraryImageFiles
WHERE ImageFileId = @Id";

            await _databaseAccess.SaveDataAsync(sql, new { Id = imageFileId.ToString() });
        }
        #endregion
    }
}
