using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User?> FindByUsername(string username);
    }
}