using System;
using System.Collections.Generic;

namespace PictureLibrary_API.Model
{
    public enum ImageExtension : int
    {
        JPG,
        BMP,
        GIF,
        PNG,
        TIFF,
        NONE
    }

    public static class ImageExtensionHelper
    {
        public static ImageExtension GetExtension(string extension)
        {
            switch (extension)
            {
                case ".jpg": return ImageExtension.JPG;
                case ".jpeg": return ImageExtension.JPG;
                case ".jfif": return ImageExtension.JPG;
                case ".pjpeg": return ImageExtension.JPG;
                case ".pjp": return ImageExtension.JPG;
                case ".png": return ImageExtension.PNG;
                case ".bmp": return ImageExtension.BMP;
                case ".gif": return ImageExtension.GIF;
                case ".tiff": return ImageExtension.TIFF;
                case ".tif": return ImageExtension.TIFF;
            }
            return ImageExtension.NONE;
        }

        public static string ExtensionToString(ImageExtension extension)
        {
            switch(extension)
            {
                case ImageExtension.JPG: return ".jpg";
                case ImageExtension.BMP: return ".bmp";
                case ImageExtension.GIF: return ".gif";
                case ImageExtension.PNG: return ".png";
                case ImageExtension.TIFF: return ".tiff";
                case ImageExtension.NONE: return "";
            }

            return null;
        }
    }

    public class ImageFile : ILibraryEntity
    {
        public string Name { get; set; }
        public ImageExtension Extension { get; set; }
        public string FullName { get; set; }
        public string LibraryFullName { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Size { get; set; }
        public List<Tag> Tags { get; set; }

        public ImageFile()
        {

        }

        public ImageFile(string name, string extension, string source, string librarysource, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, long size, List<Tag> tags)
        {
            Name = name;
            Extension = ImageExtensionHelper.GetExtension(extension); 
            FullName = source;
            LibraryFullName = librarysource;
            CreationTime = creationTime;
            LastAccessTime = lastAccessTime;
            LastWriteTime = lastWriteTime;
            Size = size;
            Tags = tags;
        }
    }
}
