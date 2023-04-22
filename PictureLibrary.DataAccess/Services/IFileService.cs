namespace PictureLibrary.DataAccess.Services
{
    public interface IFileService
    {
        FileStream OpenFile(string path);
    }
}
