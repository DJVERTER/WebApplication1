using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class LibrarieContext : DbContext
    {
        public LibrarieContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
