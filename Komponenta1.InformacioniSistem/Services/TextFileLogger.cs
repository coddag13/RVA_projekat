using System;
using System.IO;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Services
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
