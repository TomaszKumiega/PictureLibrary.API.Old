using PictureLibraryModel.Services;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace PictureLibraryModel.Model
{
    public class Directory : IFileSystemEntity
    {
        private IFileSystemService FileSystemService { get; }

        public string FullPath { get; }
        public string Name { get; }
        public ObservableCollection<object> Children { get; }


        public Directory(string fullPath, string name, IFileSystemService fileSystemService)
        {

            if (!System.IO.Directory.Exists(fullPath)) throw new DirectoryNotFoundException();
            FullPath = fullPath;
            Name = name;
            FileSystemService = fileSystemService;
            this.Children = new ObservableCollection<object>();
        }

        /// <summary>
        /// Initializes new instance of <see cref="Directory"/> class, children aren't loaded on runtime and are specified as argument in constructor
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="children"></param>
        public Directory(string fullPath, ObservableCollection<object> children)
        {
            if (!System.IO.Directory.Exists(fullPath)) throw new DirectoryNotFoundException();
            FullPath = fullPath;
            Name = (new System.IO.DirectoryInfo(fullPath)).Name;
            FileSystemService = null;
            this.Children = children;
        }

        /// <summary>
        /// Initializes new instance of <see cref="Directory"/> class. Children aren't loaded on runtime and are specified as argument in constructor.
        /// Path of the directory is null.
        /// </summary>
        /// <param name="children">Specified children files and directories</param>
        /// <param name="name">Name of the directory</param>
        public Directory(ObservableCollection<object> children, string name)
        {
            FullPath = null;
            FileSystemService = null;
            Name = name;
            this.Children = children;
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
