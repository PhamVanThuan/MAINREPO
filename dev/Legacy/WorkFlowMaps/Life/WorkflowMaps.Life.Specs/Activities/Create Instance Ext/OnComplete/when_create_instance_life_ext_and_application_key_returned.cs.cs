using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Create_Instance_Ext.OnComplete
{
    [Subject("Activity => Create_Instance_Ext => OnComplete")]
    internal class when_create_instance_life_ext_and_application_key_returned : WorkflowSpecLife
    {
        private static bool result;
        private static string message;
        private static int applicationKeyExpected;
        private static string nameExpected;
        private static string subjectExpected;
        private static int priorityExpected;

        private Establish context = () =>
        {
            result = false;

            int applicationKey;
            string name;
            string subject;
            int priority;
            workflowData.LoanNumber = 1;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.AssignTo = "test";

            applicationKeyExpected = 1;
            nameExpected = "test";
            subjectExpected = "test";
            priorityExpected = 1;

            var mocks = new MockRepository();
            var client = mocks.StrictMock<ILife>();

            client.Expect(x => x.CreateInstance((IDomainMessageCollection)messages, workflowData.LoanNumber, instanceData.ID, workflowData.AssignTo,
                out applicationKey, out name, out subject, out priority)).OutRef(applicationKeyExpected, nameExpected, subjectExpected,
                priorityExpected);
            domainServiceLoader.RegisterMockForType<ILife>(client);
            mocks.ReplayAll();
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Instance_Ext(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_offer_key_data_property = () =>
        {
            workflowData.OfferKey.ShouldEqual(applicationKeyExpected);
        };

        private It should_set_the_instance_name_property = () =>
        {
            instanceData.Name.ShouldBeTheSameAs(nameExpected);
        };

        private It should_set_instance_subject_property = () =>
        {
            instanceData.Subject.ShouldBeTheSameAs(subjectExpected);
        };

        private It should_set_instance_priority_property = () =>
        {
            instanceData.Priority.ShouldEqual(priorityExpected);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}