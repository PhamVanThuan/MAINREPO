using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Collections.ObjectModel;
using System.ServiceModel.Description;

namespace SAHL.WCFServices.ComcorpConnector.Server
{
    public class StructureMapServiceBehavior : IServiceBehavior
    {
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.InstanceProvider =
                            new StructureMapInstanceProvider(serviceDescription.ServiceType);
                    }
                }
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, 
                                         Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            // Nothing to do
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Nothing to do
        }
    }
}
