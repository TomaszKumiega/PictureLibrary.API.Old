namespace PictureLibrary.DataAccess.Services
{
    public interface IFileService
    {
        FileStream OpenFile(string path);
        string CreateFile(string fileName);
        void DeleteFile(string filePath);
    }
}
