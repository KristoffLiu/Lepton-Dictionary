using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Lepton_Dictionary.ViewModels
{
    public class MainPageViewModel : PageViewModelBase
    {
        private bool _IsSwitchAppSplitViewOpen = false;
        private Visibility _TextBlock_AppTileBarTitleVisibility = Visibility.Visible;
        private string _SwitchButtonGlyph = "\uEC12";

        private Visibility _SettingPageVisibility = Visibility.Collapsed;


        //public bool IsSwitchAppSplitViewOpen
        //{
        //    get { return _IsSwitchAppSplitViewOpen; }
        //    set { Set(ref _IsSwitchAppSplitViewOpen, value);
        //        if (value)
        //        {
        //            TextBlock_AppTileBarTitleVisibility = Visibility.Collapsed;
        //            SwitchButtonGlyph = "\uEC11";
        //        }
        //        else
        //        {
        //            TextBlock_AppTileBarTitleVisibility = Visibility.Visible;
        //            SwitchButtonGlyph = "\uEC12";

        //        }
        //    }
        //}
        public Visibility TextBlock_AppTileBarTitleVisibility
        {
            get { return _TextBlock_AppTileBarTitleVisibility; }
            set { Set(ref _TextBlock_AppTileBarTitleVisibility, value);
            }
        }
        public string SwitchButtonGlyph
        {
            get { return _SwitchButtonGlyph; }
            set
            {
                Set(ref _SwitchButtonGlyph, value);
            }
        }
        public Visibility SettingPageVisibility
        {
            get { return _SettingPageVisibility; }
            set
            {
                Set(ref _SettingPageVisibility, value);
            }
        }


        public double Width { get; set; }
        public double Height { get; set; }

        public void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = e.NewSize.Width;
            Height = e.NewSize.Height;
            if ( e.PreviousSize.Width >= 750 & Width < 750)
            {
                HomePageViewModel.Current.AdaptingView(Width);
            }
        }
        //public void SwitchAppSplitView()
        //{
        //    IsSwitchAppSplitViewOpen = !IsSwitchAppSplitViewOpen;
        //}

        //public void CloseSideBar()
        //{
        //    IsSwitchAppSplitViewOpen = false;
        //}
    }
}
