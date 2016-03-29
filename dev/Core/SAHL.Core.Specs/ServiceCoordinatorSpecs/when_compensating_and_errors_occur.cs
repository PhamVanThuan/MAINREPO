using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System.Collections.Generic;

namespace SAHL.Core.Specs.ServiceCoordinatorSpecs
{
    public class when_compensating_and_errors_occur : WithFakes
    {
        private static IServiceCoordinator coordinator;
        private static Dictionary<string, string> work;
        private static IServiceCoordinatorExecutor executor;

        private Establish context = () =>
        {
            work = new Dictionary<string, string>() { { "1", "init" }, { "2", "init" }, { "3", "init" } };
            coordinator = new ServiceCoordinator();

            // setup three tasks
            var errors = SystemMessageCollection.Empty();
            errors.AddMessage(new SystemMessage("Error", SystemMessageSeverityEnum.Error));

            var compErrors = SystemMessageCollection.Empty();
            compErrors.AddMessage(new SystemMessage("CompError", SystemMessageSeverityEnum.Error));

            // add the first task
            executor = coordinator.StartSequence(() => { work["1"] = "done"; return SystemMessageCollection.Empty(); },
                                      () => { work["1"] = "compensated"; return compErrors; }).
                // add the second task
                Then(() => { work["2"] = "done"; return SystemMessageCollection.Empty(); },
                     () => { work["2"] = "compensated"; return SystemMessageCollection.Empty(); }).

                // add the third task
                Then(() => { work["3"] = "in-progress"; return errors; },
                     () => { work["3"] = "compensated"; return SystemMessageCollection.Empty(); }).
                     EndSequence();
        };

        private Because of = () =>
        {
            executor.Run();
        };

        private It should_not_compensate_the_failed_task = () =>
        {
            work["3"].ShouldEqual("in-progress");
        };

        private It should_compensate_the_previous_tasks = () =>
        {
            work["1"].ShouldEqual("compensated");
            work["2"].ShouldEqual("compensated");
        };
    }
}