using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;

namespace Lepton_Dictionary.Models
{
    public class AppService
    {
        public AppService()
        {
            ClipboardSense.Init();
            ExtensionManager = new ExtensionManager("Lepton.Dictionary.Extension");
            InitAsync();
        }
        public static ExtensionManager ExtensionManager { get; set; }
        public async void InitAsync()
        {
            await SQLiteManagerBase.InitDatabaseAsync();
            OfflineSource1SQLHelper.InitDatabase();
        }

        public void App_Init()
        {
            if (IsFirstInitialized)
            {
                InstalledTime = DateTime.Now;
                IsFirstInitialized = false;
            }
            else
            {
                
            }
        }

        public static bool IsFirstInitialized
        {
            get { return LocalSetting.GetValueOrDefault("IsFirstInitialized", true); }
            set { LocalSetting.AddOrUpdateValue("IsFirstInitialized", value); }
        }

        public static DateTime InstalledTime
        {
            get { return LocalSetting.GetValueOrDefault("InstalledTime", DateTime.Now); }
            set { LocalSetting.AddOrUpdateValue("InstalledTime", value); }
        }

        public static bool IsCompactView
        {
            get { return LocalSetting.GetValueOrDefault("IsCompactView", false); }
            set { LocalSetting.AddOrUpdateValue("IsCompactView", value); }
        }

        public static int HowLongInstalled()
        {
            return (Convert.ToInt32(DateTime.Now.Subtract(InstalledTime).TotalDays) + 1);
        }

        public static bool IsUpdated()
        {
            Package package = Package.Current;
            var _currentversion = package.Id.Version.Major.ToString() + "." + package.Id.Version.Minor.ToString() + "." + package.Id.Version.Build.ToString() + "." + package.Id.Version.Revision.ToString();
            if(CurrentVersion == _currentversion)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool SyncAppVersion()
        {
            Package package = Package.Current;
            var _currentversion = package.Id.Version.Major.ToString() + "." + package.Id.Version.Minor.ToString() + "." + package.Id.Version.Build.ToString() + "." + package.Id.Version.Revision.ToString();
            CurrentVersion = _currentversion;
            return true;
        }

        public static string CurrentVersion
        {
            get { return LocalSetting.GetValueOrDefault("IsFirstInitialized", "2.0.6"); }
            set { LocalSetting.AddOrUpdateValue("IsFirstInitialized", value); }
        }

        //public static void IndexPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        //{
        //    //分享一个链接
        //    string shareLinkString = “https://www.baidu.com”;

        //    //创建一个数据包
        //    DataPackage dataPackage = new DataPackage();

        //    //把要分享的链接放到数据包里
        //    dataPackage.SetWebLink(new Uri(shareLinkString));

        //    //数据包的标题（内容和标题必须提供）
        //    dataPackage.Properties.Title = "数据包的标题";
        //    //数据包的描述
        //    dataPackage.Properties.Description = "ONE for Windows10";

        //    //给dataRequest对象赋值
        //    DataRequest request = args.Request;
        //    request.Data = dataPackage;

        //}
    }
}
