using MediatR;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Queries.Responses;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.GetFile
{
    public class GetFileHandler : IRequestHandler<GetFileQuery, GetFileQueryResponse>
    {
        private readonly IFileService _fileService;
        private readonly IImageFileRepository _imageFileRepository;

        public GetFileHandler(
            IFileService fileService,
            IImageFileRepository imageFileRepository)
        {
            _fileService = fileService;
            _imageFileRepository = imageFileRepository;    
        }

        public async Task<GetFileQueryResponse> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            ImageFile? imageFile = await _imageFileRepository.FindImageFileById(request.ImageFileId) ?? throw new NotFoundException(nameof(ImageFile));
        
            if (!(imageFile.Libraries?.Any(x => x.Owners?.Any(owner => owner.Id == request.UserId) == true) == true)) 
            {
                throw new NotFoundException(nameof(ImageFile));
            }

            return new GetFileQueryResponse(imageFile, _fileService.OpenFile(imageFile.FilePath));
        }
    }
}
