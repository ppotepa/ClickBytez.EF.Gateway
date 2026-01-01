#nullable enable

using PortalZ.Abstractions.Entities;
using System;
using System.Collections.Generic;

namespace PortalZ.Drivers
{
    /// <summary>
    /// Abstraction for data access operations. Implementations can use EF Core, raw SQL, 
    /// or any other data store mechanism.
    /// </summary>
    public interface IDataDriver
    {
        /// <summary>
        /// Discovers all entity types available in the data store.
        /// </summary>
        Type[] DiscoverEntities();

        /// <summary>
        /// Creates (inserts) a new entity into the data store.
        /// </summary>
        int Create(IEntity entity);

        /// <summary>
        /// Reads entities with optional filtering.
        /// </summary>
        IEnumerable<object> Read(Type entityType, string[]? filters = null);

        /// <summary>
        /// Updates an existing entity in the data store.
        /// </summary>
        int Update(IEntity entity);

        /// <summary>
        /// Deletes an entity from the data store.
        /// </summary>
        int Delete(IEntity entity);
    }
}
