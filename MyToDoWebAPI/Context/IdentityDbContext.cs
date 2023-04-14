using Microsoft.EntityFrameworkCore;
using MyToDoWebAPI.Entities;

namespace MyToDoWebAPI.Context
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public IdentityDbContext(DbContextOptions options):base(options)
        {

        }
    }
}
