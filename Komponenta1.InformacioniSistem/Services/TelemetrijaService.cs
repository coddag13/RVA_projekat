using System;
using System.Collections.Generic;
using System.Linq;

namespace Projekat.Komponenta1
{
    public class TelemetrijaService : ITelemetrijaService
    {
        private readonly DataStore storage;
        private readonly ILogger logger;
        private readonly SimulatorStanjaVoznje simulatorStanja;

        public TelemetrijaService(DataStore storage, ILogger logger, SimulatorStanjaVoznje simulatorStanja)
        {
            this.storage = storage;
            this.logger = logger;
            this.simulatorStanja = simulatorStanja;
        }

        public List<BiciklistickaTelemetrija> GetAll()
        {
            return storage.Telemetrije;
        }

        public List<BiciklistickaTelemetrija> Search(double brzinaVoznje, double pulsVozaca, StanjeVoznje stanje)
        {
            return storage.Telemetrije
                .Where(t =>
                    (brzinaVoznje <= 0 || t.BrzinaVoznje == brzinaVoznje) &&
                    (pulsVozaca <= 0 || t.PulsVozaca == pulsVozaca) &&
                    t.Stanje == stanje)
                .ToList();
        }

        public void Add(BiciklistickaTelemetrija telemetrija)
        {
            if (telemetrija == null)
            {
                throw new ArgumentNullException(nameof(telemetrija));
            }

            storage.Telemetrije.Add(telemetrija);
            logger.Log($"Dodata biciklisticka telemetrija: {telemetrija}");
        }

        public void Update(BiciklistickaTelemetrija telemetrija)
        {
            if (telemetrija == null)
            {
                throw new ArgumentNullException(nameof(telemetrija));
            }

            BiciklistickaTelemetrija existing = storage.Telemetrije.FirstOrDefault(t =>
                t.BiciklId == telemetrija.BiciklId &&
                t.VremeOcitavanja == telemetrija.VremeOcitavanja);

            if (existing == null)
            {
                throw new InvalidOperationException("Biciklisticka telemetrija nije pronadjena.");
            }

            existing.BrzinaVoznje = telemetrija.BrzinaVoznje;
            existing.PulsVozaca = telemetrija.PulsVozaca;
            existing.Stanje = telemetrija.Stanje;

            logger.Log($"Izmijenjena biciklisticka telemetrija: {existing}");
        }

        public void Delete(Guid biciklId, DateTime vremeOcitavanja)
        {
            BiciklistickaTelemetrija existing = storage.Telemetrije.FirstOrDefault(t =>
                t.BiciklId == biciklId &&
                t.VremeOcitavanja == vremeOcitavanja);

            if (existing == null)
            {
                throw new InvalidOperationException("Biciklisticka telemetrija nije pronadjena.");
            }

            storage.Telemetrije.Remove(existing);
            logger.Log($"Obrisana biciklisticka telemetrija: {existing}");
        }

        public void SimulirajPromjenuStanja(BiciklistickaTelemetrija telemetrija)
        {
            if (telemetrija == null)
            {
                throw new ArgumentNullException(nameof(telemetrija));
            }

            simulatorStanja.PromijeniStanje(telemetrija);
            logger.Log($"Promijenjeno stanje telemetrije: {telemetrija}");
        }
    }
}