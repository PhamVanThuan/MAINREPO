using System.Collections.Generic;
using System.Linq;
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
    public class when_no_other_user_can_be_found_for_capability : WithFakes
    {
        private static IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand> domainRuleManager;
        private static UserModel lastUser;
        private static GetActiveUsersWithCapabilityQueryResult newUserWithcapability;
        private static ReturnInstanceToLastUserInCapabilityCommandHandler handler;
        private static ReturnInstanceToLastUserInCapabilityCommand command;
        private static ISystemMessageCollection messages;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IEventRaiser eventRaiser;
        private static IServiceRequestMetadata metadata;
        private static Capability capability;
        private static IWorkflowCaseDataManager dataManager;
        private static int instanceId, userOrgStructureKey;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ADUserDataModel adUser;
        private static List<CapabilityDataModel> capabilities;

        private Establish that = () =>
        {
            adUser = new ADUserDataModel(11111, @"SAHL\testuser", (int)GeneralStatus.Active, string.Empty, string.Empty, string.Empty, 123456);
            serviceQueryRouter = An<IServiceQueryRouter>();
            dataManager = An<IWorkflowCaseDataManager>();
            eventRaiser = An<IEventRaiser>();
            instanceId = 1002;
            capability = Capability.InvoiceApprover;
            userOrgStructureKey = 11100011;
            lastUser = new UserModel("userName", userOrgStructureKey, "some user");
            metadata = new ServiceRequestMetadata();
            serviceCommandRouter = An<IServiceCommandRouter>();
            domainRuleManager = An<IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand>>();
            capabilities = new List<CapabilityDataModel>
            {
                new CapabilityDataModel(222, "Invoice Processor")
            };
            dataManager.WhenToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId)).Return(lastUser);
            dataManager.WhenToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey)).Return(adUser);
            dataManager.WhenToldTo(x => x.GetCapabilitiesForUserOrganisationStructureKey(userOrgStructureKey))
                .Return(capabilities);

            serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param<GetActiveUsersWithCapabilityQuery>.Matches(y => y.CapabilityKey == (int)command.Capability)))
            .Return<GetActiveUsersWithCapabilityQuery>
                ((query) =>
                {
                    IEnumerable<GetActiveUsersWithCapabilityQueryResult> result = new List<GetActiveUsersWithCapabilityQueryResult> { };
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

        private It should_get_the_user_with_provided_capability_who_last_worked_on_the_case = () =>
        {
            dataManager.WasToldTo(x => x.GetLastUserAssignedInCapability((int)capability, instanceId));
        };

        private It should_confirm_that_aduser_is_active = () =>
        {
            dataManager.WasToldTo(x => x.GetADUserByUserOrganisationStructureKey(userOrgStructureKey));
        };

        private It should_get_the_capabilities_for_the_uosKey = () =>
        {
            dataManager.WasToldTo(x => x.GetCapabilitiesForUserOrganisationStructureKey(userOrgStructureKey));
        };

        private It should_get_user_with_provided_capability = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Param<GetActiveUsersWithCapabilityQuery>.Matches(y => y.CapabilityKey == (int)command.Capability)));
        };

        private It should_not_assign_the_case = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<AssignWorkflowCaseCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message
                .ShouldEqual(string.Format("User with UserOrganisationStructureKey {0} could not be assigned to and no other user with CapabilityKey {1} could be found.",
                    userOrgStructureKey, (int)command.Capability
                ));
        };
    }
}