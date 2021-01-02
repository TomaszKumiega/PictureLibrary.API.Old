using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibraryModel.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public async Task<ImageFile> AddAsync(Image image)
        {
            var filePath = Path.GetFileNameWithoutExtension(image.ImageFile.LibrarySource) + '/' + image.ImageFile.Name + image.ImageFile.Extension;
            var path = await Task.Run(() => _fileSystemService.AddFile(filePath, image.ImageContent));
            var imageFile = new ImageFile();

            var fileInfo = new FileInfo(path);

            imageFile.Name = image.ImageFile.Name;
            imageFile.Extension = image.ImageFile.Extension;
            imageFile.Source = path;
            imageFile.LibrarySource = image.ImageFile.LibrarySource;
            imageFile.CreationTime = fileInfo.CreationTime;
            imageFile.LastAccessTime = fileInfo.LastAccessTimeUtc;
            imageFile.LastWriteTime = fileInfo.LastWriteTimeUtc;
            imageFile.Size = fileInfo.Length;

            return imageFile;
        }

        public async Task<IEnumerable<ImageFile>> AddRangeAsync(IEnumerable<Image> entities)
        {
            var imageFiles = new List<ImageFile>();

            foreach (var i in entities)
            {
                imageFiles.Add(await AddAsync(i));
            }

            return imageFiles;
        }

        public async Task<IEnumerable<byte[]>> GetAllAsync(string libraryName)
        {
            var imagePaths = _fileSystemService.FindFiles("*.*", libraryName + "/Images");
            var images = new List<byte[]>();

            foreach(var i in imagePaths)
            {
                images.Add(await Task.Run(() => _fileSystemService.GetFile(i)));
            }

            return images;
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
