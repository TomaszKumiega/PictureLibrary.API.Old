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
SELECT _imageFile.* FROM ImageFiles _imageFile
LEFT JOIN LibraryImageFiles _libraryImageFile
ON _imageFile.Id = _libraryImageFile.ImageFileId
WHERE _libraryImageFile.LibraryId = @LibraryId";

            return await _databaseAccess.LoadDataAsync(sql, new { LibraryId = libraryId });
        }
    }
}
