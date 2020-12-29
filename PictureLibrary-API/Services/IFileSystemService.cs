﻿using System.Collections.Generic;
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
        /// Creates a file and returns FileStream
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        FileStream CreateFile(string fileName, string directory);

        /// <summary>
        /// Creates a file and writes all bytes to it. Returns path to the file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <returns>Path of the file</returns>
        string AddFile(string fileName, byte[] file);

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
