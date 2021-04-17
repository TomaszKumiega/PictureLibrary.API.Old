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
        User Authenticate(string username, string password);
        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAll();
        /// <summary>
        /// Returns user with specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetById(Guid id);
        /// <summary>
        /// Creates user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User Create(User user, string password);
        /// <summary>
        /// Updates specified user.
        /// </summary>
        /// <param name="userParam"></param>
        /// <param name="password"></param>
        void Update(User userParam, string password = null);
        /// <summary>
        /// Removes user with specified id.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}
