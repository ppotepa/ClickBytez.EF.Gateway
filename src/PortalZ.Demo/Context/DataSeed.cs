using PortalZ.Model;
using Microsoft.EntityFrameworkCore;

namespace PortalZ.Context
{
    public static class DataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var users = new List<User>();
            for (int i = 1; i <= 20; i++)
            {
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Name = $"User {i}",
                    Email = $"user{i}@example.com"
                });
            }
            modelBuilder.Entity<User>().HasData(users);

            var addresses = new List<Address>();
            for (int i = 1; i <= 20; i++)
            {
                addresses.Add(new Address
                {
                    Id = Guid.NewGuid(),
                    Street = $"{i * 10} Example St",
                    City = $"City {i}",
                    State = $"State {i}",
                    PostalCode = $"{10000 + i}",
                    Country = "USA",
                    UserId = users[i - 1].Id
                });
            }
            modelBuilder.Entity<Address>().HasData(addresses);

            var articles = new List<Article>();
            for (int i = 1; i <= 20; i++)
            {
                articles.Add(new Article
                {
                    Id = Guid.NewGuid(),
                    Title = $"Article {i}",
                    Content = $"This is the content of article {i}.",
                    UserId = users[i - 1].Id
                });
            }
            modelBuilder.Entity<Article>().HasData(articles);
        }
    };
}





