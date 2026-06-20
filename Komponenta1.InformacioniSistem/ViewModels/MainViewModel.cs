using System;
using System.Threading.Tasks;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace Komponenta1.InformacioniSistem
{
    public class MainViewModel : ViewModelBase
    {
        private BicikliViewModel bicikliViewModel;
        private TelemetrijaViewModel telemetrijaViewModel;
        private GrafikonViewModel grafikonViewModel;
        private readonly IDataStorage dataStorage;
        private readonly DataStore dataStore;

        public MainViewModel(
            BicikliViewModel bicikliViewModel,
            TelemetrijaViewModel telemetrijaViewModel,
            GrafikonViewModel grafikonViewModel,
            IDataStorage dataStorage,
            DataStore dataStore)
        {
            this.bicikliViewModel = bicikliViewModel;
            this.telemetrijaViewModel = telemetrijaViewModel;
            this.grafikonViewModel = grafikonViewModel;
            this.dataStorage = dataStorage;
            this.dataStore = dataStore;

            SacuvajCommand = new RelayCommand(_ => SacuvajPodatke());
            this.bicikliViewModel.BicikliPromijenjeni += OnBicikliPromijenjeni;
            this.telemetrijaViewModel.TelemetrijaPromijenjena += OnTelemetrijaPromijenjena;
        }

        public ICommand SacuvajCommand { get; set; }

        public BicikliViewModel BicikliVM
        {
            get { return bicikliViewModel; }
            set
            {
                bicikliViewModel = value;
                OnPropertyChanged();
            }
        }

        public TelemetrijaViewModel TelemetrijaVM
        {
            get { return telemetrijaViewModel; }
            set
            {
                telemetrijaViewModel = value;
                OnPropertyChanged();
            }
        }

        public GrafikonViewModel GrafikonVM
        {
            get { return grafikonViewModel; }
            set
            {
                grafikonViewModel = value;
                OnPropertyChanged();
            }
        }

        public void RefreshGrafikon()
        {
            GrafikonVM.Refresh();
        }

        public void SacuvajPodatke()
        {
            dataStorage.Save(dataStore);
            RefreshGrafikon();
            ShowToast("Podaci su sacuvani.");
        }

        private void OnTelemetrijaPromijenjena()
        {
            RefreshGrafikon();
            ShowToast("Grafikon je azuriran.");
        }

        private void OnBicikliPromijenjeni()
        {
            TelemetrijaVM.LoadBicikli();
            ShowToast("Lista bicikala je azurirana.");
        }

        private string toastMessage;
        private bool isToastVisible;

        public string ToastMessage
        {
            get { return toastMessage; }
            set
            {
                toastMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsToastVisible
        {
            get { return isToastVisible; }
            set
            {
                isToastVisible = value;
                OnPropertyChanged();
            }
        }

        public async void ShowToast(string message)
        {
            ToastMessage = message;
            IsToastVisible = true;
            await Task.Delay(3000);
            IsToastVisible = false;
        }
    }
}
