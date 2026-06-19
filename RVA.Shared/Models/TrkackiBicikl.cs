using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
    public class TrkackiBicikl
    {
        public Guid Id { get; set; }
        public string Tim { get; set; }
        public string Vozac { get; set; }
        public double Tezina { get; set; }
        public bool Sprinter { get; set; }

        public TrkackiBicikl()
        {
            Id = Guid.NewGuid();
            Tim = string.Empty;
            Vozac = string.Empty;
        }

        public TrkackiBicikl(Guid id, string tim, string vozac, double tezina, bool sprinter)
        {
            Id = id;
            Tim = tim;
            Vozac = vozac;
            Tezina = tezina;
            Sprinter = sprinter;
        }

        public override string ToString()
        {
            return $"{Tim} - {Vozac}";
        }
    }
}

