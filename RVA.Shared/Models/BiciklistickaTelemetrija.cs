using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace RVA.Shared.Models
{
    [DataContract]
    public class BiciklistickaTelemetrija
    {
        [DataMember] public Guid BiciklId { get; set; }
        [DataMember] public DateTime VremeOcitavanja { get; set; }
        [DataMember] public double BrzinaVoznje { get; set; }
        [DataMember] public double PulsVozaca { get; set; }
        [DataMember] public StanjeVoznje Stanje { get; set; }

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
