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

        private User GetUserSample(string username, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user =
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Email = "randomEmail@example.com",
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
        public void Authenticate_ShouldReturnUser_WhenUsernameAndPasswordAreValid()
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

            var result = userService.Authenticate(username, password);

            Assert.True(result.Id == user.Id);            
        }

        [Fact]
        public void Authenticate_ShouldThrowArgumentException_WhenUsernameOrPasswordAreNullOrEmpty()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            Assert.Throws<ArgumentException>(() => userService.Authenticate(null, "gdagadgd"));
            Assert.Throws<ArgumentException>(() => userService.Authenticate("gadgag", null));
            Assert.Throws<ArgumentException>(() => userService.Authenticate(String.Empty, "gdagdag"));
            Assert.Throws<ArgumentException>(() => userService.Authenticate("gadgag", String.Empty));
        }

        [Fact]
        public void Authenticate_ShouldReturnNull_WhenPasswordIsNotValid()
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

            var result = userService.Authenticate(username, "azxczxcxzc");

            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_ShouldReturnNull_WhenUserWithSpecifiedUsernameDoesntExist()
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

            var result = userService.Authenticate("username", password);

            Assert.Null(result);
        }
        #endregion

        #region Create
        [Fact]
        public void Create_ShouldThrowArgumentException_WhenUserModelIsNull()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();
            UserModel userModel = null;

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            Assert.Throws<ArgumentNullException>(() => userService.Create(userModel));
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_WhenUsernameOrPasswordAreNullOrEmpty()
        {
            var contextMock = new Mock<IDatabaseContext>();
            var loggerMock = new Mock<ILogger<UserService>>();

            var userService = new UserService(loggerMock.Object, contextMock.Object);

            var userModel = GetUserModelSample(null, "password");
            Assert.Throws<ArgumentException>(() => userService.Create(userModel));
            userModel = GetUserModelSample("username", null);
            Assert.Throws<ArgumentException>(() => userService.Create(userModel));
            userModel = GetUserModelSample(String.Empty, "password");
            Assert.Throws<ArgumentException>(() => userService.Create(userModel));
            userModel = GetUserModelSample("username", String.Empty);
            Assert.Throws<ArgumentException>(() => userService.Create(userModel));
        }

        [Fact]
        public void Create_ShouldThrowUserAlreadyExistsException_WhenUserWithSpecifiedUsernameAlreadyExists()
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

            Assert.Throws<UserAlreadyExistsException>(() => userService.Create(userModel));
        }

        [Fact]
        public void Create_ShouldAddUserToDatabase_WhenUserInfoIsValid()
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
            userService.Create(userModel);

            Assert.True(dbSet.FirstOrDefault(x => x.Username == userModel.Username) != null);
            contextMock.Verify(x => x.SaveChanges());
        }

        #endregion
    }
}
