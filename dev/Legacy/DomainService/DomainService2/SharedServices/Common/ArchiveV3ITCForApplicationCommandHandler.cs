using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.SharedServices.Common
{
    public class ArchiveV3ITCForApplicationCommandHandler : IHandlesDomainServiceCommand<ArchiveV3ITCForApplicationCommand>
    {
        private ICastleTransactionsService service;
        private IUIStatementService uistatementservice;

        public ArchiveV3ITCForApplicationCommandHandler(ICastleTransactionsService service, IUIStatementService uistatementservice)
        {
            this.service = service;
            this.uistatementservice = uistatementservice;
        }

        public void Handle(IDomainMessageCollection messages, ArchiveV3ITCForApplicationCommand command)
        {
            if (command.ApplicationKey != 0)
            {
                string query = uistatementservice.GetStatement("COMMON", "ArchiveV3ITCForOffer").Replace("@OfferKey", command.ApplicationKey.ToString());
                service.ExecuteNonQueryOnCastleTran(query, Databases.TwoAM, null);
            }
        }
    }
}