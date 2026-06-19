using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public interface IBiciklService
	{
		List<TrkackiBicikl> GetAll();

		List<TrkackiBicikl> Search(string tim, string vozac, double tezina, bool sprinter);

		void Add(TrkackiBicikl bicikl);
		void Update(TrkackiBicikl bicikl);

		void Delete(Guid id);
	}
}
