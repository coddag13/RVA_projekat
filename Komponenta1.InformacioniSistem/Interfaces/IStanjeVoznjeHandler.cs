using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem.Interfaces
{
	public interface IStanjeVoznjeHandler
	{
		StanjeVoznje GetStanje();

		StanjeVoznje SledeceStanje(BiciklistickaTelemetrija telemetrija);
	}
}
