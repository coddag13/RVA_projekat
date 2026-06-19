using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
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
