using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities._30_Year_Term_Detail.OnStart
{
    [Subject("Activity => Convert_to_30_Year_Term => OnStart")]
    internal class when_application_is_not_a_30yr_term : WorkflowSpecApplicationCapture
    {
        static bool result;
        static IApplicationManagement appMan;

        Establish context = () =>
        {
            result = false;
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.Check30YearTermApplicationRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                  .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        Because of = () =>
        {
            result = workflow.OnStartActivity_30_Year_Term_Detail(instanceData, workflowData, paramsData, messages);
        };

        It should_run_the_check_30_year_term_rule = () =>
        {
            appMan.WasToldTo(x => x.Check30YearTermApplicationRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
