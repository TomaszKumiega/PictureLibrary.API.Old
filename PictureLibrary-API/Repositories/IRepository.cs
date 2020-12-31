using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetBySourceAsync(string source);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        Task RemoveAsync(string source);
    }
}
