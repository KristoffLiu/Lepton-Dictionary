using Microsoft.Toolkit.Extensions;
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
    public sealed partial class LeptonTextBlockControl : UserControl
    {
        public LeptonTextBlockControl()
        {
            this.InitializeComponent();
        }

        public string Headline
        {
            get { return (string)GetValue(HeadlineProperty); }
            set { SetValue(HeadlineProperty, value); }
        }
        public static readonly DependencyProperty HeadlineProperty =
            DependencyProperty.Register("Headline", typeof(string), typeof(LeptonTextBlockControl), new PropertyMetadata("", OnHeadlineChanged));
        private static void OnHeadlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LeptonTextBlockControl), new PropertyMetadata("", OnTextChanged));
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(LeptonTextBlockControl), new PropertyMetadata("", OnPlaceholderTextChanged));
        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public string InvalidNoticeText
        {
            get { return (string)GetValue(InvalidNoticeTextProperty); }
            set { SetValue(InvalidNoticeTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InvalidNoticeTextProperty =
            DependencyProperty.Register("InvalidNoticeText", typeof(string), typeof(LeptonTextBlockControl), new PropertyMetadata("", OnInvalidNoticeTextChanged));
        private static void OnInvalidNoticeTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(LeptonTextBlockControl), new PropertyMetadata(Lepton_Library.Helper.LanguageHelper.ENGLISH_WORD_MAX_LENGTH, OnMaxLengthPropertyChanged));
        private static void OnMaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(LeptonTextBlockControl), new PropertyMetadata(false, OnIsValidChanged));
        private static void OnIsValidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool _IsValid = true;
            var _item = ((TextBox)sender).Text;
            if (string.IsNullOrEmpty(_item))
            {
                InvalidNoticeText = "不能为空文本";
                _IsValid = false;
            }
            else if(_item.Length >= MaxLength)
            {
                InvalidNoticeText = "不能超过" + MaxLength.ToString() + "个字符";
                ((TextBox)sender).Text = _item.Substring(0, MaxLength);
                _IsValid = false;
            }
            else
            {
                InvalidNoticeText = "";
                _IsValid = true;
            }
            IsValid = _IsValid;
        }
    }
}
