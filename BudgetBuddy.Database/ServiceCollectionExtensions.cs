namespace BudgetBuddy.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();
        services.AddScoped<IDbContext, ApplicationDbContext>();
        return services;
    }
}