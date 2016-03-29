using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;
using SAHL.Services.WorkflowAssignmentDomain.CommandHandlers;
using SAHL.Services.WorkflowAssignmentDomain.Managers;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.CommandHandlers
{
    public class when_last_user_is_no_longer_active : WithFakes
    {
        private static IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand> domainRuleManager;
        private static UserModel lastUser;
        private static GetActiveUsersWithCapabilityQueryResult newUserWithcapability;
        private static ReturnInstanceToLastUserInCapabilityCommandHandler handler;
        private static ReturnInstanceToLastUserInCapabilityCommand command;
        private static ISystemMessageCollection messages;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IServiceRequestMetadata metadata;
        private static IWorkflowCaseDataManager dataManager;
        private static long instanceId;
        private static int userOrgStructureKey;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ADUserDataModel adUser;
        private static Capability capability;

        private Establish that = () =>
        {
            adUser = new ADUserDataModel(11111, @"SAHL\testuser", (int)GeneralStatus.Inactive, string.Empty, string.Empty, string.Empty, 123456);
            serviceQueryRouter = An<IServiceQueryRouter>();
            messages = SystemMessageCollection.Empty();
            dataManager = An<IWorkflowCaseDataManager>();
            instanceId = 1002L;
            capability = Capability.InvoiceProcessor;
            userOrgStructureKey = 11100011;
            lastUser = new UserModel("userName", userOrgStructureKey, "some user");
            metadata = new ServiceRequestMetadata();
            serviceCommandRouter = An<IServiceCommandRouter>();
            domainRuleManager = An<IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand>>();
            dataManager.WhenToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId)).Return(lastUser);
            dataManager.WhenToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey)).Return(adUser);
            newUserWithcapability = new GetActiveUsersWithCapabilityQueryResult { FullName = "tom", UserName = "tom", UserOrganisationStructureKey = 10011 };
            serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param<GetActiveUsersWithCapabilityQuery>.Matches(y => y.CapabilityKey == (int)command.Capability)))
            .Return<GetActiveUsersWithCapabilityQuery>
                ((query) =>
                {
                    IEnumerable<GetActiveUsersWithCapabilityQueryResult> result = new List<GetActiveUsersWithCapabilityQueryResult> { newUserWithcapability };
                    ServiceQueryResult<GetActiveUsersWithCapabilityQueryResult> serviceQueryResult = new ServiceQueryResult<GetActiveUsersWithCapabilityQueryResult>(result);
                    query.Result = serviceQueryResult;
                    return SystemMessageCollection.Empty();
                }
                );
            handler = new ReturnInstanceToLastUserInCapabilityCommandHandler(domainRuleManager, dataManager, serviceCommandRouter, serviceQueryRouter);
            command = new ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType.ThirdPartyInvoice, 2, capability, instanceId);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_execute_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<SystemMessageCollection>(), command));
        };

        private It should_get_the_user_with_proveded_capability_who_last_touched_the_case = () =>
        {
            dataManager.WasToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId));
        };

        private It should_confirm_that_aduser_is_no_longer_active = () =>
        {
            dataManager.WasToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey));
        };

        private It should_not_need_to_confirm_if_the_user_has_the_provided_capability = () =>
        {
            dataManager.WasNotToldTo(x => x.GetCapabilitiesForUserOrganisationStructureKey(userOrgStructureKey));
        };

        private It should_get_user_with_the_provided_capability = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetActiveUsersWithCapabilityQuery>.Matches(y => y.CapabilityKey == (int)command.Capability)));
        };

        private It should_assign_the_case_to_the_new_user = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<AssignWorkflowCaseCommand>
                .Matches(y => y.Capability == command.Capability
                    && y.InstanceId == instanceId
                    && y.UserOrganisationStructureKey == newUserWithcapability.UserOrganisationStructureKey), metadata));
        };

        private It should_not_return_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}