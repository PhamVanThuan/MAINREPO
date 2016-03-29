using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class AddDetailTypeInstructionSentCommandHandler : IHandlesDomainServiceCommand<AddDetailTypeInstructionSentCommand>
    {
        IApplicationRepository applicationRepository;
        IAccountRepository accountRepository;
        ILookupRepository lookupRepository;
        ICastleTransactionsService castleTransactionService;

        public AddDetailTypeInstructionSentCommandHandler(IApplicationRepository applicationRepository, ICastleTransactionsService castleTransactionService)
        {
            this.applicationRepository = applicationRepository;
            this.castleTransactionService = castleTransactionService;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, AddDetailTypeInstructionSentCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            int accountKey = application.ReservedAccount.Key;

            string SQL = string.Format("UPDATE [2am].dbo.DETAIL SET	DetailTypekey 	= 3,DetailDate 	= GETDATE() WHERE Accountkey = {0} AND DetailTypekey = 2", accountKey);
            this.castleTransactionService.ExecuteNonQueryOnCastleTran(SQL, Databases.TwoAM, null);
            SQL = string.Format("UPDATE sahldb.dbo.REGMAIL SET 	DetailTypeNumber = 3, RegMailDateTime  = GETDATE() WHERE 	LoanNumber = {0} AND DetailTypeNumber = 2", accountKey);
            this.castleTransactionService.ExecuteNonQueryOnCastleTran(SQL, Databases.TwoAM, null);

            //IReadOnlyEventList<IDetail> details = accountRepository.GetDetailByAccountKeyAndDetailType(application.ReservedAccount.Key, (int)DetailTypes.InstructionNotSent);
            //foreach (var detail in details)
            //{
            //    detail.DetailType = lookupRepository.DetailTypes.ObjectDictionary[Convert.ToString((int)DetailTypes.InstructionSent)];
            //    detail.DetailDate = DateTime.Now;
            //    accountRepository.SaveDetail(detail);
            //}
        }
    }
}