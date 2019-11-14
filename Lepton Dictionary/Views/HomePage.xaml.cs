using Lepton_Dictionary.ViewModels;
using Lepton_Library.Interaction;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Lepton_Dictionary.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public static HomePage Current;
        HomePageViewModel ViewModel = new HomePageViewModel();

        public HomePage()
        {
            this.InitializeComponent();
            Current = this;
            DataContext = ViewModel;

            this.NavigationCacheMode = NavigationCacheMode.Required;

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
                .Select(async temp => await ViewModel.UpdateSuggestionListAsync(temp.Sender.Text));
            var notUserInput = input
                .ObserveOnDispatcher()
                .Where(temp => temp.EventArgs.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
                .Select(temp => Task.FromResult<List<WordSuggestionViewModel>>(null));
            var merge = Observable
                .Merge(notUserInput, userInput);
            merge
                .ObserveOnDispatcher()
                .Subscribe(async suggestions =>
                {
                    AutoSuggestBox.ItemsSource = await suggestions;
                });
        }


        //.Where(temp => !string.IsNullOrEmpty(temp.Sender.Text))
        //.Do(async temp => await ViewModel.UpdateSuggestionListAsync(AutoSuggestBox.Text));

        bool IsOpen = false;

        private void TextBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                //DefinationFrame.Navigate(typeof(Dictionary_DefinationPage), QueryTextBox.Text);
            }
        }

        public void Open()
        {
            DoubleAnimation doubleAnimation = SearchSuggestionPageOpen.Children[0] as DoubleAnimation;
            doubleAnimation.To = 0;
            SearchSuggestionPageOpen.Begin();
            IsOpen = true;
        }

        public void Close()
        {
            DoubleAnimation doubleAnimation = SearchSuggestionPageClose.Children[0] as DoubleAnimation;
            doubleAnimation.To = height + 10;
            SearchSuggestionPageClose.Begin();
            IsOpen = false;
        }

        double height = 0;
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            height = e.NewSize.Height;
            if (IsOpen == true)
            {

            }
            else
            {
                SearchSuggestionPageTranslateTransform.Y = ( e.NewSize.Height + 10);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsOpen == true)
            {
                Close();
            }
            else
            {
                Open();
            }
        }


        //语音识别 Voice Recognition
        private async void StartRecognizing_Click(object sender, RoutedEventArgs e)
        {
            if(await SpeechRecognition.RequestMicrophonePermission())
            {
                // Create an instance of SpeechRecognizer.
                var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

                // Compile the dictation grammar by default.
                await speechRecognizer.CompileConstraintsAsync();

                // Start recognition.
                Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

                // Do something with the recognition result.
                HomePageViewModel.Current.QueryWord(speechRecognitionResult.Text);
                //var messageDialog = new Windows.UI.Popups.MessageDialog(speechRecognitionResult.Text, "Text spoken");
                //await messageDialog.ShowAsync();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            textbox.SelectAll();
        }
    }
}
