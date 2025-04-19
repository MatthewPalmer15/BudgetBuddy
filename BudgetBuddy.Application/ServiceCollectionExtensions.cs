using BudgetBuddy.Database;
using BudgetBuddy.Infrastructure;

namespace BudgetBuddy.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddInfrastructure();
        return services;
    }
}