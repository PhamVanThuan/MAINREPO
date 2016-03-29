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
    internal class when_resubmit_where_ework_folder_exists_and_at_correct_ework_state : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IApplicationManagement appMan;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsResub = false;
            workflowData.EWorkFolderID = "HasEWorkFolderID";

            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            ((ParamsDataStub)paramsData).ADUserName = "ExpectedADUserName";
            ((ParamsDataStub)paramsData).StateName = "ExpectedStateName";

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckEWorkAtCorrectStateRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Resubmit(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_is_resub_to_true = () =>
        {
            workflowData.IsResub.ShouldBeTrue();
        };

        private It should_return_non_disbursed_loan_to_prospect = () =>
        {
            appMan.WasToldTo(x => x.ReturnNonDisbursedLoanToProspect((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_update_offer_status_to_open = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, -1));
        };

        private It should_create_new_revision = () =>
        {
            common.WasToldTo(x => x.CreateNewRevision((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_check_ework_at_correct_state = () =>
        {
            appMan.WasToldTo(x => x.CheckEWorkAtCorrectStateRule((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_calculate_and_save_the_application = () =>
        {
            common.WasToldTo(x => x.CalculateAndSaveApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey, false));
        };

        private It should_perform_x2_resub_ework_action = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages,
                workflowData.EWorkFolderID,
                SAHL.Common.Constants.EworkActionNames.X2RESUB,
                workflowData.ApplicationKey,
                paramsData.ADUserName,
                paramsData.StateName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}