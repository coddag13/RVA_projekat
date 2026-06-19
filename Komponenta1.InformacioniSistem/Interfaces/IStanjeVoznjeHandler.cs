using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public interface IStanjeVoznjeHandler
	{
		StanjeVoznje GetStanje();

		StanjeVoznje SledeceStanje(BiciklistickaTelemetrija telemetrija);
	}
}
