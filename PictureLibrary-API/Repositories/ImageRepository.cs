using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibraryModel.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Image = PictureLibrary_API.Model.Image;

namespace PictureLibrary_API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ILogger<ImageRepository> _logger;
        private IFileSystemService _fileSystemService;

        public ImageRepository(ILogger<ImageRepository> logger, IFileSystemService fileSystemService)
        {
            _logger = logger;
            _fileSystemService = fileSystemService;
        }

        public Task<ImageFile> AddAsync(Image image)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageFile>> AddRangeAsync(IEnumerable<Image> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<byte[]>> GetAllAsync(string libraryName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Icon>> GetAllIconsAsync(string libraryName)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetBySourceAsync(string source)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string source)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ImageFile entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<ImageFile> entities)
        {
            throw new NotImplementedException();
        }

        public Task<ImageFile> UpdateAsync(ImageFile entity)
        {
            throw new NotImplementedException();
        }

        public Task<ImageFile> UpdateAsync(Image entity)
        {
            throw new NotImplementedException();
        }
    }
}
