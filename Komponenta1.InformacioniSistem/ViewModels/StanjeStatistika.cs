using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem.ViewModels
{
    public class StanjeStatistika
    {
        public StanjeVoznje Stanje { get; set; }
        public int BrojInstanci { get; set; }

        public StanjeStatistika()
        {
        }

        public StanjeStatistika(StanjeVoznje stanje, int brojInstanci)
        {
            Stanje = stanje;
            BrojInstanci = brojInstanci;
        }
    }
}