using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PictureLibrary_API.Model;
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
        public User Owner { get; set; }

        public Library()
        {
            Tags = new List<Tag>();
            Images = new List<ImageFile>();
        }

        public Library(string fullPath, string name, string decription, User owner)
        {
            FullPath = fullPath;
            Name = name;
            Description = decription;
            Owner = owner;
            Tags = new List<Tag>();
            Images = new List<ImageFile>();
        }

        public Library(string fullPath, string name, string description, User owner, List<Tag> tags, List<ImageFile> images)
        {
            FullPath = fullPath;
            Name = name;
            Description = description;
            Owner = owner;
            Tags = tags;
            Images = images;
        }
    }
}
