using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.UnsecuredLendingApplicationMailingAddressSpecs
{
    [Subject(typeof(UnsecuredLendingApplicationMailingAddress))]
    public class When_application_has_one_or_more_mailing_addresses : RulesBaseWithFakes<UnsecuredLendingApplicationMailingAddress>
    {

        Establish context = () =>
            {

                IAddress address1 = An<IAddress>();
                IApplicationMailingAddress mailingAddress1 = An<IApplicationMailingAddress>();
                mailingAddress1.WhenToldTo(x => x.Address).Return(address1);

                IAddress address2 = An<IAddress>();
                IApplicationMailingAddress mailingAddress2 = An<IApplicationMailingAddress>();
                mailingAddress2.WhenToldTo(x => x.Address).Return(address2);

                IEventList<IApplicationMailingAddress> mailingAddresses = new EventList<IApplicationMailingAddress>();
                mailingAddresses.Add(null, mailingAddress1);
                mailingAddresses.Add(null, mailingAddress2);

                IApplicationUnsecuredLending unsecuredLending = An<IApplicationUnsecuredLending>();
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

        It rule_should_pass = () =>
            {
                messages.Count.ShouldEqual(0);
            };
    }
}
