using System.Collections.Generic;

namespace DomainService2.Workflow.Origination.Valuations
{
    public class GetValuationDataCommand : StandardDomainServiceCommand
    {
        public GetValuationDataCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public Dictionary<string, object> ValuationDataResult
        {
            get;
            set;
        }
    }
}