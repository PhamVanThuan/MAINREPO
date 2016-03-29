using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowSqlApiExceptionCommandHandler : IHandlesDomainServiceCommand<ThrowSqlApiExceptionCommand>
    {
        private ICastleTransactionsService castleTransactionsService;

        public ThrowSqlApiExceptionCommandHandler(ICastleTransactionsService castleTransactionsService)
        {
            this.castleTransactionsService = castleTransactionsService;
        }

        public void Handle(IDomainMessageCollection messages, ThrowSqlApiExceptionCommand command)
        {
            castleTransactionsService.ExecuteHaloAPI_uiS_OnCastleTranForRead("COMMON", "ThrowApiError", null);
        }
    }
}