using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
{
	public interface IBiciklService
	{
		List<TrkackiBicikl> GetAll();

		List<TrkackiBicikl> Search(string tim, string vozac, double? tezina, bool? sprinter);

		void Add(TrkackiBicikl bicikl);
		void Update(TrkackiBicikl bicikl);

		void Delete(Guid id);
	}
}
