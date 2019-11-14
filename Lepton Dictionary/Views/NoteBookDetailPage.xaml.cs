using Lepton_Dictionary.Models;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Lepton_Dictionary.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NoteBookDetailPage : Page
    {
        public NoteBookControlViewModel ViewModel;
        public NoteBookDetailPage()
        {
            DataContext = ViewModel;
            this.InitializeComponent();
            UpdateGradientColorSelector();
        }

        public async void UpdateGradientColorSelector()
        {
            GradientColorSelector.ItemsSource = await GradientColorResource.GetLinearGradientCollectionAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var _parameter = e.Parameter as NoteBookControlViewModel;
            _parameter.WordListItemsCountNum = _parameter.WordNoteViewModelCollection.Count;
            ViewModel = _parameter;
            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(CardViewGrid);
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void CollapsedButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            WordNoteBookPage.Current.SelectedID = ViewModel.ID;
            WordNoteBookPage.Current.IsBack = true;
            CancelMultiSelectionState();
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackwardConnectedAnimation", CardViewGrid);
            SearchSuggestionPage.Current.WordNotePageFrame.GoBack();
        }

        private void MultiSelectButton_Click(object sender, RoutedEventArgs e)
        {
            MultiSelectButton.IsEnabled = false;
            WordListView.IsMultiSelectCheckBoxEnabled = true;
            WordListView.SelectionMode = ListViewSelectionMode.Multiple;
            WordListView.IsItemClickEnabled = false;
            CommandBar.Visibility = Visibility.Visible;
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            WordListView.SelectAll();
        }
        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            WordListView.SelectedItem = null;
        }
        private void DeleteSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            var selecteditems = WordListView.SelectedItems.ToList();
            foreach (var item in selecteditems)
            {
                var _item = item as WordNoteViewModel;
                if (WordNoteHelper.Delete(_item.ID))
                {
                    ViewModel.WordNoteViewModelCollection.Remove(_item);
                }
            }
            CancelMultiSelectionState();
        }
        private void CancelDeleteOperation_Click(object sender, RoutedEventArgs e)
        {
            CancelMultiSelectionState();
        }

        public void CancelMultiSelectionState()
        {
            MultiSelectButton.IsEnabled = true;
            WordListView.IsMultiSelectCheckBoxEnabled = false;
            WordListView.SelectionMode = ListViewSelectionMode.None;
            WordListView.IsItemClickEnabled = true;
            CommandBar.Visibility = Visibility.Collapsed;
        }

        private async void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            GradientColorSelector.ItemsSource = await GradientColorResource.GetLinearGradientCollectionAsync();
            GradientColorSelector.SelectedIndex = await GradientColorResource.GetCurrentColorIndexAsync
                (ViewModel.WordNoteBookViewModel.GradientBackground1, ViewModel.WordNoteBookViewModel.GradientBackground2);
            ShowGradientColorSelectorMenu(false, (Button)sender);
            isopened = true;
        }

        bool isopened = false;

        private void ShowGradientColorSelectorMenu(bool isTransient, FrameworkElement fe)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            ColorCommandBarFlyout.ShowAt(fe, myOption);
        }

        private void GradientColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isopened)
            {
                var item = e.AddedItems.ToList()[0] as LinearGradientColor;
                ViewModel.Change_Color(item.GradientColor1, item.GradientColor2);
            }
        }

        private void ColorCommandBarFlyout_Closed(object sender, object e)
        {
            isopened = false;
        }

        private void DeleteThisNoteBookButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDeleteThisNoteBookMenu(false, (Button)sender);
        }

        private void ShowDeleteThisNoteBookMenu(bool isTransient, FrameworkElement fe)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            DeleteCommandBarFlyout.ShowAt(fe, myOption);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchSuggestionPage.Current.WordNotePageFrame.GoBack();
            WordNoteBookPageViewModel.Current.Delete(ViewModel.WordNoteBookViewModel);
        }

        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            RenameGrid.Visibility = Visibility.Visible;
            SetRenameGrid();
        }

        public void SetRenameGrid()
        {
            RenameSubHeadlineTextBox.Text = ViewModel.WordNoteBookViewModel.SubHeadline;
            RenameHeadlineTextBox.Text = ViewModel.WordNoteBookViewModel.Headline;
        }
        public void ClearRenameGrid()
        {
            RenameSubHeadlineTextBox.Text = "";
            RenameHeadlineTextBox.Text = "";
            RenameGrid.Visibility = Visibility.Collapsed;
        }

        private void RenameSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if(WordNoteBookHelper.Rename(ViewModel.ID,RenameSubHeadlineTextBox.Text, RenameHeadlineTextBox.Text))
            {
                ViewModel.WordNoteBookViewModel.SubHeadline = RenameSubHeadlineTextBox.Text;
                ViewModel.WordNoteBookViewModel.Headline = RenameHeadlineTextBox.Text;
                ClearRenameGrid();
            }
        }

        private void RenaemCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearRenameGrid();
        }
    }
}
