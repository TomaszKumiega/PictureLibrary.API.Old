using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
   public interface IImageRepository 
    {
        Task<IEnumerable<byte[]>> GetAllAsync();
        Task<byte[]> GetBySourceAsync(string source);

        Task<ImageFile> AddAsync(byte[] entity);
        Task<IEnumerable<ImageFile>> AddRangeAsync(IEnumerable<byte[]> entities);

        Task RemoveAsync(string source);

        Task RemoveAsync(ImageFile entity);
        Task RemoveRangeAsync(IEnumerable<ImageFile> entities);

        Task<byte[]> FindAsync(System.Predicate<ImageFile> predicate);

        Task UpdateAsync(ImageFile entity);
    }
}
