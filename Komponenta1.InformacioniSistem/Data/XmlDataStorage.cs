using RVA.Shared.Models;
using System.IO;
using System.Xml.Serialization;

using Komponenta1.InformacioniSistem.Interfaces;
namespace Komponenta1.InformacioniSistem.Data
{
    public class XmlDataStorage : IDataStorage
    {
        private readonly string filePath;

        public XmlDataStorage(string filePath)
        {
            this.filePath = filePath;
        }

        public DataStore Load()
        {
            if (!File.Exists(filePath))
            {
                return new DataStore();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(DataStore));

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return serializer.Deserialize(stream) as DataStore ?? new DataStore();
            }
        }

        public void Save(DataStore dataStore)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(DataStore));

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, dataStore);
            }
        }
    }
}
