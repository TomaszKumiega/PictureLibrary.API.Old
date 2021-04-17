using Microsoft.Extensions.Logging;
using PictureLibrary_API.Exceptions;
using PictureLibrary_API.Model;
using PictureLibrary_API.Model.Builders;
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
        private IImageFileBuilder ImageFileBuilder { get; }

        public ImageRepository(ILogger<ImageRepository> logger, IDirectoryService directoryService, IFileService fileService, IImageFileBuilder imageFileBuilder)
        {
            Logger = logger;
            DirectoryService = directoryService;
            FileService = fileService;
            ImageFileBuilder = imageFileBuilder;
        }

        public async Task<ImageFile> AddAsync(Image image)
        {
            var libraryDirectory = FileService.GetFileInfo(image.ImageFile.LibraryFullName).Directory.FullName;
            if (!libraryDirectory.EndsWith("\\")) libraryDirectory += "\\";

            var filePath = FileSystemInfo.FileSystemInfo.RootDirectory + libraryDirectory + FileSystemInfo.FileSystemInfo.ImagesDirectory + Guid.NewGuid().ToString() + ImageExtensionHelper.ExtensionToString(image.ImageFile.Extension);
            var path = await Task.Run(() => FileService.AddFile(filePath, image.ImageContent));

            var fileInfo = FileService.GetFileInfo(path);

            var imageFile =
                ImageFileBuilder
                    .StartBuilding()
                    .WithName(image.ImageFile.Name)
                    .WithExtension(image.ImageFile.Extension)
                    .WithFullName(path)
                    .WithLibraryFullName(image.ImageFile.LibraryFullName)
                    .WithCreationTime(fileInfo.CreationTimeUtc)
                    .WithLastAccessTime(fileInfo.LastAccessTimeUtc)
                    .WithLastWriteTime(fileInfo.LastWriteTimeUtc)
                    .WithSize(fileInfo.Length)
                    .WithTags(new List<Tag>())
                    .Build();

            Logger.LogInformation("New added image: " + imageFile.FullName);

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
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { }

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

            Logger.LogInformation("Removed image: " + fullPath);
        }

        public async Task RemoveAsync(ImageFile entity)
        {
            if (entity == null) throw new ArgumentException();

            await RemoveAsync(entity.FullName);
        }

        public async Task RemoveRangeAsync(IEnumerable<ImageFile> entities)
        {
            if (entities == null) throw new ArgumentException();
            if (!entities.Any()) throw new ArgumentException();

            foreach(var i in entities)
            {
                await RemoveAsync(i.FullName);
            }
        }

        public async Task<ImageFile> UpdateAsync(ImageFile entity)
        {
            if (!FileService.FileExists(entity.FullName))
            {
                throw new ContentNotFoundException("Couldn't find specified file.");
            }

            await Task.Run(() => FileService.RenameFile(entity.FullName, entity.Name + ImageExtensionHelper.ExtensionToString(entity.Extension)));

            var oldFileInfo = await Task.Run(() => FileService.GetFileInfo(entity.FullName));
            var newFileInfo = await Task.Run(() => FileService.GetFileInfo(oldFileInfo.Directory.FullName + "\\" + entity.Name + ImageExtensionHelper.ExtensionToString(entity.Extension)));

            var imageFile =
                ImageFileBuilder
                    .StartBuilding()
                    .WithName(newFileInfo.Name)
                    .WithExtension(newFileInfo.Extension)
                    .WithFullName(newFileInfo.FullName)
                    .WithLibraryFullName(entity.LibraryFullName)
                    .WithCreationTime(newFileInfo.CreationTimeUtc)
                    .WithLastAccessTime(newFileInfo.LastAccessTimeUtc)
                    .WithLastWriteTime(newFileInfo.LastWriteTimeUtc)
                    .WithSize(newFileInfo.Length)
                    .WithTags(entity.Tags)
                    .Build();

            Logger.LogInformation("Updated imagefile info: " + imageFile.FullName);

            return imageFile;
        }

        public async Task<ImageFile> UpdateAsync(Image entity)
        {
            if(!FileService.FileExists(entity.ImageFile.FullName))
            {
                throw new ContentNotFoundException("Couldn't find specified file.");
            }

            // overwrites the file
            var path = await Task.Run(() => FileService.AddFile(entity.ImageFile.FullName, entity.ImageContent));

            Logger.LogInformation("Updated content of an image: " + path);

            return entity.ImageFile;
        }
    }
}
