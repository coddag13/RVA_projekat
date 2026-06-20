using System;
using System.IO;
using System.ServiceModel;
using System.Windows;
using RVA.Shared.Contracts;
using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem
{
    public partial class MainWindow : Window
    {
        private readonly IDataStorage dataStorage;
        private readonly DataStore dataStore;
        private ServiceHost wcfHost;

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

            // WCF HOST
            StartWcfHost(biciklService, telemetrijaService, logger);

            this.Closed += (s, e) => wcfHost?.Close();
        }

        private void StartWcfHost(IBiciklService biciklService, ITelemetrijaService telemetrijaService, ILogger logger)
        {
            try
            {
                var wcfService = new Komponenta1Service(biciklService, telemetrijaService);
                wcfHost = new ServiceHost(wcfService, new Uri("net.tcp://localhost:8000/Komponenta1Service"));
                wcfHost.AddServiceEndpoint(typeof(IKomponenta1Service), new NetTcpBinding(SecurityMode.None), "");
                wcfHost.Open();
                logger.Log("WCF servis pokrenut na net.tcp://localhost:8000/Komponenta1Service");
            }
            catch (Exception ex)
            {
                logger.Log($"Greska pri pokretanju WCF servisa: {ex.Message}");
                MessageBox.Show($"WCF host nije pokrenut: {ex.Message}", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
