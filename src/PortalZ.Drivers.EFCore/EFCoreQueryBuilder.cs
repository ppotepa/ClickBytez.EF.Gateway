using PortalZ.Drivers;
using PortalZ.Filters.Querying;
using System;
using System.Linq;

namespace PortalZ.Drivers.EFCore
{
    /// <summary>
    /// Query builder for Entity Framework Core.
    /// Translates filter strings into LINQ expression trees.
    /// </summary>
    public class EFCoreQueryBuilder : IQueryBuilder
    {
        /// <summary>
        /// Applies filters to an EF Core IQueryable using LINQ expression trees.
        /// </summary>
        public IQueryable ApplyFilters(IQueryable source, string[]? filters)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (filters == null || filters.Length == 0) return source;

            // Delegate to the existing regex-based filter extension in PortalZ.Core
            return source.ApplyFiltersRegex(filters);
        }
    }
}
