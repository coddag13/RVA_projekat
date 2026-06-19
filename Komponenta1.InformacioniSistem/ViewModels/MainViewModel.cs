using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
{
    public class MainViewModel : ViewModelBase
    {
        private BicikliViewModel bicikliViewModel;
        private TelemetrijaViewModel telemetrijaViewModel;
        private GrafikonViewModel grafikonViewModel;

        public MainViewModel(
            BicikliViewModel bicikliViewModel,
            TelemetrijaViewModel telemetrijaViewModel,
            GrafikonViewModel grafikonViewModel)
        {
            this.bicikliViewModel = bicikliViewModel;
            this.telemetrijaViewModel = telemetrijaViewModel;
            this.grafikonViewModel = grafikonViewModel;
        }

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
    }
}
