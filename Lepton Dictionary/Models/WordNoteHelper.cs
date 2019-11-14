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
    public class WordNoteHelper
    {
        public static WordNoteSQLHelper SQLManager = new WordNoteSQLHelper();

        public static WordNoteModel Add(string _word, int _wordnotebookid,string sourcelanguage, string targetlanguage)
        {
            var newitem = new WordNoteModel()
            {
                Word = _word,
                WordNoteBookID = _wordnotebookid,
                SourceLanguage = sourcelanguage,
                TargetLanguage = targetlanguage
            };
            SQLManager.Add(newitem);
            return newitem;
        }

        public static bool Delete(int _id)
        {
            SQLManager.Delete(SQLManager.QueryByID(_id));
            return true;
        }

        public static void Delete(string _word, int _notebookid)
        {
            SQLManager.Delete(SQLManager.Query(_word, _notebookid));
        }

        public static void DeleteAll()
        {
            SQLManager.DeleteAll<WordNoteModel>();
        }

        public static void Update(WordNoteModel updatingTaskModel)
        {
            SQLManager.Update(updatingTaskModel);
        }

        public static List<WordNoteModel> All()
        {
            return SQLManager.QueryAll<WordNoteModel>();
        }

        public static List<WordNoteModel> QueryByNoteBookID(int _wordNoteBookID)
        {
            return SQLManager.QueryByNoteBookID(_wordNoteBookID);
        }

        public static bool IsWordSaved(string _word, int NoteBookID)
        {
            return SQLManager.IsWordSaved(_word, NoteBookID);
        }
    }


    public class WordNoteSQLHelper : SQLiteManagerBase
    {
        public WordNoteSQLHelper()
        {
            CreateTableIfNotExist<WordNoteModel>();
        }

        public List<WordNoteModel> Query()
        {
            TableQuery<WordNoteModel> t = Connection.Table<WordNoteModel>();
            ParallelQuery<WordNoteModel> q = from s in t.AsParallel()
                                                orderby s.ID descending
                                                select s;
            return q.ToList();
        }

        public List<WordNoteModel> QueryByNoteBookID(int _wordNoteBookID)
        {
            TableQuery<WordNoteModel> t = Connection.Table<WordNoteModel>();
            ParallelQuery<WordNoteModel> q = from s in t.AsParallel()
                                             where s.WordNoteBookID == _wordNoteBookID
                                             orderby s.OrderedIndex ascending
                                             select s;
            return q.ToList();
        }

        public bool IsWordSaved(string _word, int NoteBookID)
        {
            TableQuery<WordNoteModel> t = Connection.Table<WordNoteModel>();
            ParallelQuery<WordNoteModel> q = from s in t.AsParallel()
                                             where s.Word == _word & s.WordNoteBookID == NoteBookID
                                             select s;
            if (q.ToList().Count == 1) return true;
            else return false;
        }

        public WordNoteModel QueryByID(int ID)
        {
            TableQuery<WordNoteModel> t = Connection.Table<WordNoteModel>();
            ParallelQuery<WordNoteModel> q = from s in t.AsParallel()
                                                 where s.ID == ID
                                                 select s;
            return q.ToList()[0];
        }

        public WordNoteModel Query(string _word, int _notebookid)
        {
            TableQuery<WordNoteModel> t = Connection.Table<WordNoteModel>();
            ParallelQuery<WordNoteModel> q = from s in t.AsParallel()
                                             where s.Word == _word & s.WordNoteBookID == _notebookid
                                             select s;
            return q.ToList()[0];
        }
    }
    public class WordNoteModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Word { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public int WordNoteBookID { get; set; }
        public int OrderedIndex { get; set; }
    }

}
