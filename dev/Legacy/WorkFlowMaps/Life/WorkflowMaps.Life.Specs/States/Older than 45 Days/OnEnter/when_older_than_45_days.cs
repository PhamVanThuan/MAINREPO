using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.States.Older_than_45_Days.OnEnter
{
    [Subject("State => Older_than_45_Days => OnEnter")]
    internal class when_older_than_45_days : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;

        private Establish context = () =>
        {
            result = false;
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Older_than_45_Days(instanceData, workflowData, paramsData, messages);
        };

        private It should_archive_applications_older_than_45_days = () =>
        {
            client.WasToldTo(x => x.OlderThan45Days(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<long>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}