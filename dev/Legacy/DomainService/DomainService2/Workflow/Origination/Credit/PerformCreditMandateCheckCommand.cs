using System.Collections.Generic;

namespace DomainService2.Workflow.Origination.Credit
{
    public class PerformCreditMandateCheckCommand : StandardDomainServiceCommand
    {
        public PerformCreditMandateCheckCommand(int applicationKey, long instanceID, List<string> loadBalanceStates, bool loadBalanceIncludeStates, bool loadBalance1stPass, bool loadBalance2ndPass)
        {
            this.ApplicationKey = applicationKey;
            this.InstanceID = instanceID;
            this.LoadBalanceStates = loadBalanceStates;
            this.LoadBalanceIncludeStates = loadBalanceIncludeStates;
            this.LoadBalance1stPass = loadBalance1stPass;
            this.LoadBalance2ndPass = loadBalance2ndPass;
        }

        public int ApplicationKey { get; protected set; }

        public long InstanceID { get; protected set; }

        public List<string> LoadBalanceStates { get; protected set; }

        public bool LoadBalanceIncludeStates { get; protected set; }

        public bool LoadBalance1stPass { get; protected set; }

        public bool LoadBalance2ndPass { get; protected set; }
    }
}