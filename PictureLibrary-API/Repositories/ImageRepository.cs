using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
    public class ImageRepository : IRepository<Dictionary<ImageFile, byte[]>>
    {
        public Task<Dictionary<ImageFile, byte[]>> AddAsync(Dictionary<ImageFile, byte[]> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Dictionary<ImageFile, byte[]>>> AddRangeAsync(IEnumerable<Dictionary<ImageFile, byte[]>> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<ImageFile, byte[]>> FindAsync(Predicate<Dictionary<ImageFile, byte[]>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Dictionary<ImageFile, byte[]>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<ImageFile, byte[]>> GetBySourceAsync(string source)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string source)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Dictionary<ImageFile, byte[]> entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<Dictionary<ImageFile, byte[]>> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Dictionary<ImageFile, byte[]> entity)
        {
            throw new NotImplementedException();
        }
    }
}
