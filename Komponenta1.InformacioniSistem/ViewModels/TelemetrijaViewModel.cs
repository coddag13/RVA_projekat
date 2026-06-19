using System;
using RVA.Shared.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Komponenta1.InformacioniSistem
{
    public class TelemetrijaViewModel : ViewModelBase
    {
        private readonly ITelemetrijaService telemetrijaService;
        private readonly UndoRedoManager undoRedoManager;

        private ObservableCollection<BiciklistickaTelemetrija> telemetrije;
        private BiciklistickaTelemetrija izabranaTelemetrija;
        private double pretragaBrzinaVoznje;
        private double pretragaPulsVozaca;
        private StanjeVoznje pretragaStanje;

        public TelemetrijaViewModel(ITelemetrijaService telemetrijaService, UndoRedoManager undoRedoManager)
        {
            this.telemetrijaService = telemetrijaService;
            this.undoRedoManager = undoRedoManager;

            Telemetrije = new ObservableCollection<BiciklistickaTelemetrija>();

            AddTelemetrijaCommand = new RelayCommand(_ => AddTelemetrija());
            UpdateTelemetrijaCommand = new RelayCommand(_ => UpdateTelemetrija(), _ => IzabranaTelemetrija != null);
            DeleteTelemetrijaCommand = new RelayCommand(_ => DeleteTelemetrija(), _ => IzabranaTelemetrija != null);
            SearchTelemetrijaCommand = new RelayCommand(_ => SearchTelemetrija());
            SimulirajStanjeTelemetrijaCommand = new RelayCommand(_ => SimulirajStanje(), _ => IzabranaTelemetrija != null);
            UndoCommand = new RelayCommand(_ => Undo(), _ => undoRedoManager.CanUndo());
            RedoCommand = new RelayCommand(_ => Redo(), _ => undoRedoManager.CanRedo());

            LoadTelemetrije();
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

        public void AddTelemetrija()
        {
            BiciklistickaTelemetrija novaTelemetrija = new BiciklistickaTelemetrija
            {
                BiciklId = Guid.Empty,
                VremeOcitavanja = DateTime.Now,
                BrzinaVoznje = 30,
                PulsVozaca = 120,
                Stanje = StanjeVoznje.Stabilna
            };

            IUndoableCommand command = new AddTelemetrijaCommand(telemetrijaService, novaTelemetrija);
            undoRedoManager.ExecuteCommand(command);
            LoadTelemetrije();
        }

        public void UpdateTelemetrija()
        {
            if (IzabranaTelemetrija == null)
            {
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
            LoadTelemetrije();
        }

        public void DeleteTelemetrija()
        {
            if (IzabranaTelemetrija == null)
            {
                return;
            }

            IUndoableCommand command = new DeleteTelemetrijaCommand(telemetrijaService, IzabranaTelemetrija);
            undoRedoManager.ExecuteCommand(command);
            LoadTelemetrije();
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
                return;
            }

            telemetrijaService.SimulirajPromjenuStanja(IzabranaTelemetrija);
            LoadTelemetrije();
        }

        public void Undo()
        {
            undoRedoManager.Undo();
            LoadTelemetrije();
        }

        public void Redo()
        {
            undoRedoManager.Redo();
            LoadTelemetrije();
        }
    }
}