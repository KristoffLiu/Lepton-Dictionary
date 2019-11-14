using Lepton_Library.Helper;
using Lepton_Library.Storage;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Lepton_Dictionary.Models
{
    [LeptonDictionaryModel]
    public class OfflineSourceE2C_1_DictionaryModel : DictionaryModel
    {
        public OfflineSourceE2C_1_DictionaryModel()
        {
            Name = "离线英汉词典";
            Description = "";
            SourceLanguage = Language.EN;
            TargetLanguage = Language.CN;
            Background = "#FFfa709a";
            IsInternetRequired = false;
        } 
        public async override Task<DictionaryQueryModel> GetDictionaryQueryModelAsync(string word)
        {
            return ResultModel = await OfflineSource1Helper.QueryWordAsync(word);
        }
    }

    public class OfflineSource1Helper
    {
        public static OfflineSource1SQLHelper SQLManager = new OfflineSource1SQLHelper();
        public async static Task<DictionaryQueryModel> QueryWordAsync(string _word)
        {
            DictionaryQueryModel result = new DictionaryQueryModel();
            try
            {
                var preresult = await SQLManager.QueryWordAsync(_word);
                result.Result.Word = preresult.word;

                var preresult_mean = await SQLManager.QueryMeanAsync(preresult.ID);
                List<Pos_OfflineSource1> preresult_POS = new List<Pos_OfflineSource1>();
                foreach (var item in preresult_mean)
                {
                    var pos = await SQLManager.QueryPosAsync(item.posID);
                    var newdefinition = new Definition();
                    newdefinition.PartOfSpeech = pos.name;
                    newdefinition.Meanings.Add(item.meanings);
                    result.Result.Definitions.Add(newdefinition);
                }
            }
            catch
            {
                result.ResultStatus = ResultStatus.WordCantFind;
            }
            return result;
        }

        //public static async Task<DictionaryQueryModel> QueryWordAsync(string _word)
        //{
        //    DictionaryQueryModel t = await Task.Run(() => QueryWord(_word));
        //    return t;
        //}

        public static List<WordSuggestionModel> GetWordSuggestionModel(string _lettercontained)
        {
            List<WordSuggestionModel> result = new List<WordSuggestionModel>();
            var wordlist = SQLManager.QueryWords_OfflineSource1ForWordSuggestion(_lettercontained);
            for (int i = 0; i < wordlist.Count; i++)
            {
                var item = SQLManager.QueryMean_OfflineSource1ForWordSuggestion(wordlist[i].ID);
                result.Add(new WordSuggestionModel()
                {
                    ID = wordlist[i].ID,
                    Word = wordlist[i].word,
                    Translation = item.meanings,
                    POS = SQLManager.QueryPos_OfflineSource1ForWordSuggestion(item.posID)
                });
            }
            return result;
        }

        public static async Task<List<WordSuggestionModel>> GetWordSuggestionModelAsync(string _lettercontained)
        {
            List<WordSuggestionModel> t = await Task.Run(() => GetWordSuggestionModel(_lettercontained));
            return t;
        }
    }

    public class OfflineSource1SQLHelper : SQLiteManagerBase
    {
        public OfflineSource1SQLHelper()
        {
            InitDatabase();
            CreateTableIfNotExist<Mean_OfflineSource1>();
            CreateTableIfNotExist<Pos_OfflineSource1>();
            CreateTableIfNotExist<Words_OfflineSource1>();
        }
        public new static SQLiteConnection Connection;
        public async new static void InitDatabase()
        {
            //string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "OfflineSqliteSource1.db");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/DictionarySource/OfflineSqliteSource1.db"));
            Connection = new SQLiteConnection(new SQLitePlatformWinRT(), file.Path);

            //string path = "ms-appx:///Assets/DictionarySource/OfflineSqliteSource1.db";
            //Connection = new SQLiteConnection(new SQLitePlatformWinRT(), path);
        }

        public async new static Task InitDatabaseAsync()
        {
            await Task.Run(() => InitDatabase());
        }

        public bool IfWordExist(string _word)
        {
            TableQuery<Word> t = Connection.Table<Word>();
            ParallelQuery<Word> q = from s in t.AsParallel()
                                    where s.Spelling == _word
                                    select s;
            if (q.ToList().Count == 1) return true;
            else return false;
        }

        public async Task<Words_OfflineSource1> QueryWordAsync(string _word)
        {
            return await Task.Run(() => QueryWord(_word));
        }

        public Words_OfflineSource1 QueryWord(string _word)
        {
            TableQuery<Words_OfflineSource1> t = Connection.Table<Words_OfflineSource1>();
            ParallelQuery<Words_OfflineSource1> q = from s in t.AsParallel()
                                                    where s.word == _word
                                                    select s;
            var l = q.ToList();
            return l.Count > 0 ? l[0] : null;
        }

        public List<Words_OfflineSource1> QueryWords_OfflineSource1ForWordSuggestion(string _lettercontained)
        {
            try
            {
                string querystr =
                      "SELECT ID, word, times FROM " + "Words_OfflineSource1 " //*
                    + "WHERE substr(word,1," + _lettercontained.Count().ToString() + ") = "
                    + "'" + _lettercontained + "' COLLATE NOCASE "
                    + "ORDER BY length(word) ASC, word ASC , times DESC  "
                    + "LIMIT 10 ";
                return Connection.Query<Words_OfflineSource1>(querystr);
            }
            catch
            {
                return new List<Words_OfflineSource1>();
            }
        }

        public string QueryPos_OfflineSource1ForWordSuggestion(int _pos_id)
        {
            try
            {
                string querystr =
                      "SELECT name FROM " + "Pos_OfflineSource1 "
                    + "WHERE ID = " + _pos_id.ToString() + " "
                    + "LIMIT 1 ";
                return Connection.Query<Pos_OfflineSource1>(querystr)[0].name;
            }
            catch
            {
                return " ";
            }
        }

        public Mean_OfflineSource1 QueryMean_OfflineSource1ForWordSuggestion(int _word_id)
        {
            try
            {
                string querystr =
                      "SELECT meanings , posID  FROM " + "Mean_OfflineSource1 "
                    + "WHERE wordID = " + _word_id.ToString() + " "
                    + "LIMIT 1 ";
                return Connection.Query<Mean_OfflineSource1>(querystr)[0];
            }
            catch
            {
                return new Mean_OfflineSource1() { meanings = "", posID= -1};
            }
        }

        public List<Mean_OfflineSource1> QueryMean(int _word_id)
        {
            TableQuery<Mean_OfflineSource1> t = Connection.Table<Mean_OfflineSource1>();
            ParallelQuery<Mean_OfflineSource1> q = from s in t.AsParallel()
                                                   where s.wordID == _word_id
                                                   select s;
            return q.ToList();
        }

        public async Task<List<Mean_OfflineSource1>> QueryMeanAsync(int _word_id)
        {
            return await Task.Run(() => QueryMean(_word_id));
        }

        public Pos_OfflineSource1 QueryPos(int _pos_id)
        {
            TableQuery<Pos_OfflineSource1> t = Connection.Table<Pos_OfflineSource1>();
            ParallelQuery<Pos_OfflineSource1> q = from s in t.AsParallel()
                                                  where s.ID == _pos_id
                                                  select s;
            return q.ToList()[0];
        }

        public async Task<Pos_OfflineSource1> QueryPosAsync(int _pos_id)
        {
            return await Task.Run(() => QueryPos(_pos_id));
        }
    }
    public class Mean_OfflineSource1
    {
        [PrimaryKey, AutoIncrement]
        public int wordID { get; set; }
        public int posID { get; set; }
        public string meanings { get; set; }
    }
    public class Pos_OfflineSource1
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public string means { get; set; }
    }
    public class Words_OfflineSource1
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string word { get; set; }
        public string exchange { get; set; }
        public string voice { get; set; }
        public int times { get; set; }
    }

    public class Exchange_OfflineSource1
    {
        public string[] word_third { get; set; }
        public string[] word_done { get; set; }
        public string[] word_pl { get; set; }
        public string[] word_est { get; set; }
        public string[] word_ing { get; set; }
        public string[] word_er { get; set; }
        public string[] word_past { get; set; }
    }

    public class Voice_OfflineSource1
    {
        public string ph_en { get; set; }
        public string ph_am { get; set; }
        public string ph_en_mp3 { get; set; }
        public string ph_am_mp3 { get; set; }
        public string ph_other { get; set; }
        public string ph_tts_mp3 { get; set; }
    }

    public class WordSuggestionModel
    {
        public WordSuggestionModel()
        {

        }

        public WordSuggestionModel(int id, string _word, string _translation, string _translationmode)
        {
            ID = id;
            Word = _word;
            Translation = _translation;
            TranslationMode = _translationmode;
        }

        [PrimaryKey]
        public int ID { get; set; }
        public string Word { get; set; }
        public string POS { get; set; }
        public string Translation { get; set; }
        public string TranslationMode { get; set; }
    }



    //Word_Pl    Word_Pl   
    //Word_Past  Word_Past 
    //Word_Done  Word_Done 
    //Word_Ing   Word_Ing  
    //Word_Third Word_Third
    //Word_Er    Word_Er   
    //Word_Est   Word_Est  
}
