using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.CATS.Queries;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcesses.Rules
{
    public class PreviousBatchFileShouldNotHaveFailedRule : IDomainRule<PayThirdPartyInvoiceProcessModel>
    {
        private ICATSServiceClient catsService;
        public PreviousBatchFileShouldNotHaveFailedRule(ICATSServiceClient catsService)
        {
            this.catsService = catsService;
        }
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, PayThirdPartyInvoiceProcessModel ruleModel)
        {
            var query = new GetPreviousFileFailureQuery(Core.BusinessModel.Enums.CATSPaymentBatchType.ThirdPartyInvoice);
            catsService.PerformQuery(query);
            if (query.Result.Results.Any())
            {
                messages.AddMessage(new SystemMessage("The previous batch file processing failed", SystemMessageSeverityEnum.Error));
            }
        }
    }
}