using System;
using System.Collections.Generic;
using System.Text;

namespace RVA.Shared.Models
{
    public class BiciklistickaTelemetrija
    {
        public Guid BiciklId { get; set; }
        public DateTime VremeOcitavanja { get; set; }
        public double BrzinaVoznje { get; set; }
        public double PulsVozaca { get; set; }
        public StanjeVoznje Stanje { get; set; }

        public BiciklistickaTelemetrija()
        {
            BiciklId = Guid.Empty;
            VremeOcitavanja = DateTime.Now;
            Stanje = StanjeVoznje.Stabilna;
        }

        public BiciklistickaTelemetrija(Guid biciklId, DateTime vremeOcitavanja, double brzinaVoznje, double pulsVozaca, StanjeVoznje stanje)
        {
            BiciklId = biciklId;
            VremeOcitavanja = vremeOcitavanja;
            BrzinaVoznje = brzinaVoznje;
            PulsVozaca = pulsVozaca;
            Stanje = stanje;
        }

        public override string ToString()
        {
            return $"{VremeOcitavanja}: {BrzinaVoznje} km/h, puls {PulsVozaca}, stanje {Stanje}";
        }
    }
}
