using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;
using PictureLibrary.Tools;

namespace PictureLibrary.DataAccess.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly IHashAndSalt _hashAndSalt;
        private readonly IUserRepository _userRepository;

        public AddUserHandler(
            IHashAndSalt hashAndSalt,
            IUserRepository userRepository)
        {
            _hashAndSalt = hashAndSalt;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByUsername(request.Username!);

            if (user != null)
                throw new ResourceAlreadyExistsException(nameof(User));

            _hashAndSalt.CreateHash(request.Password!, out byte[] passwordHash, out byte[] passwordSalt);

            Guid userId = await _userRepository.AddUser(new User()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Username = request.Username!,
                EmailAddress = request.EmailAddress!,
                Role = Model.Users.UserRole.Regular,
            });

            return userId;
        }
    }
}
