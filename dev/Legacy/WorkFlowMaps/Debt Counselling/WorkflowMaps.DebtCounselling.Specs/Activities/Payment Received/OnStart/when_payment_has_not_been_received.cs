﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_Received.OnStart
{
    internal class when_payment_has_not_been_received : WorkflowSpecDebtCounselling
    {
        private static IDebtCounselling client;
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.CheckPaymentReceivedRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Payment_Received(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}