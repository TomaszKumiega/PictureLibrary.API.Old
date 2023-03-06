using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> AddUser(User user);
        Task<User?> FindByUsername(string username);
        Task DeleteUser(Guid userId);
    }
}