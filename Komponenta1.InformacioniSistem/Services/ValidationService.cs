using System;
using System.Collections.Generic;
using RVA.Shared.Models;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Services
{
    public class ValidationService : IValidationService
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
            else if (bicikl.Tim.Trim().Length < 2 || bicikl.Tim.Trim().Length > 60)
            {
                greske.Add("Tim mora imati izmedju 2 i 60 karaktera.");
            }

            if (string.IsNullOrWhiteSpace(bicikl.Vozac))
            {
                greske.Add("Vozac je obavezan.");
            }
            else if (bicikl.Vozac.Trim().Length < 2 || bicikl.Vozac.Trim().Length > 60)
            {
                greske.Add("Vozac mora imati izmedju 2 i 60 karaktera.");
            }
            else if (SadrziCifru(bicikl.Vozac))
            {
                greske.Add("Ime vozaca ne smije sadrzati cifre.");
            }

            if (bicikl.Tezina <= 0)
            {
                greske.Add("Tezina mora biti veca od nule.");
            }
            else if (bicikl.Tezina < 4 || bicikl.Tezina > 25)
            {
                greske.Add("Tezina bicikla mora biti izmedju 4 i 25 kg.");
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

            if (telemetrija.VremeOcitavanja > DateTime.Now.AddMinutes(1))
            {
                greske.Add("Vrijeme ocitavanja ne moze biti u buducnosti.");
            }

            if (telemetrija.BrzinaVoznje < 0)
            {
                greske.Add("Brzina voznje ne moze biti negativna.");
            }
            else if (telemetrija.BrzinaVoznje > 120)
            {
                greske.Add("Brzina voznje mora biti manja ili jednaka 120 km/h.");
            }

            if (telemetrija.PulsVozaca <= 0)
            {
                greske.Add("Puls vozaca mora biti veci od nule.");
            }
            else if (telemetrija.PulsVozaca < 30 || telemetrija.PulsVozaca > 240)
            {
                greske.Add("Puls vozaca mora biti izmedju 30 i 240.");
            }

            if (!Enum.IsDefined(typeof(StanjeVoznje), telemetrija.Stanje))
            {
                greske.Add("Stanje voznje nije validno.");
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

        private bool SadrziCifru(string tekst)
        {
            foreach (char karakter in tekst)
            {
                if (char.IsDigit(karakter))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
