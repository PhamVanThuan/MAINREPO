using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SAHL.Core.Specs.TaskSpecs
{
    public class when_asked_to_queue_a_task : WithFakes
    {
        private static SAHL.Core.Tasks.TaskManager fac;
        private static Action task;
        private static TaskAwaiter<bool> taskAwaiter;

        public static void testMethod()
        {
            //this function is a test function it does nothing by design
        }

        private Establish context = () =>
        {
            fac = new SAHL.Core.Tasks.TaskManager();
            task = new Action(testMethod);
        };

        private Because of = () =>
        {
            Task<bool> taskRef = fac.QueueTask(task);
            taskAwaiter = taskRef.GetAwaiter();
            taskRef.Wait();
        };

        private It should_have_called_the_task = () =>
        {
            taskAwaiter.GetResult().ShouldEqual(true);
        };
    }
}