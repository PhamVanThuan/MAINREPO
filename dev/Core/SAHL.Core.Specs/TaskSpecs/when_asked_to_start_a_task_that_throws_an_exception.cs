using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SAHL.Core.Specs.TaskSpecs
{
    public class when_asked_to_start_a_task_that_throws_an_exception : WithFakes
    {
        private static SAHL.Core.Tasks.TaskManager fac;
        private static Action task;
        private static Task taskRef;
        private static string exceptionMessage;

        public static void testMethod()
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

            fac = new SAHL.Core.Tasks.TaskManager(actionBlockConfiguration);
            task = new Action(testMethod);
        };

        private Because of = () =>
        {
            taskRef = fac.StartTask(task);
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