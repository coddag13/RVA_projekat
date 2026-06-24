using System.Collections.Generic;
using RVA.Shared.Models;

namespace Komponenta1.InformacioniSistem.Interfaces
{
    public interface IValidationService
    {
        List<string> VratiGreskeBicikla(TrkackiBicikl bicikl);

        List<string> VratiGreskeTelemetrije(BiciklistickaTelemetrija telemetrija);

        bool ValidirajBicikl(TrkackiBicikl bicikl);

        bool ValidirajTelemetriju(BiciklistickaTelemetrija telemetrija);
    }
}
