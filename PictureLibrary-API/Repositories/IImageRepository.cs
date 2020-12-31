using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
   public interface IImageRepository : IRepository<byte[]>
    {
        Task RemoveAsync(ImageFile entity);
        Task RemoveRangeAsync(IEnumerable<ImageFile> entities);

        Task<byte[]> FindAsync(System.Predicate<ImageFile> predicate);

        Task UpdateAsync(ImageFile entity);
    }
}
