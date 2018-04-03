using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DataProtocol;
using Project.Commands;

namespace Project.ViewModels {
    class ConversionViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged  = (sender, args) => { };//so no need to null check

        private CurrencyFields _selectedCurrency { get; set; }

        private List<CurrencyFields> _convertedList { get; set; }

        private double _convertAmount { get; set; } = 1;

        private ICommand _updateConversions { get; set; }

        public ConversionViewModel()
        {
            Task.Factory.StartNew(Initialize);
        }

        /// <summary>
        /// necessary initializations for smoothness and such
        /// </summary>
        private void Initialize()
        {
            UpdateConversions = new ButtonCommand(ConvertAll, () => _convertAmount > 0);//only invoke if amount is positive
            SelectedCurrency = new CurrencyFields { //default
                Code = "USD",
                Value = 1
            };
        }

        /// <summary>
        /// update converted list with current currency selection and multiplier
        /// </summary>
        private async void ConvertAll()
        {
            ConvertedList = await new Models.ConversionModel().ConvertCurrencyListAsync(SelectedCurrency.Code, ConvertAmount);
        }

       public ICommand UpdateConversions {
            get => _updateConversions;

            set {
                if (_updateConversions == value) return; 
                _updateConversions = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UpdateConversions"));
            }
        }

        public CurrencyFields SelectedCurrency {
            get => _selectedCurrency;
            set {
                if (value == null || value == _selectedCurrency) return; 
                _selectedCurrency = value;
                if (UpdateConversions != null)
                    if (UpdateConversions.CanExecute(null))
                        UpdateConversions.Execute(null);
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }

        public string ConvertAmount {//string because we can't trust the view to know how to convert double
            get => _convertAmount.ToString();
            set {
                if (value == _convertAmount.ToString()) return;
                _convertAmount = Convert.ToDouble(value);
                PropertyChanged(this, new PropertyChangedEventArgs("convertAmount"));
            }
        }

        public List<CurrencyFields> ConvertedList {
            get => _convertedList;
            set {
                if (_convertedList == value) return; 
                _convertedList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("convertedList"));
            }
        }

    }
}
