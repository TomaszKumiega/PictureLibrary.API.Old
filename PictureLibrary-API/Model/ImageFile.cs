using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace PictureLibraryModel.Model
{
    public class ImageFile : ILibraryEntity
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Source { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Size { get; set; }

        public ImageFile()
        {

        }

        public ImageFile(string name, string extension, string source, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, long size)
        {
            Name = name;
            Extension = extension;
            Source = source;
            CreationTime = creationTime;
            LastAccessTime = lastAccessTime;
            LastWriteTime = lastWriteTime;
            Size = size;
        }
    }
}
