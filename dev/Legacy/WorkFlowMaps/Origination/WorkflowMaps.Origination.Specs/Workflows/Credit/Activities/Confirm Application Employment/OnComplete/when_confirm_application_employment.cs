using Machine.Specifications;
using Machine.Fakes;
using X2DomainService.Interface.Common;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Confirm_Application_Employment.OnComplete
{
    [Subject("Activity => Confirm_Application_Employment => OnComplete")]
    internal class when_confirm_application_employment : WorkflowSpecCredit
    {
        static bool result;
        static ICommon common;
        static int expectedSelectedEmploymentType;

        Establish context = () =>
        {
            result = false;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.ProcessApplicationForManuallySelectedEmploymentType(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>(), Param.IsAny<int>()));
            paramsData = new ParamsDataStub() { Data = 99 };
            expectedSelectedEmploymentType = 99;
        };

        Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Confirm_Application_Employment(instanceData, workflowData, paramsData, messages, ref message);
        };

        It should_process_application_for_manually_selected_employment_type = () =>
        {
            common.WasToldTo(x => x.ProcessApplicationForManuallySelectedEmploymentType((IDomainMessageCollection)messages, workflowData.ApplicationKey, false, expectedSelectedEmploymentType));
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
