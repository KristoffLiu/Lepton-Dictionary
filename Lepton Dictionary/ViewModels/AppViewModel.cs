using Lepton_Dictionary.Views;
using Lepton_Library.Common;
using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lepton_Dictionary.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        public static AppViewModel Current;
        public AppViewModel()
        {
            Current = this;
        }
        
        public static void AsignValueToApplicationView()
        {
            if (CoreApplication.GetCurrentView().IsMain)
            {
                MainView = ApplicationView.GetForCurrentView();
            }
            else
            {
                CompactOverlayView = ApplicationView.GetForCurrentView();
            }
        }

        public static ApplicationView MainView { get; set; }
        public static ApplicationView CompactOverlayView { get; set; }

        public static CoreApplicationView CompactOverlayCoreApplicationView { get; set; }

        private ElementTheme _theme = ElementTheme.Light;
        public ElementTheme Theme
        {
            get { return _theme; }
            set
            {
                Set(ref _theme, value);

                PageViewModelBase.SwitchTheme(value);
            }
        }


        public void SwitchView()
        {

        }


        public static bool IsInCompactOverlayMode = false; //记录是否在画中画模式

        public static bool IsCompactOverlayModeSupported  //确定当前设备是否支持制定的视图模式，这里就是画中画模式
        {
            get { return ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay); }
        }

        public static async Task<bool> OpenCompactOverlayView()
        {
            bool _modeswitchstatus = false; // 切换模式成功指示器
            if (CompactOverlayView == null)
            {
                CompactOverlayCoreApplicationView = CoreApplication.CreateNewView();
                int newViewId = 0;
                await CompactOverlayCoreApplicationView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Frame frame = new Frame();
                    frame.Navigate(typeof(CompactOverlay_HomePage), null);
                    Window.Current.Content = frame;
                    // You have to activate the window in order to show it later.
                    Window.Current.Activate();
                    CompactOverlayView = ApplicationView.GetForCurrentView();
                    bool a = CompactOverlayView.IsViewModeSupported(ApplicationViewMode.CompactOverlay);
                    newViewId = CompactOverlayView.Id;
                });
                bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
                ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                compactOptions.CustomSize = new Windows.Foundation.Size(350, 50); //调整画中画模式的窗口初始大小
                await CompactOverlayView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
                //进入默认视图模式
            }
            else
            {
                ShowCompactOverlayView();
            }
            return IsInCompactOverlayMode = _modeswitchstatus ? !IsInCompactOverlayMode : IsInCompactOverlayMode; //如果切换模式成功，则逆转这个值
        }

        public static async void NewCompactOverlayView()
        {
        }

        public static async void ShowMainView()
        {
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(MainView.Id);
        }

        public static async void ShowCompactOverlayView()
        {
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(CompactOverlayView.Id);
            ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
            compactOptions.CustomSize = new Windows.Foundation.Size(350, 100); //调整画中画模式的窗口初始大小
            await CompactOverlayView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
        }



        public static void UpdateTextBoxAndQueryWord(string _word)
        {
            if (ApplicationView.GetForCurrentView().Id == MainView.Id)
            {
                HomePage.Current.AutoSuggestBox.Text = _word;
                HomePageViewModel.Current.QueryWord(_word);
            }
            else
            {
                CompactOverlay_HomePage.Current.AutoSuggestBox.Text = _word;
                CompactOverlay_HomePageViewModel.Current.CompactOverlay_QueryWord(_word);

            }
        }

        public static void UpdateTextBoxOnly(string _word)
        {
            if (ApplicationView.GetForCurrentView().Id == MainView.Id)
            {
                HomePage.Current.AutoSuggestBox.Text = _word;
            }
            else
            {
                CompactOverlay_HomePage.Current.AutoSuggestBox.Text = _word;
            }
        }
    }
}
