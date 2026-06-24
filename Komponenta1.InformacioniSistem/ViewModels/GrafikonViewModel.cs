using System;
using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using RVA.Shared.Models;
using SkiaSharp;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.ViewModels
{
    public class GrafikonViewModel : ViewModelBase
    {
        private readonly ITelemetrijaService telemetrijaService;
        private ObservableCollection<StanjeStatistika> statistikaStanja;
        private ObservableCollection<ISeries> stanjeSeries;
        private Axis[] xAxes;
        private Axis[] yAxes;
        private SolidColorPaint pozadinaGrafikona;
        private DrawMarginFrame okvirGrafikona;

        public GrafikonViewModel(ITelemetrijaService telemetrijaService)
        {
            this.telemetrijaService = telemetrijaService;
            StatistikaStanja = new ObservableCollection<StanjeStatistika>();
            StanjeSeries = new ObservableCollection<ISeries>();
            XAxes = new Axis[0];
            YAxes = new Axis[0];
            PozadinaGrafikona = new SolidColorPaint(SKColors.White);
            OkvirGrafikona = new DrawMarginFrame
            {
                Fill = new SolidColorPaint(SKColors.AliceBlue),
                Stroke = new SolidColorPaint(SKColor.Parse("#E8D7C3")) { StrokeThickness = 1 }
            };
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

        public ObservableCollection<ISeries> StanjeSeries
        {
            get { return stanjeSeries; }
            set
            {
                stanjeSeries = value;
                OnPropertyChanged();
            }
        }

        public Axis[] XAxes
        {
            get { return xAxes; }
            set
            {
                xAxes = value;
                OnPropertyChanged();
            }
        }

        public Axis[] YAxes
        {
            get { return yAxes; }
            set
            {
                yAxes = value;
                OnPropertyChanged();
            }
        }

        public SolidColorPaint PozadinaGrafikona
        {
            get { return pozadinaGrafikona; }
            set
            {
                pozadinaGrafikona = value;
                OnPropertyChanged();
            }
        }

        public DrawMarginFrame OkvirGrafikona
        {
            get { return okvirGrafikona; }
            set
            {
                okvirGrafikona = value;
                OnPropertyChanged();
            }
        }

        public void Refresh()
        {
            StanjeVoznje[] stanja = Enum.GetValues(typeof(StanjeVoznje))
                .Cast<StanjeVoznje>()
                .ToArray();

            StatistikaStanja = new ObservableCollection<StanjeStatistika>(
                stanja.Select(stanje => new StanjeStatistika
                {
                    Stanje = stanje,
                    BrojInstanci = telemetrijaService.GetAll().Count(t => t.Stanje == stanje)
                }));

            int maxVrijednost = Math.Max(1, StatistikaStanja.Max(s => s.BrojInstanci));

            StanjeSeries = new ObservableCollection<ISeries>
            {
                new ColumnSeries<int>
                {
                    Name = "Broj metrika",
                    Values = StatistikaStanja.Select(s => s.BrojInstanci).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#2F6F73")),
                    Stroke = new SolidColorPaint(SKColor.Parse("#264F52")) { StrokeThickness = 2 },
                    MaxBarWidth = 70,
                    DataLabelsSize = 14,
                    DataLabelsPaint = new SolidColorPaint(SKColor.Parse("#263238")),
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top
                }
            };

            XAxes = new[]
            {
                new Axis
                {
                    Labels = stanja.Select(s => s.ToString()).ToArray(),
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#263238")),
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#FFF7EF")) { StrokeThickness = 1 }
                }
            };

            YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = maxVrijednost + 1,
                    MinStep = 1,
                    ForceStepToMin = true,
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#E8D7C3")) { StrokeThickness = 1 },
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#263238")),
                    TextSize = 12
                }
            };
        }
    }
}
