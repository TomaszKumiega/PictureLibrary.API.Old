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

        public async Task<Guid> AddUser(User user)
        {
            user.Id = Guid.NewGuid();

            string sql = @"
INSERT INTO Users (Id, Username, PasswordSalt, PasswordHash, EmailAddress, Role)
VALUES (@Id, @Username, @PasswordSalt, @PasswordHash, @EmailAddress, @Role)";

            await _databaseAccess.SaveDataAsync(sql, user);

            return user.Id;
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

        public async Task DeleteUser(Guid userId)
        {
            var parameters = new { Id = userId.ToString() };
            string sql = @"
DELETE FROM Users
WHERE Id = @Id";

            await _databaseAccess.SaveDataAsync(sql, parameters);
        }

        public async Task<User?> FindById(Guid userId)
        {
            var parameters = new { Id = userId.ToString() };
            string sql = @"
SELECT * FROM Users
WHERE Id = @Id";

            var users = await _databaseAccess.LoadDataAsync(sql, parameters);
            
            return users.SingleOrDefault();
        }

        public async Task<IEnumerable<User>> FindByPartialUsername(string username)
        {
            var parameters = new { Username = username };
            string sql = @"
SELECT * FROM Users 
WHERE Username LIKE '%@Username%'";

            return await _databaseAccess.LoadDataAsync(sql, parameters);
        }
    }
}
