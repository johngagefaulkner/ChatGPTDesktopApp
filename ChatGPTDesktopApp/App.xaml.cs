namespace ChatGPTDesktopApp;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// The current singleton-instance of the <see cref="MainWindow"/> for the application.
    /// </summary>
    internal static MainWindow MainWindowInstance { get; private set; } = null!;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        if (MainWindowInstance is null)
        {
            MainWindowInstance = new();
        }
        
        MainWindowInstance.Activate();
        Log.LogInfo("Application launched.");
    }

    /// <summary>
    /// Changes the <see cref="ElementTheme"/> applied to the application's content and updates the <see cref="TitleBar"/> theme accordingly.
    /// </summary>
    /// <param name="newTheme">The targeted <see cref="ElementTheme"/> to apply.</param>
    public static void ChangeAppTheme(ElementTheme newTheme) => MainWindowInstance.ChangeAppContentTheme(newTheme);
}
