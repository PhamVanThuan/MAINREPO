using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Credit.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Check_Passed.OnStart
{
    [Subject("Activity => Check_Passed => OnStart")]
    internal class when_check_passed : WorkflowSpecCredit
    {
        private static bool result;
        private static ICommon common;
        private static ICredit client;

        private Establish context = () =>
        {
            result = false;
            workflowData.ApplicationKey = 1;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            client = An<ICredit>();
            domainServiceLoader.RegisterMockForType<ICredit>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Check_Passed(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_the_loan_conditions = () =>
        {
            client.WasToldTo(x => x.UpdateConditions((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_update_the_offer_information_type = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open,
                (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer));
        };

        private It should_ask_credit_host_to_send_decision_mail = () =>
        {
            client.WasToldTo(x => x.SendCreditDecisionMail(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<int>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}