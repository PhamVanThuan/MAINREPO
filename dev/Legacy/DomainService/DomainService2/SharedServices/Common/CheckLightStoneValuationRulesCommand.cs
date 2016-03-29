namespace DomainService2.SharedServices.Common
{
    public class CheckLightStoneValuationRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckLightStoneValuationRulesCommand(int applicationKey, bool ignoreWarnings)
            : base(ignoreWarnings, "LightStoneValuation")
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}