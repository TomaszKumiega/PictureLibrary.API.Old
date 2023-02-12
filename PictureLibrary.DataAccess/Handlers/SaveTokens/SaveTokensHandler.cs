using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Repositories;

namespace PictureLibrary.DataAccess.Handlers
{
    public class SaveTokensHandler : IRequestHandler<SaveTokensCommand>
    {
        private readonly ITokensRepository _tokensRepository;

        public SaveTokensHandler(ITokensRepository tokensRepository)
        {
            _tokensRepository = tokensRepository;
        }

        public async Task<Unit> Handle(SaveTokensCommand request, CancellationToken cancellationToken)
        {
            var tokens = await _tokensRepository.FindByUserIdAsync(request.Tokens.UserId);
            if (tokens is not null)
            {
                await _tokensRepository.UpdateTokensAsync(request.Tokens);
            }
            else
            {
                await _tokensRepository.AddTokensAsync(request.Tokens);
            }

            return Unit.Value;
        }
    }
}
