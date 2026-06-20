using System;
using RVA.Shared.Models;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Komponenta1.InformacioniSistem
{
    public class JsonDataStorage : IDataStorage
    {
        private readonly string filePath;

        public JsonDataStorage(string filePath)
        {
            this.filePath = filePath;
        }

        public DataStore Load()
        {
            if (!File.Exists(filePath))
            {
                return new DataStore();
            }

            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new DataStore();
            }

            return JsonSerializer.Deserialize<DataStore>(json) ?? new DataStore();
        }

        public void Save(DataStore dataStore)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(dataStore, options);
            File.WriteAllText(filePath, json);
        }
    }
}
