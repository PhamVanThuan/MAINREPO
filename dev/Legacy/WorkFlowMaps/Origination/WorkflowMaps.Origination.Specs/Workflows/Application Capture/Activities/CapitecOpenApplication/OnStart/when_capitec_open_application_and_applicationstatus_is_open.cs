using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.CapitecOpenApplication.OnStart
{
    [Subject("Activity => CapitecOpenApplication => OnStart")]
    internal class when_capitec_open_application_and_applicationstatus_is_open : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static ICommon common;
        private static int expectedApplicationStatusKey;

        private Establish context = () =>
        {
            result = false;
            common = An<ICommon>();

            domainServiceLoader.RegisterMockForType<ICommon>(common);

            expectedApplicationStatusKey = (int)SAHL.Common.Globals.OfferStatuses.Open;
            common.WhenToldTo(x => x.GetApplicationStatus(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(expectedApplicationStatusKey);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_CapitecOpenApplication(instanceData, workflowData, paramsData, messages);
        };

        private It should_get_the_application_status = () =>
        {
            common.WasToldTo(x => x.GetApplicationStatus(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}