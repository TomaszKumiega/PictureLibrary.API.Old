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

        public void Add(Library entity)
        {
            // create library file
            var fileStream = _fileSystemService.CreateFile(entity.Name, "Libraries/");

            if (fileStream == null) throw new Exception("File creation error");

            // write all owners in one string
            string owners = "";
            foreach (var o in entity.Owners) owners += o.Id.ToString() + ',';

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

        }

        public void AddRange(IEnumerable<Library> entities)
        {
            foreach (var l in entities) Add(l);
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
            throw new NotImplementedException();
        }

        public void Remove(string name)
        {
            throw new NotImplementedException();
        }

        public void Remove(Library entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Library> entities)
        {
            foreach (var l in entities) Remove(l); 
        }

        public void Update(Library entity)
        {
            throw new NotImplementedException();
        }

        private Library ReadLibraryFromFileStream(FileStream fileStream)
        {
            return null;
        }
    }
}
