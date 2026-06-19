using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
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
