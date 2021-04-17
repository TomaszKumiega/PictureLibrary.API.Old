using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        void CreateFile(string filePath);

        /// <summary>
        /// Creates a file and writes all bytes to it. Returns path to the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="file"></param>
        /// <returns>Path of the file</returns>
        string AddFile(string filePath, byte[] file);

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
        byte[] ReadAllBytes(string path);

        /// <summary>
        /// Renames the file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="newName"></param>
        void RenameFile(string file, string newName);

        /// <summary>
        /// Provides file info of specified file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileInfo GetFileInfo(string path);

        /// <summary>
        /// Extracts icon from a specified file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Icon ExtractAssociatedIcon(string path);

        /// <summary>
        /// Creates of opens the file, write the specified string array to the file, and then closes the file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="contents"></param>
        void WriteAllLines(string fullPath, string[] contents);

        /// <summary>
        /// Returns true if file exists or false when file doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool FileExists(string path);
    }
}
