using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PictureLibraryModel.Services;

namespace PictureLibraryModel.Model
{
    public class Library : ILibraryEntity
    {
        public string FullPath { get; }
        public string Name { get; }
        public List<Tag> Albums { get; }

        public Library()
        {
            Albums = new List<Tag>();
        }

        public Library(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
            Albums = new List<Tag>();
        }

        public Library(string fullPath, string name, List<Tag> albums)
        {
            FullPath = fullPath;
            Name = name;
            Albums = albums;
        }
    }
}
