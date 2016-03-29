using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Exchange.WebServices.Data;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Exchange.Specs.Fakes;
using System;
using System.Linq;

namespace SAHL.Core.Exchange.Specs.ExchangeProviderSpecs
{
    public class when_connecting_to_a_mailbox : WithExchangeProviderFakes
    {

        private static bool isOpened;

        private Establish that = () =>
        {
            isOpened = false;
        };

        private Because of = () =>
        {
            isOpened = exchangeProvider.OpenMailboxConnection();
        };

        private It should_open_the_mailbox_connection = () =>
        {
            isOpened.ShouldBeTrue();
        };
    }
}