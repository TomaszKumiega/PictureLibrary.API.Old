namespace PictureLibrary.DataAccess.Services
{
    public class FileService : IFileService
    {
        private static string AppFolder => "app/data";

        public void AppendFile(string filePath, byte[] buffer)
        {
            using var fileStream = new FileStream(filePath, FileMode.Append);
            fileStream.Write(buffer, 0, buffer.Length);
        }

        public string CreateFile(string fileName)
        {
            var path = AppFolder + Path.PathSeparator + fileName;
            File.Create(path);

            return path;
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        public string GetFileName(string filePath)
        {
            return filePath.Replace(GetFileExtension(filePath), string.Empty);
        }

        public long GetFileSize(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        public FileStream OpenFile(string path)
        {
            return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
        }
    }
}
