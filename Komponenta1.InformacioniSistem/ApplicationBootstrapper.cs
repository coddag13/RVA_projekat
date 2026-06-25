using System;
using System.IO;
using System.ServiceModel;
using RVA.Shared.Contracts;
using RVA.Shared.Models;

using Komponenta1.InformacioniSistem.Commands;
using Komponenta1.InformacioniSistem.Data;
using Komponenta1.InformacioniSistem.Helpers;
using Komponenta1.InformacioniSistem.Interfaces;
using Komponenta1.InformacioniSistem.Services;
using Komponenta1.InformacioniSistem.State;
using Komponenta1.InformacioniSistem.ViewModels;
namespace Komponenta1.InformacioniSistem
{
    public class ApplicationBootstrapper : IDisposable
    {
        private ServiceHost wcfHost;

        public MainViewModel CreateMainViewModel()
        {
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string xmlFilePath = Path.Combine(dataFolder, "podaci.xml");
            string jsonFilePath = Path.Combine(dataFolder, "podaci.json");
            string logFilePath = Path.Combine(dataFolder, "aktivnosti.txt");

            IDataStorage xmlDataStorage = new XmlDataStorage(xmlFilePath);
            IDataStorage jsonDataStorage = new JsonDataStorage(jsonFilePath);
            DataStore dataStore = xmlDataStorage.Load();

            SeedDataHelper.DodajPocetnePodatkeAkoJePrazno(dataStore);

            ILogger logger = new TextFileLogger(logFilePath);
            SimulatorStanjaVoznje simulatorStanja = new SimulatorStanjaVoznje();
            IValidationService validationService = new ValidationService();

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
                xmlDataStorage,
                jsonDataStorage,
                dataStore);

            xmlDataStorage.Save(dataStore);
            logger.Log("Pokrenuta Komponenta 1 - Informacioni sistem.");
            StartWcfHost(biciklService, telemetrijaService, logger);

            mainViewModel.ShowToast("Sistem je spreman za rad.");
            return mainViewModel;
        }

        public void Dispose()
        {
            if (wcfHost == null)
            {
                return;
            }

            try
            {
                wcfHost.Close();
            }
            catch (CommunicationObjectFaultedException)
            {
                wcfHost.Abort();
            }
        }

        private void StartWcfHost(IBiciklService biciklService, ITelemetrijaService telemetrijaService, ILogger logger)
        {
            try
            {
                Komponenta1Service wcfService = new Komponenta1Service(biciklService, telemetrijaService);
                wcfHost = new ServiceHost(wcfService, new Uri("net.tcp://localhost:8000/Komponenta1Service"));
                wcfHost.AddServiceEndpoint(typeof(IKomponenta1Service), new NetTcpBinding(SecurityMode.None), string.Empty);
                wcfHost.Open();
                logger.Log("WCF servis pokrenut na net.tcp://localhost:8000/Komponenta1Service");
            }
            catch (Exception ex)
            {
                logger.Log($"Greska pri pokretanju WCF servisa: {ex.Message}");
            }
        }
    }
}
