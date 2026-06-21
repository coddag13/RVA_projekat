using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;
using Komponenta2.Statistika.Services;
using Microsoft.Win32;
using RVA.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Komponenta2.Statistika.Helpers;



namespace Komponenta2.Statistika.ViewModels
{
    public class StatistikaViewModel : ViewModelBase
    {
        private readonly IBiciklStatistikaAdapter adapter;
        private readonly StatistickaObrada obrada;
        private readonly ICsvExporter csvExporter;
        private readonly ILogger logger;

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
        public ICommand ExportCsvCommand { get; }

        public StatistikaViewModel(
                IBiciklStatistikaAdapter adapter,
                StatistickaObrada obrada,
                List<IStatistickaMetoda> metode,
                ICsvExporter csvExporter,
                ILogger logger)
            {
                    this.adapter = adapter;
                    this.obrada = obrada;
                    this.csvExporter = csvExporter;
                    this.logger = logger;

                    BicikliStatistike = new ObservableCollection<BiciklStatistika>();
                    DostupneMetode = new ObservableCollection<IStatistickaMetoda>(metode);
                    IzabranaMetoda = DostupneMetode.Count > 0 ? DostupneMetode[0] : null;

                    UcitajPodatkeCommand = new RelayCommand(_ => UcitajPodatke());
                    IzracunajCommand = new RelayCommand(_ => Izracunaj(), _ => IzabranaMetoda != null && BicikliStatistike.Count > 0);
                    ExportCsvCommand = new RelayCommand(_ => ExportCsv(), _ => Rezultat != null && Rezultat.Stavke.Count > 0);
                    
                    logger.Log("Komponenta 2 - Statistika pokrenuta.");
                    UcitajPodatke();
            }

        private void UcitajPodatke()
        {
            List<TrkackiBicikl> bicikli;
            List<BiciklistickaTelemetrija> telemetrije;
            bool koristiDefault = false;

            try
            {
                using (var client = new Komponenta1Client())
                {
                    bicikli = client.GetBicikli();
                    telemetrije = client.GetTelemetrije();
                }
                logger.Log($"Učitano {bicikli.Count} bicikala i {telemetrije.Count} telemetrija iz Komponente 1.");
            }
            catch (Exception ex)
            {
                var (defBicikli, defTelemetrije) = SeedDataHelper.DobijDefaultPodatke();
                bicikli = defBicikli;
                telemetrije = defTelemetrije;
                koristiDefault = true;
                logger.Log($"Komponenta 1 nedostupna ({ex.Message}). Učitani default podaci.");
            }

            var statistike = adapter.Adapt(bicikli, telemetrije);

            BicikliStatistike.Clear();
            foreach (var s in statistike)
            {
                BicikliStatistike.Add(s);
            }

            if (koristiDefault)
            {
                StatusPoruka = $"Komponenta 1 nije dostupna - učitano {statistike.Count} default bicikala.";
            }
            else
            {
                StatusPoruka = $"Učitano {statistike.Count} bicikala iz Komponente 1.";
            }
        }

        private void Izracunaj()
        {
            if (IzabranaMetoda == null) return;

            obrada.SetStrategy(IzabranaMetoda);
            var podaci = new List<BiciklStatistika>(BicikliStatistike);
            Rezultat = obrada.Obradi(podaci);

            StatusPoruka = $"Izračunato: {Rezultat.NazivMetode}";
            logger.Log($"Izvršena statistička obrada metodom: {Rezultat.NazivMetode}. Broj rezultata: {Rezultat.Stavke.Count}");
        }

        private void ExportCsv()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV fajl (*.csv)|*.csv",
                FileName = $"statistika_{Rezultat.NazivMetode}.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    csvExporter.Export(Rezultat, dialog.FileName);
                    StatusPoruka = $"Sačuvano u: {dialog.FileName}";
                    logger.Log($"Rezultat statistike ({Rezultat.NazivMetode}) eksportovan u CSV: {dialog.FileName}");
                    MessageBox.Show("CSV fajl je uspešno sačuvan.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    StatusPoruka = $"GREŠKA pri čuvanju: {ex.Message}";
                    logger.Log($"GREŠKA pri eksportovanju CSV: {ex.Message}");
                    MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
