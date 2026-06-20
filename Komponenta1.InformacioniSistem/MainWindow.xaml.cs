using System;
using System.IO;
using System.Windows;
using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem
{
    public partial class MainWindow : Window
    {
        private readonly IDataStorage dataStorage;
        private readonly DataStore dataStore;

        public MainWindow()
        {
            InitializeComponent();

            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string dataFilePath = Path.Combine(dataFolder, "podaci.xml");
            string logFilePath = Path.Combine(dataFolder, "aktivnosti.txt");

            dataStorage = new XmlDataStorage(dataFilePath);
            dataStore = dataStorage.Load();

            SeedDataHelper.DodajPocetnePodatkeAkoJePrazno(dataStore);

            ILogger logger = new TextFileLogger(logFilePath);
            SimulatorStanjaVoznje simulatorStanja = new SimulatorStanjaVoznje();
            ValidationService validationService = new ValidationService();

            IBiciklService biciklService = new BiciklService(dataStore, logger);
            ITelemetrijaService telemetrijaService = new TelemetrijaService(dataStore, logger, simulatorStanja);

            UndoRedoManager undoRedoManager = new UndoRedoManager();

            BicikliViewModel bicikliViewModel = new BicikliViewModel(biciklService, undoRedoManager, validationService);
            TelemetrijaViewModel telemetrijaViewModel = new TelemetrijaViewModel(telemetrijaService, biciklService, undoRedoManager, validationService);
            GrafikonViewModel grafikonViewModel = new GrafikonViewModel(telemetrijaService);

            MainViewModel mainViewModel = new MainViewModel(
                bicikliViewModel,
                telemetrijaViewModel,
                grafikonViewModel,
                dataStorage,
                dataStore);

            mainViewModel.ShowToast("Sistem je spreman za rad.");

            DataContext = mainViewModel;

            dataStorage.Save(dataStore);
            logger.Log("Pokrenuta Komponenta 1 - Informacioni sistem.");
        }
    }
}
