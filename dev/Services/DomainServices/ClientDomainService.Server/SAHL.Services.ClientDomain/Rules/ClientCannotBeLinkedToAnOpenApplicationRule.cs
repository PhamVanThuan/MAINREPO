using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class ClientCannotBeLinkedToAnOpenApplicationRule : IDomainRule<NaturalPersonClientRuleModel>
    {
        private IClientDataManager clientDataManager;

        public const string ErrorMessage = "Client is linked to an open application.";

        public ClientCannotBeLinkedToAnOpenApplicationRule(IClientDataManager clientDataManager)
        {
            this.clientDataManager = clientDataManager;
        }

        public void ExecuteRule(ISystemMessageCollection messages, NaturalPersonClientRuleModel ruleModel)
        {
            // check the client is not linked to an open account
            IEnumerable<int> openAccounts = this.clientDataManager.FindOpenApplicationNumbersForClient(ruleModel.ClientKey);
            if (openAccounts.Any())
            {
                messages.AddMessage(new SystemMessage(ErrorMessage, SystemMessageSeverityEnum.Error));
            }
        }
    }
}