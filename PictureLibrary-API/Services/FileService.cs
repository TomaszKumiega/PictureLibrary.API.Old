using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public class FileService : IFileService
    {
        public string AddFile(string filePath, byte[] file)
        {
            File.WriteAllBytes(filePath, file);
            return filePath;
        }

        public void CreateFile(string filePath)
        {
            var fileStream = File.Create(filePath);
            fileStream.Close();
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public Icon ExtractAssociatedIcon(string path)
        {
            return Icon.ExtractAssociatedIcon(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public FileInfo GetFileInfo(string path)
        {
            return new FileInfo(path);
        }

        public FileStream OpenFile(string path, FileMode mode)
        {
            return new FileStream(path, mode);
        }

        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public void RenameFile(string file, string newName)
        {
            FileSystem.RenameFile(file, newName);
        }

        public void WriteAllLines(string fullPath, string[] contents)
        {
            File.WriteAllLines(fullPath, contents);
        }
    }
}
