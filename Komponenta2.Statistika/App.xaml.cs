using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Services;
using Komponenta2.Statistika.ViewModels;


namespace Komponenta2.Statistika
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Composition root - svi konkretni objekti se kreiraju ovde

            string dataFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string logFilePath = System.IO.Path.Combine(dataFolder, "aktivnosti_komponenta2.txt");

            ILogger logger = new TextFileLogger(logFilePath);
            IBiciklStatistikaAdapter adapter = new BiciklStatistikaAdapter();
            ICsvExporter csvExporter = new CsvExporter();
            IPodaciProvider podaciProvider = new Komponenta1Client();

            var metode = new List<IStatistickaMetoda>
            {
                new ProsekStatistika(),
                new MedianaStatistika(),
                new MinMaxStatistika()
            };

            StatistickaObrada obrada = new StatistickaObrada(metode[0]);

            var viewModel = new StatistikaViewModel(adapter, obrada, metode, csvExporter, logger, podaciProvider);

            var mainWindow = new MainWindow();
            mainWindow.DataContext = viewModel;
            mainWindow.Show();
        }
    }
}
