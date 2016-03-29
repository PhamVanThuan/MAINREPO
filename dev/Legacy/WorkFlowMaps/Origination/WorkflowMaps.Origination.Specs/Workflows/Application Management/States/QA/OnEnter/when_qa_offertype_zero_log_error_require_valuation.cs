using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.QA.OnEnter
{
    [Subject("State => QA => OnEnter")]
    internal class when_qa_offertype_zero_log_error_require_valuation : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon client;

        private Establish context = () =>
        {
            result = false;
            workflowData.OfferTypeKey = 0;
            workflowData.RequireValuation = false;

            client = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            client.WhenToldTo(x => x.GetApplicationType((IDomainMessageCollection)messages, workflowData.ApplicationKey)).Return(0);

            var appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_QA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_requirevaluation_property = () =>
        {
            workflowData.RequireValuation.ShouldBeTrue();
        };
    }
}