using Lepton_Dictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepton_Dictionary.ViewModels
{
    public class UpdateInfoPageViewModel : PageViewModelBase
    {
        bool _IsNightMode = Models.Personalization.IsNightMode;
        public bool IsNightMode
        {
            get { return _IsNightMode; }
            set
            {
                Set(ref _IsNightMode, value);
                Models.Personalization.IsNightMode = value;
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
    }
}
