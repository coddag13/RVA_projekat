using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RVA.Shared.Models;

using Komponenta1.InformacioniSistem.Interfaces;
using Komponenta1.InformacioniSistem.Commands;
namespace Komponenta1.InformacioniSistem.ViewModels
{
    public class BicikliViewModel : ViewModelBase
    {
        private readonly IBiciklService biciklService;
        private readonly UndoRedoManager undoRedoManager;
        private readonly IValidationService validationService;

        private ObservableCollection<TrkackiBicikl> bicikli;
        private TrkackiBicikl izabraniBicikl;
        private string pretragaId;
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
            IValidationService validationService)
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

                if (izabraniBicikl != null)
                {
                    PopuniFormu(izabraniBicikl);
                }
            }
        }

        public string PretragaId
        {
            get { return pretragaId; }
            set
            {
                pretragaId = value;
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
            IzabraniBicikl = null;
            OnBicikliPromijenjeni();
        }

        public void UpdateBicikl()
        {
            if (IzabraniBicikl == null)
            {
                PorukaValidacije = "Izaberite bicikl za izmjenu.";
                return;
            }

            TrkackiBicikl stariBicikl = KopirajBicikl(IzabraniBicikl);
            TrkackiBicikl izmijenjeniBicikl = new TrkackiBicikl
            {
                Id = IzabraniBicikl.Id,
                Tim = NoviTim,
                Vozac = NoviVozac,
                Tezina = NovaTezina,
                Sprinter = NoviSprinter
            };

            if (!validationService.ValidirajBicikl(izmijenjeniBicikl))
            {
                PorukaValidacije = string.Join(" ", validationService.VratiGreskeBicikla(izmijenjeniBicikl));
                return;
            }

            IUndoableCommand command = new UpdateBiciklCommand(biciklService, stariBicikl, izmijenjeniBicikl);
            undoRedoManager.ExecuteCommand(command);
            PorukaValidacije = "Bicikl je izmijenjen.";
            LoadBicikli();
            IzabraniBicikl = Bicikli.FirstOrDefault(b => b.Id == izmijenjeniBicikl.Id);
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
            IzabraniBicikl = null;
            ResetujUnos();
            OnBicikliPromijenjeni();
        }

        public void SearchBicikli()
        {
            Guid? id = null;

            if (!string.IsNullOrWhiteSpace(PretragaId))
            {
                Guid parsiraniId;

                if (!Guid.TryParse(PretragaId, out parsiraniId))
                {
                    PorukaValidacije = "Id bicikla nije u ispravnom formatu.";
                    return;
                }

                id = parsiraniId;
            }

            if (PretragaTezina.HasValue && PretragaTezina.Value < 0)
            {
                PorukaValidacije = "Tezina za pretragu ne moze biti negativna.";
                return;
            }

            Bicikli = new ObservableCollection<TrkackiBicikl>(
                biciklService.Search(id, PretragaTim, PretragaVozac, PretragaTezina, PretragaSprinter));
            PorukaValidacije = "Pretraga bicikala je zavrsena.";
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

        private void PopuniFormu(TrkackiBicikl bicikl)
        {
            NoviTim = bicikl.Tim;
            NoviVozac = bicikl.Vozac;
            NovaTezina = bicikl.Tezina;
            NoviSprinter = bicikl.Sprinter;
        }

        private TrkackiBicikl KopirajBicikl(TrkackiBicikl bicikl)
        {
            return new TrkackiBicikl
            {
                Id = bicikl.Id,
                Tim = bicikl.Tim,
                Vozac = bicikl.Vozac,
                Tezina = bicikl.Tezina,
                Sprinter = bicikl.Sprinter
            };
        }

        private void OnBicikliPromijenjeni()
        {
            BicikliPromijenjeni?.Invoke();
        }
    }
}
