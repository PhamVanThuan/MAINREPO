namespace DomainService2.Workflow.Cap2
{
    public class OnCompleteActivity_Promotion_ClientCommand : StandardDomainServiceCommand
    {
        public OnCompleteActivity_Promotion_ClientCommand(int applicationKey, int applicationDetailKey, int promotion,
                                                            int capStatusKey, int capPaymentOptionKey, string name)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationDetailKey = applicationDetailKey;
            this.Promotion = promotion;
            this.CapStatusKey = capStatusKey;
            this.CapPaymentOptionKey = capPaymentOptionKey;
            this.Name = name;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public int ApplicationDetailKey
        {
            get;
            protected set;
        }

        public int Promotion
        {
            get;
            protected set;
        }

        public int CapStatusKey
        {
            get;
            protected set;
        }

        public int CapPaymentOptionKey
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }
    }
}