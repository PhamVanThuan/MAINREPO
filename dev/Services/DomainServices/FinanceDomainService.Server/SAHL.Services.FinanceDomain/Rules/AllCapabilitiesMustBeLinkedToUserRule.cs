using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System.Linq;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules
{
    public class AllCapabilitiesMustBeLinkedToUserRule : IDomainRule<ServiceRequestMetadata>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        public AllCapabilitiesMustBeLinkedToUserRule(IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager)
        {
            this.thirdPartyInvoiceDataManager = thirdPartyInvoiceDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, ServiceRequestMetadata ruleModel)
        {
            if (ruleModel.UserOrganisationStructureKey.HasValue)
            {
                var linkedUserCapabilities = thirdPartyInvoiceDataManager
                    .GetUserCapabilitiesByUserOrgStructureKey(ruleModel.UserOrganisationStructureKey.Value);

                var allCapabilitiesAreLinkedToUser = linkedUserCapabilities.All(ruleModel.CurrentUserCapabilities.Contains)
                    && ruleModel.CurrentUserCapabilities.Length == linkedUserCapabilities.Count
                    && ruleModel.CurrentUserCapabilities.Length > 0;

                if (!allCapabilitiesAreLinkedToUser)
                {
                    messages.AddMessage(new SystemMessage("One or more capabilities are not linked to user", SystemMessageSeverityEnum.Error));
                }
            }
            else if(ruleModel.CurrentUserCapabilities.Length > 0)
            {
                messages.AddMessage(new SystemMessage("One or more capabilities are not linked to user", SystemMessageSeverityEnum.Error));
            }
        }
    }
}