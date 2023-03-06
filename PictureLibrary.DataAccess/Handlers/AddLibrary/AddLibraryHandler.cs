using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Repositories;

namespace PictureLibrary.DataAccess.Handlers
{
    public class AddLibraryHandler : IRequestHandler<AddLibraryCommand, Guid>
    {
        private readonly ILibraryRepository _libraryRepository;

        public AddLibraryHandler(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<Guid> Handle(AddLibraryCommand request, CancellationToken cancellationToken)
            => await _libraryRepository.AddLibrary(request.Library, request.UserId);
    }
}
