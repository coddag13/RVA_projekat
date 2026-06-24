using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem.Interfaces
{
	public interface IDataStorage
	{
		DataStore Load();

		void Save(DataStore dataStore);
	}
}
