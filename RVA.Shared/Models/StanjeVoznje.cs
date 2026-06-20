using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace RVA.Shared.Models
{
    [DataContract]
    public enum StanjeVoznje
    {
        [EnumMember] Stabilna,
        [EnumMember] VelikiNapori,
        [EnumMember] Umor,
        [EnumMember] Odustajanje
    }
}
