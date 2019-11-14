using Lepton_Dictionary.ViewModels;
using Lepton_Dictionary.Views;
using Lepton_Library.Helper;
using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Lepton_Dictionary.Models
{
    public class ClipboardSense
    {
        public static string ClipboardText { get; set; } = "";
        public static void Init()
        {

        }

        public static void ContentChanged()
        {
            Clipboard.ContentChanged += async (s, e) =>
            {
                DataPackageView dataPackageView = Clipboard.GetContent();
                if (dataPackageView.Contains(StandardDataFormats.Text))
                {
                    string text = await dataPackageView.GetTextAsync();
                    ClipboardText = text;
                }
                UpdateInternalNofinication();
            };
        }

        public async static void Update(int _index)
        {
            if (IsClipBoardSenseOn)
            {
                DataPackageView dataPackageView = Clipboard.GetContent();
                if (dataPackageView.Contains(StandardDataFormats.Text))
                {
                    string text = await dataPackageView.GetTextAsync();
                    if (ClipboardText != text)
                    {
                        if (LanguageHelper.IsValidWord(text))
                        {
                            ClipboardText = text;
                            if (_index == 0)
                            {
                                UpdateInternalNofinication();

                            }
                            else
                            {
                                CompactOverlay_UpdateInternalNofinication();
                            }
                        }
                    }
                }
            }
        }

        public static void UpdateInternalNofinication()
        {
            NotificationBar clipboardnotification = new NotificationBar();
            clipboardnotification.HeadlineText = "剪贴板感知";
            clipboardnotification.DescriptionText = "检测到您复制了 " + ClipboardText + " 。";
            clipboardnotification.Button1.Content = "查词";
            clipboardnotification.Button2.Content = "仅黏贴";
            clipboardnotification.Button3.Content = "取消";
            clipboardnotification.Button1.Click += (s, e) =>
            {
                AppViewModel.UpdateTextBoxAndQueryWord(ClipboardText);
                clipboardnotification.IsOpen = false;
            };
            clipboardnotification.Button2.Click += (s, e) =>
            {
                AppViewModel.UpdateTextBoxOnly(ClipboardText);
                clipboardnotification.IsOpen = false;
            };
            clipboardnotification.Button3.Click += (s, e) =>
            {
                clipboardnotification.IsOpen = false;
            };
                MainPage.Add_Notification(clipboardnotification);
        }

        public static void CompactOverlay_UpdateInternalNofinication()
        {
            CompactOverlay_NotificationBar clipboardnotification = new CompactOverlay_NotificationBar();
            clipboardnotification.HeadlineText = "剪贴板感知";
            clipboardnotification.DescriptionText = "检测到您复制了 " + ClipboardText + " 。";
            clipboardnotification.Button1.Content = "查词";
            clipboardnotification.Button2.Content = "仅黏贴";
            clipboardnotification.Button3.Content = "取消";
            clipboardnotification.Button1.Click += (s, e) =>
            {
                AppViewModel.UpdateTextBoxAndQueryWord(ClipboardText);
                clipboardnotification.IsOpen = false;
            };
            clipboardnotification.Button2.Click += (s, e) =>
            {
                AppViewModel.UpdateTextBoxOnly(ClipboardText);
                clipboardnotification.IsOpen = false;
            };
            clipboardnotification.Button3.Click += (s, e) =>
            {
                clipboardnotification.IsOpen = false;
            };
            CompactOverlay_HomePage.Add_Notification(clipboardnotification);
        }

        public static bool IsClipBoardSenseOn
        {
            get { return LocalSetting.GetValueOrDefault("IsClipBoardSenseOn", true); }
            set { LocalSetting.AddOrUpdateValue("IsClipBoardSenseOn", value); }
        }
    }
}
