using Castle.Core.Internal;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Exceptions;
using PictureLibrary_API.Model;
using PictureLibrary_API.Model.Builders;
using PictureLibrary_API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PictureLibrary_API.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private ILogger<LibraryRepository> Logger { get; }
        private IDirectoryService DirectoryService { get; }
        private IFileService FileService { get; }
        private IImageFileBuilder ImageFileBuilder { get; }

        public LibraryRepository(ILogger<LibraryRepository> logger, IDirectoryService directoryService, IFileService fileService, IImageFileBuilder imageFileBuilder)
        {
            Logger = logger;
            DirectoryService = directoryService;
            FileService = fileService;
            ImageFileBuilder = imageFileBuilder;
        }

        public async Task<Library> AddAsync(Library entity)
        {
            // create library folder with images subfolder
            var libraryFolderPath = FileSystemInfo.FileSystemInfo.RootDirectory + entity.Name + '-' + Guid.NewGuid().ToString();
            DirectoryService.CreateDirectory(libraryFolderPath + '\\' + FileSystemInfo.FileSystemInfo.ImagesDirectory);

            // create library file
            var path = libraryFolderPath + '\\' + entity.Name + ".plib";
            await Task.Run(() => FileService.CreateFile(path));

            // write library to file
            var fileStream = await Task.Run(() => FileService.OpenFile(path, FileMode.Open));
            await WriteLibraryToFileStreamAsync(fileStream, entity);

            entity.FullName = path;

            Logger.LogInformation("New added library: " + path);

            return entity;
        }

        public async Task<IEnumerable<Library>> AddRangeAsync(IEnumerable<Library> entities)
        {
            var libraries = new List<Library>();

            foreach (var l in entities)
            {
                libraries.Add(await AddAsync(l));
            }

            return libraries;
        }

        public async Task<Library> FindAsync(Predicate<Library> predicate)
        {
            var libraries = await GetAllAsync();

            return libraries.ToList().Find(predicate);
        }

        public async Task<IEnumerable<Library>> GetAllAsync()
        {
            var filePaths = await Task.Run(() => DirectoryService.FindFiles(FileSystemInfo.FileSystemInfo.RootDirectory, "*.plib"));
            var libraries = new List<Library>();

            foreach(var f in filePaths)
            {
                var fileStream = await Task.Run(() => FileService.OpenFile(f, FileMode.Open));
                libraries.Add(await ReadLibraryFromFileStreamAsync(fileStream));
            }

            return libraries;
        }

        public async Task<Library> GetBySourceAsync(string fullPath)
        {
            Library library = null;

            try
            {
                var fileStream = await Task.Run(() => FileService.OpenFile(fullPath, FileMode.Open));
                library = await ReadLibraryFromFileStreamAsync(fileStream);
            }
            catch(FileNotFoundException)
            {
                return null;
            }
            catch(DirectoryNotFoundException)
            {
                return null;
            }

            return library;
        }

        public async Task RemoveAsync(string fullPath)
        {
            if (fullPath.IsNullOrEmpty()) throw new ArgumentException();

            await Task.Run(() => FileService.DeleteFile(fullPath));
        }

        public async Task RemoveAsync(Library entity)
        {
            if (entity == null) throw new ArgumentException();

            await Task.Run(()=>FileService.DeleteFile(entity.FullName));

            Logger.LogInformation("Removed library: " + entity.FullName);
        }

        public async Task RemoveRangeAsync(IEnumerable<Library> entities)
        {
            if (entities == null) throw new ArgumentException();

            foreach (var l in entities) await RemoveAsync(l); 
        }

        public async Task UpdateAsync(Library entity)
        {
            if (entity == null)
            {
                throw new ArgumentException();
            }
            if (!File.Exists(entity.FullName))
            {
                throw new ContentNotFoundException("Library \"" + entity.FullName + "\" doesn't exist.");
            }

            // Load file for potential recovery
            XmlDocument document = new XmlDocument();
            var stream = await Task.Run(() => FileService.OpenFile(entity.FullName, FileMode.Open));
            await Task.Run(()=>document.Load(stream));
            stream.Close();

            // Remove contents of the file
            string[] text = { "" };
            await Task.Run(() => FileService.WriteAllLines(entity.FullName, text));

            var fileStream = FileService.OpenFile(entity.FullName, FileMode.Open);

            try
            {
                // Write library to the file
                await WriteLibraryToFileStreamAsync(fileStream, entity);
                Logger.LogInformation("Updated library: " + entity.FullName);
            }
            catch (Exception e)
            {
                // log the error and recover old file
                Logger.LogError(e, e.Message);
                var recoveredFileStream = FileService.OpenFile(entity.FullName, FileMode.OpenOrCreate);
                await Task.Run(()=>document.Save(recoveredFileStream));
                throw new Exception("Couldn't save updated library", e);
            }
        }

        private async Task<Library> ReadLibraryFromFileStreamAsync(FileStream fileStream)
        {
            var tags = new List<Tag>();
            var images = new List<ImageFile>();
            var library = new Library();

            if (fileStream.Length == 0) throw new ArgumentException("Given stream is empty");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;

            using (var reader = XmlReader.Create(fileStream, settings))
            {
                while(reader.Read())
                {
                    if(reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "library":
                                {
                                    var libraryElement = await Task.Run(() => XNode.ReadFrom(reader)) as XElement;

                                    library.Name = libraryElement.Attribute("name").Value;
                                    library.FullName = fileStream.Name;
                                    library.Description = libraryElement.Attribute("description").Value;
                                    library.Owners = new List<Guid>();

                                    foreach (var t in libraryElement.Attribute("owners").Value.Split(','))
                                    {
                                        library.Owners.Add(Guid.Parse(t));
                                    }
                                }
                                break;
                            case "tag":
                                {
                                    var tagElement = await Task.Run(() => XNode.ReadFrom(reader)) as XElement;

                                    var tag = new Tag();
                                    tag.Name = tagElement.Attribute("name").Value;
                                    tag.Description = tagElement.Attribute("description").Value;
                                    tag.Color = tagElement.Attribute("color").Value;

                                    tags.Add(tag);
                                }
                                break;
                            case "imageFile":
                                {
                                    var imageElement = await Task.Run(()=>XNode.ReadFrom(reader)) as XElement;
                                    var imageTags = new List<Tag>();

                                    foreach (var t in imageElement.Attribute("tags").Value.Split(','))
                                    {
                                        imageTags.Add(tags.Find(x => x.Name == t));
                                    }

                                    var fullName = imageElement.Attribute("source").Value;
                                    var imageFileInfo = FileService.GetFileInfo(fullName);

                                    var imageFile =
                                        ImageFileBuilder
                                            .StartBuilding()
                                            .WithName(imageElement.Attribute("name").Value)
                                            .WithExtension(ImageExtensionHelper.GetExtension(imageElement.Attribute("extension").Value))
                                            .WithFullName(fullName)
                                            .WithLibraryFullName(library.FullName)
                                            .WithCreationTime(imageFileInfo.CreationTimeUtc)
                                            .WithLastAccessTime(imageFileInfo.LastAccessTimeUtc)
                                            .WithLastWriteTime(imageFileInfo.LastWriteTimeUtc)
                                            .WithSize(imageFileInfo.Length)
                                            .WithTags(imageTags)
                                            .Build();
                                           
                                    images.Add(imageFile);
                                }
                                break;
                        }
                    }
                }
            }

            library.Tags = tags;
            library.Images = images;

            return library;
        }

        private async Task WriteLibraryToFileStreamAsync(FileStream fileStream, Library entity)
        {
            if (fileStream == null) throw new Exception("File creation error");

            // write all owners in one string
            string owners = "";

            for(int i=0;i<entity.Owners.Count-1;i++)
            {
                owners += entity.Owners[i].ToString() + ',';
            }

            owners += entity.Owners[entity.Owners.Count - 1].ToString();

            // create library element
            var libraryElement = new XElement("library", new XAttribute("name", entity.Name),
                new XAttribute("description", entity.Description), new XAttribute("owners", owners));

            // create tags elements
            var tagsElement = new XElement("tags");

            foreach (var t in entity.Tags)
            {
                var tagElement = new XElement("tag", new XAttribute("name", t.Name), new XAttribute("description", t.Description), new XAttribute("color", t.Color));

                tagsElement.Add(tagElement);
            }

            libraryElement.Add(tagsElement);

            // create images elements
            var imagesElement = new XElement("images");

            foreach (var i in entity.Images)
            {
                // write all tags to one string
                string tags = "";

                for(int it=0;it<i.Tags.Count-1;it++)
                {
                    tags += i.Tags[it].Name + ',';
                }

                tags += i.Tags[i.Tags.Count - 1].Name;


                var imageFileElement = new XElement("imageFile", new XAttribute("name", i.Name), new XAttribute("extension", ImageExtensionHelper.ExtensionToString(i.Extension)),
                    new XAttribute("source", i.FullName), new XAttribute("tags", tags));

                imagesElement.Add(imageFileElement);
            }

            libraryElement.Add(imagesElement);

            // save elements to the file
            try
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    var xmlWriter = new XmlTextWriter(streamWriter);

                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.Indentation = 4;

                    await Task.Run(()=>libraryElement.Save(xmlWriter));
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
            }
        }
    }
}
