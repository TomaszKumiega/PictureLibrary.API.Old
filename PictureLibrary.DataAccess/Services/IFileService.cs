namespace PictureLibrary.DataAccess.Services
{
    public interface IFileService
    {
        FileStream OpenFile(string path);
        string CreateFile(string fileName);
        void DeleteFile(string filePath);
        void AppendFile(string filePath, byte[] buffer);
        string GetFileExtension(string filePath);
        string GetFileName(string filePath);
        long GetFileSize(string filePath);
    }
}
