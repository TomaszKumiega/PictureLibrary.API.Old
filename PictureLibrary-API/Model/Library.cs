using System.Collections.Generic;

namespace PictureLibrary_API.Model
{
    public class Library : ILibraryEntity
    {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ImageFile> Images { get; set; }
        public List<User> Owners { get; set; }

        public Library()
        {
            Tags = new List<Tag>();
            Images = new List<ImageFile>();
        }

        public Library(string fullPath, string name, string decription, List<User> owners)
        {
            FullPath = fullPath;
            Name = name;
            Description = decription;
            Owners = owners;
            Tags = new List<Tag>();
            Images = new List<ImageFile>();
        }

        public Library(string fullPath, string name, string description, List<User> owners, List<Tag> tags, List<ImageFile> images)
        {
            FullPath = fullPath;
            Name = name;
            Description = description;
            Owners = owners;
            Tags = tags;
            Images = images;
        }
    }
}
