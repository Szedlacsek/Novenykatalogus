using Microsoft.EntityFrameworkCore;
using PlantCatalogue.Models;

namespace PlantCatalogue.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Plants> Plants { get; set; }
        public DbSet<Tools> Tools { get; set; }
        public DbSet<Switch> Switch { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
