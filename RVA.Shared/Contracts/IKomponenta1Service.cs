using RVA.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RVA.Shared.Contracts
{
    [ServiceContract]
    public interface IKomponenta1Service
    {
        [OperationContract]
        List<TrkackiBicikl> GetBicikli();

        [OperationContract]
        List<BiciklistickaTelemetrija> GetTelemetrije();
    }

}
