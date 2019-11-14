using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lepton_Library.Helper;

namespace Lepton_Dictionary.Models
{
    public interface IDictionaryModel
    {
        string Name { get; set; }
        string Description { get; set; }
        Language SourceLanguage { get; set; }
        Language TargetLanguage { get; set; }
        bool IsInternetRequired { get; set; }
        bool IsAvailable { get; set; }
        string Background { get; set; }
        DictionaryType DictionaryType { get; set; }
        DictionaryQueryModel ResultModel { get; set; }
        Task Query(string word, params object[] args);
        DictionaryQueryModel GetDictionaryQueryModel(string word);
    }

    public enum DictionaryType
    {
        BuiltIn = 0,
        Package = 1,
        Extension = 2
    }
}
