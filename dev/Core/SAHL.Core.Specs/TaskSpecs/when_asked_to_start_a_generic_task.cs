using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Tasks;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SAHL.Core.Specs.TaskSpecs
{
    public class when_asked_to_start_a_generic_task : WithFakes
    {
        private static GenericTaskManager<int> fac;
        private static Action<int> task;
        private static bool taskCalled;

        public static void testMethod(int testVar)
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

            fac = new GenericTaskManager<int>(actionBlockConfiguration);
            task = new Action<int>(testMethod);
            taskCalled = false;
        };

        private Because of = () =>
        {
            Task taskRef = fac.StartTask(task, 1);
            taskRef.Wait();
        };

        private It should_have_called_the_task = () =>
        {
            taskCalled.ShouldEqual(true);
        };
    }
}