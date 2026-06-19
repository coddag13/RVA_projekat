using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public class UmorStanje : IStanjeVoznjeHandler
	{
        public StanjeVoznje GetStanje()
        {
            return StanjeVoznje.Umor;
        }

        public StanjeVoznje SledeceStanje(BiciklistickaTelemetrija telemetrija)
        {
            return StanjeVoznje.Odustajanje;
        }
    }
}
