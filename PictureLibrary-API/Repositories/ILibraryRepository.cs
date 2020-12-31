using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
    public interface ILibraryRepository 
    {
        Task<IEnumerable<Library>> GetAllAsync();
        Task<Library> GetBySourceAsync(string source);

        Task<Library> AddAsync(Library entity);
        Task<IEnumerable<Library>> AddRangeAsync(IEnumerable<Library> entities);

        Task RemoveAsync(string source);

        Task RemoveAsync(Library entity);
        Task RemoveRangeAsync(IEnumerable<Library> entities);

        Task<Library> FindAsync(System.Predicate<Library> predicate);

        Task UpdateAsync(Library entity);
    }
}
