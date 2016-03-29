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
using SAHL.Services.WorkflowAssignmentDomain.CommandHandlers;
using SAHL.Services.WorkflowAssignmentDomain.Managers;

namespace SAHL.Services.WorkflowAssignmentDomain.Specs.CommandHandlers
{
    public class when_last_user_still_has_capability : WithFakes
    {
        private static IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand> domainRuleManager;
        private static UserModel lastUser;
        private static ReturnInstanceToLastUserInCapabilityCommandHandler handler;
        private static ReturnInstanceToLastUserInCapabilityCommand command;
        private static ISystemMessageCollection messages;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IServiceRequestMetadata metadata;
        private static IWorkflowCaseDataManager dataManager;
        private static int instanceId, userOrgStructureKey;
        private static Capability capability;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ADUserDataModel adUser;
        private static List<CapabilityDataModel> capabilities;

        private Establish that = () =>
        {
            adUser = new ADUserDataModel(111, @"SAHL\HaloUser", 1, string.Empty, string.Empty, string.Empty, 134567);
            messages = SystemMessageCollection.Empty();
            dataManager = An<IWorkflowCaseDataManager>();
            instanceId = 1002;
            capability = Capability.InvoiceProcessor;
            userOrgStructureKey = 11100011;
            capabilities = new List<CapabilityDataModel>
            {
                new CapabilityDataModel(1, "Invoice Processor")
            };
            lastUser = new UserModel("userName", userOrgStructureKey, "some user");
            metadata = new ServiceRequestMetadata();
            command = new ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType.ThirdPartyInvoice, 2, capability, instanceId);
            serviceCommandRouter = An<IServiceCommandRouter>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            domainRuleManager = An<IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand>>();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command)).
                Callback<ISystemMessageCollection>(y => { SystemMessageCollection.Empty(); });
            dataManager.WhenToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId)).Return(lastUser);
            dataManager.WhenToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey))
                .Return(adUser);
            dataManager.WhenToldTo(x => x.GetCapabilitiesForUserOrganisationStructureKey(userOrgStructureKey))
                .Return(capabilities);
            handler = new ReturnInstanceToLastUserInCapabilityCommandHandler(domainRuleManager, dataManager, serviceCommandRouter, serviceQueryRouter);
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

        private It should_confirm_that_aduser_is_active = () =>
        {
            dataManager.WasToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey));
        };

        private It should_confirm_that_the_user_still_has_a_provided_capability = () =>
        {
            dataManager.WasToldTo(x => x.GetCapabilitiesForUserOrganisationStructureKey(userOrgStructureKey));
        };

        private It should_assign_the_case_to_user = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<AssignWorkflowCaseCommand>
                .Matches(y => y.Capability == command.Capability
                    && y.InstanceId == instanceId
                    && y.UserOrganisationStructureKey == userOrgStructureKey), metadata));
        };

        private It should_not_return_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}