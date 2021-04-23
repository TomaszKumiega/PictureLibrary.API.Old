namespace PictureLibrary_API.Model
{
    public class Tag
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public Tag()
        {

        }

        public Tag(string name, string description, string color)
        {
            Name = name;
            Description = description;
            Color = color;
        }
    }
}
