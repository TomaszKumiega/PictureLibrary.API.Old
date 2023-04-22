using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model.UploadSession;

namespace PictureLibrary.DataAccess.Repositories
{
    public class UploadSessionRepository : IUploadSessionRepository
    {
        private readonly IDatabaseAccess<UploadSession> _databaseAccess;

        public UploadSessionRepository(IDatabaseAccess<UploadSession> databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public async Task<UploadSession?> FindUploadSessionById(Guid id)
        {
            string sql = @"
SELECT * FROM UploadSessions
WHERE Id = @Id";

            var uploadSessions = await _databaseAccess.LoadDataAsync(sql, new { Id = id.ToString() });

            return uploadSessions.FirstOrDefault();
        }

        public async Task<Guid> AddUploadSession(UploadSession uploadSession)
        {
            uploadSession.Id = Guid.NewGuid();

            string sql = @"
INSERT INTO UploadSessions
VALUES (@Id, @ContentRange, @FilePath)";

            await _databaseAccess.SaveDataAsync(sql, uploadSession);

            return uploadSession.Id;
        }

        public async Task UpdateUploadSession(UploadSession uploadSession)
        {
            string sql = @"
UPDATE UploadSessions
SET ContentRange = @ContentRange,
    FilePath = @FilePath
WHERE Id = @Id";

            await _databaseAccess.SaveDataAsync(sql, uploadSession);
        }

        public async Task DeleteUploadSession(Guid id)
        {
            string sql = @"
DELETE FROM UploadSessions
WHERE Id = @Id";

            await _databaseAccess.SaveDataAsync(sql, new { Id = id.ToString() });
        }
    }
}
