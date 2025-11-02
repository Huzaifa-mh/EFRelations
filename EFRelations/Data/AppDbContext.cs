using EFRelations.Models;
using Microsoft.EntityFrameworkCore;

namespace EFRelations.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Profile> Profiles => Set<Profile>();
    }

}
