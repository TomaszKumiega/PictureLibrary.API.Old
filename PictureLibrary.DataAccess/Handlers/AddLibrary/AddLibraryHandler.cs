using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

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
        {
            Library library = new()
            {
                Name = request.Library.Name,
                Description = request.Library.Description,
            };

            return await _libraryRepository.AddLibrary(library, request.UserId);
        }
    }
}
