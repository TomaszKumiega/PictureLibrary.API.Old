using MediatR;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers
{
    public class FindLibraryByIdHandler : IRequestHandler<FindLibraryByIdQuery, Library?>
    {
        private readonly ILibraryRepository _libraryRepository;

        public FindLibraryByIdHandler(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<Library?> Handle(FindLibraryByIdQuery request, CancellationToken cancellationToken)
            => await _libraryRepository.FindByIdAsync(request.LibraryId);
    }
}
