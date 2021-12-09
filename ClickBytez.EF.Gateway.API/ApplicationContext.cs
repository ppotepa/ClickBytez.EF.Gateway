using ClickBytez.EF.Gateway.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClickBytez.EF.Gateway.API
{
    public class ApplicationContext : DbContext
    {
        private static IEnumerable<Type> Entities
        {
            get
            {
                if (_entities is null)
                {
                    _entities = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IExtendedEntity<Guid>))));
                }

                return _entities;
            }
        }
        private static IEnumerable<Type> _entities = default;
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=efapiexperimental;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var extended = typeof(IExtendedEntity<Guid>);

            foreach (Type entity in Entities)
            {
                modelBuilder.Entity(entity).HasKey(nameof(IExtendedEntity<byte>.Id));

                modelBuilder.Entity(entity).Property(extended.GetProperty(nameof(IExtendedEntity.CreatedOn)).PropertyType, nameof(IExtendedEntity.CreatedOn));
                modelBuilder.Entity(entity).Property(extended.GetProperty(nameof(IExtendedEntity.CreatedBy)).PropertyType, nameof(IExtendedEntity.CreatedBy));

                modelBuilder.Entity(entity).Property(extended.GetProperty(nameof(IExtendedEntity.DeletedOn)).PropertyType, nameof(IExtendedEntity.DeletedOn));
                modelBuilder.Entity(entity).Property(extended.GetProperty(nameof(IExtendedEntity.DeletedBy)).PropertyType, nameof(IExtendedEntity.DeletedBy));

                modelBuilder.Entity(entity).Property(extended.GetProperty(nameof(IExtendedEntity.ModifiedBy)).PropertyType, nameof(IExtendedEntity.ModifiedBy));
                modelBuilder.Entity(entity).Property(extended.GetProperty(nameof(IExtendedEntity.ModifiedOn)).PropertyType, nameof(IExtendedEntity.ModifiedOn));
            }
        }

        public override int SaveChanges()
        {
            const BindingFlags PRIVATE_FIELD_BINDING_ATTRS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase;

            ChangeTracker.DetectChanges();

            var entries = new
            {
                added = ChangeTracker.Entries().Where(entry => entry.State is EntityState.Added).Select(enry => enry.Entity as IEntity).ToList(),
                modified = ChangeTracker.Entries().Where(entry => entry.State is EntityState.Modified).Select(entry => entry.Entity as IEntity).ToList(),
                deleted = ChangeTracker.Entries().Where(entry => entry.State is EntityState.Deleted).Select(entry => entry.Entity as IEntity).ToList(),
            };

            entries.added.ForEach(added =>
            {
                typeof(IExtendedEntity<Guid>).GetField($"{nameof(IExtendedEntity<byte>.CreatedOn)}", PRIVATE_FIELD_BINDING_ATTRS).SetValue(added, DateTime.Now);
                typeof(IExtendedEntity<Guid>).GetField($"{nameof(IExtendedEntity<byte>.CreatedBy)}", PRIVATE_FIELD_BINDING_ATTRS).SetValue(added, Guid.NewGuid());
            });

            entries.modified.ForEach(modified =>
            {
                typeof(IExtendedEntity<Guid>).GetField($"{nameof(IExtendedEntity<byte>.ModifiedOn)}", PRIVATE_FIELD_BINDING_ATTRS).SetValue(modified, DateTime.Now);
                typeof(IExtendedEntity<Guid>).GetField($"{nameof(IExtendedEntity<byte>.ModifiedBy)}", PRIVATE_FIELD_BINDING_ATTRS).SetValue(modified, Guid.NewGuid());
            });

            entries.deleted.ForEach(deleted =>
            {
                typeof(IExtendedEntity<Guid>).GetField($"{nameof(IExtendedEntity<byte>.DeletedOn)}", PRIVATE_FIELD_BINDING_ATTRS).SetValue(deleted, DateTime.Now);
                typeof(IExtendedEntity<Guid>).GetField($"{nameof(IExtendedEntity<byte>.DeletedBy)}", PRIVATE_FIELD_BINDING_ATTRS).SetValue(deleted, Guid.NewGuid());
            });

            return base.SaveChanges();
        }
    }
}
