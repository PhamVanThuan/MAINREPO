using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.QA.OnEnter
{
    [Subject("State => QA => OnEnter")]
    internal class when_qa_where_offer_type_key_data_property_not_set_and_get_offer_type_returns_other : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static int expectedOfferTypeKey;
        private static ICommon client;

        private Establish context = () =>
        {
            result = false;
            expectedOfferTypeKey = 2; //or 3 or 4 or 5 or 7 or 9 or 10
            workflowData.OfferTypeKey = 0;
            workflowData.RequireValuation = true;

            client = An<ICommon>();
            client.WhenToldTo(x => x.GetApplicationType((IDomainMessageCollection)messages, workflowData.ApplicationKey))
                .Return(expectedOfferTypeKey);
            domainServiceLoader.RegisterMockForType<ICommon>(client);

            var appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_QA(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_offer_type_key_data_property = () =>
        {
            workflowData.OfferTypeKey.ShouldEqual(expectedOfferTypeKey);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_requirevaluation_property_to_true = () =>
        {
            workflowData.RequireValuation.ShouldBeTrue();
        };
    }
}