using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PictureLibrary_API.Exceptions;
using PictureLibrary_API.Helpers;
using PictureLibrary_API.Model;
using PictureLibrary_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PictureLibrary_API.Tests.ServicesTests
{
    public class UserServiceTests
    {
        #region Private helper methods
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
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

        private User GetUserSample(string username, string password, string email = null)
        {
            byte[] passwordHash = null;
            byte[] passwordSalt = null;

            if(password != null) CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user =
                    new User()
                    {
                        Id = Guid.NewGuid(),
                        Username = username,
                        Email = email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    };

            return user;
        }

        private UserModel GetUserModelSample(string username, string password, string email=null)
        {
            return new UserModel()
            {
                Username = username,
                Password = password,
                Email = email
            };

        }

        #endregion

        #region Authenticate 
        [Fact]
        public async void Authenticate_ShouldReturnUser_WhenUsernameAndPasswordAreValid()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var username = "testUser";
            var password = "passw";

            var user = GetUserSample(username, password);
            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var result = await userService.AuthenticateAsync(username, password);

            Assert.True(result.Id == user.Id);            
        }

        [Fact]
        public async void Authenticate_ShouldThrowArgumentException_WhenUsernameOrPasswordAreNullOrEmpty()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => userService.AuthenticateAsync(null, "gdagadgd"));
            await Assert.ThrowsAsync<ArgumentException>(() => userService.AuthenticateAsync("gadgag", null));
            await Assert.ThrowsAsync<ArgumentException>(() => userService.AuthenticateAsync(String.Empty, "gdagdag"));
            await Assert.ThrowsAsync<ArgumentException>(() => userService.AuthenticateAsync("gadgag", String.Empty));
        }

        [Fact]
        public async void Authenticate_ShouldReturnNull_WhenPasswordIsNotValid()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var username = "testUser";
            var user = GetUserSample(username, "gadgadgadg");

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var result = await userService.AuthenticateAsync(username, "azxczxcxzc");

            Assert.Null(result);
        }

        [Fact]
        public async void Authenticate_ShouldReturnNull_WhenUserWithSpecifiedUsernameDoesntExist()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var password = "gadgadgadg";
            var user = GetUserSample("username1", password);

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var result = await userService.AuthenticateAsync("username", password);

            Assert.Null(result);
        }
        #endregion

        #region Create
        [Fact]
        public async void Create_ShouldThrowArgumentException_WhenUserModelIsNull()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();
            UserModel userModel = null;

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.CreateAsync(userModel));
        }

        [Fact]
        public async void Create_ShouldThrowArgumentException_WhenUsernameOrPasswordAreNullOrEmpty()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var userModel = GetUserModelSample(null, "password");
            await Assert.ThrowsAsync<ArgumentException>(() => userService.CreateAsync(userModel));
            userModel = GetUserModelSample("username", null);
            await Assert.ThrowsAsync<ArgumentException>(() => userService.CreateAsync(userModel));
            userModel = GetUserModelSample(String.Empty, "password");
            await Assert.ThrowsAsync<ArgumentException>(() => userService.CreateAsync(userModel));
            userModel = GetUserModelSample("username", String.Empty);
            await Assert.ThrowsAsync<ArgumentException>(() => userService.CreateAsync(userModel));
        }

        [Fact]
        public async void Create_ShouldThrowUserAlreadyExistsException_WhenUserWithSpecifiedUsernameAlreadyExists()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var username = "testUser";
            var user = GetUserSample(username, "randomPassword");
            var userModel = GetUserModelSample(username, "password");


            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);
            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => userService.CreateAsync(userModel));
        }

        [Fact]
        public async void Create_ShouldAddUserToDatabase_WhenUserInfoIsValid()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var userModel = GetUserModelSample("testUser", "password");

            var dbSet = new TestDbSet<User>();

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);
            contextMock.Setup(x => x.SaveChanges())
                .Verifiable();

            var userService = new UserService(loggerMock.Object, contextMock.Object);
            await userService.CreateAsync(userModel);

            Assert.True(dbSet.FirstOrDefault(x => x.Username == userModel.Username) != null);
            contextMock.Verify(x => x.SaveChanges());
        }

        [Fact]
        public async void Create_ShouldReturnUser_WhenUserInfoIsValid()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var userModel = GetUserModelSample("testUser", "password");
            var dbSet = new TestDbSet<User>();

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);
            var result = await userService.CreateAsync(userModel);

            Assert.True(result.Username == userModel.Username);
        }
        #endregion

        #region Delete
        [Fact]
        public async void Delete_ShouldThrowContentNotFoundException_WhenUserWasNotFound()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user = GetUserSample("name", "password");
            var guid = Guid.NewGuid();

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            await Assert.ThrowsAsync<ContentNotFoundException>(() => userService.DeleteAsync(guid));
        }

        [Fact]
        public async void Delete_ShouldRemoveUserFromDatabase_WhenUserHaSpecifiedId()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user = GetUserSample("name", "password");

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);
            contextMock.Setup(x => x.SaveChanges())
                .Verifiable();

            var userService = new UserService(loggerMock.Object, contextMock.Object);
            await userService.DeleteAsync(user.Id);

            Assert.True(!dbSet.Any(x => x.Id == user.Id));
            contextMock.Verify(x => x.SaveChanges());
        }
        #endregion

        #region GetAll
        [Fact]
        public async void GetAll_ShouldReturnAllUsersFromDatabase()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user1 = GetUserSample("name", "password");
            var user2 = GetUserSample("name2", "password2");

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user1);
            dbSet.Add(user2);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);
            var result = await userService.GetAllAsync();

            Assert.Contains(user1, result);
            Assert.Contains(user2, result);
        }
        #endregion

        #region Find
        [Fact]
        public async void Find_ShouldReturnFirstElementSatisfyingTheCondition()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user1 = GetUserSample("name", "password");
            var user2 = GetUserSample("name2", "password2");

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user1);
            dbSet.Add(user2);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);
            var result = await userService.FindAsync(x => x.Id == user2.Id);

            Assert.Equal(user2, result);
        }

        [Fact]
        public async void Find_ShouldReturnNullWhenNoElementSatisfiesTheCondition()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user1 = GetUserSample("name", "password");
            var user2 = GetUserSample("name2", "password2");

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user1);
            dbSet.Add(user2);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);
            var result = await userService.FindAsync(x => x.Id == Guid.NewGuid());

            Assert.Null(result);
        }
        #endregion

        #region Update
        [Fact]
        public async void Update_ShouldThrowContentNotFoundException_WhenUserWithSpecifiedIdDoesntExist()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user1 = GetUserSample("name", "password");
            var user2 = GetUserSample("name2", "password2");

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user1);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            await Assert.ThrowsAsync<ContentNotFoundException>(() => userService.UpdateAsync(user2));
        }

        [Fact]
        public async void Update_ShouldThrowArgumentException_WhenAllUserPropertiesAreEitherNullOrEmpty()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user = GetUserSample("name", "password");

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var updateUser = GetUserSample(null, null, null);
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser));
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser, ""));
            updateUser.Email = "";
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser));
            updateUser = GetUserSample("", null, null);
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser));
            updateUser = GetUserSample("", null, "");
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser));
            updateUser = GetUserSample(null, "", "");
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser, ""));
            updateUser = GetUserSample("", "", null);
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser, ""));
            updateUser = GetUserSample("", "", "");
            updateUser.Id = user.Id;
            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(updateUser, ""));
        }

        [Fact]
        public async void Update_ShouldUpdateUserInDatabase_WhenOneOfUserPropertiesIsAssigned()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();

            var user = GetUserSample("name", null);

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var newPassword = "gdagda";
            var updateUser = GetUserSample(null, newPassword);
            updateUser.Id = user.Id;
            await userService.UpdateAsync(updateUser, newPassword);
            Assert.True(VerifyPasswordHash(newPassword, user.PasswordHash, user.PasswordSalt));

            var newUsername = "username";
            updateUser = GetUserSample(newUsername, null);
            updateUser.Id = user.Id;
            await userService.UpdateAsync(updateUser);
            Assert.True(user.Username == newUsername);

            var newEmail = "email@example.com";
            updateUser = GetUserSample("name", null, newEmail);
            updateUser.Id = user.Id;
            await userService.UpdateAsync(updateUser);
            Assert.True(user.Email == newEmail);
        }

        [Fact]
        public async void Update_ShouldThrowUserAlreadyExistsException_WhenUserWithSpecifiedNameExistsInDatabase()
        {
            var loggerMock = new Mock<ILogger<UserService>>();
            var contextMock = new Mock<IDatabaseContext>();
            
            var repeatedUsername = "newUsername";
            
            var user = GetUserSample("name", null);
            var user2 = GetUserSample(repeatedUsername, null);

            var dbSet = new TestDbSet<User>();
            dbSet.Add(user);
            dbSet.Add(user2);

            contextMock.Setup(x => x.Users)
                .Returns(dbSet);

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var updateUser = GetUserSample(repeatedUsername, null);
            updateUser.Id = user.Id;

            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => userService.UpdateAsync(updateUser));
        }
        #endregion
    }
}
