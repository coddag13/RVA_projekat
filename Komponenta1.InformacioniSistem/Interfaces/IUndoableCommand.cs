
namespace Komponenta1.InformacioniSistem.Interfaces
{
	public interface IUndoableCommand
	{
		void Execute();

		void Unexecute();

		string GetOpis();
	}
}
