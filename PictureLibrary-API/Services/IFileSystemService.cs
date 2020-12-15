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
        /// Finds all files in a root directory matching the search pattern
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        IEnumerable<FileStream> FindFiles(string rootDirectory, string searchPattern);

        /// <summary>
        /// Finds all files matching the search pattern from all drives
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        IEnumerable<FileStream> FindFiles(string searchPattern);

        /// <summary>
        /// Creates a file in specified directory and returns FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        FileStream CreateFile(string fileName, string directory);

        /// <summary>
        /// Deletes specified file 
        /// </summary>
        /// <param name="filePath"></param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Returns a FileStream of a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileStream OpenFile(string path, FileMode mode);

        /// <summary>
        /// Returns contents of a file as byte array
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        byte[] GetFile(string path);

        /// <summary>
        /// Returns extension of a specified file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string? GetExtension(string path); 
    }
}
