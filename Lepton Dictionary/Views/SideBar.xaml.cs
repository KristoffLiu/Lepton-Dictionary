using Lepton_Dictionary.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Lepton_Dictionary.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SideBar : Page
    {
        SideBarViewModel ViewModel = new SideBarViewModel();
        public SideBar()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        private async void CompactOverlayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainPage.Current.CommandBarFlyout1.Hide();
            await AppViewModel.OpenCompactOverlayView();
        }

        private void NightModeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainPage.Current.CommandBarFlyout1.Hide();
            ViewModel.IsNightMode = !ViewModel.IsNightMode;
        }

        private void OpenSettingPageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainPage.Current.MainFrame.Navigate(typeof(SettingPage));
            //MainPage.Current.CommandBarFlyout1.Hide();
            //MainPage.Current.SettingPageGrid.Children.Add(new SettingPage());
            //MainPage.Current.TopBarButton.Visibility = Visibility.Collapsed;
        }
    }
}
