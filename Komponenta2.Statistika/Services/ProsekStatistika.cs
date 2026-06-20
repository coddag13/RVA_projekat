using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;

namespace Komponenta2.Statistika.Services
{
    public class ProsekStatistika : IStatistickaMetoda
    {
        public string Naziv => "Prosek";

        public StatistikaRezultat Izracunaj(List<BiciklStatistika> podaci)
        {
            var rezultat = new StatistikaRezultat { NazivMetode = Naziv };

            if (podaci == null || podaci.Count == 0)
                return rezultat;

            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Prosečna brzina svih vozača",
                Vrednost = podaci.Average(p => p.ProsecnaBrzina)
            });
            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Prosečan puls svih vozača",
                Vrednost = podaci.Average(p => p.ProsecanPuls)
            });
            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Prosečna težina bicikla",
                Vrednost = podaci.Average(p => p.Tezina)
            });

            return rezultat;
        }
    }
}
