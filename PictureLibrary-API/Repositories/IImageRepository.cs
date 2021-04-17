using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Image = PictureLibrary_API.Model.Image;

namespace PictureLibrary_API.Repositories
{
   public interface IImageRepository 
    {
        /// <summary>
        /// Returns all image contents from specified library.
        /// </summary>
        /// <param name="libraryFullPath"></param>
        /// <returns></returns>
        Task<IEnumerable<byte[]>> GetAllAsync(string libraryFullPath);
        /// <summary>
        /// Returns image content from specified file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        Task<byte[]> GetBySourceAsync(string fullPath);
        /// <summary>
        /// Saves image file on storage.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        Task<ImageFile> AddAsync(Image image);
        /// <summary>
        /// Saves image files on storage.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<ImageFile>> AddRangeAsync(IEnumerable<Image> entities);
        /// <summary>
        /// Removes specified image file from storage.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        Task RemoveAsync(string fullPath);
        /// <summary>
        /// Removes specified image file from storage.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task RemoveAsync(ImageFile entity);
        /// <summary>
        /// Removes specified image files from storage.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task RemoveRangeAsync(IEnumerable<ImageFile> entities);
        /// <summary>
        /// Updates image file name and extension.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ImageFile> UpdateAsync(ImageFile entity);
        /// <summary>
        /// Updates image file content.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ImageFile> UpdateAsync(Image entity);
        /// <summary>
        /// Returns file icons from specified paths.
        /// </summary>
        /// <param name="imageFullPaths"></param>
        /// <returns></returns>
        Task<IEnumerable<Icon>> GetIcons(IEnumerable<string> imageFullPaths);
        /// <summary>
        /// Returns icon of specified file.
        /// </summary>
        /// <param name="imageFullPath"></param>
        /// <returns></returns>
        Task<Icon> GetIcon(string imageFullPath);
    }
}
