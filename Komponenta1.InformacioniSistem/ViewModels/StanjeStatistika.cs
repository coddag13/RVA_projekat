using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;

namespace Komponenta1.InformacioniSistem
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