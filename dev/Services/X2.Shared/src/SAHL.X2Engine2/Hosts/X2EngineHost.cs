using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Hosts
{
    public class X2EngineHost : IX2EngineHost
    {
        private IX2Engine x2Engine;

        public X2EngineHost(IX2Engine x2Engine)
        {
            this.x2Engine = x2Engine;
        }

        public void Start()
        {
            this.x2Engine.Initialise();
        }

        public X2Response SendCreateWorkFlowInstanceRequest(X2CreateInstanceRequest request)
        {
            return this.x2Engine.ReceiveRequest<X2CreateInstanceRequest>(request);
        }

        public X2Response SendCreateWorkFlowInstanceWithCompleteRequest(X2CreateInstanceWithCompleteRequest request)
        {
            return this.x2Engine.ReceiveRequest<X2CreateInstanceWithCompleteRequest>(request);
        }


        public X2Response SendActivityStartRequest(X2RequestForExistingInstance request)
        {
            return this.x2Engine.ReceiveRequest<X2RequestForExistingInstance>(request);
        }

        public X2Response SendActivityCompleteRequest(X2RequestForExistingInstance request)
        {
            return this.x2Engine.ReceiveRequest<X2RequestForExistingInstance>(request);
        }

        public X2Response SendActivityCancelRequest(X2RequestForExistingInstance request)
        {
            return this.x2Engine.ReceiveRequest<X2RequestForExistingInstance>(request);
        }

        public X2Response SendExternalActivityRequest(X2ExternalActivityRequest request)
        {
            return this.x2Engine.ReceiveExternalActivityRequest(request);
        }

        public void Stop()
        {
        }
    }
}