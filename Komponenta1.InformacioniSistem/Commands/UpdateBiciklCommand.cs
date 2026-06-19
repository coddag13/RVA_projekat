using RVA.Shared.Models;
namespace Komponenta1.InformacioniSistem
{
    public class UpdateBiciklCommand : IUndoableCommand
    {
        private readonly IBiciklService service;
        private readonly TrkackiBicikl oldBicikl;
        private readonly TrkackiBicikl newBicikl;

        public UpdateBiciklCommand(IBiciklService service, TrkackiBicikl oldBicikl, TrkackiBicikl newBicikl)
        {
            this.service = service;
            this.oldBicikl = oldBicikl;
            this.newBicikl = newBicikl;
        }

        public void Execute()
        {
            service.Update(newBicikl);
        }

        public void Unexecute()
        {
            service.Update(oldBicikl);
        }

        public string GetOpis()
        {
            return "Izmjena trkackog bicikla";
        }
    }
}