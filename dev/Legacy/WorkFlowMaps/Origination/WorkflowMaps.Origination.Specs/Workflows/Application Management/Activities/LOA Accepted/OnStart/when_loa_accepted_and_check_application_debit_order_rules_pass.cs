using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.LOA_Accepted.OnStart
{
    [Subject("Activity => LOA_Accepted => OnStart")]
    internal class when_loa_accepted_and_check_application_debit_order_rules_pass : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            appMan = An<IApplicationManagement>();
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            appMan.WhenToldTo(x => x.CheckApplicationDebitOrderCollectionRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_LOA_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_application_debit_order_collection_rules = () =>
        {
            appMan.WasToldTo(x => x.CheckApplicationDebitOrderCollectionRule((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}