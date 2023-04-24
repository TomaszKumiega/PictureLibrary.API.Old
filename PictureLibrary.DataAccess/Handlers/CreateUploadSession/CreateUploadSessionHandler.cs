using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Model;
using PictureLibrary.Model.UploadSession;

namespace PictureLibrary.DataAccess.Handlers.CreateUploadSession
{
    public class CreateUploadSessionHandler : IRequestHandler<CreateUploadSessionCommand, Guid>
    {
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public CreateUploadSessionHandler(
            IFileService fileService,
            IUserRepository userRepository,
            ILibraryRepository libraryRepository,
            IUploadSessionRepository uploadSessionRepository)
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _libraryRepository = libraryRepository;
            _uploadSessionRepository = uploadSessionRepository;
        }

        public async Task<Guid> Handle(CreateUploadSessionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindById(request.UserId) ?? throw new NotFoundException(nameof(User));

            List<Library> libraries = new();

            foreach (var libraryId in request.LibraryIds)
            {
                var library = await _libraryRepository.FindByIdAsync(libraryId) ?? throw new NotFoundException(nameof(Library));
                libraries.Add(library);
            }

            var filePath = _fileService.CreateFile(request.FileName);

            var uploadSession = new UploadSession()
            {
                FilePath = filePath,
                ContentRange = request.ContentRange,
                UserId = user.Id,
                Libraries = libraries,
            };

            return await _uploadSessionRepository.AddUploadSession(uploadSession);
        }
    }
}
