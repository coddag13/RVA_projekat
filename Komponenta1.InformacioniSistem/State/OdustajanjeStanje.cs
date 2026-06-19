using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
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
