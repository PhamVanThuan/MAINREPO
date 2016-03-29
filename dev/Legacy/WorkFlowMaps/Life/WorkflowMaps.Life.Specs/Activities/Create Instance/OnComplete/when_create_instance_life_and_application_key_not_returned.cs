using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Create_Instance.OnComplete
{
    [Subject("Activity => Create_Instance => OnComplete")]
    internal class when_create_instance_life_and_application_key_not_returned : WorkflowSpecLife
    {
        private static bool result;
        private static string message;
        private static int applicationKeyExpected;
        private static string nameExpected;
        private static string subjectExpected;
        private static int priorityExpected;

        private Establish context = () =>
        {
            result = true;

            int applicationKey;
            string name;
            string subject;
            int priority;
            workflowData.LoanNumber = 1;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.AssignTo = "test";

            applicationKeyExpected = -1;
            nameExpected = "test";
            subjectExpected = "test";
            priorityExpected = 1;

            var client = An<ILife>();
            client.Expect(x => x.CreateInstance((IDomainMessageCollection)messages, workflowData.LoanNumber, instanceData.ID, workflowData.AssignTo,
                out applicationKey, out name, out subject, out priority)).OutRef(applicationKeyExpected, nameExpected, subjectExpected,
                priorityExpected).IgnoreArguments();
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Instance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };
    }
}