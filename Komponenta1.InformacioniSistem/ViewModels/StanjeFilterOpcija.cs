using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem.ViewModels
{
    public class StanjeFilterOpcija
    {
        public string Naziv { get; set; }

        public StanjeVoznje? Stanje { get; set; }

        public StanjeFilterOpcija()
        {
            Naziv = string.Empty;
        }

        public StanjeFilterOpcija(string naziv, StanjeVoznje? stanje)
        {
            Naziv = naziv;
            Stanje = stanje;
        }
    }
}
