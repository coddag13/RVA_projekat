using System;
using RVA.Shared.Models;
using System.Collections.Generic;

namespace Komponenta1.InformacioniSistem.Interfaces
{
	public interface ITelemetrijaService
	{
		List<BiciklistickaTelemetrija> GetAll();

		List<BiciklistickaTelemetrija> Search(Guid? biciklId, DateTime? vremeOcitavanja, double brzinaVoznje, double pulsVozaca, StanjeVoznje? stanje);

		void Add(BiciklistickaTelemetrija telemetrija);

		void Update(BiciklistickaTelemetrija telemetrija);

		void Delete(Guid biciklId, DateTime vremeOcitavanja);

		void SimulirajPromjenuStanja(BiciklistickaTelemetrija telemetrija);
	}
}
