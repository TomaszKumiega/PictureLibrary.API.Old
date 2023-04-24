using MediatR;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.GetTags
{
    public class GetTagsHandler : IRequestHandler<GetTagsQuery, IEnumerable<Tag>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILibraryRepository _libraryRepository;

        public GetTagsHandler(
            ITagRepository tagRepository,
            ILibraryRepository libraryRepository)
        {
            _tagRepository = tagRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<IEnumerable<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var library = await _libraryRepository.FindByIdAsync(request.LibraryId) ?? throw new NotFoundException(nameof(Library));

            if (library.Owners?.Any(x => x.Id == request.UserId) == false)
                throw new NotFoundException(nameof(Library));

            return await _tagRepository.GetTags(request.LibraryId);
        }
    }
}
