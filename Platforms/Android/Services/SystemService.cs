#if ANDROID

using Android.Content;
using Android.Content.Res;

namespace BudgetBuddy.Platforms.Android.Services;

public class SystemService(Context context)
{
    public AppTheme GetSystemPreference()
    {
        if (AppInfo.RequestedTheme != AppTheme.Unspecified)
            return AppInfo.RequestedTheme;

        var currentNightMode = context?.Resources?.Configuration?.UiMode & UiMode.NightMask;
        return ((currentNightMode & UiMode.NightMask) == UiMode.NightYes) ? AppTheme.Dark : AppTheme.Light;
    }


}
#endif