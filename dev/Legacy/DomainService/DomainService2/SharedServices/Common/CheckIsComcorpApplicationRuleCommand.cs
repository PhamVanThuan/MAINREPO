namespace DomainService2.SharedServices.Common
{
    public class CheckIsComcorpApplicationRuleCommand : RuleDomainServiceCommand
    {
        public CheckIsComcorpApplicationRuleCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, SAHL.Common.Rules.ComcorpApplicationRequired)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}