using Castle.Core.Internal;
using Microsoft.AspNetCore.Builder.Extensions;
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
        private readonly ILogger<FileSystemService> _logger;
        private List<string> TargetDirectories { get; set; }
        private List<string> RecoveryDirectories { get; set; }

        public FileSystemService()
        {
            LoadTargetDirectories();
            LoadRecoveryDirectories();
        }

        private void LoadTargetDirectories()
        {
            string path;

            for(int i=0;i<100;i++)
            {
                path = "Directory" + i.ToString();

                if (Directory.Exists(path))
                {
                    TargetDirectories.Add(path);
                }
                else
                {
                    return;
                }
            }
        }

        private void LoadRecoveryDirectories()
        {
            string path;

            for(int i=0;i<100;i++)
            {
                path = "RecoveryDirectory" + i.ToString();

                if(Directory.Exists(path))
                {
                    RecoveryDirectories.Add(path);
                }
                else
                {
                    return;
                }
            }
        }

        public FileStream CreateFile(string fileName, string directory)
        {
            if (fileName.IsNullOrEmpty() || directory.IsNullOrEmpty()) throw new ArgumentException();
            
            if(directory.EndsWith('/'))
            {
                return File.Create(directory + fileName);
            }
            else
            {
                return File.Create(directory + '/' + fileName);
            }
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

        public byte[] GetFile(string path)
        {
            return File.ReadAllBytes(path);
        }

        public FileStream OpenFile(string path, FileMode mode)
        {
            return new FileStream(path, mode);
        }
    }
}
