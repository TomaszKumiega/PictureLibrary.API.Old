using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public interface IDirectoryService
    {
        /// <summary>
        /// Finds all files in a specified directory matching the search pattern
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        List<string> FindFiles(string directory, string searchPattern);
    }
}
