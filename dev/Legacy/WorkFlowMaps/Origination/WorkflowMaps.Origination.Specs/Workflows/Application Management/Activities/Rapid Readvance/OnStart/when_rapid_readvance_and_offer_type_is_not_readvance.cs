using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Rapid_Readvance.OnStart
{
    [Subject("Activity => Rapid_Readvance => OnStart")]
    internal class when_rapid_readvance_and_offer_type_is_not_readvance : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment assignment;
        private static IFL fl;

        private Establish context = () =>
        {
            result = true;
            workflowData.OfferTypeKey = (int)SAHL.Common.Globals.OfferTypes.FurtherAdvance;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            ((InstanceDataStub)instanceData).ID = 2;

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rapid_Readvance(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_check_rapid_should_go_to_credit_rules = () =>
        {
            fl.WasNotToldTo(x => x.CheckRapidShouldGotoCreditRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}