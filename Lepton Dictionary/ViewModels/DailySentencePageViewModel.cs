using Lepton_Dictionary.Models;
using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Lepton_Dictionary.ViewModels
{
    public class DailySentencePageViewModel: PageViewModelBase
    {
        public DailySentencePageViewModel()
        {
            if(DictionaryService.CheckQueryAvailability() == WebStatus.InternetAvailable)
            {
                Cover_Visibility = Visibility.Collapsed;
                UpdateDailySentenceForToday();
                UpdateDailySentenceFor();
            }
            else if(DailySentenceService.IsDailySentenceAvailableUnderMeteredInternet & DictionaryService.CheckQueryAvailability() == WebStatus.MeteredInternet)
            {
                Cover_Visibility = Visibility.Collapsed;
                UpdateDailySentenceForToday();
                UpdateDailySentenceFor();
            }
            else
            {
                Cover_Visibility = Visibility.Visible;
            }

        }

        public ObservableCollection<DailySentenceViewModel> dailySentenceViewModels = new ObservableCollection<DailySentenceViewModel>();

        BitmapImage _largepictureurl_Today = new BitmapImage();
        public BitmapImage LargePictureUrl_Today
        {
            get { return _largepictureurl_Today; }
            set { Set(ref _largepictureurl_Today, value); }
        }

        Visibility _Cover_Visibility = Visibility.Collapsed;
        public Visibility Cover_Visibility
        {
            get { return _Cover_Visibility; }
            set { Set(ref _Cover_Visibility, value); }
        }

        string _EnglishSentence_Today = "";
        public string EnglishSentence_Today
        {
            get { return _EnglishSentence_Today; }
            set { Set(ref _EnglishSentence_Today, value); }
        }

        string _ChineseSentence_Today = "";
        public string ChineseSentence_Today
        {
            get { return _ChineseSentence_Today; }
            set { Set(ref _ChineseSentence_Today, value); }
        }

        public async void UpdateDailySentenceForToday()
        {
            try
            {
                var sentencetoday = new DailySentenceViewModel(await DailySentenceService.RequestByTodayAsync());
                LargePictureUrl_Today = sentencetoday.LargePicture;
                EnglishSentence_Today = sentencetoday.content;
                ChineseSentence_Today = sentencetoday.note;
            }
            catch
            {

            }
        }
        public async void UpdateDailySentenceFor()
        {
            try
            {
                for (int i = 0; i <= 5; i++)
                {
                    var datetime = DateTime.Today.AddDays(-i);
                    dailySentenceViewModels.Add(new DailySentenceViewModel(await DailySentenceService.RequestByDateTimeAsync(datetime)));
                }
            }
            catch
            {

            }

        }
    }

    public class DailySentenceViewModel : ViewModelBase
    {
        public DailySentenceViewModel()
        {
            
        }

        public DailySentenceViewModel(DailySentenceModel model)
        {
            this.index = model.sid;
            this.mp3 = model.tts;
            this.content = model.content;
            this.note = model.note;
            this.translation = model.translation.Substring(5);
            this.LargePicture = new BitmapImage(new Uri(model.picture2))
            {
                DecodePixelHeight = 300,
                DecodePixelWidth = 450
            };
            this.caption = model.caption;
            this.dateline = model.dateline;
        }

        int _index = 0;
        public int index
        {
            get { return _index; }
            set { Set(ref _index, value); }
        }

        string _mp3 = "";
        public string mp3
        {
            get { return _mp3; }
            set { Set(ref _mp3, value); }
        }

        string _content = "";
        public string content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        string _note = "";
        public string note
        {
            get { return _note; }
            set { Set(ref _note, value); }
        }

        string _translation = "";
        public string translation
        {
            get { return _translation; }
            set { Set(ref _translation, value); }
        }

        BitmapImage _largePicture = new BitmapImage();
        public BitmapImage LargePicture
        {
            get { return _largePicture; }
            set { Set(ref _largePicture, value); }
        }

        string _caption = "";
        public string caption
        {
            get { return _caption; }
            set { Set(ref _caption, value); }
        }

        string _dateline = "";
        public string dateline
        {
            get { return _dateline; }
            set { Set(ref _dateline, value); }
        }
    }

    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string _str = (string)value;
            int _monthvalue = System.Convert.ToInt32( _str[5].ToString() + _str[6].ToString());
            string[] _monthlist = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };
            return _monthlist[_monthvalue - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string _str = (string)value;
            return _str[8].ToString() + _str[9].ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
