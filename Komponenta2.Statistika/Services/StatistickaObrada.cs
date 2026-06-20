using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;

namespace Komponenta2.Statistika.Services
{
    public class StatistickaObrada
    {
        private IStatistickaMetoda strategy;

        public StatistickaObrada(IStatistickaMetoda strategy)
        {
            this.strategy = strategy;
        }

        public void SetStrategy(IStatistickaMetoda strategy)
        {
            this.strategy = strategy;
        }

        public StatistikaRezultat Obradi(List<BiciklStatistika> podaci)
        {
            if (strategy == null)
                return new StatistikaRezultat();

            return strategy.Izracunaj(podaci);
        }
    }
}
