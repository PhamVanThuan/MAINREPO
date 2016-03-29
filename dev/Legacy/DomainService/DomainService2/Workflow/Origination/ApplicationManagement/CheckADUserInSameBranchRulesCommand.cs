using SAHL.Common;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckADUserInSameBranchRulesCommand : RuleSetDomainServiceCommand
    {
        public CheckADUserInSameBranchRulesCommand(int applicationKey, string adUserName, bool ignoreWarnings)
            : base(ignoreWarnings, RuleSets.ApplicationManagementCheckUserOrganisationalStructure)
        {
            this.ADUserName = adUserName;
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }

        public string ADUserName { get; set; }
    }
}