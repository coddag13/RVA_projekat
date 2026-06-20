using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Komponenta2.Statistika.ViewModels;


namespace Komponenta2.Statistika
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IBiciklStatistikaAdapter adapter = new BiciklStatistikaAdapter();
            ICsvExporter csvExporter = new CsvExporter();

            var metode = new List<IStatistickaMetoda>
            {
                new ProsekStatistika(),
                new MedianaStatistika(),
                new MinMaxStatistika()
            };

            StatistickaObrada obrada = new StatistickaObrada(metode[0]);

            DataContext = new StatistikaViewModel(adapter, obrada, metode, csvExporter);
        }
    }
}