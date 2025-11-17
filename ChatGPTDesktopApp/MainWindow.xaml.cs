using Microsoft.UI;
using Microsoft.UI.Windowing;

namespace ChatGPTDesktopApp;

/// <summary>
/// The main <see cref="Window"/> for the application.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        AppWindow.TitleBar.PreferredTheme = Microsoft.UI.Windowing.TitleBarTheme.UseDefaultAppMode;
        AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
        AppWindow.TitleBar.BackgroundColor = Colors.Transparent;
        AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        AppWindow.TitleBar.InactiveBackgroundColor = Colors.Transparent;
        AppWindow.SetIcon("Assets/Icons/ChatGPTDesktopAppIcon.ico");

        LoadAndApplyUserSettings();

        Log.LogInfo("MainWindow initialized.");
    }

    private void AppTitleBar_BackRequested(TitleBar sender, object args)
    {
        if (MainPage.CurrentAppFrame?.CanGoBack == true)
        {
            MainPage.CurrentAppFrame.GoBack();
        }
    }

    private void AppTitleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        MainPage.Current?.ToggleNavigationViewPane();
    }

    /// <summary>
    /// Changes the <see cref="ElementTheme"/> applied to the application's content and updates the <see cref="TitleBar"/> theme accordingly.
    /// </summary>
    /// <param name="newTheme">The targeted <see cref="ElementTheme"/> to apply.</param>
    public void ChangeAppContentTheme(ElementTheme newTheme)
    {
        AppRootGrid.RequestedTheme = newTheme;

        // Map ElementTheme to TitleBarTheme
        AppWindow.TitleBar.PreferredTheme = newTheme switch
        {
            ElementTheme.Default => TitleBarTheme.UseDefaultAppMode,
            ElementTheme.Light => TitleBarTheme.Light,
            ElementTheme.Dark => TitleBarTheme.Dark,
            _ => TitleBarTheme.UseDefaultAppMode,
        };

        // Persist the new theme setting
        AppDataService.AddOrUpdateValue("AppTheme", (int)newTheme);
        Log.LogInfo($"Application theme changed to {newTheme}.");
    }

    /// <summary>
    /// Gets the current <see cref="ElementTheme"/> applied to the application's content.
    /// </summary>
    /// <returns>The current <see cref="ElementTheme"/>.</returns>
    public ElementTheme GetAppContentTheme() => AppRootGrid.RequestedTheme;

    /// <summary>
    /// Loads and applies user settings from persistent storage.
    /// </summary>
    private void LoadAndApplyUserSettings()
    {
        Log.LogInfo("Loading and applying custom UserSettings, please wait.. ");

        if (AppDataService.ContainsKey("AppTheme"))
        {
            var savedAppTheme = AppDataService.GetValue<int>("AppTheme");
            ChangeAppContentTheme((ElementTheme)savedAppTheme);
        }

        else
        {
            AppDataService.AddOrUpdateValue("AppTheme", (int)ElementTheme.Default);
            ChangeAppContentTheme(ElementTheme.Default);
        }

        Log.LogInfo("UserSettings loaded and applied successfully!");
    }
}
