using System;
using RVA.Shared.Models;
using RVA.Shared;

namespace Komponenta1.InformacioniSistem
{
    public static class SeedDataHelper
    {
        public static void DodajPocetnePodatkeAkoJePrazno(DataStore dataStore)
        {
            if (dataStore == null)
            {
                throw new ArgumentNullException(nameof(dataStore));
            }

            if (dataStore.Bicikli.Count > 0 || dataStore.Telemetrije.Count > 0)
            {
                return;
            }

            TrkackiBicikl prviBicikl = new TrkackiBicikl(
                Guid.NewGuid(),
                "Team Sprint",
                "Marko Markovic",
                7.2,
                true);

            TrkackiBicikl drugiBicikl = new TrkackiBicikl(
                Guid.NewGuid(),
                "Mountain Pro",
                "Nikola Nikolic",
                8.1,
                false);

            TrkackiBicikl treciBicikl = new TrkackiBicikl(
                Guid.NewGuid(),
                "Road Masters",
                "Petar Petrovic",
                6.9,
                true);

            dataStore.Bicikli.Add(prviBicikl);
            dataStore.Bicikli.Add(drugiBicikl);
            dataStore.Bicikli.Add(treciBicikl);

            dataStore.Telemetrije.Add(new BiciklistickaTelemetrija(
                prviBicikl.Id,
                DateTime.Now.AddMinutes(-30),
                42.5,
                145,
                StanjeVoznje.Stabilna));

            dataStore.Telemetrije.Add(new BiciklistickaTelemetrija(
                drugiBicikl.Id,
                DateTime.Now.AddMinutes(-20),
                35.8,
                168,
                StanjeVoznje.VelikiNapori));

            dataStore.Telemetrije.Add(new BiciklistickaTelemetrija(
                treciBicikl.Id,
                DateTime.Now.AddMinutes(-10),
                28.3,
                181,
                StanjeVoznje.Umor));
        }
    }
}