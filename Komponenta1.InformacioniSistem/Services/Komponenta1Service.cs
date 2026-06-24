using RVA.Shared.Contracts;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.ServiceModel;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Komponenta1Service : IKomponenta1Service
    {
        private readonly IBiciklService biciklService;
        private readonly ITelemetrijaService telemetrijaService;

        public Komponenta1Service(IBiciklService biciklService, ITelemetrijaService telemetrijaService)
        {
            this.biciklService = biciklService;
            this.telemetrijaService = telemetrijaService;
        }

        public List<TrkackiBicikl> GetBicikli()
        {
            return biciklService.GetAll();
        }

        public List<BiciklistickaTelemetrija> GetTelemetrije()
        {
            return telemetrijaService.GetAll();
        }
    }
}
