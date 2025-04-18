using BlazorHybrid.Infrastructure.Encryption;
using BlazorHybrid.Infrastructure.Services;
using BlazorHybrid.Infrastructure.Services.Caching;
using BlazorHybrid.Infrastructure.Services.Csv;
using BlazorHybrid.Infrastructure.Services.Notification;
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

namespace BlazorHybrid.Infrastructure;

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