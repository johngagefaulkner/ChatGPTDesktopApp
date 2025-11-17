using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace ChatGPTDesktopApp;

public sealed partial class MainPage : Page
{
    private static MainPage? _current;
    public static MainPage? Current => _current;
    public static Frame? CurrentAppFrame => Current?.AppContentFrame;
    public static NavigationView? CurrentAppNavigationView => Current?.AppNavigationView;

    private Thickness DefaultUserProfileButtonPadding;

    public MainPage()
    {
        InitializeComponent();
        _current = this;

        // Store the default padding value
        DefaultUserProfileButtonPadding = UserAccountNavigationViewButton.Padding;
    }

    private void AppContentFrame_Loaded(object sender, RoutedEventArgs e)
    {
        if (AppContentFrame.Content == null)
        {
            AppContentFrame.Navigate(typeof(Pages.NewChatPage));
        }
    }

    private void AppContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.SourcePageType is null)
        {
            return;
        }
        else if (e.SourcePageType == typeof(Pages.SettingsPage))
        {
            AppNavigationView.SelectedItem = AppNavigationView.SettingsItem;
            return;
        }
        else
        {
            //EnsureCorrectNavigationViewItemSelected(e.SourcePageType);
            EnsureCorrectNavigationViewItemSelectedViaLINQ(e.SourcePageType);
        }
    }

    /// <summary>
    /// Ensures that the navigation view selects the item corresponding to the specified page type.
    /// </summary>
    /// <remarks>This method updates the selected item in the navigation view to match the provided page type.
    /// If no matching item is found, the selection remains unchanged.</remarks>
    /// <param name="pageType">The type of the page for which the corresponding navigation view item should be selected. Must not be null.</param>
    private void EnsureCorrectNavigationViewItemSelected(Type pageType)
    {
        var pageName = pageType.Name;
        foreach (var item in AppNavigationView.MenuItems)
        {
            if (item is NavigationViewItem navigationViewItem)
            {
                if (navigationViewItem.Tag != null && navigationViewItem.Tag.ToString() == pageName)
                {
                    AppNavigationView.SelectedItem = navigationViewItem;
                    break;
                }
            }
        }
    }

    private void EnsureCorrectNavigationViewItemSelectedViaLINQ(Type pageType)
    {
        var pageName = pageType.Name;
        var selectedItem = AppNavigationView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(i => i.Tag != null && i.Tag.ToString() == pageName);

        if (selectedItem != null)
        {
            AppNavigationView.SelectedItem = selectedItem;
        }
    }

    /// <summary>
    /// Toggles the NavigationView pane open or closed, adjusting the user profile picture size and visibility accordingly.
    /// </summary>
    public void ToggleNavigationViewPane()
    {
        if (AppNavigationView.IsPaneOpen)
        {
            // If the pane is open, close it and minimize user profile picture
            UserAccountNavigationViewButton.Padding = new Thickness(1);
            UserAccountNavigationViewButton.Margin = new Thickness(1);
            UserProfilePicture.Height = 26;
            UserProfilePicture.Width = 26;
            UserProfilePicture.Margin = new Thickness(4, 4, 0, 4);
            UserProfileDisplayNameTextBlock.Visibility = Visibility.Collapsed;
            UserProfileSubscriptionStatusTextBlock.Visibility = Visibility.Collapsed;
            AppNavigationView.IsPaneOpen = false;
            return;
        }
        else
        {
            // If the pane is closed, open it and maximize user profile picture
            UserAccountNavigationViewButton.Padding = DefaultUserProfileButtonPadding;
            UserAccountNavigationViewButton.Margin = new Thickness(2);
            UserProfilePicture.Height = 40;
            UserProfilePicture.Width = 40;
            UserProfilePicture.Margin = new Thickness(4, 4, 10, 4);
            UserProfilePicture.HorizontalAlignment = HorizontalAlignment.Left;
            UserProfileDisplayNameTextBlock.Visibility = Visibility.Visible;
            UserProfileSubscriptionStatusTextBlock.Visibility = Visibility.Visible;
            AppNavigationView.IsPaneOpen = true;
            return;
        }
    }

    private void AppNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            AppContentFrame.Navigate(typeof(Pages.SettingsPage));
        }

        else
        {
            if (args.SelectedItem is NavigationViewItem selectedItem && selectedItem.Tag != null)
            {
                var pageName = selectedItem.Tag.ToString();
                var pagePath = $"ChatGPTDesktopApp.Pages.{pageName}";
                var pageType = Type.GetType(pagePath);

                if (pageType != null && AppContentFrame.CurrentSourcePageType != pageType)
                {
                    AppContentFrame.Navigate(pageType);
                }
            }
        }
    }

    private void OpenSettingsMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        AppContentFrame.Navigate(typeof(Pages.SettingsPage));
    }
}
