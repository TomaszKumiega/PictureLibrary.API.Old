using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Model;
using PictureLibrary.Model.UploadSession;
using PictureLibrary.Tools.ContentRangeValidator;

namespace PictureLibrary.DataAccess.Handlers.UploadFile
{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand, ImageFile?>
    {
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly IImageFileRepository _imageFileRepository;
        private readonly IContentRangeValidator _contentRangeValidator;
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public UploadFileHandler(
            IFileService fileService,
            IUserRepository userRepository,
            IImageFileRepository imageFileRepository,
            IContentRangeValidator contentRangeValidator,
            IUploadSessionRepository uploadSessionRepository)
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _imageFileRepository = imageFileRepository;
            _contentRangeValidator = contentRangeValidator;
            _uploadSessionRepository = uploadSessionRepository;
        }

        public async Task<ImageFile?> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindById(request.UserId) ?? throw new NotFoundException(nameof(User));
            var uploadSession = await _uploadSessionRepository.FindUploadSessionById(request.UploadSessionId) ?? throw new NotFoundException(nameof(UploadSession));

            if (!_contentRangeValidator.IsContentRangeContinuationOfOldContentRange(uploadSession.ContentRange, request.ContentRange, request.BytesRead))
            {
                throw new ArgumentException("Content range is invalid");
            }

            _fileService.AppendFile(uploadSession.FilePath, request.Buffer);

            if (_contentRangeValidator.IsUploadComplete(request.ContentRange))
            {

                await _uploadSessionRepository.DeleteUploadSession(request.UploadSessionId);

                var imageFile = new ImageFile()
                {
                    FilePath = uploadSession.FilePath,
                    Extension = _fileService.GetFileExtension(uploadSession.FilePath),
                    Name = _fileService.GetFileName(uploadSession.FilePath),
                    Size = _fileService.GetFileSize(uploadSession.FilePath),
                    Libraries = uploadSession.Libraries,
                };

                imageFile.Id = await _imageFileRepository.AddImageFile(imageFile);

                return imageFile;
                
            }
            else
            {
                uploadSession.ContentRange = request.ContentRange;

                await _uploadSessionRepository.UpdateUploadSession(uploadSession);

                return null;
            }
        }
    }
}
