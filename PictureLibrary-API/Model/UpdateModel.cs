using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Model
{
    public class UpdateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public UpdateModel()
        {

        }

        public UpdateModel(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
