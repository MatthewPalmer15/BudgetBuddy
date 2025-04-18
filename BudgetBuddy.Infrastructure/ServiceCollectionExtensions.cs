using BudgetBuddy.Infrastructure.Encryption;
using BudgetBuddy.Infrastructure.Services;
using BudgetBuddy.Infrastructure.Services.Notification;
using BudgetBuddy.Infrastructure.Services.Csv;
using BudgetBuddy.Infrastructure.Platforms.MacCatalyst;
using BudgetBuddy.Infrastructure.Services.Caching;
using BudgetBuddy.Infrastructure.Platforms.Windows;
using BudgetBuddy.Infrastructure.Platforms.iOS;
using BudgetBuddy.Infrastructure.Platforms.Android;










#if ANDROID
using BlazorHybrid.Infrastructure.Platforms.Android;
#endif

#if WINDOWS
using BlazorHybrid.Infrastructure.Platforms.Windows;
#endif

#if IOS
using BlazorHybrid.Infrastructure.Platforms.iOS;
#endif

#if MACCATALYST
using BlazorHybrid.Infrastructure.Platforms.MacCatalyst;
#endif

namespace BudgetBuddy.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<ICsvService, CsvService>();

        services.AddMemoryCache();
        services.AddScoped<ICacheManager, CacheManager>();


#if ANDROID
        services.AddScoped<INotificationEngine, NotificationEngine>();
        services.AddScoped<IFileSystemManager, FileSystemManager>();
#endif
#if WINDOWS
        services.AddScoped<INotificationEngine, NotificationEngine>();
        services.AddScoped<IFileSystemManager, FileSystemManager>();
#endif
#if IOS
        services.AddScoped<INotificationEngine, NotificationEngine>();
        services.AddScoped<IFileSystemManager, FileSystemManager>();
#endif
#if MACCATALYST
        services.AddScoped<INotificationEngine, NotificationEngine>();
        services.AddScoped<IFileSystemManager, FileSystemManager>();
#endif

        return services;
    }
}