using BudgetBuddy.Infrastructure.Encryption;
using BudgetBuddy.Infrastructure.Services;
using BudgetBuddy.Infrastructure.Services.Caching;
using BudgetBuddy.Infrastructure.Services.Csv;
using BudgetBuddy.Infrastructure.Services.Notification;
using BudgetBuddy.Infrastructure.Services.Serialization;
using BudgetBuddy.Infrastructure.Services.Toast;
#if ANDROID
using BudgetBuddy.Infrastructure.Platforms.Android;
#endif

/* Unmerged change from project 'BudgetBuddy.Infrastructure (net9.0-android)'
Before:
#if WINDOWS
After:
using BudgetBuddy.Infrastructure.Services.Serialization;
#if WINDOWS
*/

#if WINDOWS
using BudgetBuddy.Infrastructure.Platforms.Windows;
using BudgetBuddy.Infrastructure.Services.Serialization;
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
        services.AddScoped<IJsonSerializer, JsonSerializer>();
        services.AddScoped<IXmlSerializer, XmlSerializer>();


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