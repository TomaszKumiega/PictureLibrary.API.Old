using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Handlers;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;
using PictureLibrary.Tools;

namespace PictureLibrary.DataAccess.Tests
{
    public class AddUserHandlerTests
    {
        [Fact]
        public void Handle_ShouldThrowResourceAlreadyExistsException_IfUserAlreadyExists()
        {
            var user = new UserRegister()
            {
                Username = "testUser",
                Password = "testPassword",
                EmailAddress = "email@email.com",
            };

            AddUserCommand command = new(user);

            Mock<IHashAndSalt> hashAndSaltMock = new(MockBehavior.Strict);
            Mock<IUserRepository> userRepositoryMock = new(MockBehavior.Strict);

            userRepositoryMock.Setup(x => x.FindByUsername(user.Username))
                .Returns(Task.FromResult(User))
                .Verifiable();

            AddUserHandler handler = new(hashAndSaltMock.Object, userRepositoryMock.Object);

            Assert.ThrowsAsync<ResourceAlreadyExistsException>(() => handler.Handle(command, CancellationToken.None));
        }

        private static User? User => new()
        {
            Id = Guid.NewGuid(),
            Username = "username1",
            EmailAddress = "emailAddress@info.com",
            Role = "role",
            PasswordHash = Array.Empty<byte>(),
            PasswordSalt = Array.Empty<byte>(),
        };
    }
}
