using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
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
