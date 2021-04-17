using Microsoft.Extensions.Logging;
using PictureLibrary_API.Exceptions;
using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public class UserService : IUserService
    {
        private ILogger<UserService> Logger { get; }
        private DatabaseContext DatabaseContext { get; }

        public UserService(ILogger<UserService> logger, DatabaseContext databaseContext)
        {
            Logger = logger;
            DatabaseContext = databaseContext;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = DatabaseContext.Users.SingleOrDefault(x => x.Username == username);

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            } 

            return user;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required");
            }

            if (DatabaseContext.Users.Any(x => x.Username == user.Username))
            {
                throw new UserAlreadyExistsException("Username: \"" + user.Username + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            DatabaseContext.Users.Add(user);
            DatabaseContext.SaveChanges();

            return user;
        }

        public void Delete(Guid id)
        {
            var user = DatabaseContext.Users.Find(id);
            if (user != null)
            {
                DatabaseContext.Users.Remove(user);
                DatabaseContext.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return DatabaseContext.Users;
        }

        public User GetById(Guid id)
        {
            return DatabaseContext.Users.Find(id);
        }

        public void Update(User userParam, string password = null)
        {
            var user = DatabaseContext.Users.Find(userParam.Id);

            if (user == null)
            {
                throw new ContentNotFoundException("User not found");
            }

            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (DatabaseContext.Users.Any(x => x.Username == userParam.Username))
                {
                    throw new UserAlreadyExistsException("Username \"" + userParam.Username + "\" is already taken");
                }
                    
                user.Username = userParam.Username;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            DatabaseContext.Users.Update(user);
            DatabaseContext.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
