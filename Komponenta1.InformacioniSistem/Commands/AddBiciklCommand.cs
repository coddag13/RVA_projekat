using Komponenta1.InformacioniSistem;
using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem
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