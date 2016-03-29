using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Tasks;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SAHL.Core.Specs.TaskSpecs
{
    public class when_asked_to_queue_a_generic_task_that_throws_an_exception : WithFakes
    {
        private static GenericTaskManager<int> fac;
        private static Action<int> task;
        private static Task taskRef;
        private static string exceptionMessage;

        public static void testMethod(int testVar)
        {
            throw new Exception("Exception has occured");
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
        };

        private Because of = () =>
        {
            taskRef = fac.QueueTask(task, 1);
            try
            {
                taskRef.Wait();
            }
            catch (Exception e)
            {
                exceptionMessage = e.InnerException.Message;
            }
        };

        private It should_set_the_correct_status = () =>
        {
            taskRef.Status.ShouldEqual(TaskStatus.Faulted);
        };

        private It should_set_the_correct_exception_messsage = () =>
        {
            exceptionMessage.ShouldEqual("Exception has occured");
        };
    }
}