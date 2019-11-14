using Lepton_Dictionary.Models;
using Lepton_Dictionary.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using static Lepton_Dictionary.Models.DictionaryService;

namespace Lepton_Dictionary.ViewModels
{
    public class CompactOverlay_HomePageViewModel : PageViewModelBase
    {
        public static CompactOverlay_HomePageViewModel Current { get; set; }
        public CompactOverlay_HomePageViewModel()
        {
            Current = this;
            UpdatePlaceHolderText();
        }

        //bool _IsSuggestionListOpen = false;
        //public bool IsSuggestionListOpen
        //{
        //    get { return _IsSuggestionListOpen; }
        //    set { Set(ref _IsSuggestionListOpen, value); }
        //}

        string _PlaceHolderText = "输入词汇以查询";
        public string PlaceHolderText
        {
            get { return _PlaceHolderText; }
            set { Set(ref _PlaceHolderText, value); }
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

        public void CompactOverlayTextBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                CompactOverlay_QueryWord(CompactOverlay_HomePage.Current.AutoSuggestBox.Text);
            }
        }
        string _Word = "";
        string _sroucelanguage = "";
        string _targetlanguage = "";

        public string Word
        {
            get { return _Word; }
            set { Set(ref _Word, value); }
        }
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

        public void UpdateViewModel(string word, List<DictionaryModel> _dictionaryModels, string sourcelanguage, string targetlanguage)
        {
            Word = word;
            DictionaryModelCollection = _dictionaryModels;
            SourceLanguage = sourcelanguage;
            TargetLanguage = targetlanguage;
        }

        List<DictionaryModel> _DictionaryModelCollection = new List<DictionaryModel>() { new DictionaryModel() };
        public List<DictionaryModel> DictionaryModelCollection
        {
            get { return _DictionaryModelCollection; }
            set { Set(ref _DictionaryModelCollection, value); }
        }
        public async void CompactOverlay_QueryWord(string _queryword)
        {
            DictionaryService.QueryFromCompactOverlayView(_queryword);
            CompactOverlay_HomePage.Current.MenuButton_Click();
        }

        public void QueryWord(string _queryword)
        {
            DictionaryService.QueryFromAutoSuggestBox(_queryword);
        }

        public List<WordSuggestionViewModel> UpdateSuggestionList(string _word)
        {
            List<WordSuggestionViewModel> result = new List<WordSuggestionViewModel>();
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
            return result;
        }
        public async Task<List<WordSuggestionViewModel>> UpdateSuggestionListAsync(string _word)
        {
            var t = await Task.Run(() => UpdateSuggestionList(_word));
            return t;
        }

        ObservableCollection<WordSuggestionViewModel> _WordSuggestionCollection = new ObservableCollection<WordSuggestionViewModel>();
        public ObservableCollection<WordSuggestionViewModel> WordSuggestionCollection
        {
            get { return _WordSuggestionCollection; }
            set { Set(ref _WordSuggestionCollection, value); }
        }

        public void CompactOverlay_GetFocus()
        {

        }

        public void CompactOverlayAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
        }


        public void CompactOverlayAutoSuggestBox_AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var item = args.QueryText;
            CompactOverlay_QueryWord(item);
        }

        ObservableCollection<WordNoteBookViewModel> _WordNoteBookViewModelCollection = new ObservableCollection<WordNoteBookViewModel>();
        public ObservableCollection<WordNoteBookViewModel> WordNoteBookViewModelCollection
        {
            get { return _WordNoteBookViewModelCollection; }
            set { Set(ref _WordNoteBookViewModelCollection, value); }
        }

        public void GetWordNoteInfo()
        {
            WordNoteBookViewModelCollection.Clear();
            foreach (var item in WordNoteBookHelper.All())
            {
                var viewmodel = new WordNoteBookViewModel(item);
                //viewmodel.IsSaved = WordNoteHelper.IsWordSaved(Word);
                WordNoteBookViewModelCollection.Add(viewmodel);
            }
            foreach (var item in WordNoteBookViewModelCollection)
            {
                item.IsSaved = WordNoteHelper.IsWordSaved(Word, item.ID);
            }
        }
        public void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = e.ClickedItem as WordNoteBookViewModel;
            if (clickeditem.IsSaved)
            {
                DeleteFromBook(Word, clickeditem.ID);
            }
            else
            {
                AddToNoteBook(Word, clickeditem.ID);
            }
            clickeditem.IsSaved = !clickeditem.IsSaved;
        }

        public void AddToNoteBook(string _word, int _wordnotebookid)
        {
            //WordNoteHelper.Add(_word, _wordnotebookid);
            NoteBookControlViewModel.Update(_wordnotebookid);
        }

        public void DeleteFromBook(string _word, int _wordnotebookid)
        {
            WordNoteHelper.Delete(_word, _wordnotebookid);
            NoteBookControlViewModel.Update(_wordnotebookid);
        }
        public void SwitchBack()
        {
            AppViewModel.ShowMainView();
        }
    }
}
