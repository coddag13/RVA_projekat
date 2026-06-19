namespace Projekat.Komponenta1
{
    public class DeleteBiciklCommand : IUndoableCommand
    {
        private readonly IBiciklService service;
        private readonly TrkackiBicikl bicikl;

        public DeleteBiciklCommand(IBiciklService service, TrkackiBicikl bicikl)
        {
            this.service = service;
            this.bicikl = bicikl;
        }

        public void Execute()
        {
            service.Delete(bicikl.Id);
        }

        public void Unexecute()
        {
            service.Add(bicikl);
        }

        public string GetOpis()
        {
            return "Brisanje trkackog bicikla";
        }
    }
}