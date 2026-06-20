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


namespace Komponenta2.Statistika
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UcitajPodatke_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var client = new Komponenta1Client())
                {
                    var bicikli = client.GetBicikli();
                    var telemetrije = client.GetTelemetrije();

                    IBiciklStatistikaAdapter adapter = new BiciklStatistikaAdapter();
                    var statistike = adapter.Adapt(bicikli, telemetrije);

                    var prikaz = string.Join("\n\n", statistike.Select(s =>
                        $"{s.Tim} - {s.Vozac}\n" +
                        $"  Broj merenja: {s.BrojMerenja}\n" +
                        $"  Prosečna brzina: {s.ProsecnaBrzina:F2} km/h\n" +
                        $"  Prosečan puls: {s.ProsecanPuls:F2}\n" +
                        $"  Max brzina: {s.MaxBrzina:F2} km/h"
                    ));

                    ResultText.Text = prikaz;
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = $"GREŠKA: {ex.Message}";
            }
        }
    }
}