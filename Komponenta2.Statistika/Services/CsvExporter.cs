using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using Komponenta2.Statistika.Models;
using System.IO;

namespace Komponenta2.Statistika.Services
{
    public class CsvExporter : ICsvExporter
    {
        public void Export(StatistikaRezultat rezultat, string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Metoda;{rezultat.NazivMetode}");
            sb.AppendLine();
            sb.AppendLine("Opis;Vrednost");

            foreach (var stavka in rezultat.Stavke)
            {
                sb.AppendLine($"{stavka.Opis};{stavka.Vrednost:F2}");
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
