using PortalZ.Context;
using PortalZ.Model;
using Microsoft.EntityFrameworkCore;

namespace PortalZ
{
    public class ReflectiveInMemoryContext : ExtendedDbContext
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(nameof(ReflectiveInMemoryContext));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DataSeed.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}





