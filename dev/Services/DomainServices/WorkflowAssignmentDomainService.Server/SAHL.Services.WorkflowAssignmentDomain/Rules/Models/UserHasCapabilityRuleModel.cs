using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
namespace SAHL.Services.WorkflowAssignmentDomain.Rules.Models
{
    public class UserHasCapabilityRuleModel : IUserOrganizationStructure
    {
        public int UserOrganisationStructureKey { get; private set; }
        public int CapabilityKey { get; private set; }

        public UserHasCapabilityRuleModel(int userOrganisationStructureKey, int capabilityKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.CapabilityKey = capabilityKey;
        }
    }
}