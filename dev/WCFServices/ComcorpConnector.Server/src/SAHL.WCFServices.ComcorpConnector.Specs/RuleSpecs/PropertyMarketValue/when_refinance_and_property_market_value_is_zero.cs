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
    public sealed class when_refinance_and_property_market_value_is_zero : WithCoreFakes
    {
        private static PropertyMarketValueValidation rule;
        private static Application application;
        private static IValidationUtils _validationUtils;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            _validationUtils = An<IValidationUtils>();
            _validationUtils.WhenToldTo(x => x.ParseEnum<OfferType>("Refinance Loan")).Return(OfferType.RefinanceLoan);
            application = new Application();
            application.SahlLoanPurpose = "Refinance Loan";
            application.PropertyMarketValue = 0;
            rule = new PropertyMarketValueValidation(_validationUtils);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, application);
        };

        private It should_have_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEndWith("Estimated Market Value of the Home must be greater than R 0.00");
        };
    }
}
