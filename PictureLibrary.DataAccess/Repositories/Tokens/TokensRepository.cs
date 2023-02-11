using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public class TokensRepository : ITokensRepository
    {
        private readonly IDatabaseAccess<Tokens> _databaseAccess;

        public TokensRepository(IDatabaseAccess<Tokens> databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<Tokens?> FindByUserIdAsync(Guid userId)
        {
            var parameters = new { UserId = userId };
            string sql = @"
SELECT * FROM Tokens
WHERE UserId == @UserId";

            var tokens = await _databaseAccess.LoadDataAsync(sql, parameters);

            return tokens.SingleOrDefault();
        }

        public async Task AddTokensAsync(Tokens tokens)
        {
            string sql = @"
INSERT INTO Tokens (Id, UserId, AccessToken, RefreshToken, ExpiryDate)
VALUES (@Id, @UserId, @AccessToken, @RefreshToken, @ExpiryDate)";

            await _databaseAccess.SaveDataAsync(sql, tokens);
        }

        public async Task UpdateTokensAsync(Tokens tokens)
        {
            string sql = @"
UPDATE Tokens
SET AccessToken = @AccessToken, 
    RefreshToken = @RefreshToken, 
    ExpiryDate = @ExpiryDate";

            await _databaseAccess.SaveDataAsync(sql, tokens);
        }
    }
}
