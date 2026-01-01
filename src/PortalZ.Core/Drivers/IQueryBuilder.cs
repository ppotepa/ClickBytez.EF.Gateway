#nullable enable

using System;
using System.Linq;

namespace PortalZ.Drivers
{
    /// <summary>
    /// Abstraction for building and executing queries against different data stores.
    /// Different implementations can handle filter syntax differently based on the underlying store.
    /// </summary>
    public interface IQueryBuilder
    {
        /// <summary>
        /// Applies filters to a queryable data source.
        /// </summary>
        /// <param name="source">The queryable source (IQueryable for EF, IEnumerable for raw SQL results, etc.)</param>
        /// <param name="filters">Filter strings in the format "property.operator(value)"</param>
        /// <returns>Filtered queryable source</returns>
        IQueryable ApplyFilters(IQueryable source, string[]? filters);
    }
}
