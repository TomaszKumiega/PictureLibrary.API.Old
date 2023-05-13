//using PictureLibrary.DataAccess.Commands;
//using PictureLibrary.DataAccess.Exceptions;
//using PictureLibrary.DataAccess.Handlers;
//using PictureLibrary.DataAccess.Repositories;
//using PictureLibrary.Model;
//using PictureLibrary.Model.Users;
//using PictureLibrary.Tools;

//namespace PictureLibrary.DataAccess.Tests
//{
//    public class AddUserHandlerTests
//    {
//        [Fact]
//        public void Handle_ShouldThrowResourceAlreadyExistsException_IfUserAlreadyExists()
//        {
//            var user = new UserRegister()
//            {
//                Username = "testUser",
//                Password = "testPassword",
//                EmailAddress = "email@email.com",
//            };

//            AddUserCommand command = new(user);

//            Mock<IHashAndSalt> hashAndSaltMock = new(MockBehavior.Strict);
//            Mock<IUserRepository> userRepositoryMock = new(MockBehavior.Strict);

//            userRepositoryMock.Setup(x => x.FindByUsername(user.Username))
//                .Returns(Task.FromResult(User))
//                .Verifiable();

//            AddUserHandler handler = new(hashAndSaltMock.Object, userRepositoryMock.Object);

//            Assert.ThrowsAsync<ResourceAlreadyExistsException>(() => handler.Handle(command, CancellationToken.None));
//        }

//        [Fact]
//        public async Task Handle_ShouldSaveUserInDatabase()
//        {
//            User user = null!;
//            byte[] hash = new byte[] { 4, 5, 6 };
//            byte[] salt = new byte[] { 1, 2, 3 };
//            var userRegistration = new UserRegister()
//            {
//                Username = "testUser",
//                Password = "testPassword",
//                EmailAddress = "email@email.com",
//            };

//            void addUserAction(User param) => user = param;

//            AddUserCommand command = new(userRegistration);

//            Mock<IHashAndSalt> hashAndSaltMock = new(MockBehavior.Strict);

//            hashAndSaltMock.Setup(x => x.CreateHash(userRegistration.Password, out hash, out salt))
//                .Verifiable();

//            Mock<IUserRepository> userRepositoryMock = new(MockBehavior.Strict);

//            userRepositoryMock.Setup(x => x.FindByUsername(userRegistration.Username))
//                .Returns(() => Task.FromResult<User>(null!)!)
//                .Verifiable();

//            userRepositoryMock.Setup(x => x.AddUser(It.IsAny<User>()))
//                .Returns(Task.FromResult(Guid.NewGuid()))
//                .Callback((Action<User>)addUserAction);

//            AddUserHandler handler = new(hashAndSaltMock.Object, userRepositoryMock.Object);
//            await handler.Handle(command, CancellationToken.None);

//            hashAndSaltMock.Verify();
//            userRepositoryMock.Verify();

//            Assert.NotNull(user);
//            Assert.True(user.Username == userRegistration.Username);
//            Assert.Equal(user.PasswordHash, hash);
//            Assert.Equal(user.PasswordSalt, salt);
//            Assert.True(user.EmailAddress == userRegistration.EmailAddress);
//            Assert.True(user.Role == UserRole.Regular);
//        }

//        private static User? User => new()
//        {
//            Id = Guid.NewGuid(),
//            Username = "username1",
//            EmailAddress = "emailAddress@info.com",
//            Role = UserRole.Regular,
//            PasswordHash = Array.Empty<byte>(),
//            PasswordSalt = Array.Empty<byte>(),
//        };
//    }
//}
