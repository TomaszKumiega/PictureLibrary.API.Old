using Castle.Core.Internal;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
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
        private readonly ILogger<LibraryRepository> _logger;
        private IDirectoryService _directoryService;
        private IFileService _fileService;

        public LibraryRepository(ILogger<LibraryRepository> logger, IDirectoryService directoryService, IFileService fileService)
        {
            _logger = logger;
            _directoryService = directoryService;
            _fileService = fileService;
        }

        public async Task<Library> AddAsync(Library entity)
        {
            var path = entity.Name + '-' + Guid.NewGuid().ToString() + '/' + entity.Name + ".plib";

            await Task.Run(() => _fileService.CreateFile(path));
            var fileStream = await Task.Run(() => _fileService.OpenFile(path, FileMode.Open));

            await WriteLibraryToFileStreamAsync(fileStream, entity);

            entity.FullPath = fileStream.Name;
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
            var filePaths = await Task.Run(() => _directoryService.FindFiles(FileSystemInfo.FileSystemInfo.RootDirectory, "*.plib"));
            var libraries = new List<Library>();

            foreach(var f in filePaths)
            {
                var fileStream = await Task.Run(() => _fileService.OpenFile(f, FileMode.Open));
                libraries.Add(await ReadLibraryFromFileStreamAsync(fileStream));
            }

            return libraries;
        }

        public async Task<Library> GetBySourceAsync(string fullPath)
        {
            if (fullPath.IsNullOrEmpty()) throw new ArgumentException();

            var fileStream = await Task.Run(() => _fileService.OpenFile(fullPath, FileMode.Open));
            var library = await ReadLibraryFromFileStreamAsync(fileStream);

            return library;
        }

        public async Task RemoveAsync(string fullPath)
        {
            if (fullPath.IsNullOrEmpty()) throw new ArgumentException();

            await Task.Run(() => _fileService.DeleteFile(fullPath));
        }

        public async Task RemoveAsync(Library entity)
        {
            if (entity == null) throw new ArgumentException();

            await Task.Run(()=>_fileService.DeleteFile(entity.FullPath));
        }

        public async Task RemoveRangeAsync(IEnumerable<Library> entities)
        {
            if (entities == null) throw new ArgumentException();

            foreach (var l in entities) await RemoveAsync(l); 
        }

        public async Task UpdateAsync(Library entity)
        {
            if (entity == null) throw new ArgumentException();
            if (!File.Exists(entity.FullPath)) throw new ArgumentException();

            // Load file for eventual recovery
            XmlDocument document = new XmlDocument();
            await Task.Run(()=>document.Load(entity.FullPath));

            // Remove contents of the file
            string[] text = { "" };
            await Task.Run(()=>File.WriteAllLines(entity.FullPath, text));

            try
            {
                // Write library to the file
                var fileStream = _fileService.OpenFile(entity.FullPath, FileMode.Open);
                await WriteLibraryToFileStreamAsync(fileStream, entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await Task.Run(()=>document.Save(entity.FullPath));
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
                                    library.FullPath = fileStream.Name;
                                    library.Description = libraryElement.Attribute("description").Value;

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

                                    tags.Add(tag);
                                }
                                break;
                            case "imageFile":
                                {
                                    var imageElement = await Task.Run(()=>XNode.ReadFrom(reader)) as XElement;

                                    var imageFile = new ImageFile();
                                    imageFile.Name = imageElement.Attribute("name").Value;
                                    imageFile.Extension = imageElement.Attribute("extension").Value;
                                    imageFile.FullPath = imageElement.Attribute("source").Value;
                                    imageFile.CreationTime = DateTime.Parse(imageElement.Attribute("creationTime").Value);
                                    imageFile.LastAccessTime = DateTime.Parse(imageElement.Attribute("lastAccessTime").Value);
                                    imageFile.LastWriteTime = DateTime.Parse(imageElement.Attribute("lastWriteTime").Value);
                                    imageFile.Size = long.Parse(imageElement.Attribute("size").Value);
                                    
                                    foreach(var t in imageElement.Attribute("tags").Value.Split(','))
                                    {
                                        imageFile.Tags.Add(tags.Find(x=>x.Name == t));
                                    }

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
                var tagElement = new XElement("tag", new XAttribute("name", t.Name), new XAttribute("description", t.Description));

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


                var imageFileElement = new XElement("imageFile", new XAttribute("name", i.Name), new XAttribute("extension", i.Extension),
                    new XAttribute("source", i.FullPath), new XAttribute("creationTime", i.CreationTime.ToString()), new XAttribute("lastAccessTime", i.LastAccessTime.ToString()),
                    new XAttribute("lastWriteTime", i.LastWriteTime.ToString()), new XAttribute("size", i.Size.ToString()), new XAttribute("tags", tags));

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
                _logger.LogError(e, e.Message);
            }
        }
    }
}
