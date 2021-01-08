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
            var filePath = Path.GetFileNameWithoutExtension(image.ImageFile.LibraryFullPath) + "/Images/" + Guid.NewGuid().ToString() + image.ImageFile.Extension;
            var path = await Task.Run(() => _fileSystemService.AddFile(filePath, image.ImageContent));
            var imageFile = new ImageFile();

            var fileInfo = new FileInfo(path);

            imageFile.Name = image.ImageFile.Name;
            imageFile.Extension = image.ImageFile.Extension;
            imageFile.FullPath = path;
            imageFile.LibraryFullPath = image.ImageFile.LibraryFullPath;
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

        public async Task<IEnumerable<byte[]>> GetAllAsync(string libraryFullPath)
        {
            var fileInfo = _fileSystemService.GetFileInfo(libraryFullPath);
            var directory = fileInfo.Directory.FullName; 
            var imagePaths = await Task.Run(() => _fileSystemService.FindFiles("*.*", directory + "/Images"));
            var images = new List<byte[]>();

            foreach(var i in imagePaths)
            {
                images.Add(await Task.Run(() => _fileSystemService.GetFile(i)));
            }

            return images;
        }

        public async Task<byte[]> GetBySourceAsync(string fullPath)
        {
            var image = await Task.Run(() => _fileSystemService.GetFile(fullPath));

            return image;
        }

        public async Task<Icon> GetIcon(string imageFullPath)
        {
            return await Task.Run(()=>_fileSystemService.ExtractAssociatedIcon(imageFullPath));
        }

        public async Task<IEnumerable<Icon>> GetIcons(IEnumerable<string> imageFullPaths)
        {
            var icons = new List<Icon>();

            foreach(var i in imageFullPaths)
            {
                icons.Add(await GetIcon(i));
            }

            return icons;
        }

        public async Task RemoveAsync(string fullPath)
        {
            await Task.Run(() => _fileSystemService.DeleteFile(fullPath));
        }

        public async Task RemoveAsync(ImageFile entity)
        {
            if (entity == null) throw new ArgumentException();

            await RemoveAsync(entity.FullPath);
        }

        public async Task RemoveRangeAsync(IEnumerable<ImageFile> entities)
        {
            if (entities == null) throw new ArgumentException();
            if (!entities.Any()) throw new ArgumentException();

            foreach(var i in entities)
            {
                await RemoveAsync(i.FullPath);
            }
        }

        public async Task<ImageFile> UpdateAsync(ImageFile entity)
        {
            if (entity == null) throw new ArgumentException();

            await Task.Run(() => _fileSystemService.RenameFile(entity.FullPath, entity.Name + entity.Extension));

            var oldFileInfo = await Task.Run(() => _fileSystemService.GetFileInfo(entity.FullPath));
            var newFileInfo = await Task.Run(() => _fileSystemService.GetFileInfo(oldFileInfo.Directory.FullName + "\\" + entity.Name + entity.Extension));

            var imageFile = new ImageFile(newFileInfo.Name, newFileInfo.Extension, newFileInfo.FullName, entity.LibraryFullPath, newFileInfo.CreationTime, newFileInfo.LastAccessTimeUtc
                , newFileInfo.LastWriteTimeUtc, newFileInfo.Length, entity.Tags);

            return imageFile;
        }

        public async Task<ImageFile> UpdateAsync(Image entity)
        {
            var path = await Task.Run(() => _fileSystemService.AddFile(entity.ImageFile.FullPath, entity.ImageContent));

            var fileInfo = new FileInfo(path);

            return entity.ImageFile;
        }
    }
}
