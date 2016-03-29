﻿using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.SystemMessages;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.ApplicationDomain.Rules;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs
{
    public class when_offer_information_is_accepted : WithCoreFakes
    {
        private static ISystemMessageCollection _messages;
        private static OfferInformationDataModel _offerInformationDataModel;
        private static ApplicationMayNotBeAcceptedWhenDeterminingApplicationHouseholdIncomeRule _rule;

        private Establish context = () =>
        {
            _messages                  = SystemMessageCollection.Empty();
            _offerInformationDataModel = new OfferInformationDataModel(DateTime.Today.AddDays(-1), 1, (int)OfferInformationType.AcceptedOffer, "user", null, null);
            _rule                      = new ApplicationMayNotBeAcceptedWhenDeterminingApplicationHouseholdIncomeRule();
        };

        private Because of = () =>
        {
            _rule.ExecuteRule(_messages, _offerInformationDataModel);
        };

        private It should_return_error_message = () =>
        {
            _messages.HasErrors.ShouldBeTrue();
            _messages.ErrorMessages().ShouldContain(x => x.Message.Contains("Household Income cannot be determined once the application has been accepted"));
        };
    }
}
