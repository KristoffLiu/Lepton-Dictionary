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

namespace Lepton_Dictionary.ViewModels
{
    public class WordViewModel : Observable
    {
        string _Word;
        Visibility _PronunciationsVisibility = Visibility.Visible;
        Visibility _InflectionWordsVisibility = Visibility.Visible;
        Visibility _DefinitionsVisibility = Visibility.Visible;
        Visibility _SampleSentencesVisibility = Visibility.Visible;
        Visibility _IsLoadingVisibility = Visibility.Visible;
        Visibility _FailedVisibility = Visibility.Collapsed;
        string _noticetext;

        public WordViewModel()
        {

        }
        public WordViewModel(DictionaryQueryModel Querymodel, params object[] args)
        {
            var model = Querymodel.Result;
            IsLoadingVisibility = Visibility.Collapsed;
            switch (Querymodel.ResultStatus)
            {
                case ResultStatus.Succuss:
                    Word = model.Word;
                    foreach (var item in model.Pronounciations)
                    {
                        if (item.PhoneticSymbol != string.Empty || item.SoundUri != string.Empty)
                        {
                            Pronunciations.Add(new PronounciationViewModel(item));
                        }
                    }
                    if (model.InflectionWords.Count > 0)
                    {
                        foreach (var item in model.InflectionWords)
                        {
                            if (item.Words.Count != 0)
                            {
                                InflectionWords.Add(new InflectionWordViewModel(item));
                            }
                        }
                    }
                    else
                    {
                        InflectionWordsVisibility = Visibility.Collapsed;
                    }
                    if (model.Definitions.Count > 0)
                    {
                        foreach (var item in model.Definitions)
                        {
                            Definitions.Add(new DefinitionViewModel(item));
                        }
                    }
                    else
                    {
                        DefinitionsVisibility = Visibility.Collapsed;
                    }
                    if (model.SampleSentences.Count > 0)
                    {
                        foreach (var item in model.SampleSentences)
                        {
                            SampleSentences.Add(new SampleSentenceViewModel(item));
                        }
                    }
                    else
                    {
                        SampleSentencesVisibility = Visibility.Collapsed;
                    }
                    break;
                case ResultStatus.WordCantFind:
                    FailedVisibility = Visibility.Visible;
                    NoticeText = "找不到单词";
                    break;
                case ResultStatus.FailedWithUnknownReason:
                    FailedVisibility = Visibility.Visible;
                    NoticeText = "未知错误";
                    break;
                case ResultStatus.InputSentence:
                    FailedVisibility = Visibility.Visible;
                    NoticeText = "您输入的可能是句子，目前仅支持翻译单词或短词组";
                    break;
                case ResultStatus.WrongLanguage:
                    FailedVisibility = Visibility.Visible;
                    NoticeText = string.Format("您输入的语言可能是 {0}, 请切换词典", LanguageHelper.GetLanguageName((Language)args[0]));
                    break;
            }
        }

        public string Word
        {
            get { return _Word; }
            set { Set(ref _Word, value); }
        }
        public Visibility PronunciationsVisibility
        {
            get { return _PronunciationsVisibility; }
            set { Set(ref _PronunciationsVisibility, value); }
        }
        public Visibility InflectionWordsVisibility
        {
            get { return _InflectionWordsVisibility; }
            set { Set(ref _InflectionWordsVisibility, value); }
        }
        public Visibility DefinitionsVisibility
        {
            get { return _DefinitionsVisibility; }
            set { Set(ref _DefinitionsVisibility, value); }
        }
        public Visibility SampleSentencesVisibility
        {
            get { return _SampleSentencesVisibility; }
            set { Set(ref _SampleSentencesVisibility, value); }
        }
        public Visibility IsLoadingVisibility
        {
            get { return _IsLoadingVisibility; }
            set { Set(ref _IsLoadingVisibility, value); }
        }
        public Visibility FailedVisibility
        {
            get { return _FailedVisibility; }
            set { Set(ref _FailedVisibility, value); }
        }
        public string NoticeText
        {
            get { return _noticetext; }
            set { Set(ref _noticetext, value); }
        }

        public ObservableCollection<PronounciationViewModel> Pronunciations { get; set; } = new ObservableCollection<PronounciationViewModel>();
        public ObservableCollection<InflectionWordViewModel> InflectionWords { get; set; } = new ObservableCollection<InflectionWordViewModel>();
        public ObservableCollection<DefinitionViewModel>     Definitions    { get; set; } = new ObservableCollection<DefinitionViewModel>();
        public ObservableCollection<SampleSentenceViewModel> SampleSentences { get; set; } = new ObservableCollection<SampleSentenceViewModel>();

        public static string SerilizeStringFromArray(string[] input)
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
        public static string SerilizeStringFromList(List<string> _modelText)
        {
            string result = "";
            if (_modelText != null)
            {
                if (_modelText.Count == 0)
                {
                    result = "无";
                }
                else if (_modelText.Count == 1)
                {
                    result = _modelText[0];
                }
                else
                {
                    foreach (var text in _modelText)
                    {
                        result += text + "; ";
                    }
                }
            }
            else
            {
                result = "无";
            }
            return result;
        }
    }
    public class PronounciationViewModel : Observable
    {
        string _PronounciationType = "";
        string _phoneticSymbol = "";
        string _soundUri = "";

        public PronounciationViewModel(Pronounciation model)
        {
            PronounciationType = model.PronounciationType;
            PhoneticSymbol = model.PhoneticSymbol;
            SoundUri = model.SoundUri;
        }

        public string PronounciationType
        {
            get { return _PronounciationType; }
            set { Set(ref _PronounciationType, value); }
        }
        public string PhoneticSymbol
        {
            get { return _phoneticSymbol; }
            set { Set(ref _phoneticSymbol, value); }
        }
        public string SoundUri
        {
            get { return _soundUri; }
            set { Set(ref _soundUri, value); }
        }
    }
    public class InflectionWordViewModel : Observable
    {
        public InflectionWordViewModel(InflectionWord model)
        {
            InflectionType = model.InflectionType;
            Word = WordViewModel.SerilizeStringFromList(model.Words);
        }

        string _InflectionType = "";
        string _Word = "";

        public string InflectionType
        {
            get { return _InflectionType; }
            set { Set(ref _InflectionType, value); }
        }
        public string Word
        {
            get { return _Word; }
            set { Set(ref _Word, value); }
        }
    }
    public class DefinitionViewModel : Observable
    {
        string _PartOfSpeech = "";
        string _Meanings = "";

        public DefinitionViewModel(Definition definitionModel)
        {
            PartOfSpeech = definitionModel.PartOfSpeech;
            Meanings = WordViewModel.SerilizeStringFromList(definitionModel.Meanings);
        }

        public string PartOfSpeech
        {
            get { return _PartOfSpeech; }
            set { Set(ref _PartOfSpeech, value); }
        }
        public string Meanings
        {
            get { return _Meanings; }
            set { Set(ref _Meanings, value); }
        }
    }
    public class SampleSentenceViewModel : Observable
    {
        string _SourceLanguageSentence = "";
        string _TargetLanguageSentence = "";

        public SampleSentenceViewModel(SampleSentence sampleSentenceModel)
        {
            SourceLanguageSentence = sampleSentenceModel.SourceLanguageSentence;
            TargetLanguageSentence = sampleSentenceModel.TargetLanguageSentence;
        }

        public string SourceLanguageSentence
        {
            get { return _SourceLanguageSentence; }
            set { Set(ref _SourceLanguageSentence, value); }
        }
        public string TargetLanguageSentence
        {
            get { return _TargetLanguageSentence; }
            set { Set(ref _TargetLanguageSentence, value); }
        }
    }



}
