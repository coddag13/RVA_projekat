using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RVA.Shared.Models;


namespace Komponenta2.Statistika.Helpers
{
    public static class SeedDataHelper
    {
        public static (List<TrkackiBicikl>, List<BiciklistickaTelemetrija>) DobijDefaultPodatke()
        {
            var bicikl1 = new TrkackiBicikl(Guid.NewGuid(), "Default Team A", "Default Vozac 1", 7.0, true);
            var bicikl2 = new TrkackiBicikl(Guid.NewGuid(), "Default Team B", "Default Vozac 2", 8.0, false);
            var bicikl3 = new TrkackiBicikl(Guid.NewGuid(), "Default Team C", "Default Vozac 3", 7.5, true);

            var bicikli = new List<TrkackiBicikl> { bicikl1, bicikl2, bicikl3 };

            var telemetrije = new List<BiciklistickaTelemetrija>
            {
                new BiciklistickaTelemetrija(bicikl1.Id, DateTime.Now.AddMinutes(-30), 40.0, 150, StanjeVoznje.Stabilna),
                new BiciklistickaTelemetrija(bicikl2.Id, DateTime.Now.AddMinutes(-20), 32.5, 165, StanjeVoznje.VelikiNapori),
                new BiciklistickaTelemetrija(bicikl3.Id, DateTime.Now.AddMinutes(-10), 28.0, 180, StanjeVoznje.Umor)
            };

            return (bicikli, telemetrije);
        }
    }
}
