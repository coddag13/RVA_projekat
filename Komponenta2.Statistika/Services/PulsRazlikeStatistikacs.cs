using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;


namespace Komponenta2.Statistika.Services
{
    public class PulsRazlikeStatistika : IStatistickaMetoda
    {
        public string Naziv => "Prosecni puls i razlike";

        public StatistikaRezultat Izracunaj(List<BiciklStatistika> podaci)
        {
            var rezultat = new StatistikaRezultat { NazivMetode = Naziv };

            if (podaci == null || podaci.Count == 0)
                return rezultat;

            foreach (var biciklista in podaci)
            {
                if (biciklista.BrojMerenja == 0) continue;

                double prosecanPuls = biciklista.ProsecanPuls;
                double maxPuls = biciklista.MaxPuls;
                double razlika = maxPuls - prosecanPuls;

                rezultat.Stavke.Add(new StavkaRezultata
                {
                    Opis = $"{biciklista.Vozac} ({biciklista.Tim}) - Prosecni puls",
                    Vrednost = prosecanPuls
                });
                rezultat.Stavke.Add(new StavkaRezultata
                {
                    Opis = $"{biciklista.Vozac} ({biciklista.Tim}) - Max puls",
                    Vrednost = maxPuls
                });
                rezultat.Stavke.Add(new StavkaRezultata
                {
                    Opis = $"{biciklista.Vozac} ({biciklista.Tim}) - Razlika (max - prosek)",
                    Vrednost = razlika
                });
            }

            return rezultat;
        }
    }
}
