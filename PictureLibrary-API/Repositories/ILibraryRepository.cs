using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
    public interface ILibraryRepository : IRepository<Library>
    {
        Task RemoveAsync(Library entity);
        Task RemoveRangeAsync(IEnumerable<Library> entities);

        Task<Library> FindAsync(System.Predicate<Library> predicate);

        Task UpdateAsync(Library entity);
    }
}
