using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Rules
{
    public class AtLeastOneClientContactDetailShouldBeProvidedRule : IDomainRule<IClientContactDetails>
    {
        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, IClientContactDetails model)
        {
            if (string.IsNullOrWhiteSpace(model.EmailAddress)
                && string.IsNullOrWhiteSpace(model.Cellphone)
                && (string.IsNullOrWhiteSpace(model.HomePhoneCode) || string.IsNullOrWhiteSpace(model.HomePhone))
                && (string.IsNullOrWhiteSpace(model.WorkPhoneCode) || string.IsNullOrWhiteSpace(model.WorkPhone)))
            {
                messages.AddMessage(new SystemMessage("At least one valid contact detail (An Email Address, Home, Work or Cell Number) is required.", SystemMessageSeverityEnum.Error));
            }
        }
    }
}