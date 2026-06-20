using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Komponenta2.Statistika.Models;
using RVA.Shared.Models;


namespace Komponenta2.Statistika.Interfaces
{
    public interface IBiciklStatistikaAdapter
    {
        List<BiciklStatistika> Adapt(List<TrkackiBicikl> bicikli, List<BiciklistickaTelemetrija> telemetrije);
    }
}