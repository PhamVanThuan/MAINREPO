using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Rules
{
    public class IdNumberMustBeValidWhenProvidedForASACitizenRule : IDomainRule<INaturalPersonClientModel>
    {
        private IValidationUtils validationUtils;

        public IdNumberMustBeValidWhenProvidedForASACitizenRule(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, INaturalPersonClientModel ruleModel)
        {
            if (ruleModel.CitizenshipType == SAHL.Core.BusinessModel.Enums.CitizenType.SACitizen || ruleModel.CitizenshipType == SAHL.Core.BusinessModel.Enums.CitizenType.SACitizen_NonResident)
            {
                if (!String.IsNullOrWhiteSpace(ruleModel.IDNumber))
                {
                    if (!validationUtils.ValidateIDNumber(ruleModel.IDNumber))
                    {
                        messages.AddMessage(new SystemMessage("The clients Identity Number is invalid.", SystemMessageSeverityEnum.Error));
                    }
                }
            }
        }
    }
}