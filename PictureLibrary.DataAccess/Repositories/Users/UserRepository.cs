using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseAccess<User> _databaseAccess;

        public UserRepository(IDatabaseAccess<User> databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task AddUser(User user)
        {
            string sql = @"
INSERT INTO Users (Id, Username, PasswordSalt, PasswordHash, EmailAddress, Role)
VALUES (@Id, @Username, @PasswordSalt, @PasswordHash, @EmailAddress, @Role)";

            await _databaseAccess.SaveDataAsync(sql, user);
        }

        public async Task<User?> FindByUsername(string username)
        {
            var parameters = new { Username = username };
            string sql = @"
SELECT * FROM Users
WHERE Username = @Username";

            var users = await _databaseAccess.LoadDataAsync(sql, parameters);

            return users.SingleOrDefault();
        }
    }
}
