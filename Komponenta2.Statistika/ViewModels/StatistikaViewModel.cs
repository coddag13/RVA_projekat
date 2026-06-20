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
        private readonly StatistickaObrada obrada;

        public ObservableCollection<BiciklStatistika> BicikliStatistike { get; set; }
        public ObservableCollection<IStatistickaMetoda> DostupneMetode { get; set; }

        private IStatistickaMetoda izabranaMetoda;
        public IStatistickaMetoda IzabranaMetoda
        {
            get { return izabranaMetoda; }
            set
            {
                izabranaMetoda = value;
                OnPropertyChanged();
            }
        }

        private StatistikaRezultat rezultat;
        public StatistikaRezultat Rezultat
        {
            get { return rezultat; }
            set
            {
                rezultat = value;
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
        public ICommand IzracunajCommand { get; }

        public StatistikaViewModel(IBiciklStatistikaAdapter adapter, StatistickaObrada obrada, List<IStatistickaMetoda> metode)
        {
            this.adapter = adapter;
            this.obrada = obrada;

            BicikliStatistike = new ObservableCollection<BiciklStatistika>();
            DostupneMetode = new ObservableCollection<IStatistickaMetoda>(metode);
            IzabranaMetoda = DostupneMetode.Count > 0 ? DostupneMetode[0] : null;

            UcitajPodatkeCommand = new RelayCommand(_ => UcitajPodatke());
            IzracunajCommand = new RelayCommand(_ => Izracunaj(), _ => IzabranaMetoda != null && BicikliStatistike.Count > 0);
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

        private void Izracunaj()
        {
            if (IzabranaMetoda == null) return;

            obrada.SetStrategy(IzabranaMetoda);  // postavi strategiju
            var podaci = new List<BiciklStatistika>(BicikliStatistike);
            Rezultat = obrada.Obradi(podaci);    // Context delegira

            StatusPoruka = $"Izračunato: {Rezultat.NazivMetode}";
        }
    }
}
