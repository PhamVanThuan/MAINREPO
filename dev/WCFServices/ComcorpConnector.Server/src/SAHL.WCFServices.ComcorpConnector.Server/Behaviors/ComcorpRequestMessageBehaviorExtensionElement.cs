using System;
using System.ServiceModel.Configuration;

namespace SAHL.WCFServices.ComcorpConnector.Server.Behaviors
{
    public class ComcorpRequestMessageBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ComcorpRequestMessageBehavior();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(ComcorpRequestMessageBehavior);
            }
        }
    }
}