using Lepton_Dictionary.ViewModels;
using Lepton_Library.Common;
using Lepton_Library.Helper;
using Lepton_Library.Network;
using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using System.Reflection;
using static Lepton_Dictionary.Models.DictionaryExtension;

namespace Lepton_Dictionary.Models
{
    public class DictionaryService
    {
        static Language Language1 { get; set; }
        static Language Language2 { get; set; }
        static Language CurrentSourceLanguage { get; set; }
        static Language CurrentTargetLanguage { get; set; }

        static List<DictionaryModel> CurrentDictionaries = new List<DictionaryModel>();

        public static List<DictionaryModel> BuiltInDictionaries //只读 读取所有内置词典
        {
            get
            {
                List<DictionaryModel> results = new List<DictionaryModel>();
                //results.Add(new AiCiBaC2E_DictionaryModel());
                //results.Add(new AiCiBaE2C_DictionaryModel());
                //results.Add(new OfflineSourceE2C_1_DictionaryModel());

                foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if(t.GetCustomAttribute(typeof(LeptonDictionaryModel)) != null && t.IsSubclassOf(typeof(DictionaryModel)))
                    {
                        results.Add((DictionaryModel)t.GetConstructor(new Type[0]).Invoke(new object[0]));
                    }
                }

                return results;
            }
        }
        public static List<DictionaryModel> PackageDictionaries //只读 读取所有DLC词典 - 【未完成】
        {
            get
            {
                return new List<DictionaryModel>();
            }
        }
        public static List<DictionaryModel> ExtensionDictionaries//只读 读取所有扩展词典
        {
            get
            {
                List<DictionaryModel> results = new List<DictionaryModel>();
                foreach (var item in AppService.ExtensionManager.Extensions)
                {
                    item.DictionariesSupported.ForEach(x =>
                    {
                        x.ViewModel = new WordViewModel();
                        results.Add(x);
                    });
                }
                return results;
            }
        }
        public static List<DictionaryModel> AvailableDictionaries//只读 读取当前模式下的可用词典
        {
            get
            {
                List<DictionaryModel> results = new List<DictionaryModel>();
                BuiltInDictionaries.ForEach(x =>
                {
                    if ((x.SourceLanguage == Language1 & x.TargetLanguage == Language2) || (x.SourceLanguage == Language2 & x.TargetLanguage == Language1))
                    {
                        results.Add(x);
                    }
                });
                PackageDictionaries.ForEach(x =>
                {
                    if ((x.SourceLanguage == Language1 & x.TargetLanguage == Language2) || (x.SourceLanguage == Language2 & x.TargetLanguage == Language1))
                    {
                        results.Add(x);
                    }
                });
                ExtensionDictionaries.ForEach(x =>
                {
                    if ((x.SourceLanguage == Language1 & x.TargetLanguage == Language2) || (x.SourceLanguage == Language2 & x.TargetLanguage == Language1))
                    {
                        results.Add(x);
                    }
                });
                return results;
            }
        }
        public static List<DictionaryModel> FilteredDictionariesForQuery//查询时复检并返回可用的词典包
        {
            get
            {
                WebStatus queryStatus = CheckQueryAvailability(); //检查网络状态
                List<DictionaryModel> results = new List<DictionaryModel>();
                CurrentDictionaries.ForEach(x =>
                {
                    if (x.SourceLanguage == CurrentSourceLanguage & x.TargetLanguage == CurrentTargetLanguage)
                    {
                        if (!(queryStatus == WebStatus.NoInternet & x.IsInternetRequired == true))
                        {
                            results.Add(x);
                        }
                    }
                });
                return results;
            }
        }

        public static void AdjustDictionaryByIndicator(int indicator)
        {
            //temp code, for the "indicator < 10" I just mean that currently we do not use indicator that larger than 10
            //I do not suggest to use this indicator method in future versions
            if (indicator <= 10)
            {
                Language1 = Language.CN;
                switch (indicator)
                {
                    case 0://自动
                        Language2 = Language.EN;
                        break;
                    case 1://英汉
                        Language2 = Language.EN;
                        break;
                    case 2://汉语
                        Language2 = Language.CN;
                        break;
                    case 3://日汉
                        Language2 = Language.JP;
                        break;
                    case 4://韩汉
                        Language2 = Language.KO;
                        break;
                    case 5://法汉
                        Language2 = Language.FR;
                        break;
                    case 6://德汉
                        Language2 = Language.DE;
                        break;
                    case 7://葡汉
                        Language2 = Language.PT;
                        break;
                    default://自动
                        Language2 = Language.EN;
                        break;
                }

            }

            CurrentDictionaries = AvailableDictionaries;
        }

        public static void AdjustDictionaryByLangs(Language lang1, Language lang2)
        {
            Language1 = lang1;
            Language2 = lang2;
            CurrentDictionaries = FilteredDictionariesForQuery;
        }

        private static bool AdjustLanguage(string input, out Language language) {

            Language detected = LanguageHelper._CheckLanguage(input);
            language = detected;

            if (detected == Language1)
            {
                CurrentSourceLanguage = Language1;
                CurrentTargetLanguage = Language2;
            }else if(detected == Language2)
            {
                CurrentSourceLanguage = Language2;
                CurrentTargetLanguage = Language1;
            }
            else
            {
                //suggest to select another language
                return false;
            }

            return true;
        }

        private static List<DictionaryModel> QueryWord(string word, params object[] args)
        {
            List<DictionaryModel> results = FilteredDictionariesForQuery.Count > 0 ? FilteredDictionariesForQuery : AvailableDictionaries;          
            results.ForEach(x => x.Query(word, args));//后代异步加载数据，加载完毕后异步更新前台
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_queryword">待查询的单词 Word which is going to be queried</param>
        /// <param name="languageIndicatorIndex">语言指示器的当前索引 Current Index of the Language Indicator</param>
        public static void QueryFromAutoSuggestBox(string _queryword)
        {
            _queryword = DictionaryService.HandleQueryWord(_queryword);//处理、裁剪要查询的单词
            Language lang;
            if (AdjustLanguage(_queryword, out lang))
                DefinationPageViewModel.Current.UpdateViewModel(_queryword, QueryWord(_queryword), LanguageHelper.ConvertToLangCode(CurrentSourceLanguage), LanguageHelper.ConvertToLangCode(CurrentTargetLanguage));//前台更新，加载所有词典的UI
            else
                DefinationPageViewModel.Current.UpdateViewModel(_queryword, QueryWord(_queryword, lang), LanguageHelper.ConvertToLangCode(CurrentSourceLanguage), LanguageHelper.ConvertToLangCode(CurrentTargetLanguage));//前台更新，加载所有词典的UI
            HomePageViewModel.Current.SearchUIState = SearchUIState.Searched;
            WordHistoryModel _wordhistoryitem = new WordHistoryModel()
            {
                Word = DefinationPageViewModel.Current.Word,
                SourceLanguage = (int)CurrentSourceLanguage,
                TargetLanguage = (int)CurrentTargetLanguage
            };

            SearchSuggestionPageViewModel.Current.Add_WordHistory(_wordhistoryitem);
        }

        public static void QueryFromHistory(WordHistoryModel historyViewModel)
        {
            var _queryword = historyViewModel.Word;
            CurrentSourceLanguage = (Language)historyViewModel.SourceLanguage;
            CurrentTargetLanguage = (Language)historyViewModel.TargetLanguage;
            DefinationPageViewModel.Current.UpdateViewModel(_queryword, QueryWord(_queryword), LanguageHelper.ConvertToLangCode(CurrentSourceLanguage), LanguageHelper.ConvertToLangCode(CurrentTargetLanguage));//前台更新，加载所有词典的UI
            HomePageViewModel.Current.SearchUIState = SearchUIState.Searched;
        }

        public static void QueryFromCompactOverlayView(string _queryword)
        {
            _queryword = DictionaryService.HandleQueryWord(_queryword);//处理、裁剪要查询的单词
            Language lang;
            if (AdjustLanguage(_queryword, out lang))
                CompactOverlay_HomePageViewModel.Current.UpdateViewModel(_queryword, QueryWord(_queryword), LanguageHelper.ConvertToLangCode(CurrentSourceLanguage), LanguageHelper.ConvertToLangCode(CurrentTargetLanguage));//前台更新，加载所有词典的UI
            else
                QueryWord(_queryword, lang);
        }

        //public static void IndicateLanuageTarget(int index)
        //{
        //    switch (index)
        //    {
        //        case 0://自动
        //            PredictedLanguage1 = "zh-cn";
        //            PredictedLanguage2 = "en";
        //            break;
        //        case 1://英汉
        //            PredictedLanguage1 = "zh-cn";
        //            PredictedLanguage2 = "en";
        //            break;
        //        case 2://汉语
        //            PredictedLanguage1 = "zh-cn";
        //            PredictedLanguage2 = "zh-cn";
        //            break;
        //        case 3://日汉
        //            PredictedLanguage1 = "jp";
        //            PredictedLanguage2 = "zh-cn";
        //            break;
        //        case 4://韩汉
        //            PredictedLanguage1 = "ko";
        //            PredictedLanguage2 = "zh-cn";
        //            break;
        //        case 5://法汉
        //            PredictedLanguage1 = "fr";
        //            PredictedLanguage2 = "zh-cn";
        //            break;
        //        case 6://德汉
        //            PredictedLanguage1 = "ge";
        //            PredictedLanguage2 = "zh-cn";
        //            break;
        //        case 7://葡汉
        //            PredictedLanguage1 = "po";
        //            PredictedLanguage2 = "zh-cn";
        //            break;
        //        default://自动
        //            PredictedLanguage1 = "en";
        //            PredictedLanguage2 = "zh-cn";
        //            break;
        //    }
        //}

        //public static string IndicateLanuageTarget(string source, int index)
        //{
        //    if (source == "zh-cn")
        //    {
        //        switch (index)
        //        {
        //            case 0://自动
        //                return "en";
        //            case 1://英汉
        //                return "en";
        //            case 2://汉语
        //                return "zh-cn";
        //            case 3://日汉
        //                return "jp";
        //            case 4://韩汉
        //                return "ko";
        //            case 5://法汉
        //                return "fr";
        //            case 6://德汉
        //                return "ge";
        //            case 7://葡汉
        //                return "po";
        //            default://自动
        //                return "en";
        //        }
        //    }
        //    else
        //    {
        //        return "zh-cn";
        //    }
        //}

        public static string ShowLanguageDisplayText(string _sourcelanguage, string _targetlanguage)
        {
            return ShowLanguageDisplayText(_sourcelanguage) + ShowLanguageDisplayText(_targetlanguage);
        }

        public static string ShowLanguageDisplayText(string _languagecode)
        {
            switch (_languagecode)
            {
                case "zh-cn"://自动
                    return "汉";
                case "en"://英汉
                    return "英";
                case "jp"://日汉
                    return "日";
                case "ko"://韩汉
                    return "韩";
                case "fr"://法汉
                    return "法";
                case "ge"://德汉
                    return "德";
                case "po"://葡汉
                    return "葡";
                default://自动
                    return "英";
            }
        }

        public static string HandleQueryWord(string _queryword)
        {
            return _queryword.Trim();
        }

        public static WebStatus CheckQueryAvailability()
        {
            WebStatus result = WebStatus.InternetAvailable;
            if (!NetworkConnection.IsInternetAvailable())
            {
                result = WebStatus.NoInternet;
            }
            else if (NetworkConnection.IsInternetOnMeteredConnection())
            {
                result = WebStatus.MeteredInternet;
            }
            return result;
        }

        public static bool IsAvailableUnderMeteredInternet
        {
            get { return LocalSetting.GetValueOrDefault("IsUnavailableUnderMetered", true); }
            set { LocalSetting.AddOrUpdateValue("IsAvailableUnderMeteredInternet", value); }
        }
        public static bool IsOnlineQueriedEntryAutoSaved
        {
            get { return LocalSetting.GetValueOrDefault("IsOnlineQueriedEntryAutoSaved", true); }
            set { LocalSetting.AddOrUpdateValue("IsOnlineQueriedEntryAutoSaved", value); }
        }
        public static bool IsOfflineSavingDictionaryUsedPreferentially
        {
            get { return LocalSetting.GetValueOrDefault("IsOfflineSavingDictionaryUsedPreferentially", true); }
            set { LocalSetting.AddOrUpdateValue("IsOfflineSavingDictionaryUsedPreferentially", value); }
        }
    }

    public class DictionaryQueryModel
    {
        public string QueryWord { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public WordModel Result { get; set; } = new WordModel();
        public WebStatus QueryStatus { get; set; } = WebStatus.InternetAvailable;
        public ResultStatus ResultStatus { get; set; } = ResultStatus.Succuss;
    }

    public enum WebStatus
    {
        InternetAvailable = 0,
        NoInternet = 1,
        MeteredInternet = 2,
        UnavailableWithUnknownReason = 3,
        OfflineAvailable = 4
    }

    public enum ResultStatus
    {
        Succuss = 0,
        WordCantFind = 1,
        FailedWithUnknownReason = 2,
        WrongLanguage = 3,
        InputSentence = 4
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LeptonDictionaryModel : Attribute
    {

    }


    public class DictionaryModel : ViewModelBase, IDictionaryModel
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Language SourceLanguage { get; set; }
        public virtual Language TargetLanguage { get; set; }
        public virtual bool IsInternetRequired { get; set; }
        public virtual bool IsAvailable { get; set; }
        public virtual string Background { get; set; } = "Teal";
        public virtual DictionaryType DictionaryType { get; set; } = DictionaryType.BuiltIn;

        public virtual DictionaryQueryModel ResultModel { get; set; }

        WordViewModel _ViewModel = new WordViewModel();
        public WordViewModel ViewModel
        {
            get { return _ViewModel; }
            set { Set(ref _ViewModel, value); }
        }

        public async virtual Task Query(string word, params object[] args)
        {
            ResultModel = await GetDictionaryQueryModelAsync(word);
            ProcessResult(word, args);

            UpdateToViewModel(args);
        }

        protected virtual void UpdateToViewModel(params object[] args)
        {
            ViewModel = new WordViewModel(ResultModel, args);
        }

        protected virtual void ProcessResult(string word, params object[] args)
        {
            if (ResultModel.ResultStatus == ResultStatus.FailedWithUnknownReason)
            {
                if (args.Length == 1 && args[0] is Language)
                {
                    ResultModel.ResultStatus = ResultStatus.WrongLanguage;
                }
            }
            else
            {
                if (IsResultNullOrEmpty())
                {
                    if (args.Length == 1 && args[0] is Language)
                    {
                        ResultModel.ResultStatus = ResultStatus.WrongLanguage;
                    }
                    else
                    {
                        if (word.Contains(" ") || word.Contains(','))
                        {
                            ResultModel.ResultStatus = ResultStatus.InputSentence;
                        }
                        else
                        {
                            ResultModel.ResultStatus = ResultStatus.WordCantFind;
                        }
                    }
                }
                else
                {
                    ResultModel.ResultStatus = ResultStatus.Succuss;
                }
            }
        }

        public virtual DictionaryQueryModel GetDictionaryQueryModel(string word)
        {
            return new DictionaryQueryModel();
        }

        public async virtual Task<DictionaryQueryModel> GetDictionaryQueryModelAsync(string word)
        {
            return await Task.Run(() => GetDictionaryQueryModel(word));
        }

        public bool IsResultNullOrEmpty()
        {
            foreach (var item in ResultModel.Result.Definitions)
            {
                foreach (var meaning in item.Meanings)
                {
                    if (meaning != "")
                    {
                        return false;
                    }
                }
                if (item.PartOfSpeech != "")
                {
                    return false;
                }
            }
            foreach (var item in ResultModel.Result.InflectionWords)
            {
                foreach (var word in item.Words)
                {
                    if (word != "")
                    {
                        return false;
                    }
                }
            }
            foreach (var item in ResultModel.Result.Pronounciations)
            {
                if (item.SoundUri != "")
                {
                    return false;
                }
                //else if (item.PronounciationType != "")
                //{
                //    return false;
                //}
                else if (item.PhoneticSymbol != "")
                {
                    return false;
                }
            }
            foreach (var item in ResultModel.Result.SampleSentences)
            {
                if (item.SourceLanguageSentence != "")
                {
                    return false;
                }
                if (item.TargetLanguageSentence != "")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
