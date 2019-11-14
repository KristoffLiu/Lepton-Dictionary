using Lepton_Dictionary.Models;
using Lepton_Dictionary.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Lepton_Library.Native;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Lepton_Dictionary.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchSuggestionPage : Page
    {
        public static SearchSuggestionPage Current;
        public SearchSuggestionPageViewModel ViewModel { get; set; }
        public SearchSuggestionPage()
        {
            Current = this;
            ViewModel = new SearchSuggestionPageViewModel();
            DataContext = this;
            this.InitializeComponent();
            WordNotePageFrame.Navigate(typeof(WordNoteBookPage));
        }




        //private void WordHistoryListItemClick(object sender, ItemClickEventArgs e)
        //{

        //}

        

        private void WordListDropShadowPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            foreach(var item in ViewModel.WordHistoryCollection)
            {
                if(item.ID == (int)(((DropShadowPanel)sender).Tag))
                {
                    ViewModel.SelectedWordHistoryModel = item;
                }
            }
                ShowMenu(false, (DropShadowPanel)sender);
        }

        private void ShowMenu(bool isTransient,FrameworkElement fe)
        {
            Word_SelectedHistoryItem.Text = ViewModel.SelectedWordHistoryModel.Word;
            //TranslationMode_SelectedHistoryItem.Text = ViewModel.SelectedWordHistoryModel.TranslationMode;
            ViewModel.UpdateDetailedInfo(ViewModel.SelectedWordHistoryModel);

            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            CommandBarFlyout1.ShowAt(fe, myOption);
        }



        private void LoveThatWordButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void QueryWordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteWordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WordListDropShadowPanel_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
             Storyboard storyboard = ((DropShadowPanel)sender).Resources["WordListDropShadowPanelPointerOverAnimation"] as Storyboard;
             storyboard.Begin();
        }

        private void WordListDropShadowPanel_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Storyboard storyboard = ((DropShadowPanel)sender).Resources["WordListDropShadowPanelPointerExitAnimation"] as Storyboard;
            storyboard.Begin();
        }

        private void WordListDropShadowPanel_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Storyboard storyboard = ((DropShadowPanel)sender).Resources["WordListDropShadowPanelPointerPressedAnimation"] as Storyboard;
            storyboard.Begin();
        }

        private void WordListDropShadowPanel_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Storyboard storyboard = ((DropShadowPanel)sender).Resources["WordListDropShadowPanelPointerReleasedAnimation"] as Storyboard;
            storyboard.Begin();
        }

        private void WordListDropShadowPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            foreach (var item in ViewModel.WordHistoryCollection)
            {
                if (item.ID == (int)(((DropShadowPanel)sender).Tag))
                {
                    ViewModel.SelectedWordHistoryModel = item;
                }
            }
            DictionaryService.QueryFromHistory(ViewModel.SelectedWordHistoryModel);
        }


        private void Menu_FavoriteItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }


        private void Menu_QueryItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
           
        }

        private void Menu_DeleteItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //ViewModel.DeleteWordItem(((WordViewModel)((ListViewItemPresenter)e.OriginalSource).DataContext));
        }

    }
}
