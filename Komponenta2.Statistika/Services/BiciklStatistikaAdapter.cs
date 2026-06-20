using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;
using RVA.Shared.Models;


namespace Komponenta2.Statistika.Services
{
    public class BiciklStatistikaAdapter : IBiciklStatistikaAdapter
    {
        public List<BiciklStatistika> Adapt(List<TrkackiBicikl> bicikli, List<BiciklistickaTelemetrija> telemetrije)
        {
            var result = new List<BiciklStatistika>();

            foreach (var bicikl in bicikli)
            {
                var statistika = new BiciklStatistika
                {
                    Tim = bicikl.Tim,
                    Vozac = bicikl.Vozac,
                    Tezina = bicikl.Tezina,
                    Sprinter = bicikl.Sprinter,
                    Telemetrije = telemetrije.Where(t => t.BiciklId == bicikl.Id).ToList()
                };

                result.Add(statistika);
            }

            return result;
        }
    }
}