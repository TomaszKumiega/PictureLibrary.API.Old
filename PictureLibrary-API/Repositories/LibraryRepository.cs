using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibraryModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Library> entities)
        {
            throw new NotImplementedException();
        }

        public Library Find(Predicate<Library> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Library> GetAll()
        {
            throw new NotImplementedException();
        }

        public Library GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Library entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Library> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Library entity)
        {
            throw new NotImplementedException();
        }
    }
}
