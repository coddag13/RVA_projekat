using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
{
	public interface ITelemetrijaService
	{
		List<BiciklistickaTelemetrija> GetAll();

		List<BiciklistickaTelemetrija> Search(double brzinaVoznje, double pulsVozaca, StanjeVoznje stanje);

		void Add(BiciklistickaTelemetrija telemetrija);

		void Update(BiciklistickaTelemetrija telemetrija);

		void Delete(Guid biciklId, DateTime vremeOcitavanja);

		void SimulirajPromjenuStanja(BiciklistickaTelemetrija telemetrija);
	}
}
