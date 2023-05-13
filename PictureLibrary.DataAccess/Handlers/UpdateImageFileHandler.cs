using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers
{
    public class UpdateImageFileHandler : IRequestHandler<UpdateImageFileCommand>
    {
        private readonly IFileService _fileService;
        private readonly IImageFileRepository _imageFileRepository;

        public UpdateImageFileHandler(
            IFileService fileService,
            IImageFileRepository imageFileRepository)
        {
            _fileService = fileService;
            _imageFileRepository = imageFileRepository;
        }

        public async Task<Unit> Handle(UpdateImageFileCommand request, CancellationToken cancellationToken)
        {
            var imageFile = await _imageFileRepository.FindImageFileById(request.ImageFileId) ?? throw new NotFoundException(nameof(ImageFile));

            //TODO: check if user owns this image file

            imageFile.Name = request.Name;
            imageFile.FilePath = _fileService.RenameFile(imageFile.FilePath, $"{imageFile.Name}.{imageFile.Extension}");

            await _imageFileRepository.UpdateImageFile(imageFile);

            return Unit.Value;
        }
    }
}
