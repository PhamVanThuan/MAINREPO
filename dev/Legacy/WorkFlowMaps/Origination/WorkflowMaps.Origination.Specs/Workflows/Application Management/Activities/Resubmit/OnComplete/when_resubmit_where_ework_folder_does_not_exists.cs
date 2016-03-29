using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Resubmit.OnComplete
{
    [Subject("Activity => Resubmit => OnComplete")]
    internal class when_resubmit_where_ework_folder_does_not_exists : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            workflowData.IsResub = false;
            workflowData.EWorkFolderID = string.Empty;

            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            ((ParamsDataStub)paramsData).ADUserName = "ExpectedADUserName";
            ((ParamsDataStub)paramsData).StateName = "ExpectedStateName";

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Resubmit(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_set_is_resub_to_true = () =>
        {
            workflowData.IsResub.ShouldBeFalse();
        };

        private It should_not_return_non_disbursed_loan_to_prospect = () =>
        {
            appMan.WasNotToldTo(x => x.ReturnNonDisbursedLoanToProspect((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_not_update_offer_status_to_open = () =>
        {
            common.WasNotToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, -1));
        };

        private It should_not_create_new_revision = () =>
        {
            common.WasNotToldTo(x => x.CreateNewRevision((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_not_check_ework_at_correct_state = () =>
        {
            appMan.WasNotToldTo(x => x.CheckEWorkAtCorrectStateRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        private It should_not_perform_x2_resub_ework_action = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}