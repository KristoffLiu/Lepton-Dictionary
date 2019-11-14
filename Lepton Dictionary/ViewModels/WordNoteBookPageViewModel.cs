using Lepton_Dictionary.Models;
using Lepton_Dictionary.Views;
using Lepton_Library.Common;
using Lepton_Library.Helper;
using Lepton_Library.Storage;
using MaterialLibs.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Lepton_Dictionary.ViewModels
{
    public class WordNoteBookPageViewModel : PageViewModelBase
    {
        public static WordNoteBookPageViewModel Current;
        public WordNoteBookPageViewModel(StackPanel _stackpanel)
        {
            Current = this;
            WordNoteBookStackPanel = _stackpanel;
            ToggleSwitcherState = ToggleSwitcherState.Left;
            GetAll();
        }
        #region 

        string _PageHeadline;
        public string PageHeadline
        {
            get { return _PageHeadline; }
            set { Set(ref _PageHeadline, value); }
        }
        string _PageSubHeadline;
        public string PageSubHeadline
        {
            get { return _PageSubHeadline; }
            set { Set(ref _PageSubHeadline, value); }
        }

        string _InputHeadline;
        public string InputHeadline
        {
            get { return _InputHeadline; }
            set { Set(ref _InputHeadline, value); }
        }
        string _InputSubHeadline;
        public string InputSubHeadline
        {
            get { return _InputSubHeadline; }
            set { Set(ref _InputSubHeadline, value); }
        }

        int _BackgroundGradientIndex = -1;
        public int BackgroundGradientIndex
        {
            get { return _BackgroundGradientIndex; }
            set { Set(ref _BackgroundGradientIndex, value); }
        }

        public LinearGradientColor SelectedLinearGradientColor { get; set; }

        bool _IsSubHeadlineTextValid = false;
        public bool IsSubHeadlineTextValid
        {
            get { return _IsSubHeadlineTextValid; }
            set { Set(ref _IsSubHeadlineTextValid, value);
                CheckIsCreateButtonEnable();
            }
        }
        bool _IsHeadlineTextValid = false;
        public bool IsHeadlineTextValid
        {
            get { return _IsHeadlineTextValid; }
            set { Set(ref _IsHeadlineTextValid, value);
                CheckIsCreateButtonEnable();
            }
        }
        bool _IsCreateButtoneEnabled = false;
        public bool IsCreateButtoneEnabled
        {
            get { return _IsCreateButtoneEnabled; }
            set { Set(ref _IsCreateButtoneEnabled, value); }
        }
        public void CheckIsCreateButtonEnable()
        {
            if(IsSubHeadlineTextValid & IsHeadlineTextValid)
            {
                IsCreateButtoneEnabled = true;
            }
            else
            {
                IsCreateButtoneEnabled = false;
            }
        }

        ObservableCollection<WordNoteBookViewModel> _WordNoteBookViewModelCollection = new ObservableCollection<WordNoteBookViewModel>();
        public ObservableCollection<WordNoteBookViewModel> WordNoteBookViewModelCollection
        {
            get { return _WordNoteBookViewModelCollection; }
            set { Set(ref _WordNoteBookViewModelCollection, value); }
        }

        StackPanel _WordNoteBookStackPanel;
        public StackPanel WordNoteBookStackPanel
        {
            get { return _WordNoteBookStackPanel; }
            set { Set(ref _WordNoteBookStackPanel, value); }
        }

        #region 

        public void Count()
        {
            WordListItemsCountNum = WordNoteBookStackPanel.Children.Count;
        }

        public void GetAll()
        {
            WordNoteBookViewModelCollection.Clear();
            WordNoteBookStackPanel.Children.Clear();
            foreach (var item in WordNoteBookHelper.All())
            {
                WordNoteBookViewModelCollection.Add(new WordNoteBookViewModel(item));
            }
            foreach (var item in WordNoteBookViewModelCollection)
            {
                AddNewItemIntoTheGrid(item);
            }
            Count();
        }

        public void AddNewItemIntoTheGrid(WordNoteBookViewModel item)
        {
            WordNoteBookStackPanel.Children.Add(new NoteBookControl(item));
        }

        public void Add(string _headline, string _subheadline, string _gradientback1, string _gradientback2)
        {
            var item = new WordNoteBookViewModel(WordNoteBookHelper.Add(_headline, _subheadline, _gradientback1, _gradientback2));
            WordNoteBookViewModelCollection.Add(item);
            AddNewItemIntoTheGrid(item);
            Count();
        }

        public void Add_ButtonClick()
        {
            string _gradientback1 = "#FFFF9A9E";
            string _gradientback2 = "#FFFAD0C4";
            _gradientback1 = SelectedLinearGradientColor.GradientColor1;
            _gradientback2 = SelectedLinearGradientColor.GradientColor2;
            Add(InputHeadline, InputSubHeadline, _gradientback1, _gradientback2);
        }

        public void Delete(WordNoteBookViewModel viewmodelitem)
        {
            WordNoteBookHelper.DeleteByID(viewmodelitem.ID);
            WordNoteBookViewModelCollection.Remove(viewmodelitem);
            foreach(var item in WordNoteBookStackPanel.Children)
            {
                if ((int)(((NoteBookControl)item).Tag) == viewmodelitem.ID)
                {
                    WordNoteBookStackPanel.Children.Remove(item);
                }
            }
            Count();
        }

        #endregion

        ToggleSwitcherState _ToggleSwitcherState = MaterialLibs.Controls.ToggleSwitcherState.Left;
        public ToggleSwitcherState ToggleSwitcherState
        {
            get { return _ToggleSwitcherState; }
            set
            {
                Set(ref _ToggleSwitcherState, value);
                if (value == ToggleSwitcherState.Left)
                {

                }
                else
                {
                    WordNoteBookViewModelCollection.Clear();

                }
                WordListItemsCountNum = WordNoteBookViewModelCollection.Count;
            }
        }

        public void AddWordItem(WordNoteBookViewModel ItemModel)
        {
            //Model.DictionaryHistoryItemModels.Insert(0, Model.DictionaryHistoryItemModels[WordNoteBookViewModelCollection.IndexOf(ItemModel)]);
            WordNoteBookViewModelCollection.Insert(0, ItemModel);
            WordListItemsCountNum = WordNoteBookViewModelCollection.Count;
        }

        public void MoveWordItem(WordNoteBookViewModel ItemModel)
        {
            //Model.MoveHistoryItem(Model.DictionaryHistoryItemModels[WordNoteBookViewModelCollection.IndexOf(ItemModel)]);
            WordNoteBookViewModelCollection.Move(WordNoteBookViewModelCollection.IndexOf(ItemModel), 0);
        }


        public void DeleteWordItem(WordNoteBookViewModel WordNoteBookViewModel)
        {
            //删去ViewModel
            WordNoteBookViewModelCollection.Remove(WordNoteBookViewModel);
            //删去Model
            //Model.DeleteHistoryItem(WordNoteBookViewModel);

            WordListItemsCountNum = WordNoteBookViewModelCollection.Count;
        }

        public void DeleteAllWordItems()
        {
            //删去ViewModel
            WordNoteBookViewModelCollection.Clear();
            //删去Model
            //Model.DeleteAllHistory();

            WordListItemsCountNum = WordNoteBookViewModelCollection.Count;
        }

        public void GetAllWordItems()
        {

        }
        #endregion

        #region 
        int _WordListItemsCountNum = 0;
        public int WordListItemsCountNum
        {
            get { return _WordListItemsCountNum; }
            set
            {
                Set(ref _WordListItemsCountNum, value);
                if (value > 0)
                {
                    IsWordNoteBookExist = true;
                }
                else
                {
                    IsWordNoteBookExist = false;
                }
            }
        }

        bool _IsWordNoteBookExist;
        public bool IsWordNoteBookExist
        {
            get { return _IsWordNoteBookExist; }
            set
            {
                Set(ref _IsWordNoteBookExist, value);
                if (value == true)
                {
                    DeleteAllButtonVisibility = Visibility.Visible;
                    NoWordNoteBookNotificationVisibility = Visibility.Collapsed;
                    PageHeadline = "所有单词卡";
                    PageSubHeadline = "翻阅";
                }
                else
                {
                    DeleteAllButtonVisibility = Visibility.Collapsed;
                    NoWordNoteBookNotificationVisibility = Visibility.Visible;
                    PageHeadline = "单词卡";
                    PageSubHeadline = "新建";
                }
            }
        }

        Visibility _DeleteAllButtonVisibility;
        public Visibility DeleteAllButtonVisibility
        {
            get { return _DeleteAllButtonVisibility; }
            set
            {
                Set(ref _DeleteAllButtonVisibility, value);
            }
        }
        Visibility _NoWordNoteBookNotificationVisibility;
        public Visibility NoWordNoteBookNotificationVisibility
        {
            get { return _NoWordNoteBookNotificationVisibility; }
            set
            {
                Set(ref _NoWordNoteBookNotificationVisibility, value);
            }
        }
        #endregion
    }

    public class WordNoteBookViewModel : ViewModelBase
    {
        public WordNoteBookViewModel()
        {

        }
        public WordNoteBookViewModel(string _headline, string _GradientBackground1, string _GradientBackground2)
        {
            Headline = _headline;
            GradientBackground1 = _GradientBackground1;
            GradientBackground2 = _GradientBackground2;
        }

        public WordNoteBookViewModel(WordNoteBookModel item)
        {
            ID = item.ID;
            Headline = item.Headline;
            SubHeadline = item.SubHeadline;
            GradientBackground1 = item.GradientBackground1;
            GradientBackground2 = item.GradientBackground2;
        }

        int _id;
        public int ID
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }
        string _Headline;
        public string Headline
        {
            get { return _Headline; }
            set { Set(ref _Headline, value); }
        }
        string _SubHeadline;
        public string SubHeadline
        {
            get { return _SubHeadline; }
            set { Set(ref _SubHeadline, value); }
        }
        string _GradientBackground1;
        public string GradientBackground1
        {
            get { return _GradientBackground1; }
            set { Set(ref _GradientBackground1, value); }
        }
        string _GradientBackground2;
        public string GradientBackground2
        {
            get { return _GradientBackground2; }
            set { Set(ref _GradientBackground2, value); }
        }
        bool _IsSaved;
        public bool IsSaved
        {
            get { return _IsSaved; }
            set { Set(ref _IsSaved, value); }
        }

        public LinearGradientBrush LinearGradientBrush = new LinearGradientBrush();
        public void a()
        {
            
        }
    }

    public class GradientColorResource
    {
        public LinearGradientColor[] LinearGradientColors { get; set; }

        public async static Task<ObservableCollection<LinearGradientColor>> GetLinearGradientCollectionAsync()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(DateSource));
            string str;
            using (Stream _steam = await file.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(_steam))
                {
                    str = read.ReadToEnd();
                }
            }
            var result = Json.Deserialize<GradientColorResource>(str).LinearGradientColors;
            return ObservableCollectionHelper.GetObservableCollectionFromArray(result);
        }

        public async static Task<int> GetCurrentColorIndexAsync(string GradientColor1,string GradientColor2)
        {
            var resultitem = await GetLinearGradientCollectionAsync();
            int result = -1;
            foreach(var item in resultitem)
            {
                if(item.GradientColor1 == GradientColor1 & item.GradientColor2 == GradientColor2)
                {
                    result = item.id;
                }
            }
            return result;
        }

        const string DateSource = "ms-appx:///Assets/Data/GradientColorResources.json";
    }

    public class LinearGradientColor
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = "";
        public string GradientColor1 { get; set; } = "";
        public string GradientColor2 { get; set; } = "";
    }
}
