using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Managers.Client.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_finding_open_account_numbers_for_a_client : WithFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static ClientDataManager clientDataManager;
        private static int clientKey;
        private static IEnumerable<int> accountNumbers;
        private static IEnumerable<int> results;

        private Establish context = () =>
        {
            accountNumbers = new int[] { 1234567, 1122334, 2233445 };
            clientKey = 1;
            fakeDbFactory = new FakeDbFactory();
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.Select(Param.IsAny<FindOpenAccountKeysForClientStatement>())).Return(accountNumbers);
            clientDataManager = new ClientDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            results = clientDataManager.FindOpenAccountKeysForClient(clientKey);
        };

        private It should_return_the_account_numbers_from_the_query_statement = () =>
        {
            results.ShouldEqual(accountNumbers);
        };

        private It should_use_the_client_key_provided_when_finding_accounts = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select(Arg.Is<FindOpenAccountKeysForClientStatement>(
                y=>y.ClientKey == clientKey
                )));
        };
    }
}