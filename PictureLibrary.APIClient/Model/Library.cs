namespace PictureLibrary.APIClient.Model
{
    public class Library
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required List<User> Owners { get; set; }
    }
}
