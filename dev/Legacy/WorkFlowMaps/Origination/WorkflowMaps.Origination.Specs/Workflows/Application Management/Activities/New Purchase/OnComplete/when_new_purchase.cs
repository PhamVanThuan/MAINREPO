using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.New_Purchase.OnComplete
{
    [Subject("Activity => New_Purchase => OnComplete")]
    internal class when_new_purchase : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;
        private static IWorkflowAssignment assignment;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            dys = new List<string>() { "QA Administrator D" };

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_New_Purchase(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages,
                workflowData.ApplicationKey,
                (int)SAHL.Common.Globals.OfferStatuses.Open,
                (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer));
        };

        private It should_create_new_revision = () =>
        {
            common.WasToldTo(x => x.CreateNewRevision((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_deactive_qa_administrator_d_users_for_instance = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactive_branch_users_for_origination = () =>
        {
            assignment.WasToldTo(x => x.ReActivateBranchUsersForOrigination((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.AppCapIID,
                workflowData.ApplicationKey,
                "Issue AIP",
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}