using RVA.Shared.Models;
namespace Komponenta1.InformacioniSistem
{
    public class DeleteTelemetrijaCommand : IUndoableCommand
    {
        private readonly ITelemetrijaService service;
        private readonly BiciklistickaTelemetrija telemetrija;

        public DeleteTelemetrijaCommand(ITelemetrijaService service, BiciklistickaTelemetrija telemetrija)
        {
            this.service = service;
            this.telemetrija = telemetrija;
        }

        public void Execute()
        {
            service.Delete(telemetrija.BiciklId, telemetrija.VremeOcitavanja);
        }

        public void Unexecute()
        {
            service.Add(telemetrija);
        }

        public string GetOpis()
        {
            return "Brisanje biciklisticke telemetrije";
        }
    }
}