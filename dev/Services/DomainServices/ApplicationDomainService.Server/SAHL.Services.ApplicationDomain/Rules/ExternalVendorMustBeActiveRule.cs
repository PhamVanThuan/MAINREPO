using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Domicilium;
using SAHL.Services.Interfaces.ApplicationDomain.Models;

namespace SAHL.Services.ApplicationDomain.Rules
{
    public class ExternalVendorMustBeActiveRule : IDomainRule<VendorModel>
    {
        public void ExecuteRule(ISystemMessageCollection messages, VendorModel vendor)
        {
            if (vendor.GeneralStatusKey != (int)GeneralStatus.Active)
            {
                messages.AddMessage(new SystemMessage("Vendor is not active.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}