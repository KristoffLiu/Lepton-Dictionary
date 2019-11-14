using Lepton_Library.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lepton_Dictionary.Models
{
    public class DailySentenceService
    {
        public DailySentenceService()
        {
            
        }

        public static DailySentenceModel RequestByToday()
        {
            return DailySentenceModel.RequestFromWeb(DateTime.Now);
        }
        public static async Task<DailySentenceModel> RequestByTodayAsync()
        {
            return await DailySentenceModel.RequestFromWebAsync(DateTime.Now);
        }

        public static DailySentenceModel RequestByDateTime(DateTime dateTime)
        {
            return DailySentenceModel.RequestFromWeb(dateTime);
        }

        public static async Task<DailySentenceModel> RequestByDateTimeAsync(DateTime dateTime)
        {
            return await DailySentenceModel.RequestFromWebAsync(dateTime);
        }

        public static DailySentenceModel LoadFromLocal(string jsonstr)
        {
            return Json.Deserialize<DailySentenceModel>(jsonstr);
        }
        public static bool IsDailySentenceAvailableUnderMeteredInternet
        {
            get { return LocalSetting.GetValueOrDefault("IsFirstInitialized", true); }
            set { LocalSetting.AddOrUpdateValue("IsFirstInitialized", value); }
        }

    }

    public class DailySentenceModel
    {
        public DailySentenceModel()
        {

        }

        public int sid { get; set; }
        public string tts { get; set; }
        public string content { get; set; }
        public string note { get; set; }
        public int love { get; set; }
        public string translation { get; set; }
        public string picture { get; set; }
        public string picture2 { get; set; }
        public string caption { get; set; }
        public string dateline { get; set; }
        public int s_pv { get; set; }
        public int sp_pv { get; set; }
        //public tags[] tags { get; set; }
        public string fenxiang_img { get; set; }

        public static async Task<DailySentenceModel> RequestFromWebAsync(DateTime dateTime)
        {
            DailySentenceModel t = await Task.Run(() => RequestFromWeb(dateTime));
            return t;
        }

        public static DailySentenceModel RequestFromWeb(DateTime dateTime)
        {
            try
            {
                DailySentenceModel result = null;
                string FormatDateTimeString = dateTime.ToString("yyyy-MM-dd");
                WebRequest webRequest = HttpWebRequest.Create("http://open.iciba.com/dsapi/?date=" + FormatDateTimeString);
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                var jsonstring = sr.ReadToEnd();
                try
                {
                    result = Json.Deserialize<DailySentenceModel>(jsonstring);
                }
                catch
                {
                    //HandleException(DictionaryServiceException.Unsupport_Sentence, jsonstring);
                }
                return result;
            }
            catch (Exception)
            {
                return new DailySentenceModel();
            }
        }
    }

    public class tags
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
