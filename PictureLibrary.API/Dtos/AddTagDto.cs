﻿namespace PictureLibrary.API.Dtos
{
    public class AddTagDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ColorHex { get; set; }
        public List<Guid>? Libraries { get; set; }
    }
}
