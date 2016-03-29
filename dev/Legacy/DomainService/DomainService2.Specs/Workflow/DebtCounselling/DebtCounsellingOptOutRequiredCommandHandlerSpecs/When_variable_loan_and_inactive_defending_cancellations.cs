using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.DebtCounselling.DebtCounsellingOptOutRequiredCommandHandlerSpecs
{
    [Subject(typeof(DebtCounsellingOptOutRequiredCommandHandler))]
    public class When_variable_loan_and_inactive_defending_cancellations : WithFakes
    {
        static DebtCounsellingOptOutRequiredCommand command;
        static DebtCounsellingOptOutRequiredCommandHandler handler;
        static IDomainMessageCollection messages;
        static IAccountRepository accountRepository;
        static IAccount account;
        static IAccountInstallmentSummary accountInstallmentSummary;
        static IFinancialService financialService;
        static IFinancialAdjustment financialAdjustment;
        static IFinancialAdjustmentSource financialAdjustmentSource;

        Establish context = () =>
        {
            messages = new DomainMessageCollection();

            accountRepository = An<IAccountRepository>();
            account = An<IAccount>();
            accountInstallmentSummary = An<IAccountInstallmentSummary>();
            financialService = An<IFinancialService>();
            financialAdjustment = An<IFinancialAdjustment>();
            financialAdjustmentSource = An<IFinancialAdjustmentSource>();

            IEventList<IFinancialAdjustment> financialAdjustments = new EventList<IFinancialAdjustment>();

            accountRepository.WhenToldTo(x => x.GetAccountByKey(Param.IsAny<int>())).Return(account);
            account.WhenToldTo(x => x.Product.Key).Return((int)SAHL.Common.Globals.Products.VariableLoan);

            account.WhenToldTo(x => x.InstallmentSummary).Return(accountInstallmentSummary);
            accountInstallmentSummary.WhenToldTo(x => x.IsInterestOnly).Return(false);

            account.WhenToldTo(x => x.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(financialService);
            financialService.WhenToldTo(x => x.FinancialAdjustments).Return(financialAdjustments);
            financialAdjustments.Add(messages, financialAdjustment);

            financialAdjustment.WhenToldTo(x => x.FinancialAdjustmentStatus.Key).Return((int)FinancialAdjustmentStatuses.Inactive);
            financialAdjustment.WhenToldTo(x => x.FinancialAdjustmentSource).Return(financialAdjustmentSource);
            financialAdjustmentSource.WhenToldTo(x => x.Key).Return((int)FinancialAdjustmentSources.DefendingCancellation);

            command = new DebtCounsellingOptOutRequiredCommand(Param.IsAny<int>());
            handler = new DebtCounsellingOptOutRequiredCommandHandler(accountRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}