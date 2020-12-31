using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        public Task<byte[]> AddAsync(byte[] entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<byte[]>> AddRangeAsync(IEnumerable<byte[]> entities)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> FindAsync(Predicate<ImageFile> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<byte[]>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetBySourceAsync(string source)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ImageFile entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string source)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<ImageFile> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ImageFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
