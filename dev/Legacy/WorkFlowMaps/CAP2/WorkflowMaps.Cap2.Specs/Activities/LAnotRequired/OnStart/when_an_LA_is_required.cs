using CAP2_Offers;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace WorkflowMaps.Cap2.Specs.Activities.LAnotRequired.OnStart
{
    [Subject("Activity => LAnotRequired => OnStart")]
    public class when_an_LA_is_required : WorkflowSpecCap2
    {
        private static ICap2 client;
        private static bool result;

        private Establish context = () =>
        {
            workflow = new X2CAP2_Offers();
            workflowData = new X2CAP2_Offers_Data();
            client = An<ICap2>();
            client.WhenToldTo(x => x.IsLANotRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(false);
            domainServiceLoader.RegisterMockForType<ICap2>(client);
        };

        private Because of = () =>
            {
                result = workflow.OnStartActivity_LAnotRequired(instanceData, workflowData, paramsData, messages);
            };

        private It should_not_start_the_activity = () =>
            {
                result.ShouldBeFalse();
            };
    }
}