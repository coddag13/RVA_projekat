using RVA.Shared.Models;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Commands
{
    public class AddBiciklCommand : IUndoableCommand
    {
        private readonly IBiciklService service;
        private readonly TrkackiBicikl bicikl;

        public AddBiciklCommand(IBiciklService service, TrkackiBicikl bicikl)
        {
            this.service = service;
            this.bicikl = bicikl;
        }

        public void Execute()
        {
            service.Add(bicikl);
        }

        public void Unexecute()
        {
            service.Delete(bicikl.Id);
        }

        public string GetOpis()
        {
            return "Dodavanje trkackog bicikla";
        }
    }
}