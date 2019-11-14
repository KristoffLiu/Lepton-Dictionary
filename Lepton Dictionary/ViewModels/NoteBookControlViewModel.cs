using Lepton_Dictionary.Models;
using Lepton_Dictionary.Views;
using Lepton_Library.Common;
using Lepton_Library.Helper;
using MaterialLibs.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Lepton_Dictionary.ViewModels
{
    public class NoteBookControlViewModel : ViewModelBase
    {
        public static ObservableCollection<NoteBookControlViewModel> All = new ObservableCollection<NoteBookControlViewModel>();
        public NoteBookControlViewModel(WordNoteBookViewModel _wordNoteBookViewModel)
        {
            WordNoteBookViewModel = _wordNoteBookViewModel;
            ID = _wordNoteBookViewModel.ID;
            All.Add(this);
            GetAll();
        }
        #region 

        int _id;
        public int ID
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        WordNoteBookViewModel _WordNoteBookViewModel;
        public WordNoteBookViewModel WordNoteBookViewModel
        {
            get { return _WordNoteBookViewModel; }
            set { Set(ref _WordNoteBookViewModel, value); }
        }

        ObservableCollection<WordNoteViewModel> _WordNoteViewModelCollection = new ObservableCollection<WordNoteViewModel>();
        public ObservableCollection<WordNoteViewModel> WordNoteViewModelCollection
        {
            get { return _WordNoteViewModelCollection; }
            set { Set(ref _WordNoteViewModelCollection, value); }
        }
        #region 


        public void GetAll()
        {
            WordNoteViewModelCollection.Clear();
            foreach (var item in WordNoteHelper.QueryByNoteBookID(ID))
            {
                WordNoteViewModelCollection.Add(new WordNoteViewModel(item));
            }
            WordListItemsCountNum = WordNoteViewModelCollection.Count;
        }        

        public void Delete(WordNoteViewModel viewmodelitem)
        {
            WordNoteHelper.Delete(viewmodelitem.ID);
            WordNoteViewModelCollection.Remove(viewmodelitem);
        }

        public void DeleteNoteBook(WordNoteBookViewModel viewmodelitem)
        {
            WordNoteBookHelper.DeleteByID(viewmodelitem.ID);
        }

        public void RenameNoteBook(WordNoteViewModel viewmodelitem)
        {
            WordNoteBookHelper.DeleteByID(viewmodelitem.ID);
            WordNoteViewModelCollection.Remove(viewmodelitem);
        }

        #endregion

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
                    IsSearchButtonEnabled = true;
                    IsMultiSelectionButtonEnabled = true;
                }
                else
                {
                    DeleteAllButtonVisibility = Visibility.Collapsed;
                    NoWordNoteBookNotificationVisibility = Visibility.Visible;
                    IsSearchButtonEnabled = false;
                    IsMultiSelectionButtonEnabled = false;
                }
            }
        }

        bool _IsSearchButtonEnabled;
        public bool IsSearchButtonEnabled
        {
            get { return _IsSearchButtonEnabled; }
            set
            {
                Set(ref _IsSearchButtonEnabled, value);
            }
        }

        bool _IsMultiSelectionButtonEnabled;
        public bool IsMultiSelectionButtonEnabled
        {
            get { return _IsMultiSelectionButtonEnabled; }
            set
            {
                Set(ref _IsMultiSelectionButtonEnabled, value);
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

        public static void UpdateAll()
        {
            foreach(var item in All)
            {
                item.GetAll();
            }
        }

        public static void Update(int _notebookid)
        {
            foreach (var item in All)
            {
                if (item.ID == _notebookid)
                {
                    item.GetAll();
                }
            }
        }

        public void DeleteFromBook(string _word, int _wordnotebookid)
        {
            WordNoteHelper.Delete(_word, _wordnotebookid);
            Update(_wordnotebookid);
        }

        public void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as WordNoteViewModel;
            HomePageViewModel.Current.QueryWord(item.Word);
        }
        public void Change_Color(string gradient1,string gradient2)
        {
            if(WordNoteBookHelper.UpdateColor(ID, gradient1, gradient2))
            {
                WordNoteBookViewModel.GradientBackground1 = gradient1;
                WordNoteBookViewModel.GradientBackground2 = gradient2;
            }
        }

        bool _IsSubHeadlineTextValid = false;
        public bool IsSubHeadlineTextValid
        {
            get { return _IsSubHeadlineTextValid; }
            set
            {
                Set(ref _IsSubHeadlineTextValid, value);
                CheckIsCreateButtonEnable();
            }
        }
        bool _IsHeadlineTextValid = false;
        public bool IsHeadlineTextValid
        {
            get { return _IsHeadlineTextValid; }
            set
            {
                Set(ref _IsHeadlineTextValid, value);
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
            if (IsSubHeadlineTextValid & IsHeadlineTextValid)
            {
                IsCreateButtoneEnabled = true;
            }
            else
            {
                IsCreateButtoneEnabled = false;
            }
        }

    }

    public class WordNoteViewModel : ViewModelBase
    {
        public WordNoteViewModel(WordNoteModel model)
        {
            This = this;
            ID = model.ID;
            Word = model.Word;
            WordNoteBookID = model.WordNoteBookID;
            SourceLanguage = model.SourceLanguage;
            TargetLanguage = model.TargetLanguage;
            LanguageDisplayText = DictionaryService.ShowLanguageDisplayText(SourceLanguage, TargetLanguage);
            //Word_ID = model.WordID;
            GetWordInfo();
        }

        public void GetWordInfo()
        {
            //WordViewModel = new WordViewModel(WordStorageHelper.QueryWord(Word, 0));
        }

        int _id;
        public int ID
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        int _WordNoteBookID;
        public int WordNoteBookID
        {
            get { return _WordNoteBookID; }
            set { Set(ref _WordNoteBookID, value); }
        }

        string _Word;
        public string Word
        {
            get { return _Word; }
            set { Set(ref _Word, value); }
        }



        string _sroucelanguage = "";
        string _targetlanguage = "";

        public string SourceLanguage
        {
            get { return _sroucelanguage; }
            set { Set(ref _sroucelanguage, value); }
        }
        public string TargetLanguage
        {
            get { return _targetlanguage; }
            set { Set(ref _targetlanguage, value); }
        }

        string _LanguageDisplayText;
        public string LanguageDisplayText
        {
            get { return _LanguageDisplayText; }
            set { Set(ref _LanguageDisplayText, value); }
        }

        WordNoteViewModel _This;
        public WordNoteViewModel This
        {
            get { return _This; }
            set { Set(ref _This, value); }
        }

        WordViewModel _WordViewModel;
        public WordViewModel WordViewModel
        {
            get { return _WordViewModel; }
            set { Set(ref _WordViewModel, value); }
        }
    }

    public class NoteBookItemsNumTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var _int = (int)value;
            var str = _int > 0 ? "单词数量：" + _int.ToString() + "个" : "没有单词";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
