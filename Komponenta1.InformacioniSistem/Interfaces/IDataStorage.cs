using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
{
	public interface IDataStorage
	{
		DataStore Load();

		void Save(DataStore dataStore);
	}
}
