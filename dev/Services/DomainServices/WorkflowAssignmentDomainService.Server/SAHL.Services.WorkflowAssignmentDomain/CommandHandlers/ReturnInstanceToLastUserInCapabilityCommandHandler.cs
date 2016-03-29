using System.Linq;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;
using SAHL.Services.WorkflowAssignmentDomain.Managers;
using SAHL.Services.WorkflowAssignmentDomain.Rules;

namespace SAHL.Services.WorkflowAssignmentDomain.CommandHandlers
{
    public class ReturnInstanceToLastUserInCapabilityCommandHandler : IServiceCommandHandler<ReturnInstanceToLastUserInCapabilityCommand>
    {
        private IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand> domainRuleManager;
        private readonly IWorkflowCaseDataManager dataManager;
        private readonly IServiceCommandRouter serviceCommandRouter;
        private readonly IServiceQueryRouter serviceQueryRouter;

        public ReturnInstanceToLastUserInCapabilityCommandHandler(IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand> domainRuleManager
            , IWorkflowCaseDataManager dataManager
            , IServiceCommandRouter serviceCommandRouter
            , IServiceQueryRouter serviceQueryRouter)
        {
            this.dataManager = dataManager;
            this.serviceCommandRouter = serviceCommandRouter;
            this.domainRuleManager = domainRuleManager;
            this.serviceQueryRouter = serviceQueryRouter;
            domainRuleManager.RegisterRule(new UserWithCapabilityMustHavePreviouslyBeenAssignedToTheInstanceRule(dataManager));
        }

        public ISystemMessageCollection HandleCommand(ReturnInstanceToLastUserInCapabilityCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            domainRuleManager.ExecuteRules(messages, command);
            if (messages.HasErrors)
            {
                return messages;
            }
            var lastUserInCapability = dataManager.GetLastUserAssignedInCapability((int)command.Capability, command.InstanceId);
            var userOrgStructureKey = lastUserInCapability.UserOrganisationStructureKey;

            var adUserToAssignTo = dataManager.GetADUserByUserOrganisationStructureKey(userOrgStructureKey);

            bool userCannotBeAssignedTo = false;
            if (adUserToAssignTo == null)
            {
                //no user could be found
                userCannotBeAssignedTo = true;
            }
            else
            {
                if (adUserToAssignTo.GeneralStatusKey != (int)GeneralStatus.Active)
                {
                    //user is not active
                    userCannotBeAssignedTo = true;
                }
                if (!userCannotBeAssignedTo)
                {
                    var capabilitiesForUserOrgStructure = dataManager.GetCapabilitiesForUserOrganisationStructureKey(userOrgStructureKey);
                    if (!capabilitiesForUserOrgStructure.Where(x => x.CapabilityKey == (int)command.Capability).Any())
                    {
                        //user org structure no longer has required capability
                        userCannotBeAssignedTo = true;
                    }
                }
            }

            if (userCannotBeAssignedTo)
            {
                var getActiveUsersWithCapabilityQuery = new GetActiveUsersWithCapabilityQuery((int)command.Capability);
                serviceQueryRouter.HandleQuery(getActiveUsersWithCapabilityQuery);
                if (getActiveUsersWithCapabilityQuery.Result != null && getActiveUsersWithCapabilityQuery.Result.Results.Count() > 0)
                {
                    var userWithCapability = getActiveUsersWithCapabilityQuery.Result.Results.FirstOrDefault();
                    userOrgStructureKey = userWithCapability.UserOrganisationStructureKey;
                }
                else
                {
                    messages.AddMessage(
                        new SystemMessage(string.Format("User with UserOrganisationStructureKey {0} could not be assigned to and no other user with CapabilityKey {1} could be found.",
                        userOrgStructureKey, (int)command.Capability), SystemMessageSeverityEnum.Error));
                }
            }
            if (!messages.HasErrors)
            {
                serviceCommandRouter.HandleCommand(new AssignWorkflowCaseCommand(command.GenericKeyTypeKey, command.GenericKey, command.InstanceId, userOrgStructureKey,
                    command.Capability), metadata);
            }
            return messages;
        }
    }
}