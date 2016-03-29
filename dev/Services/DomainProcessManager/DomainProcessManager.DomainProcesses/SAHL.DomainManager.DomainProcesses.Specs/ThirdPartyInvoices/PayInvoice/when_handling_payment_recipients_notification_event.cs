using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.CATS.Events;
using SAHL.Core.Rules;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.ThirdPartyInvoiceProcess.PayInvoice
{
    public class when_handling_payment_recipients_notification_event : PayThirdPartyInvoiceProcessSpec
    {
        private static CATSPaymentBatchRecipientsNotifiedEvent paymentRecipientsNotifiedEvent;
        private static long case1Instance, case2Instance, case3Instance, case4Instance;
        private static int batchReference;
        private static List<PayThirdPartyInvoiceModel> invoicesWithSuccessfulNotifications, invoicesWithFailedNotifications;
        public static IDomainRuleManager<PayThirdPartyInvoiceProcessModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
            case1Instance = 1000000000000000001;
            case2Instance = 1000000000000000101;
            case3Instance = 1000000000000000104;
            case4Instance = 1000000000000000131;
            batchReference = 15;

            payThirdPartyInvoiceStateMachine.BatchReference.Returns(batchReference);

            invoicesWithSuccessfulNotifications = new List<PayThirdPartyInvoiceModel>
            {
                   new PayThirdPartyInvoiceModel(18377, case1Instance, 1399640, "SAHL-2015/07/359", PaymentProcessStep.Archived)
                ,  new PayThirdPartyInvoiceModel(18378, case2Instance, 4077922, "SAHL-2015/07/335", PaymentProcessStep.Archived )
            };

            invoicesWithFailedNotifications = new List<PayThirdPartyInvoiceModel>
            {
                   new PayThirdPartyInvoiceModel(18379, case3Instance, 4077835, "SAHL-2015/07/312", PaymentProcessStep.Archived)
                ,  new PayThirdPartyInvoiceModel(18380, case4Instance, 4080071, "SAHL-2015/07/367", PaymentProcessStep.ReadyForArchiving)
            };

            paymentRecipientsNotifiedEvent = new CATSPaymentBatchRecipientsNotifiedEvent(
                  DateTime.Now
                , invoicesWithSuccessfulNotifications.Select(y =>
                    {
                        return new CATSPaymentBatchItemDataModel(
                                y.ThirdPartyInvoiceKey
                            , 54
                            , y.AccountNumber
                            , 1200M
                            , 8493849
                            , 8327
                            , batchReference
                            , y.SAHLReference
                            , "SAHL-Ref"
                            , "Straus Daly"
                            , "Invoice Num. n"
                            , "straus@sd.com"
                            , 2005
                            , true
                            );
                    })
                , invoicesWithFailedNotifications.Select(y =>
                    {
                        return new CATSPaymentBatchItemDataModel(
                                y.ThirdPartyInvoiceKey
                            , 54
                            , y.AccountNumber
                            , 400M
                            , 4738
                            , 36
                            , batchReference
                            , y.SAHLReference
                            , "SAHL-Ref"
                            , "Kaplan Blumberg Attorneys"
                            , "Invoice Num. n"
                            , "chanel@e-lex.co.za"
                            , 474643
                            , true
                            );
                    })
            );

            payAttorneyInvoiceDomainProcess = new PayAttorneyInvoiceProcess(
                 payThirdPartyInvoiceStateMachine
               , financeDomainService
               , combGuidGenerator
               , catsService
               , x2WorkflowManager
               , communicationManager
               , rawLogger
               , loggerSource
               , loggerAppSource
               , domainRuleManager
           );
        };

        private Because of = () =>
        {
            payAttorneyInvoiceDomainProcess.Handle(paymentRecipientsNotifiedEvent, metadata);
        };

        private It should_alert_operator_of_failure_to_notify_recipients = () =>
        {
            communicationManager.Received().SendFailureToNotifyRecipientsEmail(
                  Arg.Is<string[]>(l =>
                      l.All(invoicesWithFailedNotifications.Select(i => i.SAHLReference).Contains) &&
                      l.Length == invoicesWithFailedNotifications.Count)
            );
        };
    }
}