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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Lepton_Dictionary.Views
{
    public sealed partial class NoteBookControl : UserControl
    {
        public NoteBookControlViewModel ViewModel;
        public NoteBookControl(WordNoteBookViewModel viewmodel)
        {
            this.InitializeComponent();
            ViewModel = new NoteBookControlViewModel(viewmodel);            
            Tag = ViewModel.WordNoteBookViewModel.ID;
            DataContext = ViewModel;
        }
        private void MainBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", this);
            SearchSuggestionPage.Current.WordNotePageFrame.Navigate(typeof(NoteBookDetailPage),ViewModel);
        }
    }
}
