using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.ServiceCoordinatorSpecs
{
    public class when_a_subsequent_task_fails_with_exception : WithFakes
    {
        private static IServiceCoordinator coordinator;
        private static Dictionary<string, string> work;
        private static IServiceCoordinatorExecutor executor;

        private Establish context = () =>
        {
            work = new Dictionary<string, string>() { { "1", "init" }, { "2", "init" }, { "3", "init" } };
            coordinator = new ServiceCoordinator();

            // setup three tasks

            // add the first task
            executor = coordinator.StartSequence(() => { work["1"] = "done"; return SystemMessageCollection.Empty(); },
                                      () => { work["1"] = "compensated"; return SystemMessageCollection.Empty(); }).
                // add the second task
                Then(() => { work["2"] = "in-progress"; throw new Exception(); },
                     () => { work["2"] = "compensated"; return SystemMessageCollection.Empty(); }).
                // add the third task
                Then(() => { work["3"] = "done"; return SystemMessageCollection.Empty(); },
                     () => { work["3"] = "compensated"; return SystemMessageCollection.Empty(); }).
                     EndSequence();
        };

        private Because of = () =>
        {
            executor.Run();
        };

        private It should_not_execute_subsequent_tasks = () =>
        {
            work["3"].ShouldEqual("init");
        };

        private It should_not_compensate_the_failed_task = () =>
        {
            work["2"].ShouldEqual("in-progress");
        };

        private It should_compensate_the_previous_tasks = () =>
        {
            work["1"].ShouldEqual("compensated");
        };
    }
}