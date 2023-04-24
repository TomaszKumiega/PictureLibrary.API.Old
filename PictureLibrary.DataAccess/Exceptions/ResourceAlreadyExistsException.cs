namespace PictureLibrary.DataAccess.Exceptions
{
    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException(string resourceName) 
            : base($"{resourceName} already exists.")
        {
        }
    }
}
