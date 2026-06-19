using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public class OdustajanjeStanje : IStanjeVoznjeHandler
	{
        public StanjeVoznje GetStanje()
        {
            return StanjeVoznje.Odustajanje;
        }

        public StanjeVoznje SledeceStanje(BiciklistickaTelemetrija telemetrija)
        {
            return StanjeVoznje.Stabilna;
        }
    }
}
