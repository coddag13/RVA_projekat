using RVA.Shared.Models;
using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Commands
{
    public class AddTelemetrijaCommand : IUndoableCommand
    {
        private readonly ITelemetrijaService service;
        private readonly BiciklistickaTelemetrija telemetrija;

        public AddTelemetrijaCommand(ITelemetrijaService service, BiciklistickaTelemetrija telemetrija)
        {
            this.service = service;
            this.telemetrija = telemetrija;
        }

        public void Execute()
        {
            service.Add(telemetrija);
        }

        public void Unexecute()
        {
            service.Delete(telemetrija.BiciklId, telemetrija.VremeOcitavanja);
        }

        public string GetOpis()
        {
            return "Dodavanje biciklisticke telemetrije";
        }
    }
}