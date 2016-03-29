using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ApplicationMailingAddressCannotBeAFreeTextAddressRule : IDomainRule<ApplicationMailingAddressModel>
    {
        private IDomainQueryServiceClient domainQueryService;

        public ApplicationMailingAddressCannotBeAFreeTextAddressRule(IDomainQueryServiceClient domainQueryService)
        {
            this.domainQueryService = domainQueryService;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationMailingAddressModel ruleModel)
        {
            var getclientAddressQuery = new GetClientAddressQuery(ruleModel.ClientAddressKey);
            domainQueryService.PerformQuery(getclientAddressQuery);
            if (getclientAddressQuery.Result.Results.Any())
            {
                if (getclientAddressQuery.Result.Results.First().AddressFormatKey == (int)AddressFormat.FreeText)
                {
                    messages.AddMessage(new SystemMessage("A Free Text address cannot be used as an Application Mailing Address.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}