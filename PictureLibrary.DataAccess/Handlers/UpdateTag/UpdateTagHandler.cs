using MediatR;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.UpdateTag
{
    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand>
    {
        private readonly ITagRepository _tagRepository;

        public UpdateTagHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;    
        }

        public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.FindTagById(request.TagId) ?? throw new NotFoundException(nameof(Tag));

            tag.Name = request.Name!;
            tag.Description = request.Description!;
            tag.ColorHex = request.ColorHex!;

            await _tagRepository.UpdateTag(tag, request.Libraries!);

            return Unit.Value;
        }
    }
}
