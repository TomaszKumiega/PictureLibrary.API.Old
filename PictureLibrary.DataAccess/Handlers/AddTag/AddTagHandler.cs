using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.AddTag
{
    public class AddTagHandler : IRequestHandler<AddTagCommand, Guid>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILibraryRepository _libraryRepository;

        public AddTagHandler(
            ITagRepository tagRepository,
            ILibraryRepository libraryRepository)
        {
            _tagRepository = tagRepository;
            _libraryRepository = libraryRepository;    
        }

        public async Task<Guid> Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            List<Library> libraries = new();

            foreach (var libraryId in request.Libraries!)
            {
                var library = await _libraryRepository.FindByIdAsync(libraryId) ?? throw new NotFoundException(nameof(Library));
                libraries.Add(library);
            }

            if (libraries.Select(x => x.Owners?.Any(x => x.Id == request.UserId)).Any(x => x == false))
                throw new NotFoundException(nameof(Library));

            var tag = new Tag()
            {
                Name = request.Name!,
                Description = request.Description,
                ColorHex = request.ColorHex!,
                Libraries = libraries,
            };


            return await _tagRepository.AddTag(tag);
        }
    }
}
