using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Repositories;

namespace PictureLibrary.DataAccess.Handlers
{
    public class UpdateLibraryHandler : IRequestHandler<UpdateLibraryCommand>
    {
        private readonly ILibraryRepository _libraryRepository;

        public UpdateLibraryHandler(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<Unit> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
        {
            await _libraryRepository.UpdateLibrary(request.Library);
            return Unit.Value;
        }
    }
}
