using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.FixedDebitOrder.FinancialServiceBankAccount.FinancialServiceBankAccountUpdateDebitOrderAmount
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountUpdateDebitOrderAmount))]
    public class when_finservice_bank_acc_update_debit_order_amt_with_subsidy_pass : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountUpdateDebitOrderAmount>
    {
        protected static IAccount account;
        protected static IAccountStatus accountStatus;
        protected static IGeneralStatus generalStatus;
        protected static IAccountInstallmentSummary accountInstallmentSummary;
        protected static ISubsidy subsidy;

        Establish Context = () =>
            {
                account = An<IAccount>();
                accountStatus = An<IAccountStatus>();

                double fixedPayment = 1000;
                double totalAmountDue = 1000;
                double subsidyAmount = 500;

                // set up the fixed payment              
                account.WhenToldTo(x => x.FixedPayment).Return(fixedPayment);

                // set up the total amount due
                accountInstallmentSummary = An<IAccountInstallmentSummary>();                 
                accountStatus.WhenToldTo(x => x.Key).Return((int)AccountStatuses.Open);                
                account.WhenToldTo(x => x.AccountStatus).Return(accountStatus);
                account.WhenToldTo(x=>x.InstallmentSummary).Return(accountInstallmentSummary);
                accountInstallmentSummary.WhenToldTo(x => x.TotalAmountDue).Return(totalAmountDue);

                // set up a subsidy
                subsidy = An<ISubsidy>();               
                generalStatus = An<IGeneralStatus>();
                generalStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Active);
                subsidy.WhenToldTo(x => x.GeneralStatus).Return(generalStatus);
                subsidy.WhenToldTo(x => x.StopOrderAmount).Return(subsidyAmount);
                IEventList<ISubsidy> subsidies = new EventList<ISubsidy>(new ISubsidy[] { subsidy });
                account.WhenToldTo(x => x.Subsidies).Return(subsidies);
                
				businessRule = new BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountUpdateDebitOrderAmount();
                RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccountUpdateDebitOrderAmount>.startrule.Invoke();

            };
        Because of = () =>
            {
                businessRule.ExecuteRule(messages, account);
            };
        It rule_should_pass_because_fixedpayment_greater_than_totalamt_minus_subsidy = () =>
            {
                messages.Count.ShouldEqual(0);
            };
    }
}
