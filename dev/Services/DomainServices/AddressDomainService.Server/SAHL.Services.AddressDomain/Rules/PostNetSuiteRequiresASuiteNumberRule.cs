using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.AddressDomain.Rules
{
    public class PostNetSuiteRequiresASuiteNumberRule : IDomainRule<AddressDataModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, AddressDataModel addressModel)
        {
            if (addressModel.AddressFormatKey == (int)AddressFormat.PostNetSuite && string.IsNullOrEmpty(addressModel.SuiteNumber))
            {
                messages.AddMessage(new SystemMessage("A postnet suite address requires a postnet suite number.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}