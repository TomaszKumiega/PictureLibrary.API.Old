namespace PictureLibrary.Tools
{
    public interface IHashAndSalt
    {
        void CreateHash(string text, out byte[] hash, out byte[] salt);
        bool VerifyHash(string text, byte[] storedHash, byte[] storedSalt);
    }
}