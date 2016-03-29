using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.UnsecuredLendingApplicationMailingAddressSpecs
{
    [Subject(typeof(UnsecuredLendingApplicationMailingAddress))]
    public class When_application_has_no_mailing_address : RulesBaseWithFakes<UnsecuredLendingApplicationMailingAddress>
    {

        Establish context = () =>
        {
            IApplicationType applicationType = An<IApplicationType>();
            applicationType.WhenToldTo(x => x.Key).Return((int)OfferTypes.UnsecuredLending);

            IEventList<IApplicationMailingAddress> mailingAddresses = new EventList<IApplicationMailingAddress>();

            IApplicationUnsecuredLending unsecuredLending = An<IApplicationUnsecuredLending>();
            unsecuredLending.WhenToldTo(x => x.ApplicationType).Return(applicationType);
            unsecuredLending.WhenToldTo(x => x.ApplicationMailingAddresses).Return(mailingAddresses);

            businessRule = new UnsecuredLendingApplicationMailingAddress();

            parameters = new object[]
			    {
				    unsecuredLending
			    };

            RulesBaseWithFakes<UnsecuredLendingApplicationMailingAddress>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };


    }
}
