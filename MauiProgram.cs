using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;

namespace BudgetBuddy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSyncfusionBlazor();

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzc5ODE5NUAzMjM5MmUzMDJlMzAzYjMyMzkzYktkUE5KdDBMZXo1cURzM3RlRW9ka29xVmQ4cEdlTVpnQkp5Y1Q5UXM0NVE9");

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
