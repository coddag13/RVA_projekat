using RVA.Shared.Models;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.State
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
