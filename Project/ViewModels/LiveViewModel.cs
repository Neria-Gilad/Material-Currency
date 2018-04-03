using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DataProtocol;
using CrystalJobScheduler;

namespace Project.ViewModels {
    public class LiveViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { }; //so no need to null check

        private List<CurrencyFields> _liveList { get; set; }
        private JobScheduler _jobScheduler { get; set; }
        private CurrencyFields _selectedCurrency { get; set; }

        public LiveViewModel()
        {
            Task.Factory.StartNew(Initialize);
        }

        /// <summary>
        /// necessary initializations for smoothness and such
        /// </summary>
        private void Initialize()
        {
            UpdateLiveViewAsync();

            _jobScheduler = new JobScheduler(
                TimeSpan.FromMinutes(1),
                UpdateLiveViewAsync);

            _jobScheduler.Start();
        }

        public List<CurrencyFields> LiveList {
            get => _liveList;
            set {
                if (value == _liveList) return;
                _liveList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LiveList"));
            }
        }

        public CurrencyFields SelectedCurrency {
            get => _selectedCurrency;
            set {
                if (value == null || value == _selectedCurrency) return;
                _selectedCurrency = value;
                Task.Factory.StartNew(UpdateLiveViewAsync);
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }

        /// <summary>
        /// update with live info about the currencies
        /// </summary>
        private async void UpdateLiveViewAsync()
        {
            if (LiveList != null) LiveList = null;
            Thread.Sleep(1500);//literally so user will think that something is happening
            List<CurrencyFields> tempLiveList = new List<CurrencyFields>();
            try
            {
                tempLiveList = (await new Models.LiveModel().GetLiveListAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            tempLiveList.RemoveAll(c => c.Code == "USD");
            LiveList = tempLiveList;
        }
    }
}
