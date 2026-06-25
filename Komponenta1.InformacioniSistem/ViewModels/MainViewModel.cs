using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RVA.Shared.Models;
using System.Windows.Input;

using Komponenta1.InformacioniSistem.Interfaces;
using Komponenta1.InformacioniSistem.Commands;
namespace Komponenta1.InformacioniSistem.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private BicikliViewModel bicikliViewModel;
        private TelemetrijaViewModel telemetrijaViewModel;
        private GrafikonViewModel grafikonViewModel;
        private readonly IDataStorage xmlDataStorage;
        private readonly IDataStorage jsonDataStorage;
        private readonly DataStore dataStore;
        private FormatSkladistaOpcija izabraniFormatSkladista;

        public MainViewModel(
            BicikliViewModel bicikliViewModel,
            TelemetrijaViewModel telemetrijaViewModel,
            GrafikonViewModel grafikonViewModel,
            IDataStorage xmlDataStorage,
            IDataStorage jsonDataStorage,
            DataStore dataStore)
        {
            this.bicikliViewModel = bicikliViewModel;
            this.telemetrijaViewModel = telemetrijaViewModel;
            this.grafikonViewModel = grafikonViewModel;
            this.xmlDataStorage = xmlDataStorage;
            this.jsonDataStorage = jsonDataStorage;
            this.dataStore = dataStore;

            DostupniFormatiSkladista = new ObservableCollection<FormatSkladistaOpcija>
            {
                new FormatSkladistaOpcija("XML", ".xml"),
                new FormatSkladistaOpcija("JSON", ".json")
            };
            IzabraniFormatSkladista = DostupniFormatiSkladista[0];

            SacuvajCommand = new RelayCommand(_ => SacuvajPodatke());
            UcitajCommand = new RelayCommand(_ => UcitajPodatke());
            this.bicikliViewModel.BicikliPromijenjeni += OnBicikliPromijenjeni;
            this.telemetrijaViewModel.TelemetrijaPromijenjena += OnTelemetrijaPromijenjena;
        }

        public ICommand SacuvajCommand { get; set; }

        public ICommand UcitajCommand { get; set; }

        public ObservableCollection<FormatSkladistaOpcija> DostupniFormatiSkladista { get; set; }

        public FormatSkladistaOpcija IzabraniFormatSkladista
        {
            get { return izabraniFormatSkladista; }
            set
            {
                izabraniFormatSkladista = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AktivnoSkladisteTekst));
            }
        }

        public string AktivnoSkladisteTekst
        {
            get
            {
                return IzabraniFormatSkladista == null
                    ? "Skladiste nije izabrano"
                    : $"{IzabraniFormatSkladista.Naziv} skladiste aktivno";
            }
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

        public void SacuvajPodatke()
        {
            IDataStorage aktivnoSkladiste = VratiAktivnoSkladiste();
            aktivnoSkladiste.Save(dataStore);
            RefreshGrafikon();
            ShowToast($"Podaci su sacuvani u {IzabraniFormatSkladista.Naziv} formatu.");
        }

        public void UcitajPodatke()
        {
            IDataStorage aktivnoSkladiste = VratiAktivnoSkladiste();

            if (!aktivnoSkladiste.HasData())
            {
                ShowToast($"Nema sacuvanih podataka u {IzabraniFormatSkladista.Naziv} formatu.");
                return;
            }

            DataStore ucitaniPodaci = aktivnoSkladiste.Load();
            dataStore.Bicikli.Clear();
            dataStore.Bicikli.AddRange(ucitaniPodaci.Bicikli);
            dataStore.Telemetrije.Clear();
            dataStore.Telemetrije.AddRange(ucitaniPodaci.Telemetrije);

            BicikliVM.LoadBicikli();
            TelemetrijaVM.LoadBicikli();
            TelemetrijaVM.LoadTelemetrije();
            RefreshGrafikon();
            ShowToast($"Podaci su ucitani iz {IzabraniFormatSkladista.Naziv} formata.");
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

        private IDataStorage VratiAktivnoSkladiste()
        {
            if (IzabraniFormatSkladista != null && IzabraniFormatSkladista.Naziv == "JSON")
            {
                return jsonDataStorage;
            }

            return xmlDataStorage;
        }
    }
}
