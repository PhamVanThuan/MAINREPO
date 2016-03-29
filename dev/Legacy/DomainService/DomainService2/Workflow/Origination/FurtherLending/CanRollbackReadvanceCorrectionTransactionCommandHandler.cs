using System;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class CanRollbackReadvanceCorrectionTransactionCommandHandler : IHandlesDomainServiceCommand<CanRollbackReadvanceCorrectionTransactionCommand>
    {
        private IApplicationRepository applicationRepository;
        private ILoanTransactionRepository loanTransactionRepository;

        public CanRollbackReadvanceCorrectionTransactionCommandHandler(IApplicationRepository applicationRepository, ILoanTransactionRepository loanTransactionRepository)
        {
            this.applicationRepository = applicationRepository;
            this.loanTransactionRepository = loanTransactionRepository;
        }

        public void Handle(IDomainMessageCollection messages, CanRollbackReadvanceCorrectionTransactionCommand command)
        {
            IApplication app = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);

            // Check that Tran 1140 has been written in the past 24 hours.
            IReadOnlyEventList<IFinancialTransaction> readvanceCorrectionTransactions = this.loanTransactionRepository.GetLoanTransactionsByTransactionTypeAndAccountKey(SAHL.Common.Globals.TransactionTypes.ReadvanceCorrection, app.Account.Key);
            IReadOnlyEventList<IFinancialTransaction> readvanceTransactions = this.loanTransactionRepository.GetLoanTransactionsByTransactionTypeAndAccountKey(SAHL.Common.Globals.TransactionTypes.Readvance, app.Account.Key);

            // get the latest 140
            IFinancialTransaction lastTransaction = null;
            if (readvanceTransactions.Count > 0)
            {
                lastTransaction = readvanceTransactions.OrderByDescending(x => x.InsertDate).FirstOrDefault();
            }

            // get the latest 1140
            IFinancialTransaction lastCorrection = null;

            if (readvanceCorrectionTransactions.Count > 0)
            {
                lastCorrection = readvanceCorrectionTransactions.OrderByDescending(x => x.InsertDate).FirstOrDefault();
            }

            // check the latest 140 is rolled back and check that the 1140 occurred after the 140 and the amounts match
            if ((lastTransaction != null && lastCorrection != null)
                && lastTransaction.IsRolledBack == true // last 140 has been rolled back
                && (lastCorrection.InsertDate > lastTransaction.InsertDate) // 1140 occurred after the 140
                && lastCorrection.InsertDate > DateTime.Now.AddHours(-24) // 1140 correction occurred in the last 24 hours
                && (lastTransaction.Amount + lastCorrection.Amount) == 0) // 140 and 1140 amounts match (i.e. there is a matching rollback for the 140)
            {
                command.Result = true;
            }
            else
            {
                command.Result = false;
                messages.Add(new Error("Transaction type 1140 not found in the last 24 hours", ""));
            }
        }
    }
}