using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.PropertyMarketValue
{
    public sealed class when_switch_and_property_market_value_is_greater_than_zero : WithCoreFakes
    {
        private static PropertyMarketValueValidation rule;
        private static Application application;
        private static IValidationUtils _validationUtils;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            _validationUtils = An<IValidationUtils>();
            _validationUtils.WhenToldTo(x => x.ParseEnum<OfferType>("Switch Loan")).Return(OfferType.SwitchLoan);
            application = new Application();
            application.SahlLoanPurpose = "Switch Loan";
            application.PropertyMarketValue = 10000;
            rule = new PropertyMarketValueValidation(_validationUtils);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, application);
        };

        private It should_have_no_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
