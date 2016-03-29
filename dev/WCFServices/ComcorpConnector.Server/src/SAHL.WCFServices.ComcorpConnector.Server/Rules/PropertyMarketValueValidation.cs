using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SAHL.WCFServices.ComcorpConnector.Server.Rules
{
    public class PropertyMarketValueValidation : IDomainRule<Application>
    {
        private IValidationUtils validationUtils;
        public PropertyMarketValueValidation(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }
        public void ExecuteRule(ISystemMessageCollection messages, Application application)
        {
            OfferType applicationType = validationUtils.ParseEnum<OfferType>(application.SahlLoanPurpose);
            if ((applicationType == OfferType.RefinanceLoan || applicationType == OfferType.SwitchLoan) && application.PropertyMarketValue <= 0)
            {
                messages.AddMessage(new SystemMessage("Estimated Market Value of the Home must be greater than R 0.00", SystemMessageSeverityEnum.Error));
            }

        }
    }
}