using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RVA.Shared.Models;

namespace Komponenta2.Statistika.Interfaces
{
    public interface IPodaciProvider
    {
        List<TrkackiBicikl> GetBicikli();
        List<BiciklistickaTelemetrija> GetTelemetrije();
    }
}
