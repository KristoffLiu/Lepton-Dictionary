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
    public sealed partial class DailySentencePage : Page
    {
        public DailySentencePageViewModel ViewModel;
        public DailySentencePage()
        {
            ViewModel = new DailySentencePageViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }

        private void Rc_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {

        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void FontIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void MediaElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var _sender = sender as FontIcon;
            mediaelement.Source = new Uri(((string)(_sender.Tag)));
            mediaelement.Play();
        }
    }
}
