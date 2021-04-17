using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibrary_API.Services;
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
        private ILogger<ImageRepository> Logger { get; }
        private IDirectoryService DirectoryService { get; }
        private IFileService FileService { get; }

        public ImageRepository(ILogger<ImageRepository> logger, IDirectoryService directoryService, IFileService fileService)
        {
            Logger = logger;
            DirectoryService = directoryService;
            FileService = fileService;
        }

        public async Task<ImageFile> AddAsync(Image image)
        {
            var libraryDirectory = FileService.GetFileInfo(image.ImageFile.LibraryFullPath).Directory.FullName;
            if (!libraryDirectory.EndsWith("\\")) libraryDirectory += "\\";

            var filePath = FileSystemInfo.FileSystemInfo.RootDirectory + libraryDirectory + FileSystemInfo.FileSystemInfo.ImagesDirectory + Guid.NewGuid().ToString() + ImageExtensionHelper.ExtensionToString(image.ImageFile.Extension);
            var path = await Task.Run(() => FileService.AddFile(filePath, image.ImageContent));

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
            var images = new List<byte[]>();

            var fileInfo = FileService.GetFileInfo(libraryFullPath);
            var directory = fileInfo.Directory.FullName;
            var imagePaths = await Task.Run(() => DirectoryService.FindFiles(FileSystemInfo.FileSystemInfo.RootDirectory + directory + FileSystemInfo.FileSystemInfo.ImagesDirectory, "*.*"));
            
            foreach(var i in imagePaths)
            {
                byte[] image;

                try
                {
                    image = await Task.Run(() => FileService.ReadAllBytes(i));
                }
                catch (FileNotFoundException) { continue; }
                catch (DirectoryNotFoundException) { continue; }

                images.Add(image);
            }

            return images;
        }

        public async Task<byte[]> GetBySourceAsync(string fullPath)
        {
            byte[] image = null;

            try
            {
                image = await Task.Run(() => FileService.ReadAllBytes(fullPath));
            }
            catch(FileNotFoundException)
            {
                return null;
            }
            catch(DirectoryNotFoundException)
            {
                return null;
            }

            return image;
        }

        public async Task<Icon> GetIcon(string imageFullPath)
        {
            return await Task.Run(() => FileService.ExtractAssociatedIcon(imageFullPath));
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
            await Task.Run(() => FileService.DeleteFile(fullPath));
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
            await Task.Run(() => FileService.RenameFile(entity.FullPath, entity.Name + ImageExtensionHelper.ExtensionToString(entity.Extension)));

            var oldFileInfo = await Task.Run(() => FileService.GetFileInfo(entity.FullPath));
            var newFileInfo = await Task.Run(() => FileService.GetFileInfo(oldFileInfo.Directory.FullName + "\\" + entity.Name + ImageExtensionHelper.ExtensionToString(entity.Extension)));

            var imageFile = new ImageFile(newFileInfo.Name, newFileInfo.Extension, newFileInfo.FullName, entity.LibraryFullPath, newFileInfo.CreationTime, newFileInfo.LastAccessTimeUtc
                , newFileInfo.LastWriteTimeUtc, newFileInfo.Length, entity.Tags);

            return imageFile;
        }

        public async Task<ImageFile> UpdateAsync(Image entity)
        {
            // overwrites the file
            var path = await Task.Run(() => FileService.AddFile(entity.ImageFile.FullPath, entity.ImageContent));

            return entity.ImageFile;
        }
    }
}
