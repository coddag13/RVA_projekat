using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace RVA.Shared.Models
{
    [DataContract]
    public class TrkackiBicikl
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string Tim { get; set; }
        [DataMember] public string Vozac { get; set; }
        [DataMember] public double Tezina { get; set; }
        [DataMember] public bool Sprinter { get; set; }

        public TrkackiBicikl()
        {
            Id = Guid.NewGuid();
            Tim = string.Empty;
            Vozac = string.Empty;
        }

        public TrkackiBicikl(Guid id, string tim, string vozac, double tezina, bool sprinter)
        {
            Id = id;
            Tim = tim;
            Vozac = vozac;
            Tezina = tezina;
            Sprinter = sprinter;
        }

        public override string ToString()
        {
            return $"{Tim} - {Vozac}";
        }
    }
}