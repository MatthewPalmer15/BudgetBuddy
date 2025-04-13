using BudgetBuddy.Platforms.Windows;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

namespace BudgetBuddy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSyncfusionBlazor();

        SyncfusionLicenseProvider.RegisterLicense(
            "Mzc5ODE5NUAzMjM5MmUzMDJlMzAzYjMyMzkzYktkUE5KdDBMZXo1cURzM3RlRW9ka29xVmQ4cEdlTVpnQkp5Y1Q5UXM0NVE9");

        < Button Text = "Click Me"
        BackgroundColor = "{OnPlatform iOS='Blue', Android='Green', WinUI='Red'}" />

#if WINDOWS
        builder.Services.AddWindowsServices();
#endif

#if ANDROID
        Console.WriteLine("");
#endif

#if IOS
        Console.WriteLine("");
#endif

#if MACCATALYST
        Console.Write("here");
#endif




#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        //builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachedRequestBehaviour<,>));

        return builder.Build();
    }
}