using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> AddUser(User user);
        Task<IEnumerable<User>> FindByPartialUsername(string username);
        Task<User?> FindByUsername(string username);
        Task<User?> FindById(Guid userId);
        Task DeleteUser(Guid userId);
    }
}