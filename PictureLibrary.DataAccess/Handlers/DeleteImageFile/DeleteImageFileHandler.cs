using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.DeleteImageFile
{
    public class DeleteImageFileHandler : IRequestHandler<DeleteImageFileCommand>
    {
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly IImageFileRepository _imageFileRepository;

        public DeleteImageFileHandler(
            IFileService fileService,
            IUserRepository userRepository,
            IImageFileRepository imageFileRepository)
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _imageFileRepository = imageFileRepository;
        }

        public async Task<Unit> Handle(DeleteImageFileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindById(request.UserId) ?? throw new NotFoundException(nameof(User));
            var imageFile = await _imageFileRepository.FindImageFileById(request.ImageFileId) ?? throw new NotFoundException(nameof(ImageFile));

            _fileService.DeleteFile(imageFile.FilePath);
            await _imageFileRepository.DeleteImageFile(request.ImageFileId);

            return Unit.Value;
        }
    }
}
