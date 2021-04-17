using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
    public interface ILibraryRepository 
    {
        /// <summary>
        /// Returns all libraries from storage.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Library>> GetAllAsync();
        /// <summary>
        /// Returns library from specified file.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        Task<Library> GetBySourceAsync(string fullPath);
        /// <summary>
        /// Adds library to storage.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Library> AddAsync(Library entity);
        /// <summary>
        /// Adds libraries to storage.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<Library>> AddRangeAsync(IEnumerable<Library> entities);
        /// <summary>
        /// Removes specified library file from storage.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        Task RemoveAsync(string fullPath);
        /// <summary>
        /// Removes specified library file from storage.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task RemoveAsync(Library entity);
        /// <summary>
        /// Removes specified libraries from storage.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task RemoveRangeAsync(IEnumerable<Library> entities);
        /// <summary>
        /// Finds specified library.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<Library> FindAsync(Predicate<Library> predicate);
        /// <summary>
        /// Updates specified library file.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(Library entity);
    }
}
