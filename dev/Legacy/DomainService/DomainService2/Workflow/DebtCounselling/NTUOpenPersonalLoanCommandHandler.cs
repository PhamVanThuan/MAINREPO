using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel;
using SAHL.Common;
using SAHL.Common.DataAccess;

namespace DomainService2.Workflow.DebtCounselling
{
	public class NTUOpenPersonalLoanCommandHandler : IHandlesDomainServiceCommand<NTUOpenPersonalLoanCommand>
	{
		private IDebtCounsellingRepository debtCounsellingRepository;
		private IX2Repository x2Repository;
		private ITransactionManager transactionManager;
		public NTUOpenPersonalLoanCommandHandler(IDebtCounsellingRepository debtCounsellingRepository, IX2Repository x2Repository, ITransactionManager transactionManager)
		{
			this.debtCounsellingRepository = debtCounsellingRepository;
			this.x2Repository = x2Repository;
			this.transactionManager = transactionManager;
		}
		public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, NTUOpenPersonalLoanCommand command)
		{
            try
            {
                using (var transaction = transactionManager.BeginTransactionReadUncommitted(TransactionModeEnum.New))
                {
                    var debtCounsellingCase = debtCounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);
                    foreach (var legalEntity in debtCounsellingCase.Clients)
                    {
                        var personalLoanApplication = legalEntity.PersonalLoanApplication;
                        if (personalLoanApplication != null && personalLoanApplication.IsOpen)
                        {
                            var instance = x2Repository.GetInstanceForGenericKey(personalLoanApplication.Key, Constants.WorkFlowName.PersonalLoans, Constants.WorkFlowProcessName.PersonalLoan);
                            this.x2Repository.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.PersonalLoanExternalNTU, instance.ID, SAHL.Common.Constants.WorkFlowName.PersonalLoans, SAHL.Common.Constants.WorkFlowProcessName.PersonalLoan, null);
                        }
                    }
                }
            }
            finally
            {
                transactionManager.Dispose();
            }
		}
	}
}
