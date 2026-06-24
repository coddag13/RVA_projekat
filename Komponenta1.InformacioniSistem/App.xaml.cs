using System.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Komponenta1.InformacioniSistem
{
    public partial class App : Application
    {
        private ApplicationBootstrapper bootstrapper;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LiveCharts.Configure(config => config.AddSkiaSharp().AddDefaultMappers());

            bootstrapper = new ApplicationBootstrapper();

            MainWindow mainWindow = new MainWindow
            {
                DataContext = bootstrapper.CreateMainViewModel()
            };

            MainWindow = mainWindow;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            bootstrapper?.Dispose();
            base.OnExit(e);
        }
    }
}
