using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Model
{
    public interface IDatabaseContext
    {
        DbSet<User> Users { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default);
    }
}
