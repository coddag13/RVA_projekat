using System;
using RVA.Shared.Models;
using System.Collections.Generic;

namespace Komponenta1.InformacioniSistem.Interfaces
{
	public interface IBiciklService
	{
		List<TrkackiBicikl> GetAll();

		List<TrkackiBicikl> Search(Guid? id, string tim, string vozac, double? tezina, bool? sprinter);

		void Add(TrkackiBicikl bicikl);
		void Update(TrkackiBicikl bicikl);

		void Delete(Guid id);
	}
}
