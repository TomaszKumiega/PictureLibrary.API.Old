﻿namespace PictureLibrary.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
        public string? Role { get; set; }
    }
}
