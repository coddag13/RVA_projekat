using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
{
	public interface IStanjeVoznjeHandler
	{
		StanjeVoznje GetStanje();

		StanjeVoznje SledeceStanje(BiciklistickaTelemetrija telemetrija);
	}
}
