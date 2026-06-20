using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;


namespace Komponenta2.Statistika.Services
{
    public class MedianaStatistika : IStatistickaMetoda
    {
        public string Naziv => "Medijana";

        public StatistikaRezultat Izracunaj(List<BiciklStatistika> podaci)
        {
            var rezultat = new StatistikaRezultat { NazivMetode = Naziv };

            if (podaci == null || podaci.Count == 0)
                return rezultat;

            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Medijana prosečnih brzina",
                Vrednost = Medijana(podaci.Select(p => p.ProsecnaBrzina).ToList())
            });
            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Medijana pulseva",
                Vrednost = Medijana(podaci.Select(p => p.ProsecanPuls).ToList())
            });
            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Medijana težina bicikala",
                Vrednost = Medijana(podaci.Select(p => p.Tezina).ToList())
            });

            return rezultat;
        }

        private double Medijana(List<double> vrednosti)
        {
            var sortirane = vrednosti.OrderBy(v => v).ToList();
            int n = sortirane.Count;
            if (n == 0) return 0;
            if (n % 2 == 1) return sortirane[n / 2];
            return (sortirane[n / 2 - 1] + sortirane[n / 2]) / 2.0;
        }
    }
}
