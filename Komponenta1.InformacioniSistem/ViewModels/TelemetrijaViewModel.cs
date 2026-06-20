using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem
{
    public class TelemetrijaViewModel : ViewModelBase
    {
        private readonly ITelemetrijaService telemetrijaService;
        private readonly IBiciklService biciklService;
        private readonly UndoRedoManager undoRedoManager;
        private readonly ValidationService validationService;

        private ObservableCollection<TrkackiBicikl> bicikli;
        private ObservableCollection<BiciklistickaTelemetrija> telemetrije;
        private BiciklistickaTelemetrija izabranaTelemetrija;
        private double pretragaBrzinaVoznje;
        private double pretragaPulsVozaca;
        private StanjeVoznje pretragaStanje;
        private TrkackiBicikl izabraniBiciklZaTelemetriju;
        private double novaBrzinaVoznje;
        private double noviPulsVozaca;
        private StanjeVoznje novoStanje;
        private string porukaValidacije;

        public event Action TelemetrijaPromijenjena;

        public TelemetrijaViewModel(
            ITelemetrijaService telemetrijaService,
            IBiciklService biciklService,
            UndoRedoManager undoRedoManager,
            ValidationService validationService)
        {
            this.telemetrijaService = telemetrijaService;
            this.biciklService = biciklService;
            this.undoRedoManager = undoRedoManager;
            this.validationService = validationService;

            Bicikli = new ObservableCollection<TrkackiBicikl>();
            Telemetrije = new ObservableCollection<BiciklistickaTelemetrija>();
            NovaBrzinaVoznje = 30;
            NoviPulsVozaca = 120;
            NovoStanje = StanjeVoznje.Stabilna;

            AddTelemetrijaCommand = new RelayCommand(_ => AddTelemetrija());
            UpdateTelemetrijaCommand = new RelayCommand(_ => UpdateTelemetrija(), _ => IzabranaTelemetrija != null);
            DeleteTelemetrijaCommand = new RelayCommand(_ => DeleteTelemetrija(), _ => IzabranaTelemetrija != null);
            SearchTelemetrijaCommand = new RelayCommand(_ => SearchTelemetrija());
            SimulirajStanjeTelemetrijaCommand = new RelayCommand(_ => SimulirajStanje(), _ => IzabranaTelemetrija != null);
            UndoCommand = new RelayCommand(_ => Undo(), _ => undoRedoManager.CanUndo());
            RedoCommand = new RelayCommand(_ => Redo(), _ => undoRedoManager.CanRedo());

            LoadBicikli();
            LoadTelemetrije();
        }

        public Array DostupnaStanja
        {
            get { return Enum.GetValues(typeof(StanjeVoznje)); }
        }

        public ObservableCollection<BiciklistickaTelemetrija> Telemetrije
        {
            get { return telemetrije; }
            set
            {
                telemetrije = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TrkackiBicikl> Bicikli
        {
            get { return bicikli; }
            set
            {
                bicikli = value;
                OnPropertyChanged();
            }
        }

        public BiciklistickaTelemetrija IzabranaTelemetrija
        {
            get { return izabranaTelemetrija; }
            set
            {
                izabranaTelemetrija = value;
                OnPropertyChanged();
            }
        }

        public double PretragaBrzinaVoznje
        {
            get { return pretragaBrzinaVoznje; }
            set
            {
                pretragaBrzinaVoznje = value;
                OnPropertyChanged();
            }
        }

        public double PretragaPulsVozaca
        {
            get { return pretragaPulsVozaca; }
            set
            {
                pretragaPulsVozaca = value;
                OnPropertyChanged();
            }
        }

        public StanjeVoznje PretragaStanje
        {
            get { return pretragaStanje; }
            set
            {
                pretragaStanje = value;
                OnPropertyChanged();
            }
        }

        public TrkackiBicikl IzabraniBiciklZaTelemetriju
        {
            get { return izabraniBiciklZaTelemetriju; }
            set
            {
                izabraniBiciklZaTelemetriju = value;
                OnPropertyChanged();
            }
        }

        public double NovaBrzinaVoznje
        {
            get { return novaBrzinaVoznje; }
            set
            {
                novaBrzinaVoznje = value;
                OnPropertyChanged();
            }
        }

        public double NoviPulsVozaca
        {
            get { return noviPulsVozaca; }
            set
            {
                noviPulsVozaca = value;
                OnPropertyChanged();
            }
        }

        public StanjeVoznje NovoStanje
        {
            get { return novoStanje; }
            set
            {
                novoStanje = value;
                OnPropertyChanged();
            }
        }

        public string PorukaValidacije
        {
            get { return porukaValidacije; }
            set
            {
                porukaValidacije = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTelemetrijaCommand { get; set; }
        public ICommand UpdateTelemetrijaCommand { get; set; }
        public ICommand DeleteTelemetrijaCommand { get; set; }
        public ICommand SearchTelemetrijaCommand { get; set; }
        public ICommand SimulirajStanjeTelemetrijaCommand { get; set; }
        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }

        public void LoadTelemetrije()
        {
            Telemetrije = new ObservableCollection<BiciklistickaTelemetrija>(telemetrijaService.GetAll());
        }

        public void LoadBicikli()
        {
            Bicikli = new ObservableCollection<TrkackiBicikl>(biciklService.GetAll());

            if (IzabraniBiciklZaTelemetriju == null && Bicikli.Count > 0)
            {
                IzabraniBiciklZaTelemetriju = Bicikli[0];
            }
        }

        public void AddTelemetrija()
        {
            if (IzabraniBiciklZaTelemetriju == null)
            {
                PorukaValidacije = "Izaberite bicikl za telemetriju.";
                return;
            }

            BiciklistickaTelemetrija novaTelemetrija = new BiciklistickaTelemetrija
            {
                BiciklId = IzabraniBiciklZaTelemetriju.Id,
                VremeOcitavanja = DateTime.Now,
                BrzinaVoznje = NovaBrzinaVoznje,
                PulsVozaca = NoviPulsVozaca,
                Stanje = NovoStanje
            };

            if (!validationService.ValidirajTelemetriju(novaTelemetrija))
            {
                PorukaValidacije = string.Join(" ", validationService.VratiGreskeTelemetrije(novaTelemetrija));
                return;
            }

            IUndoableCommand command = new AddTelemetrijaCommand(telemetrijaService, novaTelemetrija);
            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Telemetrija je uspjesno dodata.";
            ResetujUnos();
            LoadTelemetrije();
            OnTelemetrijaPromijenjena();
        }

        public void UpdateTelemetrija()
        {
            if (IzabranaTelemetrija == null)
            {
                PorukaValidacije = "Izaberite telemetriju za izmjenu.";
                return;
            }

            if (!validationService.ValidirajTelemetriju(IzabranaTelemetrija))
            {
                PorukaValidacije = string.Join(" ", validationService.VratiGreskeTelemetrije(IzabranaTelemetrija));
                return;
            }

            BiciklistickaTelemetrija izmijenjenaTelemetrija = new BiciklistickaTelemetrija
            {
                BiciklId = IzabranaTelemetrija.BiciklId,
                VremeOcitavanja = IzabranaTelemetrija.VremeOcitavanja,
                BrzinaVoznje = IzabranaTelemetrija.BrzinaVoznje,
                PulsVozaca = IzabranaTelemetrija.PulsVozaca,
                Stanje = IzabranaTelemetrija.Stanje
            };

            IUndoableCommand command = new UpdateTelemetrijaCommand(
                telemetrijaService,
                IzabranaTelemetrija,
                izmijenjenaTelemetrija);

            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Telemetrija je izmijenjena.";
            LoadTelemetrije();
            OnTelemetrijaPromijenjena();
        }

        public void DeleteTelemetrija()
        {
            if (IzabranaTelemetrija == null)
            {
                PorukaValidacije = "Izaberite telemetriju za brisanje.";
                return;
            }

            IUndoableCommand command = new DeleteTelemetrijaCommand(telemetrijaService, IzabranaTelemetrija);
            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Telemetrija je obrisana.";
            LoadTelemetrije();
            OnTelemetrijaPromijenjena();
        }

        public void SearchTelemetrija()
        {
            Telemetrije = new ObservableCollection<BiciklistickaTelemetrija>(
                telemetrijaService.Search(PretragaBrzinaVoznje, PretragaPulsVozaca, PretragaStanje));
        }

        public void SimulirajStanje()
        {
            if (IzabranaTelemetrija == null)
            {
                PorukaValidacije = "Izaberite telemetriju za simulaciju.";
                return;
            }

            telemetrijaService.SimulirajPromjenuStanja(IzabranaTelemetrija);
            PorukaValidacije = "Stanje telemetrije je promijenjeno.";
            LoadTelemetrije();
            OnTelemetrijaPromijenjena();
        }

        public void Undo()
        {
            undoRedoManager.Undo();
            LoadTelemetrije();
            OnTelemetrijaPromijenjena();
        }

        public void Redo()
        {
            undoRedoManager.Redo();
            LoadTelemetrije();
            OnTelemetrijaPromijenjena();
        }

        private void ResetujUnos()
        {
            NovaBrzinaVoznje = 30;
            NoviPulsVozaca = 120;
            NovoStanje = StanjeVoznje.Stabilna;
        }

        private void OnTelemetrijaPromijenjena()
        {
            TelemetrijaPromijenjena?.Invoke();
        }
    }
}
