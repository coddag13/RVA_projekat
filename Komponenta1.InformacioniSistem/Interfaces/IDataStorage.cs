using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public interface IDataStorage
	{
		DataStore Load();

		void Save(DataStore dataStore);
	}
}
