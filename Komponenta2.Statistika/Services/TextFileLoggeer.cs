using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponenta2.Statistika.Interfaces;
using System.IO;

namespace Komponenta2.Statistika.Services
{
    public class TextFileLogger : ILogger
    {
        private readonly string putanja;
        private readonly object zaKljucavanje = new object();

        public TextFileLogger(string putanja)
        {
            this.putanja = putanja;

            string folder = Path.GetDirectoryName(putanja);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public void Log(string poruka)
        {
            lock (zaKljucavanje)
            {
                string linija = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {poruka}";
                File.AppendAllText(putanja, linija + Environment.NewLine);
            }
        }
    }
}
