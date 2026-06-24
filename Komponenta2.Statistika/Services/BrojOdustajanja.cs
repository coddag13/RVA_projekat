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
    public class BrojOdustajanja : IStatistickaMetoda
    {
        public string Naziv => "Broj odustajanja";

        public StatistikaRezultat Izracunaj(List<BiciklStatistika> podaci)
        {
            var rezultat = new StatistikaRezultat { NazivMetode = Naziv };

            if (podaci == null || podaci.Count == 0)
                return rezultat;

            int ukupnoOdustajanja = 0;

            foreach (var biciklista in podaci)
            {
                int odustajanja = biciklista.Telemetrije
                    .Count(t => t.Stanje == StanjeVoznje.Odustajanje);

                ukupnoOdustajanja += odustajanja;

                rezultat.Stavke.Add(new StavkaRezultata
                {
                    Opis = $"{biciklista.Vozac} ({biciklista.Tim}) - Broj odustajanja",
                    Vrednost = odustajanja
                });
            }

            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "UKUPNO odustajanja medju svim telemetrijama",
                Vrednost = ukupnoOdustajanja
            });

            return rezultat;
        }
    }
}
