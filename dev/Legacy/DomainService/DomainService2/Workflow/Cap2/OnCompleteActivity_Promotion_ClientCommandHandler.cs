using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;

namespace DomainService2.Workflow.Cap2
{
    public class OnCompleteActivity_Promotion_ClientCommandHandler : IHandlesDomainServiceCommand<OnCompleteActivity_Promotion_ClientCommand>
    {
        private ICastleTransactionsService service;
        private IUIStatementService uistatementservice;

        public OnCompleteActivity_Promotion_ClientCommandHandler(ICastleTransactionsService service, IUIStatementService uistatementservice)
        {
            this.service = service;
            this.uistatementservice = uistatementservice;
        }

        public void Handle(IDomainMessageCollection messages, OnCompleteActivity_Promotion_ClientCommand command)
        {
            string query = uistatementservice.GetStatement("COMMON", "AcceptCapOffer");

            ParameterCollection Parameters = new ParameterCollection();
            Parameters.Add(new SqlParameter("@CapOfferKey", command.ApplicationKey));
            Parameters.Add(new SqlParameter("@CapOfferDetailKey", command.ApplicationDetailKey));
            Parameters.Add(new SqlParameter("@Promotion", command.Promotion));
            Parameters.Add(new SqlParameter("@CapStatus", command.CapStatusKey));
            Parameters.Add(new SqlParameter("@CapPaymentOptionKey", command.CapPaymentOptionKey));

            service.ExecuteQueryOnCastleTran(query, SAHL.Common.Globals.Databases.TwoAM, Parameters);
        }
    }
}