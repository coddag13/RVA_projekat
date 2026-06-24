using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;

namespace Komponenta2.Statistika.Services
{
    public class MaxBrzineStatistika : IStatistickaMetoda
    {
        public string Naziv => "Maksimalne brzine po biciklisti";

        public StatistikaRezultat Izracunaj(List<BiciklStatistika> podaci)
        {
            var rezultat = new StatistikaRezultat { NazivMetode = Naziv };

            if (podaci == null || podaci.Count == 0)
                return rezultat;

            foreach (var biciklista in podaci)
            {
                if (biciklista.BrojMerenja == 0) continue;

                rezultat.Stavke.Add(new StavkaRezultata
                {
                    Opis = $"{biciklista.Vozac} ({biciklista.Tim}) - Max brzina",
                    Vrednost = biciklista.MaxBrzina
                });
            }

            return rezultat;
        }
    }
}
