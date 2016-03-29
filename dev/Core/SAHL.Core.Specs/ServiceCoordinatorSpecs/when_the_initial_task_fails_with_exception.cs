using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.ServiceCoordinatorSpecs
{
    public class when_the_initial_task_fails_with_exception : WithFakes
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
            executor = coordinator.StartSequence(() => { work["1"] = "in-progress"; throw new Exception(); },
                                      () => { work["1"] = "compensated"; return SystemMessageCollection.Empty(); }).
                // add the second task
                Then(() => { work["2"] = "done"; return SystemMessageCollection.Empty(); },
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
            foreach (var kvp in work)
            {
                // the first task should have been started
                if (kvp.Key == "1")
                {
                    work[kvp.Key].ShouldEqual("in-progress");
                }
                else
                {
                    work[kvp.Key].ShouldEqual("init");
                }
            }
        };

        private It should_not_compensate_the_initial_task = () =>
        {
            work["1"].ShouldNotEqual("compensated");
        };
    }
}