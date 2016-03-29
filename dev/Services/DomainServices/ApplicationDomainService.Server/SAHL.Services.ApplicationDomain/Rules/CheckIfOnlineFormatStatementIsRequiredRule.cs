using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class CheckIfOnlineFormatStatementIsRequiredRule : IDomainRule<ApplicationMailingAddressModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationMailingAddressModel ruleModel)
        {
            var systemMessages = SystemMessageCollection.Empty();
            if (!ruleModel.OnlineStatementRequired)
            {
                if(ruleModel.OnlineStatementFormat != OnlineStatementFormat.NotApplicable)
                {
                    systemMessages.AddMessage(
                        new SystemMessage("An online statement format is not required when the option for the online statement has not been selected.", 
                            SystemMessageSeverityEnum.Error));

                }
            }
            else if (ruleModel.OnlineStatementFormat == OnlineStatementFormat.NotApplicable)
            {
                systemMessages.AddMessage(
                    new SystemMessage("The format of the online statement must be selected when an online statement is required.", 
                        SystemMessageSeverityEnum.Error));

            }

            messages.Aggregate(systemMessages);
        }
    }
}