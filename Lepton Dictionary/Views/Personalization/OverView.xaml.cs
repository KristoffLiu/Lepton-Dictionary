using Lepton_Dictionary.ViewModels.Personalization;
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

namespace Lepton_Dictionary.Views.Personalization
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class OverView : Page
    {
        OverViewViewModel ViewModel;
        public OverView()
        {
            ViewModel = new OverViewViewModel();
            DataContext = ViewModel;
            this.InitializeComponent();
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuFrame.Navigate(typeof(Theme));
            ShowMenu(false, (StackPanel)sender);
        }

        private void ShowMenu(bool isTransient, FrameworkElement fe)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            ManipulationFlyout.ShowAt(fe, myOption);
        }
    }
}
