using Lepton_Library.Helper;
using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lepton_Dictionary.Models
{
    public class WordModel
    {
        public string Word { get; set; }
        public string DictionarySourceName { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public List<Pronounciation> Pronounciations { get; set; } = new List<Pronounciation>();
        public List<InflectionWord> InflectionWords { get; set; } = new List<InflectionWord>();
        public List<Definition> Definitions { get; set; } = new List<Definition>();
        public List<SampleSentence> SampleSentences { get; set; } = new List<SampleSentence>();
    }
    public class Pronounciation
    {
        public string PronounciationType { get; set; }
        public string PhoneticSymbol { get; set; }
        public string SoundUri { get; set; }
    }
    public class Definition
    {
        public string PartOfSpeech { get; set; }
        public List<string> Meanings { get; set; } = new List<string>();
    }
    public class InflectionWord
    {
        public string InflectionType { get; set; }
        public List<string> Words { get; set; }
    }
    public class SampleSentence
    {
        public string SourceLanguageSentence { get; set; }
        public string TargetLanguageSentence { get; set; }
    }
}
