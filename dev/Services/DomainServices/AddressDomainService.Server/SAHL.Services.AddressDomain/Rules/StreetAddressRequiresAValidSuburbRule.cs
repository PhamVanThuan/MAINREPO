using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.AddressDomain.Rules
{
    public class StreetAddressRequiresAValidSuburbRule : IDomainRule<AddressDataModel>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, AddressDataModel addressModel)
        {
            if (!addressModel.SuburbKey.HasValue || addressModel.SuburbKey <= 0)
            {
                messages.AddMessage(new SystemMessage("No matching suburb could be found for the address.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}