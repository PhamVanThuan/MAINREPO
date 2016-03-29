using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Rules
{
    public class PassportNumberCannotBeAValidIdentityNumberRule : IDomainRule<INaturalPersonClientModel>
    {
        private IValidationUtils validationUtils;

        public PassportNumberCannotBeAValidIdentityNumberRule(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, INaturalPersonClientModel model)
        {
            if (!String.IsNullOrWhiteSpace(model.PassportNumber))
            {
                if (validationUtils.ValidateIDNumber(model.PassportNumber))
                {
                    messages.AddMessage(new SystemMessage("A passport number cannot be a valid identity number.", SystemMessageSeverityEnum.Error));
                }
            }
        }
    }
}