using RVA.Shared.Models;
using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Commands
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