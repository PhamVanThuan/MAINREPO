using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Accept_Benefits.OnComplete
{
    [Subject("Activity => Accept_Benefits => OnComplete")]
    internal class when_accept_benefits : WorkflowSpecLife
    {
        private static bool result;
        private static string message;
        private static ILife client;

        private Establish context = () =>
        {
            result = false;
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Accept_Benefits(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_accept_benefits = () =>
            {
                client.WasToldTo(x => x.AcceptBenefits(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
            };

        private It should_set_life_origination_data_property = () =>
        {
            workflowData.BenefitsDone.ShouldEqual(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}