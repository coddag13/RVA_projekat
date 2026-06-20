using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;
using Komponenta2.Statistika.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Komponenta2.Statistika.ViewModels
{
    public class StatistikaViewModel : ViewModelBase
    {
        private readonly IBiciklStatistikaAdapter adapter;

        private ObservableCollection<BiciklStatistika> bicikliStatistike;
        public ObservableCollection<BiciklStatistika> BicikliStatistike
        {
            get { return bicikliStatistike; }
            set
            {
                bicikliStatistike = value;
                OnPropertyChanged();
            }
        }

        private string statusPoruka;
        public string StatusPoruka
        {
            get { return statusPoruka; }
            set
            {
                statusPoruka = value;
                OnPropertyChanged();
            }
        }

        public ICommand UcitajPodatkeCommand { get; }

        public StatistikaViewModel(IBiciklStatistikaAdapter adapter)
        {
            this.adapter = adapter;
            BicikliStatistike = new ObservableCollection<BiciklStatistika>();
            UcitajPodatkeCommand = new RelayCommand(_ => UcitajPodatke());
        }

        private void UcitajPodatke()
        {
            try
            {
                using (var client = new Komponenta1Client())
                {
                    var bicikli = client.GetBicikli();
                    var telemetrije = client.GetTelemetrije();

                    var statistike = adapter.Adapt(bicikli, telemetrije);

                    BicikliStatistike.Clear();
                    foreach (var s in statistike)
                    {
                        BicikliStatistike.Add(s);
                    }

                    StatusPoruka = $"Učitano {statistike.Count} bicikala.";
                }
            }
            catch (Exception ex)
            {
                StatusPoruka = $"GREŠKA: {ex.Message}";
                MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
