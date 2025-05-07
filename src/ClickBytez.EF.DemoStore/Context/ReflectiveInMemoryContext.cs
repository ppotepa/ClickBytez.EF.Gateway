using ClickBytez.EF.DemoStore.Context;
using ClickBytez.EF.DemoStore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClickBytez.EF.DemoStore
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
