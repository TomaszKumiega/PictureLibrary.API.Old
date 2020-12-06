using System;
using PictureLibraryModel.Services;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PictureLibraryModel.Model
{
    public class Drive : IFileSystemEntity
    {
        private readonly ILogger<Drive> _logger;

        private IFileSystemService FileSystemService { get; }

        public string Name { get; }
        public string FullPath { get; }
        public ObservableCollection<object> Children { get; set; }
        

        public Drive(string name, IFileSystemService fileSystemService)
        {
            Name = name;
            FullPath = name;
            this.Children = new ObservableCollection<object>();
            this.FileSystemService = fileSystemService;

            Task.Run(() => Initialize()).Wait();
        }


        private async Task Initialize()
        {
            try
            {
                var directories = await Task.Run(() =>
                    FileSystemService.GetAllDirectories(Name, System.IO.SearchOption.TopDirectoryOnly));


                if (directories != null)
                {
                    foreach (var t in directories)
                    {
                        Children.Add(t);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
        }
        

        private async Task LoadChildrenDirectories()
        {
            foreach (Directory c in Children)
            {
                var directories = await Task.Run(() => FileSystemService.GetAllDirectories(c.FullPath, System.IO.SearchOption.TopDirectoryOnly));

                if (directories != null)
                {
                    foreach (var t in directories)
                    {
                        c.Children.Add(t);
                    }    
                }
            }
        }
    }
}
