using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public class VelikiNaporiStanje : IStanjeVoznjeHandler
	{
        public StanjeVoznje GetStanje()
        {
            return StanjeVoznje.VelikiNapori;
        }

        public StanjeVoznje SledeceStanje(BiciklistickaTelemetrija telemetrija)
        {
            return StanjeVoznje.Umor;
        }
    }
}
