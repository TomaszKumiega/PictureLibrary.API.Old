using Castle.Core.Internal;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PictureLibraryModel.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly ILogger<FileSystemService> _logger;
        private List<string> TargetDirectories { get; set; }
        private List<string> RecoveryDirectories { get; set; }

        public FileSystemService(ILogger<FileSystemService> logger)
        {
            _logger = logger;
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

            if (!TargetDirectories.Any()) throw new Exception("There aren't any target directories to write to");

            foreach (var t in TargetDirectories)
            {
                try
                {
                    var fileStream = File.Create(t + directory + '/' + fileName);

                    if(RecoveryDirectories.Any())
                    {
                        foreach(var r in RecoveryDirectories)
                        {
                            try
                            {
                                File.Create(r + directory + '/' + fileName);
                                break;
                            }
                            catch(Exception e)
                            {
                                _logger.LogDebug(e, e.Message);
                            }
                        }
                    }

                    return fileStream;

                }
                catch (Exception e)
                {
                    _logger.LogDebug(e, e.Message);
                }
            }

            throw new Exception("Operation failed");
        }

        public string AddFile(string fileName, byte[] file)
        {
            if (fileName.IsNullOrEmpty()) throw new ArgumentException();
            if (!TargetDirectories.Any()) throw new Exception("There aren't any target directories to write to");

            foreach (var t in TargetDirectories)
            {
                try
                {
                    File.WriteAllBytes(t + fileName, file);

                    if (RecoveryDirectories.Any())
                    {
                        foreach (var r in RecoveryDirectories)
                        {
                            try
                            {
                                File.WriteAllBytes(t + fileName, file);
                                break;
                            }
                            catch (Exception e)
                            {
                                _logger.LogDebug(e, e.Message);
                            }
                        }
                    }

                    return t+fileName;

                }
                catch (Exception e)
                {
                    _logger.LogDebug(e, e.Message);
                }
            }

            throw new Exception("Operation failed");
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public List<FileStream> FindFiles(string searchPattern)
        {
            var files = new List<string>();
            var fileStreams = new List<FileStream>();

            foreach(var t in TargetDirectories)
            {
                var items = Directory.GetFiles(t, "*.plib", SearchOption.AllDirectories).ToList();
                files.AddRange(items);
            }

            foreach(var f in files)
            {
                fileStreams.Add(OpenFile(f, FileMode.Open));
            }

            return fileStreams;
            
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
