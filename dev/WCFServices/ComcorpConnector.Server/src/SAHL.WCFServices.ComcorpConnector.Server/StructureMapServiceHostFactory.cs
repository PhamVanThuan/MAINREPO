using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SAHL.WCFServices.ComcorpConnector.Server
{
    public class StructureMapServiceHostFactory : ServiceHostFactory
    {
        public StructureMapServiceHostFactory()
        {
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new StructureMapServiceHost(serviceType, baseAddresses);
        }
    }
}