using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RVA.Shared.Models;

using Komponenta1.InformacioniSistem.Interfaces;
using Komponenta1.InformacioniSistem.Commands;
namespace Komponenta1.InformacioniSistem.ViewModels
{
    public class TelemetrijaViewModel : ViewModelBase
    {
        private readonly ITelemetrijaService telemetrijaService;
        private readonly IBiciklService biciklService;
        private readonly UndoRedoManager undoRedoManager;
        private readonly IValidationService validationService;

        private ObservableCollection<TrkackiBicikl> bicikli;
        private ObservableCollection<BiciklistickaTelemetrija> telemetrije;
        private BiciklistickaTelemetrija izabranaTelemetrija;
        private string pretragaBiciklId;
        private string pretragaVremeOcitavanja;
        private double pretragaBrzinaVoznje;
        private double pretragaPulsVozaca;
        private StanjeFilterOpcija pretragaStanje;
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
            IValidationService validationService)
        {
            this.telemetrijaService = telemetrijaService;
            this.biciklService = biciklService;
            this.undoRedoManager = undoRedoManager;
            this.validationService = validationService;

            Bicikli = new ObservableCollection<TrkackiBicikl>();
            Telemetrije = new ObservableCollection<BiciklistickaTelemetrija>();
            DostupnaStanjaPretrage = new ObservableCollection<StanjeFilterOpcija>();
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
            LoadStanjaPretrage();
        }

        public Array DostupnaStanja
        {
            get { return Enum.GetValues(typeof(StanjeVoznje)); }
        }

        public ObservableCollection<StanjeFilterOpcija> DostupnaStanjaPretrage { get; set; }

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

                if (izabranaTelemetrija != null)
                {
                    PopuniFormu(izabranaTelemetrija);
                }
            }
        }

        public string PretragaBiciklId
        {
            get { return pretragaBiciklId; }
            set
            {
                pretragaBiciklId = value;
                OnPropertyChanged();
            }
        }

        public string PretragaVremeOcitavanja
        {
            get { return pretragaVremeOcitavanja; }
            set
            {
                pretragaVremeOcitavanja = value;
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

        public StanjeFilterOpcija PretragaStanje
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

        public void LoadStanjaPretrage()
        {
            DostupnaStanjaPretrage.Clear();
            DostupnaStanjaPretrage.Add(new StanjeFilterOpcija("Sva stanja", null));

            foreach (StanjeVoznje stanje in Enum.GetValues(typeof(StanjeVoznje)).Cast<StanjeVoznje>())
            {
                DostupnaStanjaPretrage.Add(new StanjeFilterOpcija(stanje.ToString(), stanje));
            }

            PretragaStanje = DostupnaStanjaPretrage.FirstOrDefault();
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
            telemetrijaService.SimulirajPromjenuStanja(novaTelemetrija);
            PorukaValidacije = "Telemetrija je dodata i stanje je automatski promijenjeno.";
            ResetujUnos();
            LoadTelemetrije();
            IzabranaTelemetrija = null;
            OnTelemetrijaPromijenjena();
        }

        public void UpdateTelemetrija()
        {
            if (IzabranaTelemetrija == null)
            {
                PorukaValidacije = "Izaberite telemetriju za izmjenu.";
                return;
            }

            BiciklistickaTelemetrija staraTelemetrija = KopirajTelemetriju(IzabranaTelemetrija);
            BiciklistickaTelemetrija izmijenjenaTelemetrija = new BiciklistickaTelemetrija
            {
                BiciklId = IzabranaTelemetrija.BiciklId,
                VremeOcitavanja = IzabranaTelemetrija.VremeOcitavanja,
                BrzinaVoznje = NovaBrzinaVoznje,
                PulsVozaca = NoviPulsVozaca,
                Stanje = NovoStanje
            };

            if (!validationService.ValidirajTelemetriju(izmijenjenaTelemetrija))
            {
                PorukaValidacije = string.Join(" ", validationService.VratiGreskeTelemetrije(izmijenjenaTelemetrija));
                return;
            }

            IUndoableCommand command = new UpdateTelemetrijaCommand(
                telemetrijaService,
                staraTelemetrija,
                izmijenjenaTelemetrija);

            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Telemetrija je izmijenjena.";
            LoadTelemetrije();
            IzabranaTelemetrija = Telemetrije.FirstOrDefault(t =>
                t.BiciklId == izmijenjenaTelemetrija.BiciklId &&
                t.VremeOcitavanja == izmijenjenaTelemetrija.VremeOcitavanja);
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
            IzabranaTelemetrija = null;
            ResetujUnos();
            OnTelemetrijaPromijenjena();
        }

        public void SearchTelemetrija()
        {
            Guid? biciklId = null;
            DateTime? vremeOcitavanja = null;

            if (!string.IsNullOrWhiteSpace(PretragaBiciklId))
            {
                Guid parsiraniBiciklId;

                if (!Guid.TryParse(PretragaBiciklId, out parsiraniBiciklId))
                {
                    PorukaValidacije = "Bicikl Id nije u ispravnom formatu.";
                    return;
                }

                biciklId = parsiraniBiciklId;
            }

            if (!string.IsNullOrWhiteSpace(PretragaVremeOcitavanja))
            {
                DateTime parsiranoVreme;

                if (!DateTime.TryParse(PretragaVremeOcitavanja, out parsiranoVreme))
                {
                    PorukaValidacije = "Vrijeme ocitavanja nije u ispravnom formatu.";
                    return;
                }

                vremeOcitavanja = parsiranoVreme;
            }

            Telemetrije = new ObservableCollection<BiciklistickaTelemetrija>(
                telemetrijaService.Search(biciklId, vremeOcitavanja, PretragaBrzinaVoznje, PretragaPulsVozaca, PretragaStanje?.Stanje));
            PorukaValidacije = "Pretraga telemetrije je zavrsena.";
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

        private void PopuniFormu(BiciklistickaTelemetrija telemetrija)
        {
            IzabraniBiciklZaTelemetriju = Bicikli.FirstOrDefault(b => b.Id == telemetrija.BiciklId);
            NovaBrzinaVoznje = telemetrija.BrzinaVoznje;
            NoviPulsVozaca = telemetrija.PulsVozaca;
            NovoStanje = telemetrija.Stanje;
        }

        private BiciklistickaTelemetrija KopirajTelemetriju(BiciklistickaTelemetrija telemetrija)
        {
            return new BiciklistickaTelemetrija
            {
                BiciklId = telemetrija.BiciklId,
                VremeOcitavanja = telemetrija.VremeOcitavanja,
                BrzinaVoznje = telemetrija.BrzinaVoznje,
                PulsVozaca = telemetrija.PulsVozaca,
                Stanje = telemetrija.Stanje
            };
        }

        private void OnTelemetrijaPromijenjena()
        {
            TelemetrijaPromijenjena?.Invoke();
        }
    }
}
