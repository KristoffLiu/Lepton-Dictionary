using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepton_Dictionary.Models
{
    public static class MainViewSearchBoxModel
    {
        //
        public static bool IsBoxSelectedAllAllowed
        {
            get { return LocalSetting.GetValueOrDefault("IsBoxSelectedAllAllowed", true); }
            set { LocalSetting.AddOrUpdateValue("IsBoxSelectedAllAllowed", value); }
        }

        public static bool IsIntelliListActivated
        {
            get { return LocalSetting.GetValueOrDefault("IsIntelliListActivated", true); }
            set { LocalSetting.AddOrUpdateValue("IsIntelliListActivated", value); }
        }
    }
}
