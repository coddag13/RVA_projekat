using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RVA.Shared.Models;

namespace Komponenta2.Statistika.Models
{
    public class BiciklStatistika
    {
        public string Tim { get; set; }
        public string Vozac { get; set; }
        public double Tezina { get; set; }
        public bool Sprinter { get; set; }
        public List<BiciklistickaTelemetrija> Telemetrije { get; set; }

        public int BrojMerenja => Telemetrije?.Count ?? 0;
        public double ProsecnaBrzina => BrojMerenja > 0 ? Telemetrije.Average(t => t.BrzinaVoznje) : 0;
        public double ProsecanPuls => BrojMerenja > 0 ? Telemetrije.Average(t => t.PulsVozaca) : 0;
        public double MaxBrzina => BrojMerenja > 0 ? Telemetrije.Max(t => t.BrzinaVoznje) : 0;
        public double MaxPuls => BrojMerenja > 0 ? Telemetrije.Max(t => t.PulsVozaca) : 0;

        public BiciklStatistika()
        {
            Telemetrije = new List<BiciklistickaTelemetrija>();
        }
    }
}
