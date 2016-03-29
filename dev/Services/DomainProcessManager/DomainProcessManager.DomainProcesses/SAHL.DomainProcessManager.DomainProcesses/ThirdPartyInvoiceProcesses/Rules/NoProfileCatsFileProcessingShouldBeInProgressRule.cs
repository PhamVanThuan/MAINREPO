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
    public class NoCatsFileForProfileShouldBePendindingRule : IDomainRule<PayThirdPartyInvoiceProcessModel>
    {
        private ICATSServiceClient catsService;
        public NoCatsFileForProfileShouldBePendindingRule(ICATSServiceClient catsService)
        {
            this.catsService = catsService;
        }
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, PayThirdPartyInvoiceProcessModel ruleModel)
        {

            var query = new IsThereACatsFileBeingProcessedForProfileQuery((int)Core.BusinessModel.Enums.CATSPaymentBatchType.ThirdPartyInvoice);
            catsService.PerformQuery(query);
            var isThereACatsFileBeingProcessedForProfile = query.Result.Results.FirstOrDefault();
            if (isThereACatsFileBeingProcessedForProfile)
            {
                messages.AddMessage(new SystemMessage("There is a pending payment, please try again after 15 minutes.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}