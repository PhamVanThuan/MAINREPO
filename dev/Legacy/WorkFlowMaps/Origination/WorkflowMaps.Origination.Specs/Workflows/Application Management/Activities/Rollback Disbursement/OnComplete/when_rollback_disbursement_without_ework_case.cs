using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Rollback_Disbursement.OnComplete
{
    [Subject("Activity => Rollback_Disbursement => OnComplete")]
    internal class when_rollback_disbursement_without_ework_case : WorkflowSpecApplicationManagement
    {
        private static IApplicationManagement appManClient;
        private static ICommon common;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            workflowData.EWorkFolderID = String.Empty;
            workflowData.IsFL = true;

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
            result = workflow.OnCompleteActivity_Rollback_Disbursement(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_rollback_disbursement = () =>
        {
            appManClient.WasNotToldTo(x => x.ReturnDisbursedLoanToRegistration(Param.IsAny<IDomainMessageCollection>(),
                                                                                            Param.IsAny<int>()));
        };

        private It should_not_perform_ework_rollback_disbursement = () =>
        {
            common.WasNotToldTo(x => x.PerformEWorkAction(Param.IsAny<IDomainMessageCollection>(),
                                            Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<int>(),
                                            Param.IsAny<string>(), Param.IsAny<string>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}