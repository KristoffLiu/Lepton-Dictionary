using Lepton_Dictionary.Models;
using Lepton_Library.Common;
using Lepton_Library.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static Lepton_Dictionary.Models.DictionaryService;

namespace Lepton_Dictionary.ViewModels
{
    public class DefinationPageViewModel : PageViewModelBase
    {
        public DefinationPageViewModel()
        {
            Current = this;
        }

        public static DefinationPageViewModel Current
        {
            get; set;
        }

        string _Word = "";
        string _sroucelanguage = "";
        string _targetlanguage = "";

        WordViewModel _AiCiBaWordViewModel = new WordViewModel();
        public WordViewModel AiCiBaWordViewModel
        {
            get { return _AiCiBaWordViewModel; }
            set { Set(ref _AiCiBaWordViewModel, value); }
        }

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


        public void FavoriteItem(string word)
        {
            Word = word;
            //DictionaryPageViewModel.CurrentDictionaryPageViewModel.ModifyFavoriteViewModelItem(CurrentViewModel);
        }

        public void UpdateViewModel(string word,List<DictionaryModel> _dictionaryModels, string sourcelanguage, string targetlanguage)
        {
            Word = word;
            if(_dictionaryModels.Count > 0)
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

        #region 必应
        string _AmE = "";
        public string AmE
        {
            get { return _AmE; }
            set { Set(ref _AmE, value); }
        }

        string _AmEmp3 = "";
        public string AmEmp3
        {
            get { return _AmEmp3; }
            set { Set(ref _AmEmp3, value); }
        }

        string _BrE = "";
        public string BrE
        {
            get { return _BrE; }
            set { Set(ref _BrE, value); }
        }

        string _BrEmp3 = "";
        public string BrEmp3
        {
            get { return _BrEmp3; }
            set { Set(ref _BrEmp3, value); }
        }

        ObservableCollection<DefsViewModel> _defs = new ObservableCollection<DefsViewModel>();
        public ObservableCollection<DefsViewModel> defs
        {
            get { return _defs; }
            set { Set(ref _defs, value); }
        }
        ObservableCollection<SamsViewModel> _sams = new ObservableCollection<SamsViewModel>();
        public ObservableCollection<SamsViewModel> sams
        {
            get { return _sams; }
            set { Set(ref _sams, value); }
        }
        #endregion


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
            foreach(var item in WordNoteBookViewModelCollection)
            {
                item.IsSaved = WordNoteHelper.IsWordSaved( Word, item.ID);
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
                AddToNoteBook(Word,clickeditem.ID,SourceLanguage,TargetLanguage);
            }
            clickeditem.IsSaved = !clickeditem.IsSaved;
        }

        public void AddToNoteBook(string _word, int _wordnotebookid,string sourcelanguage,string targetlanguage)
        {
            WordNoteHelper.Add(_word,_wordnotebookid, sourcelanguage, targetlanguage);
            NoteBookControlViewModel.Update(_wordnotebookid);
        }

        public void DeleteFromBook(string _word, int _wordnotebookid)
        {
            WordNoteHelper.Delete(_word, _wordnotebookid);
            NoteBookControlViewModel.Update(_wordnotebookid);
        }
    }

    public class AiCiBaViewModel : ViewModelBase
    {
        public void Update(AiCiBaE2C_Model model)
        {
            Clear();
            ph_am = model.symbols[0].ph_am;
            ph_am_mp3 = model.symbols[0].ph_am_mp3;
            ph_en = model.symbols[0].ph_am;
            ph_en_mp3 = model.symbols[0].ph_en_mp3;
            word_pl = SerilizeStringFromArray(model.exchange.word_pl);
            word_past = SerilizeStringFromArray(model.exchange.word_past);
            word_done = SerilizeStringFromArray(model.exchange.word_done);
            word_ing = SerilizeStringFromArray(model.exchange.word_ing);
            word_third = SerilizeStringFromArray(model.exchange.word_third);
            word_er = SerilizeStringFromArray(model.exchange.word_er);
            word_est = SerilizeStringFromArray(model.exchange.word_est);
            foreach (var item in ObservableCollectionHelper.GetObservableCollectionFromArray(model.symbols[0].parts))
            {
                PartsViewModel.Add(new PartsViewModel_AiCiBa()
                {
                    part = item.part,
                    mean = SerilizeStringFromArray(item.means)
                });
            }
        }

        public void Clear()
        {
            PartsViewModel.Clear();
        }

        public string SerilizeStringFromArray(string[] input)
        {
            string result = "";
            if (input != null)
            {
                foreach (var item in input)
                {
                    result += item + "; ";
                }
            }
            return result;
        }

        string _ph_en = "";
        public string ph_en
        {
            get { return _ph_en; }
            set { Set(ref _ph_en, value); }
        }

        string _ph_am = "";
        public string ph_am
        {
            get { return _ph_am; }
            set { Set(ref _ph_am, value); }
        }

        string _ph_other = "";
        public string ph_other
        {
            get { return _ph_other; }
            set { Set(ref _ph_other, value); }
        }

        string _ph_en_mp3 = "";
        public string ph_en_mp3
        {
            get { return _ph_en_mp3; }
            set { Set(ref _ph_en_mp3, value); }
        }

        string _ph_am_mp3 = "";
        public string ph_am_mp3
        {
            get { return _ph_am_mp3; }
            set { Set(ref _ph_am_mp3, value); }
        }

        string _ph_tts_mp3 = "";
        public string ph_tts_mp3
        {
            get { return _ph_tts_mp3; }
            set { Set(ref _ph_tts_mp3, value); }
        }

        #region 词性
        string _word_pl = "";
        public string word_pl
        {
            get { return _word_pl; }
            set { Set(ref _word_pl, value); }
        }

        string _word_past = "";
        public string word_past
        {
            get { return _word_past; }
            set { Set(ref _word_past, value); }
        }

        string _word_done = "";
        public string word_done
        {
            get { return _word_done; }
            set { Set(ref _word_done, value); }
        }

        string _word_ing = "";
        public string word_ing
        {
            get { return _word_ing; }
            set { Set(ref _word_ing, value); }
        }

        string _word_third = "";
        public string word_third
        {
            get { return _word_third; }
            set { Set(ref _word_third, value); }
        }

        string _word_er = "";
        public string word_er
        {
            get { return _word_er; }
            set { Set(ref _word_er, value); }
        }

        string _word_est = "";
        public string word_est
        {
            get { return _word_est; }
            set { Set(ref _word_est, value); }
        }
        #endregion

        #region 释义
        ObservableCollection<PartsViewModel_AiCiBa> _PartsViewModel = new ObservableCollection<PartsViewModel_AiCiBa>();
        public ObservableCollection<PartsViewModel_AiCiBa> PartsViewModel
        {
            get { return _PartsViewModel; }
            set { Set(ref _PartsViewModel, value); }
        }
        #endregion
    }

    public class PartsViewModel_AiCiBa : Observable
    {
        string _part;
        public string part
        {
            get { return _part; }
            set { Set(ref _part, value); }
        }

        string _mean;
        public string mean
        {
            get { return _mean; }
            set { Set(ref _mean, value); }
        }
    }

    public class DefsViewModel : Observable
    {
        string _pos = "";
        string _def = "";

        public string pos
        {
            get { return _pos; }
            set { Set(ref _pos, value); }
        }
        public string def
        {
            get { return _def; }
            set { Set(ref _def, value); }
        }
    }

    public class SamsViewModel : Observable
    {
        string _eng = "";
        string _chn = "";
        string _mp3Url = "";
        string _mp4Url = "";


        public string eng
        {
            get { return _eng; }
            set { Set(ref _eng, value); }
        }
        public string chn
        {
            get { return _chn; }
            set { Set(ref _chn, value); }
        }
        public string mp3Url
        {
            get { return _mp3Url; }
            set { Set(ref _mp3Url, value); }
        }
        public string mp4Url
        {
            get { return _mp4Url; }
            set { Set(ref _mp4Url, value); }
        }
    }

    public class IsSavedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool result = (bool)value;
            return result == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }



}
