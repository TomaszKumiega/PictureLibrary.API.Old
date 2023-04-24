using MediatR;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers
{
    public class GetUserLibrariesHandler : IRequestHandler<GetUserLibrariesQuery, IEnumerable<Library>>
    {
        private readonly ILibraryRepository _libraryRepository;

        public GetUserLibrariesHandler(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<IEnumerable<Library>> Handle(GetUserLibrariesQuery request, CancellationToken cancellationToken)
            => await _libraryRepository.GetAll(request.UserId);
    }
}
