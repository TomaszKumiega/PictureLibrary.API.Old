﻿using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;
using PictureLibrary.Tools;

namespace PictureLibrary.DataAccess.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand>
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

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByUsername(request.User.Username!);

            if (user != null)
                throw new ResourceAlreadyExists(nameof(User));

            _hashAndSalt.CreateHash(request.User.Password!, out byte[] passwordHash, out byte[] passwordSalt);

            await _userRepository.AddUser(new User()
            {
                Id = Guid.NewGuid(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Username = request.User.Username!,
                EmailAddress = request.User.EmailAddress!,
                Role = "User",
            });

            return Unit.Value;
        }
    }
}
