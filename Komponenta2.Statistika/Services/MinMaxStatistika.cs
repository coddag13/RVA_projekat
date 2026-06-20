using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;


namespace Komponenta2.Statistika.Services
{
    public class MinMaxStatistika : IStatistickaMetoda
    {
        public string Naziv => "Min/Max";

        public StatistikaRezultat Izracunaj(List<BiciklStatistika> podaci)
        {
            var rezultat = new StatistikaRezultat { NazivMetode = Naziv };

            if (podaci == null || podaci.Count == 0)
                return rezultat;

            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Minimalna prosečna brzina",
                Vrednost = podaci.Min(p => p.ProsecnaBrzina)
            });
            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Maksimalna prosečna brzina",
                Vrednost = podaci.Max(p => p.ProsecnaBrzina)
            });
            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Minimalni prosečan puls",
                Vrednost = podaci.Min(p => p.ProsecanPuls)
            });
            rezultat.Stavke.Add(new StavkaRezultata
            {
                Opis = "Maksimalni prosečan puls",
                Vrednost = podaci.Max(p => p.ProsecanPuls)
            });

            return rezultat;
        }
    }
}