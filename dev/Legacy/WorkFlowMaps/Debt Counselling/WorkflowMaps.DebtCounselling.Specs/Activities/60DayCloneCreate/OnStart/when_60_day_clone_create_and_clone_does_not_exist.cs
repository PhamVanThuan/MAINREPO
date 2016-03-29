using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._60DayCloneCreate.OnStart
{
    [Subject("Activity => 60_Day_Clone_Create => OnStart")]
    internal class when_60_day_clone_create_and_clone_does_not_exist : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CloneExistsForParent(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<List<string>>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_60DayCloneCreate(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}