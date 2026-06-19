using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public class SimulatorStanjaVoznje
	{
        private IStanjeVoznjeHandler trenutnoStanje;

        public SimulatorStanjaVoznje()
        {
            trenutnoStanje = new StabilnoStanje();
        }

        public void SetStanje(StanjeVoznje stanje)
        {
            trenutnoStanje = KreirajStanje(stanje);
        }

        public void PromijeniStanje(BiciklistickaTelemetrija telemetrija)
        {
            if (telemetrija == null)
            {
                throw new ArgumentNullException(nameof(telemetrija));
            }

            SetStanje(telemetrija.Stanje);
            telemetrija.Stanje = trenutnoStanje.SledeceStanje(telemetrija);
        }

        private IStanjeVoznjeHandler KreirajStanje(StanjeVoznje stanje)
        {
            switch (stanje)
            {
                case StanjeVoznje.Stabilna:
                    return new StabilnoStanje();
                case StanjeVoznje.VelikiNapori:
                    return new VelikiNaporiStanje();
                case StanjeVoznje.Umor:
                    return new UmorStanje();
                case StanjeVoznje.Odustajanje:
                    return new OdustajanjeStanje();
                default:
                    return new StabilnoStanje();
            }
        }
    }
}
