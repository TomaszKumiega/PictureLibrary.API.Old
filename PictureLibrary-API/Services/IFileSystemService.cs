using PictureLibraryModel.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace PictureLibraryModel.Services
{
    /// <summary>
    /// Defines file system control logic
    /// </summary>
    public interface IFileSystemService
    {

        /// <summary>
        /// Deletes specified file 
        /// </summary>
        /// <param name="filePath"></param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Returns extension of a specified file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string? GetExtension(string path); 
    }
}
