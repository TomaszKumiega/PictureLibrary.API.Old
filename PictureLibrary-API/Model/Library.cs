using System;
using System.Collections.Generic;
using System.IO;
using PictureLibraryModel.Services;

namespace PictureLibraryModel.Model
{
    public class Library : ILibraryEntity
    {
        public string FullPath { get; }
        public string Name { get; }
        public List<Album> Albums { get; }

        public Library()
        {
            Albums = new List<Album>();
        }

        public Library(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
            Albums = new List<Album>();
        }

        public Library(string fullPath, string name, List<Album> albums)
        {
            FullPath = fullPath;
            Name = name;
            Albums = albums;
        }

        public void AddImage(List<string> albumNames, string path)
        {

        }

        public void AddAlbum(string albumName)
        {

        }
    }
}
