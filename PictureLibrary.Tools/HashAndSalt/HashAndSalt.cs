namespace PictureLibrary.Tools
{
    public class HashAndSalt : IHashAndSalt
    {
        public void CreateHash(string text, out byte[] passwordHash, out byte[] passwordSalt)
        {
            ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));

            using var hmac = new System.Security.Cryptography.HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));
        }

        public bool VerifyHash(string text, byte[] storedHash, byte[] storedSalt)
        {
            ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                    return false;
            }

            return true;
        }
    }
}
