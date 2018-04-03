using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DataProtocol;
using LiveCharts;
using LiveCharts.Defaults;

namespace Project.ViewModels {
    public class GraphViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { }; //so no need to null check

        private ChartValues<DateTimePoint> _viewList { get; set; }
        private List<CurrencyFields> _optionList { get; set; }
        private CurrencyFields _selectedOption { get; set; }
        private List<string> _spanOptions { get; set; }
        private int _selectedSpan { get; set; }

        private Func<double, string> _xAxis { get; set; }
        private Func<double, string> _yAxis { get; set; }


        public GraphViewModel()
        {
            Task.Factory.StartNew(Initialize);  
        }

        /// <summary>
        /// necessary initializations for smoothness and such
        /// </summary>
        private void Initialize()
        {
            UpdateOptionListAsync();
            SpanOptions = new List<string>() { "Week", "Month", "Year" };
            SelectedSpan = 365;
            YAxis = val => val.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }

        /// <summary>
        /// get list of possible currencies
        /// </summary>
        private async void UpdateOptionListAsync()
        {
            var tempOptionList = await new Models.GraphModel().GetAllCurrencies();
            tempOptionList.RemoveAll(c => c.Code == "USD"); //the graph will always be 1 for USD

            OptionList = tempOptionList;
        }

        public List<CurrencyFields> OptionList {
            get => _optionList;
            set {
                if (value == _optionList) return; 
                _optionList = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(OptionList)));
            }
        }

        public CurrencyFields SelectedOption {
            get => _selectedOption;
            set {
                if (value == null || value == _selectedOption) return;
                _selectedOption = value;
                UpdateViewListAsync(); //update the graph
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOption"));
            }
        }

        /// <summary>
        /// get list of currency values for selected timeframe
        /// </summary>
        private async void UpdateViewListAsync()
        {
            if(SelectedSpan == 7)
                XAxis = val => new DateTime((long)val).DayOfWeek.ToString();//change format to days
            else 
            XAxis = val => new DateTime((long)val).ToString("dd MMM yyyy");//change format to date
            ViewList = ConvertHistoryToPoint(await new Models.GraphModel().GetRelevanList(SelectedSpan));
        }

        public Func<double, string> XAxis {
            get => _xAxis;
            set {
                if (_xAxis == value) return; 
                _xAxis = value;
                PropertyChanged(this, new PropertyChangedEventArgs("XAxis"));
            }
        }

        public Func<double, string> YAxis {
            get => _yAxis;
            set {
                if (_yAxis == value) return; 
                _yAxis = value;
                PropertyChanged(this, new PropertyChangedEventArgs("YAxis"));
            }
        }

        public List<String> SpanOptions {
            get => _spanOptions;
            set {
                if (value == null) return;
                _spanOptions = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SpanOptions"));
            }
        }

        public int SelectedSpan {
            get => _selectedSpan;
            set {
                if (value == _selectedSpan) return; 
                _selectedSpan = value;
                if(SelectedOption != null) UpdateViewListAsync();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedSpan"));
            }
        }

        public ChartValues<DateTimePoint> ViewList {
            get => _viewList;
            set {
                if (value == _viewList) return; 
                _viewList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ViewList"));
            }
        }

        /// <summary>
        /// convert values to form that is readable by LiveCharts
        /// </summary>
        /// <param name="history">what to convert</param>
        /// <returns>parsed value</returns>
        private ChartValues<DateTimePoint> ConvertHistoryToPoint(IEnumerable<HistoricalCurrencyEntity> history)
        {
            var ret = new ChartValues<DateTimePoint>();
            var lst = from h in history
                      select new DateTimePoint() {
                          DateTime = DateTime.Parse(h.date),
                          Value = 1 / h.GetValue(SelectedOption.Code)
                      }; //so graph will show value by USD
            ret.AddRange(lst);
            return ret;
        }

    }
}
