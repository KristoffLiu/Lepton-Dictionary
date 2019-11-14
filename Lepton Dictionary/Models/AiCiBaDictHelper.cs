using Lepton_Library.Helper;
using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Lepton_Dictionary.Models.DictionaryService;

namespace Lepton_Dictionary.Models
{
    public class AiCiBaDictHelper
    {
        //Example: "dict-co.iciba.com/api/dictionary.php?w=go&type=json&key=FD6631A6A5BC97C124D85CDCAFCA7806"
        //Key: "FD6631A6A5BC97C124D85CDCAFCA7806"
        const string APIHeader = "http://dict-co.iciba.com/api/dictionary.php?w=";
        const string APITail = "&type=json&key=FD6631A6A5BC97C124D85CDCAFCA7806";
        const int DictionarySourceCode = 0;
        AiCiBaE2C_Model DictModel = new AiCiBaE2C_Model();

        public static DictionaryQueryModel QueryResultFromWebAPI_E2C(string word)
        {
            DictionaryQueryModel result = new DictionaryQueryModel();
            try
            {
                bool IsIncorrectWord = AutoFix(ref word);
                WebRequest webRequest = HttpWebRequest.Create( APIHeader + word + APITail );
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                var jsonstring = sr.ReadToEnd();
                var aicibamodel = Json.Deserialize<AiCiBaE2C_Model>(jsonstring);
                result.Result = GetWordModelFromAiCiBaE2C(aicibamodel);
                result.ResultStatus = ResultStatus.Succuss;
            }
            catch
            {
                result.ResultStatus = ResultStatus.FailedWithUnknownReason;
            }
            return result;
        }

        public static WordModel GetWordModelFromAiCiBaE2C(AiCiBaE2C_Model _aimodel)
        {
            WordModel result = new WordModel();
            result.Word = _aimodel.word_name;

            result.Pronounciations.Add(new Pronounciation()
            {
                PronounciationType = "英",
                PhoneticSymbol = _aimodel.symbols[0].ph_en,
                SoundUri = _aimodel.symbols[0].ph_en_mp3
            });
            result.Pronounciations.Add(new Pronounciation()
            {
                PronounciationType = "美",
                PhoneticSymbol = _aimodel.symbols[0].ph_am,
                SoundUri = _aimodel.symbols[0].ph_am_mp3
            });
            result.Pronounciations.Add(new Pronounciation()
            {
                PronounciationType = "其他",
                PhoneticSymbol = _aimodel.symbols[0].ph_other,
                SoundUri = _aimodel.symbols[0].ph_tts_mp3
            });

            result.InflectionWords.Add(new InflectionWord()
            {
                InflectionType = "复数",
                Words = ListHelper.GetListFromArray(_aimodel.exchange.word_pl)
            });
            result.InflectionWords.Add(new InflectionWord()
            {
                InflectionType = "过去式",
                Words = ListHelper.GetListFromArray(_aimodel.exchange.word_past)
            });
            result.InflectionWords.Add(new InflectionWord()
            {
                InflectionType = "过去分词",
                Words = ListHelper.GetListFromArray(_aimodel.exchange.word_done)
            });
            result.InflectionWords.Add(new InflectionWord()
            {
                InflectionType = "进行时",
                Words = ListHelper.GetListFromArray(_aimodel.exchange.word_ing)
            });
            result.InflectionWords.Add(new InflectionWord()
            {
                InflectionType = "第三人称",
                Words = ListHelper.GetListFromArray(_aimodel.exchange.word_third)
            });
            result.InflectionWords.Add(new InflectionWord()
            {
                InflectionType = "比较级",
                Words = ListHelper.GetListFromArray(_aimodel.exchange.word_er)
            });
            result.InflectionWords.Add(new InflectionWord()
            {
                InflectionType = "最高级",
                Words = ListHelper.GetListFromArray(_aimodel.exchange.word_est)
            });

            foreach (var definition in _aimodel.symbols[0].parts.ToList())
            {
                result.Definitions.Add(new Definition()
                {
                    PartOfSpeech = definition.part,
                    Meanings = definition.means.ToList()
                });
            }
            return result;
        }

        public static bool CapitalFix(ref string word)
        {
            if (word != null && word.Length > 1)
            {
                bool NonFirstCapital = false;
                foreach (char c in word /* word.Substring(1, word.Length - 1) */)
                {
                    if (c >= 'A' && c <= 'Z')
                    {
                        NonFirstCapital = true;
                        break;
                    }
                }

                if (NonFirstCapital)
                {
                    word = word.ToLower();
                    return true;
                }
            }
            return false;
        }

        public static bool AutoFix(ref string word)
        {
            return CapitalFix(ref word);
        }

        public static DictionaryQueryModel QueryResultFromWebAPI_C2E(string word)
        {
            DictionaryQueryModel result = new DictionaryQueryModel();
            try
            {
                WebRequest webRequest = HttpWebRequest.Create(APIHeader + word + APITail);
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                var jsonstring = sr.ReadToEnd();
                try
                {
                    var aicibamodel = Json.Deserialize<AiCiBaC2E_Model>(jsonstring);
                    result.Result = GetWordModelFromAiCiBaC2E(aicibamodel);
                }
                catch
                {
                    var aicibamodel = Json.Deserialize<AiCiBaC2E_Model2>(jsonstring);
                    result.Result = GetWordModelFromAiCiBaC2E2(aicibamodel);
                }
                result.ResultStatus = ResultStatus.Succuss;
            }
            catch
            {
                result.ResultStatus = ResultStatus.WordCantFind;
            }
            return result;
        }
        public static WordModel GetWordModelFromAiCiBaC2E(AiCiBaC2E_Model _aimodel)
        {
            WordModel result = new WordModel();
            result.Word = _aimodel.word_name;

            result.Pronounciations.Add(new Pronounciation()
            {
                PronounciationType = "拼音",
                PhoneticSymbol = _aimodel.symbols[0].word_symbol,
                SoundUri = _aimodel.symbols[0].symbol_mp3
            });

            foreach (var definition in _aimodel.symbols[0].parts.ToList())
            {
                Definition definition1 = new Definition();
                definition1.PartOfSpeech = definition.part_name;
                foreach(var item in definition.means)
                {
                    definition1.Meanings.Add(item.word_mean);
                }
                result.Definitions.Add(definition1);
            }
            return result;
        }
        public static WordModel GetWordModelFromAiCiBaC2E2(AiCiBaC2E_Model2 _aimodel)
        {
            WordModel result = new WordModel();
            result.Word = _aimodel.word_name;

            result.Pronounciations.Add(new Pronounciation()
            {
                PronounciationType = "拼音",
                PhoneticSymbol = _aimodel.symbols[0].word_symbol,
                SoundUri = _aimodel.symbols[0].symbol_mp3
            });

            foreach (var definition in _aimodel.symbols[0].parts.ToList())
            {
                Definition definition1 = new Definition();
                definition1.PartOfSpeech = definition.part_name;
                foreach (var item in definition.means)
                {
                    definition1.Meanings.Add(item.word_mean);
                }
                result.Definitions.Add(definition1);
            }
            return result;
        }


        //public static DictionaryQueryModel QueryResult_E2C(string word)
        //{
        //    DictionaryQueryModel result = new DictionaryQueryModel();
        //    switch (dictionaryModel.QueryStatus)
        //    {
        //        case WebStatus.InternetAvailable:
        //            if (DictionaryService.IsOfflineSavingDictionaryUsedPreferentially)
        //                result.WordModel = TryQueryResultOffline_E2C(dictionaryModel);
        //            else
        //                result.WordModel = QueryResultFromWeb_E2C(dictionaryModel);
        //            break;
        //        case WebStatus.MeteredInternet:
        //            if (DictionaryService.IsAvailableUnderMeteredInternet)
        //                if (DictionaryService.IsOfflineSavingDictionaryUsedPreferentially)
        //                    result.WordModel = TryQueryResultOffline_E2C(dictionaryModel);
        //                else
        //                    result.WordModel = TryQueryResultOfflineOnly_E2C(dictionaryModel);
        //            break;
        //        case WebStatus.NoInternet:
        //            if (DictionaryService.IsOfflineSavingDictionaryUsedPreferentially)
        //                result.WordModel = TryQueryResultOfflineOnly_E2C(dictionaryModel);
        //            break;
        //        default:
        //            result.WordModel = TryQueryResultOfflineOnly_E2C(dictionaryModel);
        //            break;
        //    }
        //    return result;
        //}
        //public static WordsStorageModel QueryResultFromWeb_E2C(DictionaryQueryModel _dictionaryModel)
        //{
        //    int index = 0;
        //    AiCiBaE2C_Model _aimodel = null;
        //    while (index == 0)
        //    {
        //        WebRequest webRequest = HttpWebRequest.Create("http://dict-co.iciba.com/api/dictionary.php?w=" + _dictionaryModel.QueryWord + "&type=json&key=FD6631A6A5BC97C124D85CDCAFCA7806");
        //        WebResponse webResponse = webRequest.GetResponse();
        //        Stream stream = webResponse.GetResponseStream();
        //        StreamReader sr = new StreamReader(stream);
        //        var jsonstring = sr.ReadToEnd();
        //        _aimodel = Json.Deserialize<AiCiBaE2C_Model>(jsonstring);
        //        index++;
        //    }
        //    if (DictionaryService.IsOfflineSavingDictionaryUsedPreferentially)
        //        return WordStorageHelper.Add(_aimodel);
        //    else return WordStorageHelper.GetWordModelFromAiCiBa(_aimodel);
        //}
        //public static WordsStorageModel TryQueryResultOffline_E2C(DictionaryQueryModel _dictionaryModel)
        //{
        //    WordsStorageModel result = null;
        //    if (WordStorageHelper.IsEntryExist(_dictionaryModel.QueryWord, DictionarySourceCode))
        //        result = WordStorageHelper.QueryWord(_dictionaryModel.QueryWord, DictionarySourceCode);
        //    else
        //        result = QueryResultFromWeb_E2C(_dictionaryModel);
        //    return result;
        //}
        //public static WordsStorageModel TryQueryResultOfflineOnly_E2C(DictionaryQueryModel _dictionaryModel)
        //{
        //    if (WordStorageHelper.IsEntryExist(_dictionaryModel.QueryWord, DictionarySourceCode))
        //        return WordStorageHelper.QueryWord(_dictionaryModel.QueryWord, DictionarySourceCode);
        //    else return null;
        //}

    }

    [LeptonDictionaryModel]
    public class AiCiBaE2C_DictionaryModel : DictionaryModel
    {
        public AiCiBaE2C_DictionaryModel()
        {
            Name = "金山词霸英汉词典";
            Description = "使用金山词霸官方提供的API的词典源";
            SourceLanguage = Language.EN;
            TargetLanguage = Language.CN;
            Background = "#FFa6c0fe";
            IsInternetRequired = true;
        }

        public override DictionaryQueryModel GetDictionaryQueryModel(string word)
        {
            return ResultModel = AiCiBaDictHelper.QueryResultFromWebAPI_E2C(word);
        }
    }

    [LeptonDictionaryModel]
    public class AiCiBaC2E_DictionaryModel : DictionaryModel
    {
        public AiCiBaC2E_DictionaryModel()
        {
            Name = "金山词霸汉英词典";
            Description = "使用金山词霸官方提供的API的词典源";
            SourceLanguage = Language.CN;
            TargetLanguage = Language.EN;
            Background = "#FFD57EEB";
            IsInternetRequired = true;
        }

        public override DictionaryQueryModel GetDictionaryQueryModel(string word)
        {
            return ResultModel = AiCiBaDictHelper.QueryResultFromWebAPI_C2E(word);
        }

        protected override void ProcessResult(string word, params object[] args)
        {
            base.ProcessResult(word, args);
            if (word.Length > LanguageHelper.CHINESE_WORD_MAX_LENGTH || word.Contains('，') || word.Contains('。'))
            {
                ResultModel.ResultStatus = ResultStatus.InputSentence;
            }
        }
    } 
    public class AiCiBaE2C_Model
    {
        public string word_name { get; set; } = "";
        public int is_CRI { get; set; } = 0;
        public AiCiBaE2C_Exchange exchange { get; set; } = new AiCiBaE2C_Exchange();
        public AiCiBaE2C_Symbol[] symbols { get; set; } = { new AiCiBaE2C_Symbol() };
    }
    public class AiCiBaE2C_Exchange
    {
        public string[] word_pl { get; set; }     = {""};
        public string[] word_past { get; set; }   = {""};
        public string[] word_done { get; set; }   = {""};
        public string[] word_ing { get; set; }    = {""};
        public string[] word_third { get; set; }  = {""};
        public string[] word_er { get; set; }     = {""};
        public string[] word_est { get; set; }    = {""};
    }
    public class AiCiBaE2C_Symbol
    {
        public string ph_en { get; set; }                 = "";
        public string ph_am { get; set; }                 = "";
        public string ph_other { get; set; }              = "";
        public string ph_en_mp3 { get; set; }             = "";
        public string ph_am_mp3 { get; set; }             = "";
        public string ph_tts_mp3 { get; set; }            = "";
        public AiCiBaE2C_Part[] parts { get; set; } = { new AiCiBaE2C_Part() };

    }
    public class AiCiBaE2C_Part
    {
        public string part { get; set; } = "";
        public string[] means { get; set; } = { "" };
    }

    public class AiCiBaC2E_Model
    {
        public string word_name { get; set; } = "";
        public AiCiBaC2E_Symbol[] symbols { get; set; }
    }
    public class AiCiBaC2E_Symbol
    {
        public string word_symbol { get; set; }
        public string symbol_mp3 { get; set; }
        public AiCiBaC2E_Part[] parts { get; set; }
        public string ph_am_mp3 { get; set; }
        public string ph_en_mp3 { get; set; }
        public string ph_tts_mp3 { get; set; }
        public string ph_other { get; set; }
    }
    public class AiCiBaC2E_Part
    {
        public string part_name { get; set; } = "";
        public AiCiBaC2E_Mean[] means { get; set; }

    }
    public class AiCiBaC2E_Mean
    {
        public string word_mean { get; set; } = "";
        public string has_mean { get; set; } = "";
        public int split { get; set; }
    }

    public class AiCiBaC2E_Model2
    {
        public string word_id { get; set; } = "";
        public string word_name { get; set; } = "";
        public AiCiBaC2E_Symbol2[] symbols { get; set; }
    }
    public class AiCiBaC2E_Symbol2
    {
        public string symbol_id { get; set; }
        public string word_id { get; set; }
        public string word_symbol { get; set; }
        public string symbol_mp3 { get; set; }
        public AiCiBaC2E_Part[] parts { get; set; }
        public string ph_am_mp3 { get; set; }
        public string ph_en_mp3 { get; set; }
        public string ph_tts_mp3 { get; set; }
        public string ph_other { get; set; }
    }
    public class AiCiBaC2E_Part2
    {
        public string part_name { get; set; } = "";
        public AiCiBaC2E_Mean[] means { get; set; }

    }
    public class AiCiBaC2E_Mean2
    {
        public string mean_id { get; set; } = "";
        public string part_id { get; set; } = "";
        public string word_mean { get; set; } = "";
        public string has_mean { get; set; } = "";
        public int split { get; set; }
    }

}
