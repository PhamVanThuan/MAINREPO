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
    public class PayAttorneyInvoiceProcessShouldRunOnceADayRule : IDomainRule<PayThirdPartyInvoiceProcessModel>
    {
        private ICATSServiceClient catsService;
        public PayAttorneyInvoiceProcessShouldRunOnceADayRule(ICATSServiceClient catsService)
        {
            this.catsService = catsService;
        }
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, PayThirdPartyInvoiceProcessModel ruleModel)
        {
            var query = new DoesCatsPaymentBatchForTodayExistQuery();
            catsService.PerformQuery(query);
            if (query.Result.Results.FirstOrDefault().BatchExists)
            {
                messages.AddMessage(new SystemMessage("The Pay Attorney Invoice Process has already been executed", SystemMessageSeverityEnum.Error));
            }
        }
    }
}