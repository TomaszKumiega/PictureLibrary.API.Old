using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public class UserService : IUserService
    {
        private DatabaseContext _databaseContext;
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public User Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public User Create(User user, string password)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }
    }
}
