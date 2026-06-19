using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public class StabilnoStanje : IStanjeVoznjeHandler
	{
        public StanjeVoznje GetStanje()
        {
            return StanjeVoznje.Stabilna;
        }

        public StanjeVoznje SledeceStanje(BiciklistickaTelemetrija telemetrija)
        {
            return StanjeVoznje.VelikiNapori;
        }
    }
}
