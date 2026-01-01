using PortalZ.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PortalZ.Drivers.EFCore
{
    /// <summary>
    /// Extension methods for registering the EF Core data driver.
    /// </summary>
    public static class EFCoreDriverExtensions
    {
        /// <summary>
        /// Registers the EF Core driver and query builder.
        /// </summary>
        public static IServiceCollection AddEFCoreDriver(this IServiceCollection services, Type dbContextType)
        {
            if (!typeof(DbContext).IsAssignableFrom(dbContextType))
                throw new ArgumentException($"Type {dbContextType.Name} must inherit from DbContext", nameof(dbContextType));

            // Register the DbContext
            services.AddScoped(dbContextType);

            // Register as generic DbContext for convenience
            services.AddScoped(typeof(DbContext), (provider) =>
                provider.GetService(dbContextType) ?? throw new InvalidOperationException($"Could not resolve {dbContextType.Name}"));

            // Register the query builder
            services.AddScoped<IQueryBuilder, EFCoreQueryBuilder>();

            // Register the driver factory
            services.AddScoped<IDataDriver>(provider =>
            {
                var dbContext = provider.GetRequiredService<DbContext>();
                var queryBuilder = provider.GetRequiredService<IQueryBuilder>();
                return new EFCoreDriver(dbContext, queryBuilder);
            });

            return services;
        }
    }
}
