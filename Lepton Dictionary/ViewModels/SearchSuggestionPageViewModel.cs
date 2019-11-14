using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lepton_Dictionary.Models;
using Lepton_Dictionary.Views;
using Lepton_Library.Common;
using Lepton_Library.Helper;
using MaterialLibs.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Lepton_Dictionary.ViewModels
{
    public class SearchSuggestionPageViewModel : PageViewModelBase
    {
        public static SearchSuggestionPageViewModel Current { get; set; }
        public SearchSuggestionPageViewModel()
        {
            Current = this;
            GetAll_WordHistory();
        }

        WordHistoryModel _SelectedWordHistoryModel;
        public WordHistoryModel SelectedWordHistoryModel
        {
            get { return _SelectedWordHistoryModel; }
            set { Set(ref _SelectedWordHistoryModel, value); }
        }

        WordViewModel _DetailedWordInfo;
        public WordViewModel DetailedWordInfo
        {
            get { return _DetailedWordInfo; }
            set { Set(ref _DetailedWordInfo, value); }
        }
        int _languageindicatorindex = 0;
        public int LanguageIndicatorIndex
        {
            get { return _languageindicatorindex; }
            set { Set(ref _languageindicatorindex, value); }
        }


        public void LanguageIndicator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DictionaryService.AdjustDictionaryByIndicator(LanguageIndicatorIndex);
            DictionaryModelCollection = DictionaryService.AvailableDictionaries;
        }

        public void UpdateAvailableDictionaries()
        {
            DictionaryModelCollection = DictionaryService.AvailableDictionaries;
        }

        List<DictionaryModel> _DictionaryModelCollection = new List<DictionaryModel>();
        public List<DictionaryModel> DictionaryModelCollection
        {
            get { return _DictionaryModelCollection; }
            set { Set(ref _DictionaryModelCollection, value);
                if (DictionaryModelCollection.Count == 0)
                    DictionaryModelCollectionVisibility = Visibility.Visible;
                else
                    DictionaryModelCollectionVisibility = Visibility.Collapsed;
            }
        }

        Visibility _DictionaryModelCollectionVisibility = Visibility.Collapsed;
        public Visibility DictionaryModelCollectionVisibility
        {
            get { return _DictionaryModelCollectionVisibility; }
            set { Set(ref _DictionaryModelCollectionVisibility, value); }
        }

        public void UpdateDetailedInfo(WordHistoryModel wordHistoryViewModel)
        {
            //DictionaryService.Query(wordHistoryViewModel.Word,"","");
            //DetailedWordInfo = new WordViewModel(model.WordModel);
        }




        #region 历史记录
        ObservableCollection<WordHistoryModel> _WordHistoryCollection = new ObservableCollection<WordHistoryModel>();
        public ObservableCollection<WordHistoryModel> WordHistoryCollection
        {
            get { return _WordHistoryCollection; }
            set { Set(ref _WordHistoryCollection, value); }
        }

        public void GetAll_WordHistory()
        {
            WordHistoryCollection.Clear();
            foreach (var item in ObservableCollectionHelper.GetObservableCollectionFromList(WordHistoryHelper.QueryAll()))
            {
                WordHistoryCollection.Add(item);
            }
        }

        public void Add_WordHistory(WordHistoryModel model)
        {
            if (WordHistoryHelper.IsQueryHistoryAllowed & !IsDuplicated(model.Word))
            {
                int maxLength = (int)LeptonTextBlockControl.MaxLengthProperty.GetMetadata(typeof(LeptonTextBlockControl)).DefaultValue;
                if (model.Word.Length > maxLength)
                {
                    model.Word = model.Word.Substring(0, maxLength);
                }
                WordHistoryCollection.Insert(0, WordHistoryHelper.Add(model));
            }
        }

        //public void Add_WordHistory(string _word, string _translation, string _translationmode)
        //{
        //    if (WordHistoryHelper.IsQueryHistoryAllowed & !IsDuplicated(_word))
        //    {
        //        WordHistoryCollection.Add(WordHistoryHelper.Add(_word, _translation, _translationmode)));
        //    }
        //}

        public bool IsDuplicated(string _word)
        {
            bool result = false;
            foreach (var item in WordHistoryCollection)
            {
                if (item.Word == _word)
                {
                    result = true; 
                }
            }
            return result;
        }

        public void DeleteWordItem(int ID)
        {
            WordHistoryHelper.DeleteByID(ID);
            foreach (var item in WordHistoryCollection)
            {
                if(item.ID == ID)
                {
                    WordHistoryCollection.Remove(item);
                    return;
                }
            }
        }

        public void DeleteAllHistoryItem()
        {
            WordHistoryCollection.Clear();
        }

        #endregion

        public void DeleteButton_Click()
        {
            DeleteWordItem(SelectedWordHistoryModel.ID);
        }

        #region 历史记录相关控件
        int _WordHistoryListItemsCountNum = 0;
        public int WordHistoryListItemsCountNum
        {
            get { return _WordHistoryListItemsCountNum; }
            set
            {
                Set(ref _WordHistoryListItemsCountNum, value);
                if (value > 0)
                {
                    IsWordHistoryListExist = true;
                }
                else
                {
                    IsWordHistoryListExist = false;
                }
            }
        }

        bool _IsWordHistoryListExist;
        public bool IsWordHistoryListExist
        {
            get { return _IsWordHistoryListExist; }
            set
            {
                Set(ref _IsWordHistoryListExist, value);
                if (value == true)
                {
                    DeleteAllWordHistory_Button_Visibility = Visibility.Visible;
                    NoWordHistoryListItemNotificationVisibility = Visibility.Collapsed;
                }
                else
                {
                    DeleteAllWordHistory_Button_Visibility = Visibility.Collapsed;
                    NoWordHistoryListItemNotificationVisibility = Visibility.Visible;
                }
            }
        }

        Visibility _DeleteAllWordHistory_Button_Visibility;
        public Visibility DeleteAllWordHistory_Button_Visibility
        {
            get { return _DeleteAllWordHistory_Button_Visibility; }
            set
            {
                Set(ref _DeleteAllWordHistory_Button_Visibility, value);
            }
        }
        Visibility _NoWordHistoryListItemNotificationVisibility;
        public Visibility NoWordHistoryListItemNotificationVisibility
        {
            get { return _NoWordHistoryListItemNotificationVisibility; }
            set
            {
                Set(ref _NoWordHistoryListItemNotificationVisibility, value);
            }
        }
        #endregion

    }


    public class WordSuggestionViewModel : ViewModelBase
    {
        public WordSuggestionViewModel(WordSuggestionModel model)
        {
            ID = model.ID;
            Word = model.Word;
            POS = model.POS;
            Translation = model.Translation;
            TranslationMode = "汉英";
        }
        int _ID = 0;
        public int ID
        {
            get { return _ID; }
            set { Set(ref _ID, value); }
        }
        string _Word;
        public string Word
        {
            get { return _Word; }
            set { Set(ref _Word, value); }
        }

        string _translation;
        public string Translation
        {
            get { return _translation; }
            set { Set(ref _translation, value); }
        }

        string _POS;
        public string POS
        {
            get { return _POS; }
            set { Set(ref _POS, value); }
        }

        string _translationmode;
        public string TranslationMode
        {
            get { return _translationmode; }
            set { Set(ref _translationmode, value); }
        }
    }
    public class DictionaryTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DictionaryType result = (DictionaryType)value;
            switch (result)
            {
                case DictionaryType.BuiltIn:
                    return "内置";
                case DictionaryType.Package:
                    return "DLC";
                case DictionaryType.Extension:
                    return "扩展";
                default:
                    return "内置";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
