using PortalZ.Abstractions.Entities;
using PortalZ.Drivers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortalZ.Drivers.EFCore
{
    /// <summary>
    /// Entity Framework Core implementation of the data driver.
    /// Handles all CRUD operations and entity discovery using EF Core.
    /// </summary>
    public class EFCoreDriver : IDataDriver
    {
        private readonly DbContext _dbContext;
        private readonly IQueryBuilder _queryBuilder;
        private Type[]? _cachedEntities;

        public EFCoreDriver(DbContext dbContext, IQueryBuilder queryBuilder)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
        }

        /// <summary>
        /// Discovers entity types from the DbContext.
        /// </summary>
        public Type[] DiscoverEntities()
        {
            if (_cachedEntities is not null)
                return _cachedEntities;

            _cachedEntities = _dbContext.Model.GetEntityTypes()
                .Select(et => et.ClrType)
                .ToArray();

            return _cachedEntities;
        }

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        public int Create(IEntity entity)
        {
            _dbContext.Add(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Reads entities with optional filtering.
        /// </summary>
        public IEnumerable<object> Read(Type entityType, string[]? filters = null)
        {
            var dbSetMethod = _dbContext.GetType()
                .GetMethod(nameof(DbContext.Set), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                ?.MakeGenericMethod(entityType);

            if (dbSetMethod == null)
                throw new InvalidOperationException($"Unable to create DbSet for type {entityType.Name}");

            var queryable = dbSetMethod.Invoke(_dbContext, null) as IQueryable;
            if (queryable == null)
                throw new InvalidOperationException($"Failed to create queryable for type {entityType.Name}");

            // Apply filters if provided
            if (filters != null && filters.Length > 0)
            {
                queryable = _queryBuilder.ApplyFilters(queryable, filters);
            }

            return queryable.Cast<object>().ToList();
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        public int Update(IEntity entity)
        {
            _dbContext.Update(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        public int Delete(IEntity entity)
        {
            _dbContext.Remove(entity);
            return _dbContext.SaveChanges();
        }
    }
}
