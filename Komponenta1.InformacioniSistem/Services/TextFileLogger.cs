using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Komponenta1.InformacioniSistem
{
	public class TextFileLogger : ILogger
	{
        private readonly string filePath;

        public TextFileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public void Log(string message)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string logLine = $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - {message}";
            File.AppendAllText(filePath, logLine + Environment.NewLine);
        }
    }
}
