using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.Komponenta1
{
    public class DataStore
    {
        public List<TrkackiBicikl> Bicikli { get; set; }
        public List<BiciklistickaTelemetrija> Telemetrije { get; set; }

        public DataStore()
        {
            Bicikli = new List<TrkackiBicikl>();
            Telemetrije = new List<BiciklistickaTelemetrija>();
        }
    }
}
