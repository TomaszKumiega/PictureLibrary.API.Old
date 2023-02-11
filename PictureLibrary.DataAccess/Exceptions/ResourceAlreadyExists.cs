namespace PictureLibrary.DataAccess.Exceptions
{
    public class ResourceAlreadyExists : Exception
    {
        public ResourceAlreadyExists(string resourceName) 
            : base($"{resourceName} already exists.")
        {
        }
    }
}
