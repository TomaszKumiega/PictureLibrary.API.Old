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
        Task<IEnumerable<Icon>> GetAllIconsAsync(string librarySource);
        Task<IEnumerable<byte[]>> GetAllAsync(string librarySource);
        Task<byte[]> GetBySourceAsync(string source);

        Task<ImageFile> AddAsync(Image image);
        Task<IEnumerable<ImageFile>> AddRangeAsync(IEnumerable<Image> entities);

        Task RemoveAsync(string source);

        Task RemoveAsync(ImageFile entity);
        Task RemoveRangeAsync(IEnumerable<ImageFile> entities);

        Task<ImageFile> UpdateAsync(ImageFile entity);
        Task<ImageFile> UpdateAsync(Image entity);
    }
}
