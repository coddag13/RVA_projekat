using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komponenta2.Statistika.Models
{
    public class StatistikaRezultat
    {
        public string NazivMetode { get; set; }
        public List<StavkaRezultata> Stavke { get; set; }

        public StatistikaRezultat()
        {
            Stavke = new List<StavkaRezultata>();
        }
    }

    public class StavkaRezultata
    {
        public string Opis { get; set; }
        public double Vrednost { get; set; }
    }
}
