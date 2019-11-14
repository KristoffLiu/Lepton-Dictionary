using Lepton_Library.Helper;
using Lepton_Library.Storage;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static Lepton_Dictionary.Models.DictionaryService;

namespace Lepton_Dictionary.Models
{
    public class WordStorageHelper
    {
        public static WordSQLHelper SQLManager = new WordSQLHelper();
        public static WordsStorageModel Add(WordsStorageModel _entry)
        {
            if (!SQLManager.IfWordExist(_entry.Word))
            {
                Word _word = new Word()
                {
                    Spelling = _entry.Word
                };
                SQLManager.Add(_word);
            }
            if (!SQLManager.IfEntryExist(_entry.Word, _entry.DictionarySource_ID))
            {
                SQLManager.Add(_entry);
                int _entry_id = SQLManager.FindEntryID(_entry.Word, _entry.DictionarySource_ID);
                foreach (var item in _entry.word_pl     )
                {
                    PartOfSpeechStorage partofspeech = new PartOfSpeechStorage()
                    {
                        Entry_ID = _entry_id,
                        Part = "word_pl",
                        Word = item
                    };
                    SQLManager.Add(partofspeech);
                }
                foreach (var item in _entry.word_past       )
                {
                    PartOfSpeechStorage partofspeech = new PartOfSpeechStorage()
                    {
                        Entry_ID = _entry_id,
                        Part = "word_past",
                        Word = item
                    };
                    SQLManager.Add(partofspeech);
                }
                foreach (var item in _entry.word_done   )
                {
                    PartOfSpeechStorage partofspeech = new PartOfSpeechStorage()
                    {
                        Entry_ID = _entry_id,
                        Part = "word_done",
                        Word = item
                    };
                    SQLManager.Add(partofspeech);
                }
                foreach (var item in _entry.word_ing    )
                {
                    PartOfSpeechStorage partofspeech = new PartOfSpeechStorage()
                    {
                        Entry_ID = _entry_id,
                        Part = "word_ing",
                        Word = item
                    };
                    SQLManager.Add(partofspeech);
                }
                foreach (var item in _entry.word_third)
                {
                    PartOfSpeechStorage partofspeech = new PartOfSpeechStorage()
                    {
                        Entry_ID = _entry_id,
                        Part = "word_third",
                        Word = item
                    };
                    SQLManager.Add(partofspeech);
                }
                foreach (var item in _entry.word_er     )
                {
                    PartOfSpeechStorage partofspeech = new PartOfSpeechStorage()
                    {
                        Entry_ID = _entry_id,
                        Part = "word_er",
                        Word = item
                    };
                    SQLManager.Add(partofspeech);
                }
                foreach (var item in _entry.word_est    )
                {
                    PartOfSpeechStorage partofspeech = new PartOfSpeechStorage()
                    {
                        Entry_ID = _entry_id,
                        Part = "word_est",
                        Word = item
                    };
                    SQLManager.Add(partofspeech);
                }
                foreach (var _definition in _entry.Definitions)
                {
                    _definition.ID = Guid.NewGuid();
                    _definition.Entry_ID = _entry_id;
                    SQLManager.Add(_definition);
                    foreach (var _mean in _definition.means)
                    {
                        var newMean = new MeanStorage()
                        {
                            Definition_ID = _definition.ID,
                            Meaning = _mean
                        };
                        _definition.Means.Add(newMean);
                        SQLManager.Add(newMean);
                    }
                }
            }
            return _entry;
        }
        public static WordsStorageModel Add(AiCiBaE2C_Model model)
        {
            return Add(GetWordModelFromAiCiBa(model));
        }

        public static bool IsEntryExist(string word,int DictionarySource_ID)
        {
            bool result = false;
            if (SQLManager.IfEntryExist(word, DictionarySource_ID))
            {
                result = true;
            }
            return result;
        }
        public static WordsStorageModel QueryEntry(string _word, int _dictionarysource_id)
        {
            WordsStorageModel result = SQLManager.QueryEntry(_word, _dictionarysource_id);
            result.Definitions = SQLManager.QueryDefinitions(result.ID);
            return result;
        }

        public static WordsStorageModel QueryWord(string _word, int _dictionarysource_id)
        {
            WordsStorageModel result = QueryEntry(_word,_dictionarysource_id);
            foreach (var _partofspeech in SQLManager.QueryPartOfSpeech(result.ID))
            {
                switch (_partofspeech.Part)
                {
                    case "word_pl":
                        result.word_pl.Add(_partofspeech.Word);
                        break;
                    case "word_past":
                        result.word_past.Add(_partofspeech.Word);
                        break;
                    case "word_done":
                        result.word_done.Add(_partofspeech.Word);
                        break;
                    case "word_ing":
                        result.word_ing.Add(_partofspeech.Word);
                        break;
                    case "word_third":
                        result.word_third.Add(_partofspeech.Word);
                        break;
                    case "word_er":
                        result.word_er.Add(_partofspeech.Word);
                        break;
                    case "word_est":
                        result.word_est.Add(_partofspeech.Word);
                        break;
                }
            }
            result.Definitions = SQLManager.QueryDefinitions(result.ID);
            foreach(var _definition in result.Definitions)
            {
                _definition.Means = SQLManager.QueryMeans(_definition.ID);
                foreach (var _mean in _definition.Means)
                {
                    _definition.means.Add(_mean.Meaning);
                }
            }

            return result;
        }

        public static WordsStorageModel FindByWord(int _word_id, int _dictionarysource_id)
        {
            WordsStorageModel result = SQLManager.QueryEntry(_word_id, _dictionarysource_id);
            foreach (var _partofspeech in SQLManager.QueryPartOfSpeech(result.ID))
            {
                switch (_partofspeech.Part)
                {
                    case "word_pl":
                        result.word_pl.Add(_partofspeech.Word);
                        break;
                    case "word_past":
                        result.word_past.Add(_partofspeech.Word);
                        break;
                    case "word_done":
                        result.word_done.Add(_partofspeech.Word);
                        break;
                    case "word_ing":
                        result.word_ing.Add(_partofspeech.Word);
                        break;
                    case "word_third":
                        result.word_third.Add(_partofspeech.Word);
                        break;
                    case "word_er":
                        result.word_er.Add(_partofspeech.Word);
                        break;
                    case "word_est":
                        result.word_est.Add(_partofspeech.Word);
                        break;
                }
            }
            result.Definitions = SQLManager.QueryDefinitions(result.ID);
            foreach (var _definition in result.Definitions)
            {
                _definition.Means = SQLManager.QueryMeans(_definition.ID);
                foreach (var _mean in _definition.Means)
                {
                    _definition.means.Add(_mean.Meaning);
                }
            }
            return result;
        }

        public static WordsStorageModel GetWordModelFromAiCiBa(AiCiBaE2C_Model _aimodel)
        {
            WordsStorageModel result = new WordsStorageModel();
            result.Word = _aimodel.word_name;
            result.word_pl = ListHelper.GetListFromArray(_aimodel.exchange.word_pl);
            result.word_past = ListHelper.GetListFromArray(_aimodel.exchange.word_past);
            result.word_done = ListHelper.GetListFromArray(_aimodel.exchange.word_done);
            result.word_ing = ListHelper.GetListFromArray(_aimodel.exchange.word_ing);
            result.word_third = ListHelper.GetListFromArray(_aimodel.exchange.word_third);
            result.word_er = ListHelper.GetListFromArray(_aimodel.exchange.word_er);
            result.word_est = ListHelper.GetListFromArray(_aimodel.exchange.word_est);
            result.ph_en = _aimodel.symbols[0].ph_en;
            result.ph_am = _aimodel.symbols[0].ph_am;
            result.ph_other = _aimodel.symbols[0].ph_other;
            result.ph_en_mp3 = _aimodel.symbols[0].ph_en_mp3;
            result.ph_am_mp3 = _aimodel.symbols[0].ph_am_mp3;
            result.ph_tts_mp3 = _aimodel.symbols[0].ph_tts_mp3;
            foreach (var definition in _aimodel.symbols[0].parts.ToList())
            {
                result.Definitions.Add(new DefinitionStroage()
                {
                    part = definition.part,
                    means = definition.means.ToList()
                });
            }
            return result;
        }

        public static void DeleteAll()
        {
            SQLManager.DeleteAll<Word>();
            SQLManager.DeleteAll<DictionarySource>();
            SQLManager.DeleteAll<WordsStorageModel>();//也就是原来的Entry
            SQLManager.DeleteAll<PartOfSpeechStorage>();
            SQLManager.DeleteAll<DefinitionStroage>();
            SQLManager.DeleteAll<MeanStorage>();
        }

        public static int Count()
        {
            return SQLManager.Count();
        }

    }

    public class WordSQLHelper : SQLiteManagerBase
    {
        public WordSQLHelper()
        {
            CreateTableIfNotExist<Word>();
            CreateTableIfNotExist<DictionarySource>();
            CreateTableIfNotExist<WordsStorageModel>();//也就是原来的Entry
            CreateTableIfNotExist<PartOfSpeechStorage>();
            CreateTableIfNotExist<DefinitionStroage>();
            CreateTableIfNotExist<MeanStorage>();
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

        public int FindWordID(string _word)
        {
            TableQuery<Word> t = Connection.Table<Word>();
            ParallelQuery<Word> q = from s in t.AsParallel()
                                    where s.Spelling == _word
                                    select s;
            return q.ToList()[0].ID;
        }

        public bool IfEntryExist(string _word, int _dictionarysource_id)
        {
            TableQuery<WordsStorageModel> t = Connection.Table<WordsStorageModel>();
            ParallelQuery<WordsStorageModel> q = from s in t.AsParallel()
                                         where s.Word == _word & s.DictionarySource_ID == _dictionarysource_id
                                         select s;
            if (q.ToList().Count == 1) return true;
            else return false;
        }

        public int FindEntryID(string _word , int _dictionarysource_id)
        {
            TableQuery<WordsStorageModel> t = Connection.Table<WordsStorageModel>();
            ParallelQuery<WordsStorageModel> q = from s in t.AsParallel()
                                     where s.Word == _word & s.DictionarySource_ID == _dictionarysource_id
                                     select s;
            return q.ToList()[0].ID;
        }

        //public Guid Find_Definition_ID(int _entry_id)
        //{
        //    TableQuery<Definition> t = Connection.Table<Definition>();
        //    ParallelQuery<Definition> q = from s in t.AsParallel()
        //                                  where s.Entry_ID == _entry_id
        //                                  select s;
        //    return q.ToList()[0].ID;
        //}

        public WordsStorageModel QueryEntry(string _word, int _dictionarysource_id)
        {
            TableQuery<WordsStorageModel> t = Connection.Table<WordsStorageModel>();
            ParallelQuery<WordsStorageModel> q = from s in t.AsParallel()
                                     where s.Word == _word & s.DictionarySource_ID == _dictionarysource_id
                                     select s;
            return q.ToList()[0];
        }
        public WordsStorageModel QueryEntry(int _word_id, int _dictionarysource_id)
        {
            TableQuery<WordsStorageModel> t = Connection.Table<WordsStorageModel>();
            ParallelQuery<WordsStorageModel> q = from s in t.AsParallel()
                                         where s.Word_ID == _word_id & s.DictionarySource_ID == _dictionarysource_id
                                         select s;
            return q.ToList()[0];
        }
        public List<PartOfSpeechStorage> QueryPartOfSpeech(int _entry_id)
        {
            TableQuery<PartOfSpeechStorage> t = Connection.Table<PartOfSpeechStorage>();
            ParallelQuery<PartOfSpeechStorage> q = from s in t.AsParallel()
                                            where s.Entry_ID == _entry_id
                                            select s;
            return q.ToList();
        }

        public List<DefinitionStroage> QueryDefinitions(int _entryid)
        {
            TableQuery<DefinitionStroage> t = Connection.Table<DefinitionStroage>();
            ParallelQuery<DefinitionStroage> q = from s in t.AsParallel()
                                          where s.Entry_ID == _entryid
                                          select s;
            return q.ToList();
        }
        public List<MeanStorage> QueryMeans(Guid _definition_id)
        {
            TableQuery<MeanStorage> t = Connection.Table<MeanStorage>();
            ParallelQuery<MeanStorage> q = from s in t.AsParallel()
                                    where s.Definition_ID == _definition_id
                                    select s;
            return q.ToList();
        }

        public int Count()
        {
            try
            {
                TableQuery<WordsStorageModel> t = Connection.Table<WordsStorageModel>();
                ParallelQuery<WordsStorageModel> q = from s in t.AsParallel()
                                        select s;
                return q.ToList().Count;

            }
            catch
            {
                return 0;
            }
        }
    }

    public class Word
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string Spelling { get; set; }

        public static string String_WordModelHandler(List<string> _modelText)
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
    public class DictionarySource
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class WordsStorageModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Word { get; set; }
        public int Word_ID { get; set; }
        public int DictionarySource_ID { get; set; }
        public string ph_en { get; set; }
        public string ph_am { get; set; }
        public string ph_other { get; set; }
        public string ph_en_mp3 { get; set; }
        public string ph_am_mp3 { get; set; }
        public string ph_tts_mp3 { get; set; }
        [Ignore]
        public List<string> word_pl { get; set; } = new List<string>();
        [Ignore]
        public List<string> word_past { get; set; } = new List<string>();
        [Ignore]
        public List<string> word_done { get; set; } = new List<string>();
        [Ignore]
        public List<string> word_ing { get; set; } = new List<string>();
        [Ignore]
        public List<string> word_third { get; set; } = new List<string>();
        [Ignore]
        public List<string> word_er { get; set; } = new List<string>();
        [Ignore]
        public List<string> word_est { get; set; } = new List<string>();
        [Ignore]
        public List<DefinitionStroage> Definitions { get; set; } = new List<DefinitionStroage>();
    }
    public class DefinitionStroage
    {
        [PrimaryKey]
        public Guid ID { get; set; }
        public int Entry_ID { get; set; }
        public string part { get; set; }
        [Ignore]
        public List<MeanStorage> Means { get; set; } = new List<MeanStorage>();
        [Ignore]
        public List<string> means { get; set; } = new List<string>();
    }
    public class MeanStorage
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public Guid Definition_ID { get; set; }
        public string Meaning { get; set; }
    }
    public class PartOfSpeechStorage
    {
        public int Entry_ID { get; set; }
        public string Part { get; set; }
        public string Word { get; set; }
    }


}
