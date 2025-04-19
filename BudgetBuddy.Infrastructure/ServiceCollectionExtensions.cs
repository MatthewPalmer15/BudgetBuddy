#if ANDROID
using BudgetBuddy.Infrastructure.Platforms.Android;
#endif

using BudgetBuddy.Infrastructure.Encryption;
using BudgetBuddy.Infrastructure.Services;
using BudgetBuddy.Infrastructure.Services.Caching;
using BudgetBuddy.Infrastructure.Services.Csv;
using BudgetBuddy.Infrastructure.Services.Notification;
using BudgetBuddy.Infrastructure.Services.Toast;
#if WINDOWS
using BudgetBuddy.Infrastructure.Platforms.Windows;
#endif

#if IOS
using BudgetBuddy.Infrastructure.Platforms.iOS;
#endif

#if MACCATALYST
using BudgetBuddy.Infrastructure.Platforms.MacCatalyst;
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

        services.AddScoped<IToastManager, ToastManager>();


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