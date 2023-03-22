using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record DeleteTagCommand(Guid LibraryId, Guid TagId) : IRequest;
}
