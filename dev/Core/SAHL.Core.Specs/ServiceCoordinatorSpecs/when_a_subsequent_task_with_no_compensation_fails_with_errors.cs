using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System.Collections.Generic;

namespace SAHL.Core.Specs.ServiceCoordinatorSpecs
{
    public class when_a_subsequent_task_with_no_compensation_fails_with_errors : WithFakes
    {
        private static IServiceCoordinator coordinator;
        private static Dictionary<string, string> work;
        private static IServiceCoordinatorExecutor executor;

        private Establish context = () =>
        {
            work = new Dictionary<string, string>() { { "1", "init" }, { "2", "init" }, { "3", "init" }, { "4", "init" } };
            coordinator = new ServiceCoordinator();

            var errors = SystemMessageCollection.Empty();
            errors.AddMessage(new SystemMessage("Error", SystemMessageSeverityEnum.Error));

            // add the first task with compensation
            executor = coordinator.StartSequence(() => { work["1"] = "done"; return SystemMessageCollection.Empty(); },
                                      () => { work["1"] = "compensated"; return SystemMessageCollection.Empty(); }).
                // add the second task with no compensation
                ThenWithNoCompensationAction(() => { work["2"] = "in-progress"; return SystemMessageCollection.Empty(); }).
                // add the third task with compensation and error
                Then(() => { work["3"] = "in-progress"; return errors; },
                     () => { work["3"] = "compensated"; return SystemMessageCollection.Empty(); }).
                // add final task 
                Then(() => { work["4"] = "done"; return SystemMessageCollection.Empty(); },
                     () => { work["4"] = "compensated"; return SystemMessageCollection.Empty(); }).
                     EndSequence();
        };

        private Because of = () =>
        {
            executor.Run();
        };

        private It should_not_execute_subsequent_tasks = () =>
        {
            work["4"].ShouldEqual("init");
        };

        private It should_not_compensate_the_failed_task = () =>
        {
            work["3"].ShouldEqual("in-progress");
        };

        private It should_not_compensate_the_previous_task_with_no_compensation = () =>
        {
            work["2"].ShouldEqual("in-progress");
        };

        private It should_compensate_the_first_tasks = () =>
        {
            work["1"].ShouldEqual("compensated");
        };
    }
}