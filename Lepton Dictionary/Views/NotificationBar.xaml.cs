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
    public sealed partial class NotificationBar : UserControl
    {
        DispatcherTimer timer;//定义定时器

        public NotificationBar()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新指针
            IsOpen = true;
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value);
                if (value)
                    NotificationBarOpen.Begin();
                else
                    NotificationBarClose.Begin();
            }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(NotificationBar), new PropertyMetadata(false, OnIsOpenChanged));
        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var newvalue = (bool)(e.NewValue);
        }

        private void NotificationBarOpen_Completed(object sender, object e)
        {
            timer.Start();
        }
        private void Timer_Tick(object sender, object e)
        {
            timer.Stop();
            IsOpen = false;
        }

        private void NotificationBarClose_Completed(object sender, object e)
        {
            MainPage.Remove_Notification(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public string HeadlineText
        {
            get { return (string)GetValue(HeadlineTextProperty); }
            set { SetValue(HeadlineTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadlineTextProperty =
            DependencyProperty.Register("IsOpen", typeof(string), typeof(NotificationBar), new PropertyMetadata("0", OnHeadlineTextChanged));
        private static void OnHeadlineTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var newvalue = (bool)(e.NewValue);
        }

        public string DescriptionText
        {
            get { return (string)GetValue(DescriptionTextProperty); }
            set { SetValue(DescriptionTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionTextProperty =
            DependencyProperty.Register("IsOpen", typeof(string), typeof(NotificationBar), new PropertyMetadata("0", OnDescriptionTextPropertyChanged));
        private static void OnDescriptionTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var newvalue = (bool)(e.NewValue);
        }

        private void MessageBar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            timer.Stop();
        }

        private void MessageBar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            timer.Start();
        }

        private void MessageBar_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            timer.Start();
        }
        private void MessageBar_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
