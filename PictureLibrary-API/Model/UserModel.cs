using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Model
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        public UserModel()
        {

        }

        public UserModel(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
