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

        public Dictionary<string, List<TelemetrijaPrikaz>> AdaptToDictionary(
            List<TrkackiBicikl> bicikli,
            List<BiciklistickaTelemetrija> telemetrije)
        {
            var dictionary = new Dictionary<string, List<TelemetrijaPrikaz>>();

            if (telemetrije == null || telemetrije.Count == 0)
                return dictionary;

            // Odredimo period (min datum - max datum)
            DateTime minDatum = telemetrije.Min(t => t.VremeOcitavanja).Date;
            DateTime maxDatum = telemetrije.Max(t => t.VremeOcitavanja).Date;
            string periodKljuc = $"{minDatum:yyyy-MM-dd}, {maxDatum:yyyy-MM-dd}";

            var prikazLista = new List<TelemetrijaPrikaz>();

            foreach (var bicikl in bicikli)
            {
                var biciklTelemetrije = telemetrije.Where(t => t.BiciklId == bicikl.Id).ToList();

                if (biciklTelemetrije.Count == 0) continue;

                // Format: [brzina, puls], [brzina, puls]...
                string podaci = string.Join(", ",
                    biciklTelemetrije.Select(t => $"[{t.BrzinaVoznje:F0}, {t.PulsVozaca:F0}]"));

                prikazLista.Add(new TelemetrijaPrikaz
                {
                    Period = periodKljuc,
                    Vozac = bicikl.Vozac,
                    Podaci = podaci
                });
            }

            dictionary[periodKljuc] = prikazLista;

            return dictionary;
        }
    }
}