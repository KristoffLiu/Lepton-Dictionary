using Lepton_Dictionary.Models;
using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Lepton_Dictionary.ViewModels
{
    public class SettingPageViewModel : PageViewModelBase
    {
        public SettingPageViewModel()
        {
        }

        #region UI导航操作逻辑
        int _SettingPivotSelectedIndex = 0;
        public int SettingPivotSelectedIndex
        {
            get { return _SettingPivotSelectedIndex; }
            set { Set(ref _SettingPivotSelectedIndex, value); }
        }
        int _SettingMenuSelectedIndex = -1;
        public int SettingMenuSelectedIndex
        {
            get { return _SettingMenuSelectedIndex; }
            set { Set(ref _SettingMenuSelectedIndex, value);
                var selectedindex = SettingMenuSelectedIndex;
                Switch(selectedindex);
                AdjustUIByWidth(MainPage.Current.ViewModel.Width);
            }
        }

        Visibility _SettingMenuGridVisibility = Visibility.Visible;
        public Visibility SettingMenuGridVisibility
        {
            get { return _SettingMenuGridVisibility; }
            set { Set(ref _SettingMenuGridVisibility, value); }
        }
        Visibility _GoBackButtonVisibility = Visibility.Visible;
        public Visibility GoBackButtonVisibility
        {
            get { return _GoBackButtonVisibility; }
            set { Set(ref _GoBackButtonVisibility, value); }
        }
        


        public void ClickMenuItem()
        {

        }

        public void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustUIByWidth(e.NewSize.Width);
        }

        public void Switch(int selectedindex)
        {
            if (selectedindex>=0)
            {
                SettingPivotSelectedIndex = selectedindex;
            }
        }

        public void AdjustUIByWidth(double width)
        {
            if (width >= 750)
            {
                SettingMenuGridVisibility = Visibility.Visible;
                GoBackButtonVisibility = Visibility.Collapsed;
            }
            else
            {
                if (SettingMenuSelectedIndex == -1)
                {
                    SettingMenuGridVisibility = Visibility.Visible;
                }
                else
                {
                    GoBackButtonVisibility = Visibility.Visible;
                    SettingMenuGridVisibility = Visibility.Collapsed;
                }
            }
        }

        public void GoBack_ButtonClick()
        {
            SettingMenuSelectedIndex = -1;
            GoBackButtonVisibility = Visibility.Collapsed;
        }
        #endregion



        #region General 通用

        /// <summary>
        /// 网络
        /// </summary>
        bool _IsAvailableUnderMeteredInternet = DictionaryService.IsAvailableUnderMeteredInternet;
        public bool IsOnlineDictionaryQueryAvailableUnderMeteredInternet
        {
            get { return _IsAvailableUnderMeteredInternet; }
            set
            {
                Set(ref _IsAvailableUnderMeteredInternet, value);
                DictionaryService.IsAvailableUnderMeteredInternet = value;
            }
        }

        bool _IsDailySentenceAvailableUnderMeteredInternet = DailySentenceService.IsDailySentenceAvailableUnderMeteredInternet;
        public bool IsDailySentenceAvailableUnderMeteredInternet
        {
            get { return _IsDailySentenceAvailableUnderMeteredInternet; }
            set
            {
                Set(ref _IsDailySentenceAvailableUnderMeteredInternet, value);
                DailySentenceService.IsDailySentenceAvailableUnderMeteredInternet = value;
            }
        }


        bool _IsClipBoardSenseOn = ClipboardSense.IsClipBoardSenseOn;
        public bool IsClipBoardSenseOn
        {
            get { return _IsClipBoardSenseOn; }
            set
            {
                Set(ref _IsClipBoardSenseOn, value);
                ClipboardSense.IsClipBoardSenseOn = value;
            }
        }

        public void DeleteAllHistory()
        {
            WordHistoryHelper.DeleteAll();
            SearchSuggestionPageViewModel.Current.DeleteAllHistoryItem();
        }
        #region 快捷键导航
        #endregion

        #endregion

        #region 数据隐私通知


        bool _IsOnlineQueriedEntryAutoSaved = DictionaryService.IsOnlineQueriedEntryAutoSaved;
        public bool IsOnlineQueriedEntryAutoSaved //离线同步已查询词条
        {
            get { return _IsOnlineQueriedEntryAutoSaved; }
            set
            {
                Set(ref _IsOnlineQueriedEntryAutoSaved, value);
                DictionaryService.IsOnlineQueriedEntryAutoSaved = value;
            }
        }

        bool _IsQueryHistoryAllowed = WordHistoryHelper.IsQueryHistoryAllowed;
        public bool IsQueryHistoryAllowed
        {
            get { return _IsQueryHistoryAllowed; }
            set
            {
                Set(ref _IsQueryHistoryAllowed, value);
                WordHistoryHelper.IsQueryHistoryAllowed = value;
            }
        }

        bool _IsOfflineSavingDictionaryUsedPreferentially = DictionaryService.IsOfflineSavingDictionaryUsedPreferentially;
        public bool IsOfflineSavingDictionaryUsedPreferentially
        {
            get { return _IsOfflineSavingDictionaryUsedPreferentially; }
            set
            {
                Set(ref _IsOfflineSavingDictionaryUsedPreferentially, value);
                DictionaryService.IsOfflineSavingDictionaryUsedPreferentially = value;
            }
        }


        #endregion

    }

    public class AcrylicOpacityToSliderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)value * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (double)value / 100;
        }
    }
}
