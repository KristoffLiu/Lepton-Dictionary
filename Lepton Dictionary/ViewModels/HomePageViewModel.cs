using Lepton_Library.Common;
using System;
using System.Linq;
using MaterialLibs.Controls;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.System;
using Lepton_Dictionary.Views;
using Lepton_Dictionary.Models;
using Lepton.Views.DictionaryPage;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using Lepton_Library.Helper;
using Lepton_Library.Native;

namespace Lepton_Dictionary.ViewModels
{
    public class HomePageViewModel : PageViewModelBase
    {
        public static HomePageViewModel Current { get; set; }
        public HomePageViewModel()
        {
            Current = this;
            UpdatePlaceHolderText();
        }
        //public DictionaryHistoryModel Model = new DictionaryHistoryModel();

        string _Background_SearchPanel_Narrow = "#FFFFFFFF";
        public string Background_SearchPanel_Narrow
        {
            get { return _Background_SearchPanel_Narrow; }
            set { Set(ref _Background_SearchPanel_Narrow, value); }
        }

        string _PlaceHolderText = "输入词汇以查询";
        public string PlaceHolderText
        {
            get { return _PlaceHolderText; }
            set { Set(ref _PlaceHolderText, value); }
        }

        private bool EnableKeyNavigate
        {
            get
            {
                IntPtr hwnd = NativeHelper.CurrentHandle;

                IMEStatus status = IMEOperator.GetCurrentStatus(hwnd);

                if (status == IMEStatus.CN || status == IMEStatus.Other)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void UpdatePlaceHolderText()
        {
            string[] placeholdertextlist = {
                "输入词汇以查询（＝。＝）",
                "你看这个输入框它又大又宽♪(^∇^*)",
                "就像那个词儿它又长又难o(￣ヘ￣o＃)",
                "书山有路勤为径(๑•̀ㅂ•́)و✧",
            };
            Random ran = new Random();
            int RandKey = ran.Next(0, 3);
            PlaceHolderText = placeholdertextlist[RandKey];
        }

        SearchUIState _SearchUIState = SearchUIState.Collapsed;
        public SearchUIState SearchUIState
        {
            get { return _SearchUIState; }
            set
            {
                Set(ref _SearchUIState, value);
                switch (value)
                {
                    case SearchUIState.Collapsed:
                        HomePage.Current.SearchBoxClose.Begin();
                        Background_SearchPanel_Narrow = "#FFFFFFFF";
                        Padding_QueryTextBox = new Thickness(15, 0, 0, 1);
                        //HomePage.Current.SearchPanel.Padding = new Thickness(0, 0, 0, 0);
                        BackButtonVisibility = Visibility.Collapsed;
                        DefinitionPanelVisibility = Visibility.Collapsed;
                        HomePage.Current.Close();
                        break;
                    case SearchUIState.Activated:
                        //if (EnableKeyNavigate)
                        //{
                            HomePage.Current.SearchBoxOpen.Begin();
                            //MainPage.Current.ViewModel.Width >= 750
                            Padding_QueryTextBox = new Thickness(45, 0, 0, 1);
                            BackButtonVisibility = Visibility.Visible;
                            DefinitionPanelVisibility = Visibility.Collapsed;
                            //DefinationPage.Current.CloseMainPanel();
                            HomePage.Current.Open();
                        //}
                        break;
                    case SearchUIState.Searched:
                        //if (EnableKeyNavigate)
                        //{
                            //DefinationPage.Current.OpenMainPanel();
                            HomePage.Current.SearchBoxOpen.Begin();
                            Padding_QueryTextBox = new Thickness(45, 0, 0, 1);
                            BackButtonVisibility = Visibility.Visible;
                            DefinitionPanelVisibility = Visibility.Visible;
                            HomePage.Current.Open();
                        //}
                        break;
                    default:
                        break;
                }
            }
        }

        public void AdaptingView(double Width)
        {
            switch (SearchUIState)
            {
                case SearchUIState.Collapsed:
                    Background_SearchPanel_Narrow = "#FFFFFFFF";
                    Padding_QueryTextBox = new Thickness(15, 0, 0, 0);
                    BackButtonVisibility = Visibility.Collapsed;
                    DefinitionPanelVisibility = Visibility.Collapsed;
                    break;
                case SearchUIState.Activated:
                    if (Width >= 750)
                    {
                        DefinitionPanelVisibility = Visibility.Visible;
                    }
                    else
                    {
                        Background_SearchPanel_Narrow = "#FF330867";
                        DefinitionPanelVisibility = Visibility.Collapsed;
                    }
                    Padding_QueryTextBox = new Thickness(45, 0, 0, 0);
                    BackButtonVisibility = Visibility.Visible;
                    break;
                case SearchUIState.Searched:
                    if (Width >= 750)
                    {

                    }
                    else
                    {
                    }
                    
                    Padding_QueryTextBox = new Thickness(45, 0, 0, 0);
                    BackButtonVisibility = Visibility.Visible;
                    DefinitionPanelVisibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        Thickness _Padding_QueryTextBox = new Thickness(15,0,0,0);
        public Thickness Padding_QueryTextBox
        {
            get { return _Padding_QueryTextBox; }
            set { Set(ref _Padding_QueryTextBox, value); }
        }

        Visibility _DefinitionPanelVisibility = Visibility.Collapsed;
        public Visibility DefinitionPanelVisibility
        {
            get { return _DefinitionPanelVisibility; }
            set { Set(ref _DefinitionPanelVisibility, value); }
        }

        Visibility _BackButtonVisibility = Visibility.Collapsed;
        public Visibility BackButtonVisibility
        {
            get { return _BackButtonVisibility; }
            set { Set(ref _BackButtonVisibility, value); }
        }

        public override ElementTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value);
                if (value == ElementTheme.Light)
                {
                    BackgroundImage = new BitmapImage() { UriSource = new Uri( Models.Personalization.BackgroundImageFolderPath() +"Light.png" ) };
                }
                else
                {
                    BackgroundImage = new BitmapImage() { UriSource = new Uri( Models.Personalization.BackgroundImageFolderPath() + "Dark.png") };
                }
            }
        }

        public void ChangeBackgroundImage()
        {
            if (Theme == ElementTheme.Light)
            {
                BackgroundImage = new BitmapImage() { UriSource = new Uri( Models.Personalization.BackgroundImageFolderPath() + "Light.png") };
            }
            else
            {
                BackgroundImage = new BitmapImage() { UriSource = new Uri( Models.Personalization.BackgroundImageFolderPath() + "Dark.png") };
            }
        }

        BitmapImage _BackgroundImage = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Art/Girl_June.png") };
        public BitmapImage BackgroundImage
        {
            get { return _BackgroundImage; }
            set { Set(ref _BackgroundImage, value); }
        }



        public void QueryWord(string _queryword)
        {
            DictionaryService.QueryFromAutoSuggestBox(_queryword);
            
        }

        public string IndicateLanuageTarget(string source, int index)
        {
            if(source == "cn")
            {
                switch (index)
                {
                    case 0://自动
                        return "en";
                    case 1://英汉
                        return "en";
                    case 2://汉语
                        return "cn";
                    case 3://日汉
                        return "jp";
                    case 4://韩汉
                        return "ko";
                    case 5://法汉
                        return "fr";
                    case 6://德汉
                        return "ge";
                    case 7://葡汉
                        return "po";
                    default://自动
                        return "en";
                }
            }
            else
            {
                return "cn";
            }
        }

        #region

        public List<WordSuggestionViewModel> UpdateSuggestionList(string _word)
        {
            List<WordSuggestionViewModel> result = new List<WordSuggestionViewModel>();
            if (EnableKeyNavigate)
            {
                var resultlist = OfflineSource1Helper.GetWordSuggestionModel(_word);
                if (resultlist != null)
                {
                    foreach (var item in resultlist)
                    {
                        result.Add(new WordSuggestionViewModel(item));
                    }
                }
                else
                {

                }
            }
            return result;
        }
        public async Task<List<WordSuggestionViewModel>> UpdateSuggestionListAsync(string _word)
        {
            var t = await Task.Run(() => UpdateSuggestionList(_word));
            return t;
        }

        public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
        }

        public void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var item = args.QueryText;
            QueryWord(item);
        }

        #endregion


        public void GetFocus()
        {
            if (SearchUIState == SearchUIState.Collapsed)
            {
                SearchUIState = SearchUIState.Activated;
            }
            else
            {

            }
        }

        public void GoBack()
        {
            SearchUIState = SearchUIState.Collapsed;
        }


    }


    public enum SearchUIState
    {
        Collapsed = 0,
        Activated = 1,
        Searched = 2
    }
}

