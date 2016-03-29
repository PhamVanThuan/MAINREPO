using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.QA.OnEnter
{
    [Subject("State => QA => OnEnter")]
    internal class when_qa_where_offer_type_key_data_property_set_to_switch_loan_or_re_finance : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon client;

        private Establish context = () =>
        {
            result = false;
            workflowData.OfferTypeKey = 8; //or 6
            workflowData.RequireValuation = true;

            client = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(client);

            var appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_QA(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_get_offer_type = () =>
        {
            client.WasNotToldTo(x => x.GetApplicationType(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_requirevaluation_property_to_false = () =>
        {
            workflowData.RequireValuation.ShouldBeFalse();
        };
    }
}