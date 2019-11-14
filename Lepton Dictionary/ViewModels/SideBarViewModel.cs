using Lepton_Dictionary.Models;
using Lepton_Dictionary.Views;
using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Lepton_Dictionary.ViewModels
{
    public class SideBarViewModel : PageViewModelBase
    {
        public SideBarViewModel()
        {
            Init();
            IsNightMode = Models.Personalization.IsNightMode;
        }

        public void Init()
        {
            TimeText = AppService.HowLongInstalled().ToString();
            WordCount = WordStorageHelper.Count().ToString();
            NoteBookCount = WordNoteBookHelper.Count().ToString();
        }

        string _TimeText;
        public string TimeText
        {
            get { return _TimeText; }
            set { Set(ref _TimeText, value); }
        }

        string _WordCount;
        public string WordCount
        {
            get { return _WordCount; }
            set { Set(ref _WordCount, value); }
        }
        string _NoteBookCount;
        public string NoteBookCount
        {
            get { return _NoteBookCount; }
            set { Set(ref _NoteBookCount, value); }
        }

        bool _IsNightMode = false;
        public bool IsNightMode
        {
            get { return _IsNightMode; }
            set { Set(ref _IsNightMode, value);
                if (value)
                {

                    NightModeFontIconGlyph = "\uE793";
                    NightModeTextBlockText = "日间模式";
                }
                else
                {
                    NightModeFontIconGlyph = "\uE708";
                    NightModeTextBlockText = "夜间模式";
                }
                Models.Personalization.IsNightMode = value;
            }
        }

        string _NightModeFontIconGlyph = "\uE708";
        public string NightModeFontIconGlyph
        {
            get { return _NightModeFontIconGlyph; }
            set { Set(ref _NightModeFontIconGlyph, value); }
        }

        string _NightModeTextBlockText = "夜间模式";
        public string NightModeTextBlockText
        {
            get { return _NightModeTextBlockText; }
            set { Set(ref _NightModeTextBlockText, value); }
        }
    }
}
