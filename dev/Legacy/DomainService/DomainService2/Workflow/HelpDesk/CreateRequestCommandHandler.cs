using System;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.HelpDesk
{
    public class CreateRequestCommandHandler : IHandlesDomainServiceCommand<CreateRequestCommand>
    {
        private ICastleTransactionsService castleTransactionService;

        public CreateRequestCommandHandler(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        public void Handle(IDomainMessageCollection messages, CreateRequestCommand command)
        {
            string sb = string.Format("SELECT [2AM].[dbo].[LegalEntityLegalName]( {0}, 0)  as LegalEntityLegalName", command.LegalEntityKey.ToString());

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), SAHL.Common.Globals.Databases.X2, null);
            if (ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("Unable to find LegalEntityName");
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                command.Result = dr["LegalEntityLegalName"].ToString();
            }
        }
    }
}