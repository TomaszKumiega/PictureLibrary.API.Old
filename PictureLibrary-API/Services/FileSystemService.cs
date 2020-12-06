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

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string? GetExtension(string path)
        {
           return Path.GetExtension(path);
        }
    }
}
