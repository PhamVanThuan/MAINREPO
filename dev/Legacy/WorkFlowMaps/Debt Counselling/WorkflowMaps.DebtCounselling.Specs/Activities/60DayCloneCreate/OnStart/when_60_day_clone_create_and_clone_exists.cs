using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._60DayCloneCreate.OnStart
{
    [Subject("Activity => 60_Day_Clone_Create => OnStart")]
    internal class when_60_day_clone_create_and_clone_exists : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static List<string> statesToInclude = new List<string> { "60DayTimerHold", "Archive 60days" };
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            ((InstanceDataStub)instanceData).ID = 1;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CloneExistsForParent(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<List<string>>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_60DayCloneCreate(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_60_day_clone_exists = () =>
        {
            common.WasToldTo(x => x.CloneExistsForParent((IDomainMessageCollection)messages, instanceData.ID, statesToInclude));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}