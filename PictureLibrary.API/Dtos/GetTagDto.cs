﻿namespace PictureLibrary.API.Dtos
{
    public class GetTagDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string ColorHex { get; set; }
        public required IEnumerable<Guid> Libraries { get; set; }
    }
}
