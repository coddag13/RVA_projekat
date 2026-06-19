using System.Collections.ObjectModel;
using RVA.Shared.Models;
using System.Linq;

namespace Komponenta1.InformacioniSistem
{
    public class GrafikonViewModel : ViewModelBase
    {
        private readonly ITelemetrijaService telemetrijaService;
        private ObservableCollection<StanjeStatistika> statistikaStanja;

        public GrafikonViewModel(ITelemetrijaService telemetrijaService)
        {
            this.telemetrijaService = telemetrijaService;
            StatistikaStanja = new ObservableCollection<StanjeStatistika>();
            Refresh();
        }

        public ObservableCollection<StanjeStatistika> StatistikaStanja
        {
            get { return statistikaStanja; }
            set
            {
                statistikaStanja = value;
                OnPropertyChanged();
            }
        }

        public void Refresh()
        {
            StatistikaStanja = new ObservableCollection<StanjeStatistika>(
                telemetrijaService.GetAll()
                    .GroupBy(t => t.Stanje)
                    .Select(g => new StanjeStatistika
                    {
                        Stanje = g.Key,
                        BrojInstanci = g.Count()
                    }));
        }
    }
}