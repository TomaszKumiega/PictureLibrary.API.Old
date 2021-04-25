using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> AuthenticateAsync(string username, string password);
        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAllAsync();
        /// <summary>
        /// Returns user with specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetByIdAsync(Guid id);
        /// <summary>
        /// Creates user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> CreateAsync(UserModel userModel);
        /// <summary>
        /// Updates specified user.
        /// </summary>
        /// <param name="userParam"></param>
        /// <param name="password"></param>
        Task UpdateAsync(User userParam, string password = null);
        /// <summary>
        /// Removes user with specified id.
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Returns first element satisfying the condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<User> FindAsync(Func<User, bool> predicate);
    }
}
