using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PictureLibraryModel.Services;

namespace PictureLibraryModel.Model
{
    public class Library : ILibraryEntity
    {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ImageFile> Images { get; set; }

        public Library()
        {
            Tags = new List<Tag>();
            Images = new List<ImageFile>();
        }

        public Library(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
            Tags = new List<Tag>();
            Images = new List<ImageFile>();
        }

        public Library(string fullPath, string name, List<Tag> tags, List<ImageFile> images)
        {
            FullPath = fullPath;
            Name = name;
            Tags = tags;
            Images = images;
        }
    }
}
