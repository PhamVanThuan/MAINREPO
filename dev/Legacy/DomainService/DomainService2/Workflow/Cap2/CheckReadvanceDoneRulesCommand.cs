namespace DomainService2.Workflow.Cap2
{
    public class CheckReadvanceDoneRulesCommand : RuleDomainServiceCommand
    {
        public CheckReadvanceDoneRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, "ApplicationCap2CheckReadvancePosted")
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}