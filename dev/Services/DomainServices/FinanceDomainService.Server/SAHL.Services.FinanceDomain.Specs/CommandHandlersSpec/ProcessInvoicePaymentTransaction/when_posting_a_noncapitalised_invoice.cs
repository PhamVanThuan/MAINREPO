using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ProcessInvoicePaymentTransaction
{
    public class when_posting_a_noncapitalised_invoice : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static IFinanceDataManager financeDataManager;
        private static IEventRaiser eventRaiser;
        private static IServiceQueryRouter serviceQueryRouter;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private static ProcessTransactionsForThirdPartyInvoicePaymentCommandHandler handler;
        private static ProcessTransactionsForThirdPartyInvoicePaymentCommand command;
        private static int financialServiceKey, accountKey;
        private static GetThirdPartyInvoiceQueryResult thirdPartyInvoice;
        private static IThirdPartyInvoiceDataManager dataManager;

        Establish context = () =>
        {
            eventRaiser = An<IEventRaiser>();
            loanTransactionManager = An<ILoanTransactionManager>();
            financeDataManager = An<IFinanceDataManager>();
            serviceQueryRouter = An<IServiceQueryRouter>();
            metadata = An<IServiceRequestMetadata>();
            domainRuleManager = An<IDomainRuleManager<IThirdPartyInvoiceRuleModel>>();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            handler = new ProcessTransactionsForThirdPartyInvoicePaymentCommandHandler(loanTransactionManager, financeDataManager, eventRaiser, serviceQueryRouter,
                domainRuleManager, dataManager);
            command = new ProcessTransactionsForThirdPartyInvoicePaymentCommand(1408);

            financialServiceKey = 108;
            accountKey = 10008;

            financeDataManager.WhenToldTo(x => x.GetVariableLoanFinancialServiceKeyByAccount(Param.IsAny<int>())).Return(financialServiceKey);
            thirdPartyInvoice = new GetThirdPartyInvoiceQueryResult
            {
                InvoiceDate = DateTime.Now,
                AccountKey = accountKey,
                AmountExcludingVAT = 1000,
                CapitaliseInvoice = false,
                InvoiceStatusKey = (int)InvoiceStatus.Approved,
                InvoiceNumber = "108",
                ReceivedDate = DateTime.Now,
                ReceivedFromEmailAddress = "johnsnow@blackattorneys.com",
                SahlReference = "SAHL 14/08/88",
                ThirdPartyId = new Guid(),
                ThirdPartyInvoiceKey = 1405,
                ThirdPartyRegisteredName = "John Snow",
                TotalAmountIncludingVAT = 1140,
                VATAmount = 140
            };

            var thirdPartyInvoiceList = new List<GetThirdPartyInvoiceQueryResult> { thirdPartyInvoice };
            var results = new ServiceQueryResult<GetThirdPartyInvoiceQueryResult>(thirdPartyInvoiceList);
            serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param.IsAny<GetThirdPartyInvoiceQuery>())).Return<GetThirdPartyInvoiceQuery>(y =>
            {
                y.Result = results;
                return SystemMessageCollection.Empty();
            });

            loanTransactionManager.WhenToldTo(x =>
                    x.PostTransaction(financialServiceKey, LoanTransactionTypeEnum.CapitalisedLegalFeeReversalTransaction, (decimal)thirdPartyInvoice.TotalAmountIncludingVAT, 
                    Param.IsAny<DateTime>(), thirdPartyInvoice.SahlReference, metadata.UserName)
                ).Return(SystemMessageCollection.Empty());

        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        It should_not_process_the_non_reversal_transaction_types = () =>
        {
            var loanTransactionTypes = Enum.GetValues(typeof(LoanTransactionTypeEnum)) as LoanTransactionTypeEnum[];
            var validTransactionTypes = new List<LoanTransactionTypeEnum>() { 
                    LoanTransactionTypeEnum.NonCapitalisedLegalFeeTransaction
            };

            foreach (var invalidTransactionType in loanTransactionTypes)
            {
                if (!validTransactionTypes.Contains(invalidTransactionType))
                {
                    loanTransactionManager.WasNotToldTo(x =>
                        x.PostTransaction(Param.IsAny<int>(),
                        invalidTransactionType,
                        Param.IsAny<decimal>(),
                        Param.IsAny<DateTime>(),
                        Param.IsAny<string>(),
                        Param.IsAny<string>()
                        ));
                }
            }
        };
    }
}
