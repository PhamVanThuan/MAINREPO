using Machine.Specifications;
using Machine.Fakes;
using X2DomainService.Interface.Origination;
using SAHL.Common.Collections.Interfaces;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Convert_to_30_Year_Term.OnStart
{
    [Subject("Activity => Convert_to_30_Year_Term => OnStart")]
    internal class when_convert_to_30_year_term : WorkflowSpecApplicationManagement
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
            string message = string.Empty;
            result = workflow.OnStartActivity_Convert_to_30_Year_Term(instanceData, workflowData, paramsData, messages);
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
