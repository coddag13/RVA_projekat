using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
	public interface IUndoableCommand
	{
		void Execute();

		void Unexecute();

		string GetOpis();
	}
}
