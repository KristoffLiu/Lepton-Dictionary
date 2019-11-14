using Lepton_Library.Common;
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
    public class WordHistoryHelper
    {
        public static WordHistorySQLHelper SQLManager = new WordHistorySQLHelper();

        public static WordHistoryModel Add(WordHistoryModel wordHistoryModel)
        {
            wordHistoryModel.ID = SQLManager.QueryMaxID();
            SQLManager.Add(wordHistoryModel);
            return wordHistoryModel;
        }
        
        public static void Delete(WordHistoryModel task)
        {
            SQLManager.Delete(task);
        }

        public static void DeleteByID(int ID)
        {
            SQLManager.Delete(SQLManager.QueryByID(ID));            
        }

        public static void DeleteAll()
        {
            SQLManager.DeleteAll<WordHistoryModel>();
        }

        public static void Update(WordHistoryModel updatingTaskModel)
        {
            SQLManager.Update(updatingTaskModel);
        }

        public static List<WordHistoryModel> QueryAll()
        {
            return SQLManager.QueryAllOrderByIDDescending();
        }

        public static bool IsQueryHistoryAllowed
        {
            get { return LocalSetting.GetValueOrDefault("IsQueryHistoryAllowed", true); }
            set { LocalSetting.AddOrUpdateValue("IsQueryHistoryAllowed", value); }
        }

    }


    public class WordHistorySQLHelper : SQLiteManagerBase
    {
        public WordHistorySQLHelper()
        {
            CreateTableIfNotExist<WordHistoryModel>();
        }

        public List<WordHistoryModel> Query()
        {
            TableQuery<WordHistoryModel> t = Connection.Table<WordHistoryModel>();
            ParallelQuery<WordHistoryModel> q = from s in t.AsParallel()
                                                orderby s.ID descending
                                                select s;
            return q.ToList();
        }

        public WordHistoryModel QueryByID(int ID)
        {
            TableQuery<WordHistoryModel> t = Connection.Table<WordHistoryModel>();
            ParallelQuery<WordHistoryModel> q = from s in t.AsParallel()
                                                where s.ID == ID
                                                select s;
            var l = q.ToList();
            return l.Count > 0 ? l[0] : null;
        }

        public List<WordHistoryModel> QueryAllOrderByIDDescending()
        {
            TableQuery<WordHistoryModel> t = Connection.Table<WordHistoryModel>();
            ParallelQuery<WordHistoryModel> q = from s in t.AsParallel()
                                                orderby s.ID descending
                                                select s;
            return q.ToList();
        }

        public int QueryMaxID()
        {
            int maxid = 0;
            TableQuery<WordHistoryModel> t = Connection.Table<WordHistoryModel>();
            ParallelQuery<WordHistoryModel> q = from s in t.AsParallel()
                                                select s;
            var list = q.ToList();
            if (list != null)
            {
                foreach (var item in list)
                {
                    while(item.ID >= maxid)
                    {
                        maxid++;
                    }
                }
                return maxid;
            }
            else
            {
                return 0;
            }
        }



        public List<WordHistoryModel> QueryByTaskID(Guid taskID)
        {
            TableQuery<WordHistoryModel> t = Connection.Table<WordHistoryModel>();
            ParallelQuery<WordHistoryModel> q = from s in t.AsParallel()
                                                orderby s.ID descending
                                                select s;
            return q.ToList();
        }

    }



    public class WordHistoryModel
    {
        public WordHistoryModel()
        {

        }

        public WordHistoryModel(int id, string _word, string _translation, string _translationmode, int _sourcelanguage, int _targetlanguage)
        {
            ID = id;
            Word = _word;
            SourceLanguage = _sourcelanguage;
            TargetLanguage = _targetlanguage;
        }

        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string Word { get; set; }
        public int SourceLanguage { get; set; }
        public int TargetLanguage { get; set; }
    }
}
