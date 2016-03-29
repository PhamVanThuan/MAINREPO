using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.WorkflowAssignmentDomain.Managers.Statements;

namespace SAHL.Services.WorkflowAssignmentDomain.Managers
{
    public class WorkflowCaseDataManager : IWorkflowCaseDataManager
    {
        private readonly IDbFactory dbFactory;

        public WorkflowCaseDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public void AssignWorkflowCase(AssignWorkflowCaseCommand command)
        {
            using (var context = dbFactory.NewDb().InAppContext())
            {
                var statement = new AssignWorkflowCaseStatement(command.InstanceId, command.UserOrganisationStructureKey, (int)command.Capability);
                context.ExecuteNonQuery(statement);
                context.Complete();
            }
        }

        public IEnumerable<CapabilityDataModel> GetCapabilitiesForUserOrganisationStructureKey(int userOrganisationStructureKey)
        {
            using (var context = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetCapabilitiesForUserOrganisationStructureKeyStatement(userOrganisationStructureKey);
                var results = context.Select(statement).ToList(); //check if ToList needed
                return results;
            }
        }

        public UserModel GetLastUserAssignedInCapability(int capabilityKey, long instanceId)
        {
            using (var context = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetLastAssignedUserByCapabilityForInstanceStatement(instanceId, capabilityKey);
                var results = context.Select(statement).FirstOrDefault();
                return results;
            }
        }

        public ADUserDataModel GetADUserByUserOrganisationStructureKey(int userOrganisationStructureKey)
        {
            using (var context = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var statement = new GetADUserByOrganisationStructureKeyStatement(userOrganisationStructureKey);
                return context.Select<ADUserDataModel>(statement).FirstOrDefault();
            }
        }
    }
}