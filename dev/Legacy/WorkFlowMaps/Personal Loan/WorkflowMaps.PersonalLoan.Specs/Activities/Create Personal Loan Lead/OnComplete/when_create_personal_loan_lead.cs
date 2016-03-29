using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Create_Personal_Loan_Lead.OnComplete
{
    [Subject("State => Create_Personal_Loan_Lead => OnComplete")]
    internal class when_create_personal_loan_lead : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static IPersonalLoan personalLoanClient;
        private static IWorkflowAssignment assignmentClient;
        private static string expectedSubject;
        private static int expectedApplicationKey;

        private Establish context = () =>
        {
            expectedSubject = "SubjectTest";
            expectedApplicationKey = 1234567;
            instanceData.Subject = String.Empty;
            instanceData.Name = String.Empty;
            workflowData.ApplicationKey = expectedApplicationKey;

            //Mocks
            personalLoanClient = An<IPersonalLoan>();
            domainServiceLoader.RegisterMockForType<IPersonalLoan>(personalLoanClient);
            assignmentClient = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);

            personalLoanClient.WhenToldTo(x => x.GetInstanceSubjectForPersonalLoan((IDomainMessageCollection)messages, expectedApplicationKey)).Return(expectedSubject);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Personal_Loan_Lead
                (instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_instance_name_data_property_to_workflow_applicationkey_data_property = () =>
        {
            instanceData.Name.ShouldEqual<string>(expectedApplicationKey.ToString());
        };

        private It should_get_instance_subject_and_set_instance_subject_data_property = () =>
        {
            instanceData.Subject.ShouldEqual<string>(expectedSubject);
        };

        private It should_assign_workflow_role_to_aduser_for_application = () =>
        {
            assignmentClient.WasToldTo(x => x.AssignWorkflowRoleForADUser
                ((IDomainMessageCollection)messages, instanceData.ID, paramsData.ADUserName, SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, expectedApplicationKey, ""));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}