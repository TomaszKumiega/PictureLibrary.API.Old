using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetByName(string name);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Remove(string name);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        T Find(System.Predicate<T> predicate);

        void Update(T entity);
    }
}
