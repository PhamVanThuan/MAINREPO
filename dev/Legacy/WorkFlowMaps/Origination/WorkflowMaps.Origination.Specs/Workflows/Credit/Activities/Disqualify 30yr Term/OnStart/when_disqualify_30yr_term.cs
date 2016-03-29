using Machine.Specifications;
using Machine.Fakes;
using X2DomainService.Interface.Origination;
using SAHL.Common.Collections.Interfaces;

namespace WorkflowMaps.Credit.Specs.Activities.Disqualify_30yr_Term.OnStart
{
    [Subject("Activity => Disqualify_30yr_Term => OnStart")]
    internal class when_disqualify_30yr_term : WorkflowSpecCredit
    {
        static bool result;
        static ICredit credit;

        Establish context = () =>
        {
            result = false;
            credit = An<ICredit>();
            credit.WhenToldTo(x => x.CheckApplicationIsNewBusinessRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                  .Return(true);
            domainServiceLoader.RegisterMockForType<ICredit>(credit);
        };

        Because of = () =>
        {
            result = workflow.OnStartActivity_Disqualify_30yr_Term(instanceData, workflowData, paramsData, messages);
        };

        It should_run_the_check_application_is_new_business_rule = () =>
        {
            credit.WasToldTo(x => x.CheckApplicationIsNewBusinessRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}