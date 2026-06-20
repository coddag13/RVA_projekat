using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Models;

namespace Komponenta2.Statistika.Interfaces
{
    public interface ICsvExporter
    {
        void Export(StatistikaRezultat rezultat, string path);
    }
}