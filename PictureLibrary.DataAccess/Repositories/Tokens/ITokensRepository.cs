using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public interface ITokensRepository
    {
        Task AddTokensAsync(Tokens tokens);
        Task UpdateTokensAsync(Tokens tokens);
        Task<Tokens?> FindByUserIdAsync(Guid userId);
    }
}