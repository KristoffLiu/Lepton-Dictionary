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
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Lepton_Dictionary.Views
{
    public sealed partial class DefinitionControl : UserControl
    {
        public DefinitionControl()
        {
            this.InitializeComponent();
        }

        public WordViewModel ViewModel
        {
            get { return (WordViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(WordViewModel), typeof(NotificationBar), new PropertyMetadata(null, OnViewModelPropertyChanged));
        private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var newvalue = (bool)(e.NewValue);
        }


    }
}
