using System;
using RVA.Shared.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Komponenta1.InformacioniSistem
{
    public class BicikliViewModel : ViewModelBase
    {
        private readonly IBiciklService biciklService;
        private readonly UndoRedoManager undoRedoManager;

        private ObservableCollection<TrkackiBicikl> bicikli;
        private TrkackiBicikl izabraniBicikl;
        private string pretragaTim;
        private string pretragaVozac;
        private double pretragaTezina;
        private bool pretragaSprinter;

        public BicikliViewModel(IBiciklService biciklService, UndoRedoManager undoRedoManager)
        {
            this.biciklService = biciklService;
            this.undoRedoManager = undoRedoManager;

            Bicikli = new ObservableCollection<TrkackiBicikl>();

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

        public double PretragaTezina
        {
            get { return pretragaTezina; }
            set
            {
                pretragaTezina = value;
                OnPropertyChanged();
            }
        }

        public bool PretragaSprinter
        {
            get { return pretragaSprinter; }
            set
            {
                pretragaSprinter = value;
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
                Tim = "Novi tim",
                Vozac = "Novi vozac",
                Tezina = 70,
                Sprinter = false
            };

            IUndoableCommand command = new AddBiciklCommand(biciklService, noviBicikl);
            undoRedoManager.ExecuteCommand(command);
            LoadBicikli();
        }

        public void UpdateBicikl()
        {
            if (IzabraniBicikl == null)
            {
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
            LoadBicikli();
        }

        public void DeleteBicikl()
        {
            if (IzabraniBicikl == null)
            {
                return;
            }

            IUndoableCommand command = new DeleteBiciklCommand(biciklService, IzabraniBicikl);
            undoRedoManager.ExecuteCommand(command);
            LoadBicikli();
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
        }

        public void Redo()
        {
            undoRedoManager.Redo();
            LoadBicikli();
        }
    }
}