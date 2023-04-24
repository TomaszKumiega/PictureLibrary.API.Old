using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record GetTagsQuery(Guid UserId, Guid LibraryId) : IRequest<IEnumerable<Tag>>;
}
