namespace ChatGPTDesktopApp.Pages;

/// <summary>
/// The SettingsPage class represents the settings page of the ChatGPT Desktop Application.
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
        Log.LogInfo("SettingsPage initialized.");
    }

    /// <summary>
    /// Determines if the provided tag matches the specified theme index.
    /// </summary>
    /// <param name="tag">The tag to check.</param>
    /// <param name="themeIndex">The theme index to match against.</param>
    /// <returns>True if the tag matches the theme index; otherwise, false.</returns>
    private static bool MatchesTheme(object? tag, int themeIndex)
    {
        if (tag is null)
            return false;
        return int.TryParse(tag.ToString(), out int tagIndex) && tagIndex == themeIndex;
    }

    private void ThemeSelectorComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        // Load the AppTheme value saved from user settings
        if (AppDataService.ContainsKey("AppTheme"))
        {
            var savedThemeIndex = AppDataService.GetValue<int>("AppTheme");
            var savedMatchingItem = ThemeSelectorComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => MatchesTheme(item.Tag, savedThemeIndex));

            if (savedMatchingItem is not null)
            {
                ThemeSelectorComboBox.SelectedItem = savedMatchingItem;
            }
        }

        else
        {
            ThemeSelectorComboBox.SelectedIndex = (int)App.MainWindowInstance.GetAppContentTheme();
        }
    }

    private void ThemeSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            if (int.TryParse(selectedItem.Tag.ToString(), out int themeIndex))
            {
                var newTheme = (ElementTheme)themeIndex;

                if (newTheme != App.MainWindowInstance.GetAppContentTheme())
                {
                    // Save the selected theme index to user settings
                    App.ChangeAppTheme(newTheme);
                }
            }
        }
    }
}
