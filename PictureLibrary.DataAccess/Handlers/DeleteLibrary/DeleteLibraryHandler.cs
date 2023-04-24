using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.DeleteLibrary
{
    public class DeleteLibraryHandler : IRequestHandler<DeleteLibraryCommand>
    {
        private readonly ILibraryRepository _libraryRepository;

        public DeleteLibraryHandler(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<Unit> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
        {
            var library = await _libraryRepository.FindByIdAsync(request.LibraryId) ?? throw new NotFoundException(nameof(Library));
            
            //TODO: delete image files from filesystem if they are not used by other library

            await _libraryRepository.DeleteLibrary(library);

            return Unit.Value;
        }
    }
}
