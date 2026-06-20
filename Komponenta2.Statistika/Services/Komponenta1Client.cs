using RVA.Shared.Contracts;
using RVA.Shared.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Komponenta2.Statistika.Services
{
    public class Komponenta1Client : IDisposable
    {
        private readonly ChannelFactory<IKomponenta1Service> channelFactory;
        private IKomponenta1Service proxy;

        public Komponenta1Client()
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            var endpoint = new EndpointAddress("net.tcp://localhost:8000/Komponenta1Service");
            channelFactory = new ChannelFactory<IKomponenta1Service>(binding, endpoint);
        }

        public List<TrkackiBicikl> GetBicikli()
        {
            proxy = channelFactory.CreateChannel();
            try
            {
                return proxy.GetBicikli();
            }
            catch (Exception)
            {
                ((ICommunicationObject)proxy).Abort();
                throw;
            }
        }

        public List<BiciklistickaTelemetrija> GetTelemetrije()
        {
            proxy = channelFactory.CreateChannel();
            try
            {
                return proxy.GetTelemetrije();
            }
            catch (Exception)
            {
                ((ICommunicationObject)proxy).Abort();
                throw;
            }
        }

        public void Dispose()
        {
            channelFactory?.Close();
        }
    }
}