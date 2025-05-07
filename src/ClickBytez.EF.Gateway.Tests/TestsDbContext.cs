using ClickBytez.EF.DemoStore;
using ClickBytez.EF.Gateway.Core.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using static ClickBytez.EF.Gateway.Tests.Controller.ActionControllerTests;

namespace ClickBytez.EF.Gateway.Tests
{
    public class TestsDbContext : ExtendedDbContext
    {
        public DbSet<TestEntity> Users { get; set; }

        public static readonly IEntity UserToDelete = new TestEntity()
        {
            Id = Guid.Parse("00000000-9225-41DB-9C2C-963C7FC1E362"),
            Name = "User To Delete"
        };

        public static readonly IEntity UserToUpdate = new TestEntity()
        {
            Id = Guid.Parse("11111111-9225-41DB-9C2C-963C7FC1E362"),
            Name = "User To Update"
        };

        private static readonly IEnumerable<IEntity> SeedData = Enumerable
            .Range(1, 20)
            .Select(i => new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = $"Test Entity {i}"
            })
            .Append(UserToDelete)
            .Append(UserToUpdate)
            .ToArray();



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(nameof(TestsDbContext));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>().HasData(SeedData);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
