using Microsoft.EntityFrameworkCore;

namespace BlazorHybrid.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RunDatabaseMigrations(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        IEnumerable<Type>? contextTypes = services
            .Where(sd => sd.ServiceType.IsAssignableTo(typeof(DbContext)))
            .Select(sd => sd.ServiceType);

        foreach (var contextType in contextTypes)
        {
            var dbContext =
                (DbContext)scope.ServiceProvider.GetRequiredService(contextType);

            dbContext.Database.EnsureCreated();

            if (dbContext.Database.GetPendingMigrations().Any()) dbContext.Database.Migrate();
        }

        return services;
    }
};