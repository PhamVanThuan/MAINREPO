using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SAHL.Core.Specs.TaskSpecs
{
    public class when_asked_to_queue_a_generic_task : WithFakes
    {
        private static GenericTaskManager<int> fac;
        private static Action<int> task;
        private static TaskAwaiter<bool> taskAwaiter;

        public static void testMethod(int testVar)
        {
            //this function is a test function it does nothing by design
        }

        private Establish context = () =>
        {
            fac = new GenericTaskManager<int>();
            task = new Action<int>(testMethod);
        };

        private Because of = () =>
        {
            Task<bool> taskRef = fac.QueueTask(task, 1);
            taskAwaiter = taskRef.GetAwaiter();
            taskRef.Wait();
        };

        private It should_have_called_the_task = () =>
        {
            taskAwaiter.GetResult().ShouldEqual(true);
        };
    }
}