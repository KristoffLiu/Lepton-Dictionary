using Lepton_Dictionary.Models;
using Lepton_Dictionary.ViewModels;
using Lepton_Dictionary.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Timers;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Lepton_Dictionary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel = new MainPageViewModel();
        public static MainPage Current;

        public MainPage()
        {
            Current = this;
            this.InitializeComponent();
            DataContext = ViewModel;
            #region 标题栏设置 Title Bar Setting
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true; //扩展标题栏
            Window.Current.SetTitleBar(TitleBar); //设置某个容器空间（我一般就用的Grid）为标题栏，即可拖拽来位移窗体
            //注意：该控件必须要有颜色，哪怕是Transparent透明也行。如果不设置background，它就是空的没法进行拖拽。
            Models.TitleBar.SetTransparentColor();
            #endregion

            //ApplicationView.PreferredLaunchViewSize = new Size(200, 800);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(350, 100));

            if (AppService.IsCompactView)
            {
                MainFrame.Navigate(typeof(CompactView_HomePage));
            }
            else
            {
                MainFrame.Navigate(typeof(HomePage));
            }
            if (AppService.IsUpdated())
            {
                UpdateInfoPage.Visibility = Visibility.Visible;
            }
            else
            {
                UpdateInfoPage.Visibility = Visibility.Collapsed;
            }
        }

        public static async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
        }

        public void CompactOverlayView()
        {
            MainFrame.Navigate(typeof(CompactOverlay_HomePage));
        }
        public void NormalView()
        {
            if (AppService.IsCompactView)
            {
                MainFrame.Navigate(typeof(CompactView_HomePage));
            }
            else
            {
                MainFrame.Navigate(typeof(HomePage));
            }
        }

        private void SplitView_DragEnter(object sender, DragEventArgs e)
        {
            bool hasText = e.DataView.Contains(StandardDataFormats.Text);
            e.AcceptedOperation = hasText ? DataPackageOperation.Copy : DataPackageOperation.None;
            if (hasText)
            {
                if (e.DragUIOverride != null)
                {
                    e.DragUIOverride.Caption = "释放就可以查询这个词语啦~";
                }
            }
            DragGrid.Visibility = Visibility.Visible;
            DragGridOpen.Begin();
        }

        private void Grid_DragLeave(object sender, DragEventArgs e)
        {
            DragGridClose.Begin();
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            //如果有文字，那么就是把他放在要拖放的TextBlock
            bool hasText = e.DataView.Contains(StandardDataFormats.Text);
            //显示复制还是没有
            //拖动显示可以去我之前写的 http://blog.csdn.net/lindexi_gd/article/details/49757187?locationNum=2&fps=1
            e.AcceptedOperation = hasText ? DataPackageOperation.Copy : DataPackageOperation.None;
            if (hasText)
            {
                var text = await e.DataView.GetTextAsync();
                if (AppViewModel.IsInCompactOverlayMode)
                {
                    CompactOverlay_HomePage.Current.AutoSuggestBox.Text = text;
                    CompactOverlay_HomePageViewModel.Current.CompactOverlay_QueryWord(text);
                }
                else
                {
                    HomePage.Current.AutoSuggestBox.Text = text;
                    HomePageViewModel.Current.QueryWord(text);
                }
            }
            DragGridClose.Begin();
        }

        private void DoubleAnimation_Completed(object sender, object e)
        {
            DragGrid.Visibility = Visibility.Collapsed;
        }

        public static void Add_Notification(NotificationBar notificationBar)
        {
            Current.NotificationArea.Children.Add(notificationBar);
        }

        public static void Remove_Notification(NotificationBar notificationBar)
        {
            foreach (var item in Current.NotificationArea.Children)
            {
                if (item == notificationBar)
                {
                    Current.NotificationArea.Children.Remove(item);
                }
            }
        }



        private void TopBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TopBarButton_TranslateTransform_Jump.Begin();
            //ShowMenu(false, TopBarButton);
            TitleBar_Grid_Expand.Begin();
        }

        private void ShowMenu(bool isTransient, FrameworkElement fe)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            CommandBarFlyout1.ShowAt(fe, myOption);
        }

        private void TitleBar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TitleBar_Grid_Collapse.Begin();
            Models.TitleBar.SetTransparentColor();
            TitleBar_Grid_Close.Begin();
        }

        private void TitleBar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            TitleBar_Grid_Open.Begin();
            Models.TitleBar.SetContrastColor();
        }

        private void TitleBar_Grid_Close_Completed_1(object sender, object e)
        {
        }
        private void Page_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ClipboardSense.Update(0);
        }

        private void Page_PointerExited(object sender, PointerRoutedEventArgs e)
        {

        }

        private void TopBarButton_TranslateTransform_Jump_Completed(object sender, object e)
        {
            TopBarButton_TranslateTransform_Jump_Again.Begin();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                BackButton.Visibility = Visibility.Collapsed;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                BackButton.Visibility = Visibility.Visible;
            }
        }
    }
}
