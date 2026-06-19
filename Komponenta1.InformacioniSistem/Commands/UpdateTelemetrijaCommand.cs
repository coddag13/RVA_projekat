using RVA.Shared.Models;
namespace Komponenta1.InformacioniSistem
{
    public class UpdateTelemetrijaCommand : IUndoableCommand
    {
        private readonly ITelemetrijaService service;
        private readonly BiciklistickaTelemetrija oldTelemetrija;
        private readonly BiciklistickaTelemetrija newTelemetrija;

        public UpdateTelemetrijaCommand(
            ITelemetrijaService service,
            BiciklistickaTelemetrija oldTelemetrija,
            BiciklistickaTelemetrija newTelemetrija)
        {
            this.service = service;
            this.oldTelemetrija = oldTelemetrija;
            this.newTelemetrija = newTelemetrija;
        }

        public void Execute()
        {
            service.Update(newTelemetrija);
        }

        public void Unexecute()
        {
            service.Update(oldTelemetrija);
        }

        public string GetOpis()
        {
            return "Izmjena biciklisticke telemetrije";
        }
    }
}