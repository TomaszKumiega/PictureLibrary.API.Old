namespace PictureLibrary_API.Model
{
    public class Tag : ILibraryEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Tag()
        {

        }

        public Tag(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
