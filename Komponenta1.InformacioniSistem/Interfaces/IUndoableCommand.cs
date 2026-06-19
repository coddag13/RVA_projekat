using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
{
	public interface IUndoableCommand
	{
		void Execute();

		void Unexecute();

		string GetOpis();
	}
}
