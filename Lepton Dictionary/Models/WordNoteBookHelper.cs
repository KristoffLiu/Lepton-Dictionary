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

    public class WordNoteBookHelper
    {
        public static WordNoteBookSQLHelper SQLManager = new WordNoteBookSQLHelper();

        public static WordNoteBookModel Add(string _headline, string _subheadline, string _gradientback1, string _gradientback2)
        {
            var newitem = new WordNoteBookModel(_headline, _subheadline, _gradientback1, _gradientback2);
            SQLManager.Add(newitem);
            return newitem;
        }

        public static void Delete(WordNoteBookModel task)
        {
            SQLManager.Delete(task);
        }

        public static void DeleteByID(int ID)
        {
            SQLManager.Delete(SQLManager.QueryByID(ID));
        }

        public static void DeleteAll()
        {
            SQLManager.DeleteAll<WordNoteBookModel>();
        }

        public static void Update(WordNoteBookModel updatingTaskModel)
        {
            SQLManager.Update(updatingTaskModel);
        }

        public static bool Rename(int _id, string newsubheadline, string newheadline)
        {
            var item = SQLManager.QueryByID(_id);
            item.SubHeadline = newsubheadline;
            item.Headline = newheadline;
            SQLManager.Update(item);
            return true;
        }

        public static bool UpdateColor(int _id, string gradient1, string gradient2)
        {
            var item = SQLManager.QueryByID(_id);
            item.GradientBackground1 = gradient1;
            item.GradientBackground2 = gradient2;
            SQLManager.Update(item);
            return true;
        }

        public async static Task UpdateAsync(int _WordNoteBook_id, string _HeadLine, string _Subheadline)
        {
            await Task.Run(() => Rename(_WordNoteBook_id, _HeadLine, _Subheadline));
        }

        public static List<WordNoteBookModel> All()
        {
            return SQLManager.QueryAll<WordNoteBookModel>();
        }

        public static int Count()
        {
            return SQLManager.Count();
        }
    }
    public class WordNoteBookSQLHelper : SQLiteManagerBase
    {
        public WordNoteBookSQLHelper()
        {
            CreateTableIfNotExist<WordNoteBookModel>();
        }

        public List<WordNoteBookModel> Query()
        {
            TableQuery<WordNoteBookModel> t = Connection.Table<WordNoteBookModel>();
            ParallelQuery<WordNoteBookModel> q = from s in t.AsParallel()
                                                 orderby s.ID descending
                                                 select s;
            return q.ToList();
        }

        public WordNoteBookModel QueryByID(int ID)
        {
            TableQuery<WordNoteBookModel> t = Connection.Table<WordNoteBookModel>();
            ParallelQuery<WordNoteBookModel> q = from s in t.AsParallel()
                                                 where s.ID == ID
                                                 select s;
            return q.ToList()[0];
        }

        public List<WordNoteBookModel> QueryAllOrderByIDDescending()
        {
            TableQuery<WordNoteBookModel> t = Connection.Table<WordNoteBookModel>();
            ParallelQuery<WordNoteBookModel> q = from s in t.AsParallel()
                                                 orderby s.ID descending
                                                 select s;
            return q.ToList();
        }

        public int QueryMaxID()
        {
            int maxid = 0;
            TableQuery<WordNoteModel> t = Connection.Table<WordNoteModel>();
            ParallelQuery<WordNoteModel> q = from s in t.AsParallel()
                                             select s;
            var list = q.ToList();
            if (list != null)
            {
                foreach (var item in list)
                {
                    while (item.ID >= maxid)
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
        public List<WordNoteBookModel> QueryByTaskID(Guid taskID)
        {
            TableQuery<WordNoteBookModel> t = Connection.Table<WordNoteBookModel>();
            ParallelQuery<WordNoteBookModel> q = from s in t.AsParallel()
                                                 orderby s.ID descending
                                                 select s;
            return q.ToList();
        }

        public int Count()
        {
            try
            {
                TableQuery<WordNoteBookModel> t = Connection.Table<WordNoteBookModel>();
                ParallelQuery<WordNoteBookModel> q = from s in t.AsParallel()
                                                     select s;
                return q.ToList().Count;

            }
            catch
            {
                return 0;
            }
        }

    }
    public class WordNoteBookModel
    {
        public WordNoteBookModel()
        {

        }
        public WordNoteBookModel(string _headline, string _subheadline, string _gradientback1, string _gradientback2)
        {
            Headline = _headline;
            SubHeadline = _subheadline;
            GradientBackground1 = _gradientback1;
            GradientBackground2 = _gradientback2;
        }
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Headline { get; set; } = "";
        public string SubHeadline { get; set; } = "";
        public string GradientBackground1 { get; set; }
        public string GradientBackground2 { get; set; }
    }
}
