#if ANDROID

using Android.Content;
using Android.Content.Res;

namespace BudgetBuddy.Services.System;

public class AndroidSystemService(Context context) : ISystemService
{
    public AppTheme GetSystemPreference()
    {
        if (AppInfo.RequestedTheme != AppTheme.Unspecified)
            return AppInfo.RequestedTheme;

        var currentNightMode = context?.Resources?.Configuration?.UiMode & UiMode.NightMask;
        return (currentNightMode & UiMode.NightMask) == UiMode.NightYes ? AppTheme.Dark : AppTheme.Light;
    }

    public string GetAppId()
    {
        return "";
    }
}
#endif