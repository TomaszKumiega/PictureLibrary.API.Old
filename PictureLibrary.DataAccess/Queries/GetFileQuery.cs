using MediatR;
using PictureLibrary.DataAccess.Queries.Responses;

namespace PictureLibrary.DataAccess.Queries
{
    public record GetFileQuery(Guid UserId, Guid ImageFileId) : IRequest<GetFileQueryResponse>;
}
