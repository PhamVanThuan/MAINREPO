using System;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Cap2
{
    public class UpdateCapOfferStatusCommandHandler : IHandlesDomainServiceCommand<UpdateCapOfferStatusCommand>
    {
        private ICastleTransactionsService service;
        private IUIStatementService uistatementservice;

        public UpdateCapOfferStatusCommandHandler(ICastleTransactionsService service, IUIStatementService uistatementservice)
        {
            this.service = service;
            this.uistatementservice = uistatementservice;
        }

        public void Handle(IDomainMessageCollection messages, UpdateCapOfferStatusCommand command)
        {
            string query = uistatementservice.GetStatement("COMMON", "UpdateCapOfferStatus");
            query = query.Replace("@CapStatus", Convert.ToString(command.StatusKey));
            query = query.Replace("@CapOfferKey", command.ApplicationKey.ToString());
            service.ExecuteNonQueryOnCastleTran(query, SAHL.Common.Globals.Databases.TwoAM, null);
        }
    }
}