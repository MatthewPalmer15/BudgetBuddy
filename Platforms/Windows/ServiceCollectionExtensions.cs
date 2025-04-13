using BudgetBuddy.Platforms.Windows.Services;

namespace BudgetBuddy.Platforms.Windows;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddWindowsServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, WindowsNotificationService>();
        return services;
    }

}