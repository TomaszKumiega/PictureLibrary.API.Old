using MediatR;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.GetImageFiles
{
    public class GetImageFilesHandler : IRequestHandler<GetImageFilesQuery, IEnumerable<ImageFile>>
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IImageFileRepository _imageFileRepository;

        public GetImageFilesHandler(
            ILibraryRepository libraryRepository,
            IImageFileRepository imageFileRepository)
        {
            _libraryRepository = libraryRepository;
            _imageFileRepository = imageFileRepository;    
        }

        public async Task<IEnumerable<ImageFile>> Handle(GetImageFilesQuery request, CancellationToken cancellationToken)
        {
            var library = await _libraryRepository.FindByIdAsync(request.LibraryId) ?? throw new NotFoundException(nameof(Library));

            if (!(library.Owners?.Any(x => x.Id == request.UserId) ?? false))
                throw new NotFoundException(nameof(Library));

            return await _imageFileRepository.GetAll(library.Id);
        }
    }
}
