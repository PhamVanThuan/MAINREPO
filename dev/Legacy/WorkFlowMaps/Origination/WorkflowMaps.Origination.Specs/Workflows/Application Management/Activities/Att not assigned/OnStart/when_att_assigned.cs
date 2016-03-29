using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Att_not_assigned.OnStart
{
    [Subject("Activity => Att_not_assigned => OnStart")]
    internal class when_att_assigned : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement client;

        private Establish context = () =>
        {
            result = true;

            client = An<IApplicationManagement>();
            client.WhenToldTo(x => x.HasApplicationRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<int>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Att_not_assigned(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_application_has_conveyance_attorney = () =>
        {
            client.WasToldTo(x => x.HasApplicationRole((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferRoleTypes.ConveyanceAttorney));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}