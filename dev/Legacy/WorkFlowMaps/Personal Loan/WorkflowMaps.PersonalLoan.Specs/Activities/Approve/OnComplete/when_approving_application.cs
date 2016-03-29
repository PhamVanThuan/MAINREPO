using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.PersonalLoan;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Approve.OnComplete
{
    [Subject("Activity => Approve => OnComplete")]
    internal class when_approving_application : WorkflowSpecPersonalLoans
    {
        private static string message;
        private static bool result;
        private static IPersonalLoan client;
        private static IWorkflowAssignment wfa;
        private static ICommon common; 

        private Establish context = () =>
            {
                result = false;
                client = An<IPersonalLoan>();
                wfa = An<IWorkflowAssignment>();
                common = An<ICommon>();
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
                domainServiceLoader.RegisterMockForType<ICommon>(common);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Approve(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_update_the_offer_information_to_accepted = () =>
            {
                client.WasToldTo(x => x.UpdateOfferInformationType((IDomainMessageCollection)messages, workflowData.ApplicationKey, SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer));
            };

        private It should_round_robin_assign_a_personal_loan_consultant_user = () =>
            {
                wfa.WasToldTo(x => x.ReactivateUserOrRoundRobinForWorkflowRoleAssignment((IDomainMessageCollection)messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType,
                    SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, workflowData.ApplicationKey, instanceData.ID, SAHL.Common.Globals.RoundRobinPointers.PLConsultant));
            };
    }
}