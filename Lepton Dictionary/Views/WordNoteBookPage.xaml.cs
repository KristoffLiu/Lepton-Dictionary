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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Lepton_Dictionary.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WordNoteBookPage : Page
    {
        public static WordNoteBookPage Current;
        public WordNoteBookPageViewModel ViewModel;
        public WordNoteBookPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Current = this;
            this.InitializeComponent();
            DataContext = this;
            ViewModel = new WordNoteBookPageViewModel(WordNoteBookStackPanel);
            UpdateGradientColorSelector();
        }

        public async void UpdateGradientColorSelector()
        {
            GradientColorSelector.ItemsSource = await GradientColorResource.GetLinearGradientCollectionAsync();
        }

        public int SelectedID { get; set; }
        public bool IsBack { get; set; } = false;
        WordViewModel Selected { get; set; } = new WordViewModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(IsBack == true)
            {
                var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardConnectedAnimation");
                if (anim != null)
                {
                    foreach(var item in WordNoteBookStackPanel.Children)
                    {
                        if (SelectedID == ((int)((NoteBookControl)item).Tag))
                        {
                            anim.TryStart(item);
                        }
                    }
                }
                IsBack = false;
            }
        }

        private void DeleteButton_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void DeleteButton_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteAllWordItems();
        }


        bool IsOpen_WordNoteBook = false;

        public void Open_WordNoteBook()
        {
            DoubleAnimation doubleAnimation = WordHistoryPageOpen.Children[0] as DoubleAnimation;
            doubleAnimation.From = AddNewWordNoteBookGridTranslateTransform.Y;
            doubleAnimation.To = 0;
            WordHistoryPageOpen.Begin();
            IsOpen_WordNoteBook = true;
        }

        public void Close_WordNoteBook()
        {
            DoubleAnimation doubleAnimation = WordHistoryPageClose.Children[0] as DoubleAnimation;
            doubleAnimation.From = 0;
            doubleAnimation.To = height - 190;
            WordHistoryPageClose.Begin();
            IsOpen_WordNoteBook = false;
        }

        double height = 0;
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            height = e.NewSize.Height;
            if (IsOpen_WordNoteBook != true)
            {
                WordHistoryPageTranslateTransform.Y = (e.NewSize.Height - 190);
            }

            if (IsOpen_AddNewNoteBook != true)
            {
                AddNewWordNoteBookGridTranslateTransform.Y = (e.NewSize.Height - 190);
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsOpen_WordNoteBook == true)
            {
                Close_WordNoteBook();
            }
            else
            {
                Open_WordNoteBook();
            }
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


        bool IsOpen_AddNewNoteBook = false;

        public void Open_AddNewNoteBook()
        {
            DoubleAnimation doubleAnimation = AddNewWordNoteBookOpen.Children[0] as DoubleAnimation;
            doubleAnimation.From = AddNewWordNoteBookGridTranslateTransform.Y;
            doubleAnimation.To = 0;
            AddNewWordNoteBookOpen.Begin();
            IsOpen_AddNewNoteBook = true;
        }

        public void Close_AddNewNoteBook()
        {
            ViewModel.InputSubHeadline = "";
            ViewModel.InputHeadline = "";
            DoubleAnimation doubleAnimation = AddNewWordNoteBookClose.Children[0] as DoubleAnimation;
            doubleAnimation.From = 0;
            doubleAnimation.To = height - 190;
            AddNewWordNoteBookClose.Begin();
            IsOpen_AddNewNoteBook = false;
        }

        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (IsOpen_WordNoteBook)
            {
                if (IsOpen_AddNewNoteBook == true)
                {
                    Close_AddNewNoteBook();
                }
                else
                {
                    Open_AddNewNoteBook();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Add_ButtonClick();
            Close_AddNewNoteBook();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.PageHeadline = "单词卡";
            ViewModel.PageSubHeadline = "新建";
            Open_AddNewNoteBook();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if( ViewModel.WordListItemsCountNum > 0)
            {
                ViewModel.PageHeadline = "所有单词卡";
                ViewModel.PageSubHeadline = "翻阅";
            }
            Close_AddNewNoteBook();
        }

        private void GradientColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedLinearGradientColor = (LinearGradientColor)(GradientColorSelector.SelectedItem);
        }
    }
}
