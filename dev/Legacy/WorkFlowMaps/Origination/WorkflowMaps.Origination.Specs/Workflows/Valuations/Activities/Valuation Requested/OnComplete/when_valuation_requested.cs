using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Valuation_Requested.OnComplete
{
    [Subject("Activity => Valuation_Requested => OnComplete")]
    internal class when_valuation_requested : WorkflowSpecValuations
    {
        private static bool result;
        private static string message;
        private static ICommon client;
        private static IWorkflowAssignment assignmentClient;
        private static int expectedApplicationKey;
        private static int expectedNumberOfLoops;
        private static string expectedSubject;

        private Establish context = () =>
        {
            result = false;
            message = String.Empty;
            client = An<ICommon>();

            expectedApplicationKey = 123456;
            expectedNumberOfLoops = 0;
            expectedSubject = "SubjectTest";

            assignmentClient = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);

            workflowData.ApplicationKey = 0;
            workflowData.nLoops = 20;
            instanceData.Subject = String.Empty;
            instanceData.Name = String.Empty;

            client.WhenToldTo(x => x.GetApplicationKeyFromSourceInstanceID((IDomainMessageCollection)messages, 0)).Return(expectedApplicationKey);
            client.WhenToldTo(x => x.GetCaseName((IDomainMessageCollection)messages, expectedApplicationKey)).Return(expectedSubject);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Valuation_Requested(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_number_of_loops_data_property_to_zero = () =>
        {
            workflowData.nLoops.ShouldEqual<int>(expectedNumberOfLoops);
        };

        private It should_get_applicationkey_from_source_instance = () =>
        {
            client.WasToldTo(x => x.GetApplicationKeyFromSourceInstanceID((IDomainMessageCollection)messages, 0));
        };

        private It should_set_applicationkey_data_property = () =>
        {
            workflowData.ApplicationKey.ShouldEqual<int>(expectedApplicationKey);
        };

        private It should_get_case_name = () =>
        {
            client.WasToldTo(x => x.GetCaseName((IDomainMessageCollection)messages, expectedApplicationKey));
        };

        private It should_set_subject_data_property_to_case_name = () =>
        {
            instanceData.Subject.ShouldEqual<string>(expectedSubject);
        };

        private It should_set_instance_name_to_applicationkey = () =>
        {
            instanceData.Name.ShouldEqual<string>(expectedApplicationKey.ToString());
        };

        private It should_reactivate_user_or_round_robin = () =>
        {
            assignmentClient.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages,
                "Valuations Administrator D", expectedApplicationKey, 111, instanceData.ID,
                            "Schedule Valuation Assesment", SAHL.Common.Globals.Process.Origination,
                                    (int)SAHL.Common.Globals.RoundRobinPointers.ValuationsAdministrator));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}