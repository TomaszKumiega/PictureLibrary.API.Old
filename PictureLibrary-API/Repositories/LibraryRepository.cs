using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibraryModel.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PictureLibrary_API.Repositories
{
    public class LibraryRepository : IRepository<Library>
    {
        private readonly ILogger<LibraryRepository> _logger;
        private IFileSystemService _fileSystemService;

        public LibraryRepository(ILogger<LibraryRepository> logger, IFileSystemService fileSystemService)
        {
            _logger = logger;
            _fileSystemService = fileSystemService;
        }

        public Library Add(Library entity)
        {
            // create library file
            var fileStream = _fileSystemService.CreateFile(entity.Name, "Libraries/");

            if (fileStream == null) throw new Exception("File creation error");

            // write all owners in one string
            string owners = "";
            foreach (var o in entity.Owners) owners += o.ToString() + ',';

            // create library element
            var libraryElement = new XElement("library", new XAttribute("name", entity.Name),
                new XAttribute("description", entity.Description), new XAttribute("owners", owners));

            // create tags elements
            var tagsElement = new XElement("tags");

            foreach(var t in entity.Tags)
            {
                var tagElement = new XElement("tag", new XAttribute("name", t.Name), new XAttribute("description", t.Description));

                tagsElement.Add(tagElement);
            }

            libraryElement.Add(tagsElement);

            // create images elements
            var imagesElement = new XElement("images");

            foreach(var i in entity.Images)
            {
                var imageFileElement = new XElement("imageFile", new XAttribute("name", i.Name), new XAttribute("extension", i.Extension),
                    new XAttribute("source", i.Source), new XAttribute("creationTime", i.CreationTime.ToString()), new XAttribute("lastAccessTime", i.LastAccessTime.ToString()),
                    new XAttribute("lastWriteTime", i.LastWriteTime.ToString()), new XAttribute("size", i.Size.ToString()));

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

                    libraryElement.Save(xmlWriter);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            entity.FullPath = fileStream.Name;
            return entity;
        }

        public IEnumerable<Library> AddRange(IEnumerable<Library> entities)
        {
            foreach (var l in entities)
            {
                yield return Add(l);
            }
        }

        public Library Find(Predicate<Library> predicate)
        {
            var libraries = GetAll();

            return libraries.ToList().Find(predicate);
        }

        public IEnumerable<Library> GetAll()
        {
            var fileStreams = _fileSystemService.FindFiles("*.plib");
            var libraries = new List<Library>();

            foreach(var f in fileStreams)
            {
                libraries.Add(ReadLibraryFromFileStream(f));
            }

            return libraries;
        }

        public Library GetByName(string name)
        {
            var libraries = GetAll();
            return libraries.ToList().Find(x => x.Name == name);
        }

        public void Remove(string name)
        {
            var library = Find(x => x.Name == name);

            if (library == null) throw new ArgumentException();

            Remove(library);
        }

        public void Remove(Library entity)
        {
            if (entity == null) throw new ArgumentException();

            _fileSystemService.DeleteFile(entity.FullPath);
        }

        public void RemoveRange(IEnumerable<Library> entities)
        {
            if (entities == null) throw new ArgumentException();

            foreach (var l in entities) Remove(l); 
        }

        public void Update(Library entity)
        {
            throw new NotImplementedException();
        }

        private Library ReadLibraryFromFileStream(FileStream fileStream)
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
                                    var libraryElement = XNode.ReadFrom(reader) as XElement;

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
                                    var tagElement = XNode.ReadFrom(reader) as XElement;

                                    var tag = new Tag();
                                    tag.Name = tagElement.Attribute("name").Value;
                                    tag.Description = tagElement.Attribute("description").Value;

                                    tags.Add(tag);
                                }
                                break;
                            case "imageFile":
                                {
                                    var imageElement = XNode.ReadFrom(reader) as XElement;

                                    var imageFile = new ImageFile();
                                    imageFile.Name = imageElement.Attribute("name").Value;
                                    imageFile.Extension = imageElement.Attribute("extension").Value;
                                    imageFile.Source = imageElement.Attribute("source").Value;
                                    imageFile.CreationTime = DateTime.Parse(imageElement.Attribute("creationTime").Value);
                                    imageFile.LastAccessTime = DateTime.Parse(imageElement.Attribute("lastAccessTime").Value);
                                    imageFile.LastWriteTime = DateTime.Parse(imageElement.Attribute("lastWriteTime").Value);
                                    imageFile.Size = long.Parse(imageElement.Attribute("size").Value);

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
    }
}
