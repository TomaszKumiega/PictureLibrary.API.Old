using PictureLibrary.DataAccess.DatabaseAccess;
using PictureLibrary.Model;
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
SELECT _uploadSession.*, _library.* FROM UploadSessions _uploadSession
LEFT JOIN LibraryUploadSessions _libraryUploadSession
ON _libraryUploadSession.UploadSessionId = _uploadSession.Id
LEFT JOIN Libraries _library
ON _library.Id = _libraryUploadSession.LibraryId
WHERE _uploadSession.Id = @Id";

            IEnumerable<(UploadSession UploadSession, Library Library)> result = await _databaseAccess.LoadDataAsync<(UploadSession, Library)>(sql, new { Id = id.ToString() });

            return result.GroupBy(x => x.UploadSession.Id)
                .Select(g =>
                {
                    var uploadSession = g.First().UploadSession;
                    uploadSession.Libraries= g.Select(x => x.Library)
                    .Where(x => x != null)
                    .ToList();

                    return uploadSession;
                })
                .FirstOrDefault();
        }

        public async Task<Guid> AddUploadSession(UploadSession uploadSession)
        {
            uploadSession.Id = Guid.NewGuid();

            string sql = @"
INSERT INTO UploadSessions
VALUES (@Id, @ContentRange, @FilePath, @UserId)";

            await _databaseAccess.SaveDataAsync(sql, uploadSession);

            await AddLibraryUploadSessions(uploadSession);

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

            await DeleteLibraryUploadSessions(id);

            await _databaseAccess.SaveDataAsync(sql, new { Id = id.ToString() });
        }

        #region LibraryUploadSessions
        private async Task AddLibraryUploadSessions(UploadSession uploadSession)
        {
            string sql = @"
INSERT INTO LibraryUploadSessions
VALUES (@Id, @LibraryId, @UploadSessionId)";

            foreach (var library in uploadSession.Libraries)
            {
                var parameters = new 
                { 
                    Id = Guid.NewGuid().ToString(),
                    LibraryId = library.Id,
                    UploadSessionId = uploadSession.Id,
                };

                await _databaseAccess.SaveDataAsync(sql, parameters);
            }
        }

        private async Task DeleteLibraryUploadSessions(Guid uploadSessionId)
        {
            string sql = @"
DELETE FROM LibraryUploadSessions
WHERE UploadSessionId = @Id";

            await _databaseAccess.SaveDataAsync<object>(sql, new { Id = uploadSessionId.ToString() });
        }
        #endregion
    }
}
