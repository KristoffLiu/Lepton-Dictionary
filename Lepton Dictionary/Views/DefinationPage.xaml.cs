
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Lepton_Dictionary.ViewModels;
using Lepton_Dictionary;
using Microsoft.Toolkit.Uwp.UI.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Lepton.Views.DictionaryPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DefinationPage : Page
    {
        public static DefinationPage Current;
        public DefinationPageViewModel ViewModel { get; set; } = new DefinationPageViewModel();

        public DefinationPage()
        {
            Current = this;
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        
        //public Dictionary_DefinitionModel DictionaryManager = new Dictionary_DefinitionModel();
        #region 


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //await DictionaryManager.QueryWord(e.Parameter.ToString(),ViewModel);
            //try
            //{
            //    AmericanPronunciationText.Text = DictionaryManager.BingDictionaryService.DictionaryModel.pronunciation.AmE;
            //    BritishPronunciationText.Text = DictionaryManager.BingDictionaryService.DictionaryModel.pronunciation.BrE;
            //}
            //catch
            //{

            //}
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
        #endregion

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.FavoriteItem(QueryTextBlock.Text);
        }

        //public void OpenMainPanel()
        //{
        //    if (MainPage.Current.ViewModel.Width >= 750 )
        //    {
        //        MainPanelTranslateTransform.Y = 0;
        //    }
        //    else
        //    {
        //        MainPanelOpen.Begin();
        //    }
        //}

        //public void CloseMainPanel()
        //{
        //    if (MainPage.Current.ViewModel.Width >= 750)
        //    {
        //        MainPanelTranslateTransform.Y = -190;
        //    }
        //    else
        //    {
        //        MainPanelClose.Begin();
        //    }
        //}

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMenu(false, (Button)sender);
            ViewModel.GetWordNoteInfo();
        }

        private void ShowMenu(bool isTransient, FrameworkElement fe)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            CommandBarFlyout1.ShowAt(fe, myOption);
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
