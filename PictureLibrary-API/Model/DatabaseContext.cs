using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
