using System;
using System.Collections.Generic;
using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem
{
    public class ValidationService
    {
        public List<string> VratiGreskeBicikla(TrkackiBicikl bicikl)
        {
            List<string> greske = new List<string>();

            if (bicikl == null)
            {
                greske.Add("Bicikl nije unesen.");
                return greske;
            }

            if (string.IsNullOrWhiteSpace(bicikl.Tim))
            {
                greske.Add("Tim je obavezan.");
            }

            if (string.IsNullOrWhiteSpace(bicikl.Vozac))
            {
                greske.Add("Vozac je obavezan.");
            }

            if (bicikl.Tezina <= 0)
            {
                greske.Add("Tezina mora biti veca od nule.");
            }

            return greske;
        }

        public List<string> VratiGreskeTelemetrije(BiciklistickaTelemetrija telemetrija)
        {
            List<string> greske = new List<string>();

            if (telemetrija == null)
            {
                greske.Add("Telemetrija nije unesena.");
                return greske;
            }

            if (telemetrija.BiciklId == Guid.Empty)
            {
                greske.Add("Bicikl Id mora biti validan Guid.");
            }

            if (telemetrija.BrzinaVoznje < 0)
            {
                greske.Add("Brzina voznje ne moze biti negativna.");
            }

            if (telemetrija.PulsVozaca <= 0)
            {
                greske.Add("Puls vozaca mora biti veci od nule.");
            }

            return greske;
        }

        public bool ValidirajBicikl(TrkackiBicikl bicikl)
        {
            return VratiGreskeBicikla(bicikl).Count == 0;
        }

        public bool ValidirajTelemetriju(BiciklistickaTelemetrija telemetrija)
        {
            return VratiGreskeTelemetrije(telemetrija).Count == 0;
        }
    }
}
