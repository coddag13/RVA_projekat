using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Models;

namespace Komponenta2.Statistika.Interfaces
{
    public interface IStatistickaMetoda
    {
        string Naziv { get; }
        StatistikaRezultat Izracunaj(List<BiciklStatistika> podaci);
    }
}
