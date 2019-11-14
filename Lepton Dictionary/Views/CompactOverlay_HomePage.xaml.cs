using Lepton_Dictionary.Models;
using Lepton_Dictionary.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI;
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
    public sealed partial class CompactOverlay_HomePage : Page
    {
        public static CompactOverlay_HomePage Current;
        CompactOverlay_HomePageViewModel ViewModel;

        public CompactOverlay_HomePage()
        {
            ViewModel = new CompactOverlay_HomePageViewModel();
            this.InitializeComponent();
            Current = this;
            DataContext = ViewModel;

            #region 标题栏设置 Title Bar Setting
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true; //扩展标题栏
            Window.Current.SetTitleBar(TitleBar); //设置某个容器空间（我一般就用的Grid）为标题栏，即可拖拽来位移窗体
            //注意：该控件必须要有颜色，哪怕是Transparent透明也行。如果不设置background，它就是空的没法进行拖拽。
            var view = ApplicationView.GetForCurrentView();//来进行以下一系列设置标题栏组件颜色的操作

            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(38, 0, 0, 0);
            view.TitleBar.ButtonHoverForegroundColor = Colors.White;
            view.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(70, 0, 0, 0);
            view.TitleBar.ButtonPressedForegroundColor = Colors.White;
            view.TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            #endregion

            var changed =
                Observable.FromEventPattern<TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>, AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs>(
                handler => AutoSuggestBox.TextChanged += handler,
                handler => AutoSuggestBox.TextChanged -= handler);
            var input = changed
                .DistinctUntilChanged(temp => temp.Sender.Text)
                .Throttle(TimeSpan.FromMilliseconds(200));
            var userInput = input
                .ObserveOnDispatcher()
                .Where(temp => temp.EventArgs.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                //.Where(temp => !string.IsNullOrEmpty(temp.Sender.Text))
                .Select(async temp => await ViewModel.UpdateSuggestionListAsync(temp.Sender.Text));
            var notUserInput = input
                .ObserveOnDispatcher()
                .Where(temp => temp.EventArgs.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
                .Select(temp => Task.FromResult<List<WordSuggestionViewModel>>(null)); ;
            var merge = Observable
                .Merge(notUserInput, userInput);
            merge
                .ObserveOnDispatcher()
                .Subscribe(async suggestions =>
                {
                    AutoSuggestBox.ItemsSource = await suggestions;
                });
        }

        public void MenuButton_Click()
        {
            ShowMenu(false, AutoSuggestBox);
        }

        public void ShowMenu(bool isTransient, FrameworkElement fe)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            myOption.Placement = FlyoutPlacementMode.Bottom;
            CommandBarFlyout1.ShowAt(fe, myOption);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CommandBarSecondaryCommandsMenu.Width = e.NewSize.Width;
            AutoSuggestBox.Height = e.NewSize.Height;
        }

        private void MenuButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void AmericanPronunciationButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    PronunciationPlayer.Source = new Uri(ViewModel.AiCiBaWordViewModel.Pronunciations.ph_am_mp3);
            //    PronunciationPlayer.Play();
            //}
            //catch
            //{

            //}
        }

        private void BritishPronunciationButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    PronunciationPlayer.Source = new Uri(ViewModel.AiCiBaWordViewModel.Pronunciations.ph_en_mp3);
            //    PronunciationPlayer.Play();
            //}
            //catch
            //{

            //}
        }

        //朗读选中文本
        private async void OnRead(string text)
        {
            if (text != "")
            {
                SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync(text);
                //Me为MediaElement AutoPlay true；
                PronunciationPlayer.SetSource(stream, stream.ContentType);
            }
        }


        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMenu2(false, (Button)sender);
            ViewModel.GetWordNoteInfo();
        }

        private void ShowMenu2(bool isTransient, FrameworkElement fe)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            CommandBarFlyout1.ShowAt(fe, myOption);
        }

        public static void Add_Notification(CompactOverlay_NotificationBar notificationBar)
        {
            Current.NotificationArea.Children.Add(notificationBar);
        }

        public static void Remove_Notification(CompactOverlay_NotificationBar notificationBar)
        {
            foreach (var item in Current.NotificationArea.Children)
            {
                if (item == notificationBar)
                {
                    Current.NotificationArea.Children.Remove(item);
                }
            }
        }

        private void Page_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ClipboardSense.Update(1);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as PronounciationViewModel;
            try
            {
                PronunciationPlayer.Source = new Uri(item.SoundUri);
                PronunciationPlayer.Play();
            }
            catch
            {

            }
        }
    }
}
