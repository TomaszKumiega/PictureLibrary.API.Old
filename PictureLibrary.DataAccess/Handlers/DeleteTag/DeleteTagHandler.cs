using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.DeleteTag
{
    public class DeleteTagHandler : IRequestHandler<DeleteTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILibraryRepository _libraryRepository;

        public DeleteTagHandler(
            ITagRepository tagRepository,
            ILibraryRepository libraryRepository)
        {
            _tagRepository = tagRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var library = await _libraryRepository.FindByIdAsync(request.LibraryId) ?? throw new NotFoundException(nameof(Library));
            var tag = await _tagRepository.FindTagById(request.TagId) ?? throw new NotFoundException(nameof(Tag));
            
            await _tagRepository.DeleteTag(tag.Id, library.Id);

            return Unit.Value;
        }
    }
}
