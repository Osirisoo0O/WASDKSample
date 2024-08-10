using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AppHost.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppHost
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void HomeNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                HomeNav_Nagigate(typeof(SettingPage), args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                Type navPageType = Type.GetType(args.InvokedItemContainer.Tag.ToString());
                HomeNav_Nagigate(navPageType, args.RecommendedNavigationTransitionInfo);
            }
        }


        private void HomeNav_Loaded(object sender, RoutedEventArgs e)
        {
            AppFrame.Navigated += On_Navigated;
            HomeNav.SelectedItem = HomeNav.MenuItems[0];
            HomeNav_Nagigate(typeof(CalculatePage), new EntranceNavigationTransitionInfo());
        }

       
        private void HomeNav_Nagigate(Type navPageType,NavigationTransitionInfo transitionInfo)
        {
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = AppFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                AppFrame.Navigate(navPageType, null, transitionInfo);
            }

        }

        private void HomeNav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        bool TryGoBack()
        {
            if (!AppFrame.CanGoBack) return false;
            if (HomeNav.IsPaneOpen &&
                ( HomeNav.DisplayMode == NavigationViewDisplayMode.Compact ||
                HomeNav.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;
            AppFrame.GoBack();
            return true;
        }
        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            HomeNav.IsBackEnabled = AppFrame.CanGoBack;

            if (AppFrame.SourcePageType == typeof(SettingPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                HomeNav.SelectedItem = (NavigationViewItem)HomeNav.SettingsItem;
                HomeNav.Header = "Setting";
            }
            else if (AppFrame.SourcePageType != null)
            {
                // Select the nav view item that corresponds to the page being navigated to.
                HomeNav.SelectedItem = HomeNav.MenuItems
                            .OfType<NavigationViewItem>()
                            .First(i => i.Tag.Equals(AppFrame.SourcePageType.FullName.ToString()));

                HomeNav.Header =
                    ((NavigationViewItem)HomeNav.SelectedItem)?.Content?.ToString();

            }
        }
    }
}
