using Lepton_Dictionary.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SettingPage : Page
    {
        public static SettingPage Current;
        SettingPageViewModel ViewModel = new SettingPageViewModel();
        public SettingPage()
        {
            Current = this;
            this.InitializeComponent();
            DataContext = ViewModel;
            Open.Begin();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }





        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close.Begin();
        }

        private void BackgroundLayer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Close.Begin();
        }

        private void Close_Completed(object sender, object e)
        {
            MainPage.Current.SettingPageGrid.Children.RemoveAt(0);
            //MainPage.Current.TopBarButton.Visibility = Visibility.Visible;
        }


    }
}
