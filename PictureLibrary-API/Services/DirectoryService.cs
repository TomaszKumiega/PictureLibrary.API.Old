using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public class DirectoryService : IDirectoryService
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public List<string> FindFiles(string directory, string searchPattern)
        {
            var files = Directory.GetFiles(directory, searchPattern, System.IO.SearchOption.AllDirectories).ToList();

            return files;
        }
    }
}
