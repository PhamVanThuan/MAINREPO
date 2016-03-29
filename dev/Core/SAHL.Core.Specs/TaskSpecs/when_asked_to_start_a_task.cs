using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SAHL.Core.Specs.TaskSpecs
{
    public class when_asked_to_start_a_task : WithFakes
    {
        private static SAHL.Core.Tasks.TaskManager fac;
        private static Action task;
        private static bool taskCalled;

        public static void testMethod()
        {
            taskCalled = true;
        }

        private Establish context = () =>
        {
            var actionBlockConfiguration = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 1,
                BoundedCapacity = 1
            };

            fac = new SAHL.Core.Tasks.TaskManager(actionBlockConfiguration);
            task = new Action(testMethod);
            taskCalled = false;
        };

        private Because of = () =>
        {
            Task taskRef = fac.StartTask(task);
            taskRef.Wait();
        };

        private It should_have_called_the_task = () =>
        {
            taskCalled.ShouldEqual(true);
        };
    }
}