using Microsoft.Extensions.Logging;
using PictureLibraryModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace PictureLibraryModel.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly ILogger<Drive> _logger;

        private readonly IFileSystemEntitiesFactory _fileSystemEntitiesFactory;

        public FileSystemService(IFileSystemEntitiesFactory fileSystemEntitiesFactory)
        {
            _fileSystemEntitiesFactory = fileSystemEntitiesFactory;
        }

        public FileStream CreateFile(string fileName, string directory)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public IEnumerable<FileStream> FindFiles(string rootDirectory, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileStream> FindFiles(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public string? GetExtension(string path)
        {
           return Path.GetExtension(path);
        }

        public byte[] GetFileAsByteArray(string path)
        {
            throw new NotImplementedException();
        }

        public FileStream GetFileAsFileStream(string path)
        {
            throw new NotImplementedException();
        }
    }
}
