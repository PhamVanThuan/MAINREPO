using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.DebtCounselling
{
    public class DebtCounsellingOptOutRequiredCommandHandler : IHandlesDomainServiceCommand<DebtCounsellingOptOutRequiredCommand>
    {
        private IAccountRepository accountRepository;

        public DebtCounsellingOptOutRequiredCommandHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Handle(IDomainMessageCollection messages, DebtCounsellingOptOutRequiredCommand command)
        {
            bool optOutRequired = false;

            var acc = accountRepository.GetAccountByKey(command.AccountKey);

            if (acc != null)
            {
                //non variables must be opted out
                if (acc.Product.Key != (int)Products.VariableLoan && acc.Product.Key != (int)Products.NewVariableLoan)
                {
                    optOutRequired = true;
                }
                else
                {
                    //Interest Only must be deactivated
                    if (acc.InstallmentSummary.IsInterestOnly)
                    {
                        optOutRequired = true;
                    }
                    else
                    {
                        //opt out active Defending Cancellations and Staff
                        foreach (IFinancialAdjustment fa in acc.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan).FinancialAdjustments)
                        {
                            if (fa.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active
                                && (fa.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.DefendingCancellation
                                    || fa.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.Staff
                                ))
                            {
                                optOutRequired = true;
                                break;
                            }
                        }
                    }
                }
            }

            command.Result = optOutRequired;
        }
    }
}