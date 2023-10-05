using Microsoft.EntityFrameworkCore;
using DevsFreeWebAPI.Models;  // Adjust this namespace if your models are in a different location.

namespace DevsFreeWebAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        //DbSet properties
        public DbSet<Collaborator> Collaborators { get; set; } = default!;
        public DbSet<Partner> Partners { get; set; } = default!;
    }
}