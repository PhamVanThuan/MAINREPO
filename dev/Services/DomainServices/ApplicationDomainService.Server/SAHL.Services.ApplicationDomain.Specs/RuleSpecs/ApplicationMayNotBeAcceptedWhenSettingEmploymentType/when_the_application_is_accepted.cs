using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicationMayNotBeAcceptedWhenSettingEmploymentType
{
    public class when_the_application_is_accepted : WithFakes
    {
        private static ApplicationMayNotBeAcceptedWhenSettingApplicationEmploymentTypeRule rule;
        private static OfferInformationDataModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = new OfferInformationDataModel(1234, DateTime.Now, 1408282, (int)OfferInformationType.AcceptedOffer, @"SAHL\ClintonS", DateTime.Now, (int)Product.NewVariableLoan);
            rule = new ApplicationMayNotBeAcceptedWhenSettingApplicationEmploymentTypeRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Application employment type cannot be set once the application has been accepted");
        };
    }
}
