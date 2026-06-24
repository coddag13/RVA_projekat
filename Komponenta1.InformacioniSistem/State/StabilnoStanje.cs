using RVA.Shared.Models;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.State
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
