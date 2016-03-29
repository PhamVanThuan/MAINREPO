using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Rollback_Disbursement.OnComplete
{
    [Subject("Activity => Rollback_Disbursement => OnComplete")]
    internal class when_rollback_disbursement_is_not_further_lending_with_ework_case : WorkflowSpecApplicationManagement
    {
        private static IApplicationManagement appManClient;
        private static ICommon common;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            workflowData.EWorkFolderID = "123456789123456";

            appManClient = An<IApplicationManagement>();
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appManClient);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            common.WhenToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, workflowData.EWorkFolderID,
                                      SAHL.Common.Constants.EworkActionNames.X2ROLLBACKDISBURSEMENT, workflowData.ApplicationKey,
                                                   paramsData.ADUserName, paramsData.StateName)).Return(true);
        };

        private Because of = () =>
        {
            result =
                workflow.OnCompleteActivity_Rollback_Disbursement(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_rollback_disbursement = () =>
        {
            appManClient.WasToldTo(x => x.ReturnDisbursedLoanToRegistration((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_perform_ework_rollback_disbursement = () =>
        {
            common.WasToldTo(x => x.PerformEWorkAction((IDomainMessageCollection)messages, workflowData.EWorkFolderID,
                                     SAHL.Common.Constants.EworkActionNames.X2ROLLBACKDISBURSEMENT, workflowData.ApplicationKey,
                                                  paramsData.ADUserName, paramsData.StateName));
        };

        private It should_return_what_perform_ework_action_returns = () =>
        {
            result.ShouldEqual<bool>(true);
        };
    }
}