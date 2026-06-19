using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Projekat.Komponenta1
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
            XmlSerializer serializer = new XmlSerializer(typeof(DataStore));

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, dataStore);
            }
        }
    }
}
