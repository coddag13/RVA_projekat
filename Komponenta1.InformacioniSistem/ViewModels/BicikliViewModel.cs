using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem
{
    public class BicikliViewModel : ViewModelBase
    {
        private readonly IBiciklService biciklService;
        private readonly UndoRedoManager undoRedoManager;
        private readonly ValidationService validationService;

        private ObservableCollection<TrkackiBicikl> bicikli;
        private TrkackiBicikl izabraniBicikl;
        private string pretragaTim;
        private string pretragaVozac;
        private double? pretragaTezina;
        private bool? pretragaSprinter;
        private string noviTim;
        private string noviVozac;
        private double novaTezina;
        private bool noviSprinter;
        private string porukaValidacije;

        public event Action BicikliPromijenjeni;

        public BicikliViewModel(
            IBiciklService biciklService,
            UndoRedoManager undoRedoManager,
            ValidationService validationService)
        {
            this.biciklService = biciklService;
            this.undoRedoManager = undoRedoManager;
            this.validationService = validationService;

            Bicikli = new ObservableCollection<TrkackiBicikl>();
            NoviTim = string.Empty;
            NoviVozac = string.Empty;
            NovaTezina = 7.0;

            AddBiciklCommand = new RelayCommand(_ => AddBicikl());
            UpdateBiciklCommand = new RelayCommand(_ => UpdateBicikl(), _ => IzabraniBicikl != null);
            DeleteBiciklCommand = new RelayCommand(_ => DeleteBicikl(), _ => IzabraniBicikl != null);
            SearchBiciklCommand = new RelayCommand(_ => SearchBicikli());
            UndoCommand = new RelayCommand(_ => Undo(), _ => undoRedoManager.CanUndo());
            RedoCommand = new RelayCommand(_ => Redo(), _ => undoRedoManager.CanRedo());

            LoadBicikli();
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

        public TrkackiBicikl IzabraniBicikl
        {
            get { return izabraniBicikl; }
            set
            {
                izabraniBicikl = value;
                OnPropertyChanged();
            }
        }

        public string PretragaTim
        {
            get { return pretragaTim; }
            set
            {
                pretragaTim = value;
                OnPropertyChanged();
            }
        }

        public string PretragaVozac
        {
            get { return pretragaVozac; }
            set
            {
                pretragaVozac = value;
                OnPropertyChanged();
            }
        }

        public double? PretragaTezina
        {
            get { return pretragaTezina; }
            set
            {
                pretragaTezina = value;
                OnPropertyChanged();
            }
        }

        public bool? PretragaSprinter
        {
            get { return pretragaSprinter; }
            set
            {
                pretragaSprinter = value;
                OnPropertyChanged();
            }
        }

        public string NoviTim
        {
            get { return noviTim; }
            set
            {
                noviTim = value;
                OnPropertyChanged();
            }
        }

        public string NoviVozac
        {
            get { return noviVozac; }
            set
            {
                noviVozac = value;
                OnPropertyChanged();
            }
        }

        public double NovaTezina
        {
            get { return novaTezina; }
            set
            {
                novaTezina = value;
                OnPropertyChanged();
            }
        }

        public bool NoviSprinter
        {
            get { return noviSprinter; }
            set
            {
                noviSprinter = value;
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

        public ICommand AddBiciklCommand { get; set; }
        public ICommand UpdateBiciklCommand { get; set; }
        public ICommand DeleteBiciklCommand { get; set; }
        public ICommand SearchBiciklCommand { get; set; }
        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }

        public void LoadBicikli()
        {
            Bicikli = new ObservableCollection<TrkackiBicikl>(biciklService.GetAll());
        }

        public void AddBicikl()
        {
            TrkackiBicikl noviBicikl = new TrkackiBicikl
            {
                Id = Guid.NewGuid(),
                Tim = NoviTim,
                Vozac = NoviVozac,
                Tezina = NovaTezina,
                Sprinter = NoviSprinter
            };

            if (!validationService.ValidirajBicikl(noviBicikl))
            {
                PorukaValidacije = string.Join(" ", validationService.VratiGreskeBicikla(noviBicikl));
                return;
            }

            IUndoableCommand command = new AddBiciklCommand(biciklService, noviBicikl);
            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Bicikl je uspjesno dodat.";
            ResetujUnos();
            LoadBicikli();
            OnBicikliPromijenjeni();
        }

        public void UpdateBicikl()
        {
            if (IzabraniBicikl == null)
            {
                PorukaValidacije = "Izaberite bicikl za izmjenu.";
                return;
            }

            if (!validationService.ValidirajBicikl(IzabraniBicikl))
            {
                PorukaValidacije = string.Join(" ", validationService.VratiGreskeBicikla(IzabraniBicikl));
                return;
            }

            TrkackiBicikl izmijenjeniBicikl = new TrkackiBicikl
            {
                Id = IzabraniBicikl.Id,
                Tim = IzabraniBicikl.Tim,
                Vozac = IzabraniBicikl.Vozac,
                Tezina = IzabraniBicikl.Tezina,
                Sprinter = IzabraniBicikl.Sprinter
            };

            IUndoableCommand command = new UpdateBiciklCommand(biciklService, IzabraniBicikl, izmijenjeniBicikl);
            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Bicikl je izmijenjen.";
            LoadBicikli();
            OnBicikliPromijenjeni();
        }

        public void DeleteBicikl()
        {
            if (IzabraniBicikl == null)
            {
                PorukaValidacije = "Izaberite bicikl za brisanje.";
                return;
            }

            IUndoableCommand command = new DeleteBiciklCommand(biciklService, IzabraniBicikl);
            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Bicikl je obrisan.";
            LoadBicikli();
            OnBicikliPromijenjeni();
        }

        public void SearchBicikli()
        {
            Bicikli = new ObservableCollection<TrkackiBicikl>(
                biciklService.Search(PretragaTim, PretragaVozac, PretragaTezina, PretragaSprinter));
        }

        public void Undo()
        {
            undoRedoManager.Undo();
            LoadBicikli();
            OnBicikliPromijenjeni();
        }

        public void Redo()
        {
            undoRedoManager.Redo();
            LoadBicikli();
            OnBicikliPromijenjeni();
        }

        private void ResetujUnos()
        {
            NoviTim = string.Empty;
            NoviVozac = string.Empty;
            NovaTezina = 7.0;
            NoviSprinter = false;
        }

        private void OnBicikliPromijenjeni()
        {
            BicikliPromijenjeni?.Invoke();
        }
    }
}
