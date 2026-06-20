using System.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Komponenta1.InformacioniSistem
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LiveCharts.Configure(config => config.AddSkiaSharp().AddDefaultMappers());
        }
    }
}
