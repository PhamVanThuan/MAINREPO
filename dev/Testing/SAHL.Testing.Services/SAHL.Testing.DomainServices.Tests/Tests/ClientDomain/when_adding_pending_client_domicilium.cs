using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_adding_pending_client_domicilium : ServiceTestBase<IClientDomainServiceClient>
    {
        private IEnumerable<GetClientAddressesQueryResult> _addresses;
        private int _clientAddressKey;
        private ActiveNewBusinessApplicantsDataModel _newBusinessApplicant;

        [SetUp]
        public void OnSetup()
        {
            _newBusinessApplicant = TestApiClient.Get<ActiveNewBusinessApplicantsDataModel>(new { hasdomicilium = 0, hasresidentialaddress = 1 }, limit: 1).First();
            var query = new GetClientAddressesQuery(_newBusinessApplicant.LegalEntityKey);
            base.PerformQuery(query);
            _addresses = query.Result.Results;
            _clientAddressKey = _addresses.First().LegalEntityAddressKey;
        }

        [TearDown]
        public void OnTestTeardown()
        {
            if (_newBusinessApplicant != null)
            {
                var command = new RemoveApplicantFromActiveNewBusinessApplicantsCommand(_newBusinessApplicant.OfferRoleKey);
                base.PerformCommand(command);
                _newBusinessApplicant = null;
            }
            _addresses = null;
            _clientAddressKey = 0;
        }

        [Test]
        public void when_successful()
        {
            ClientAddressAsPendingDomiciliumModel model = new ClientAddressAsPendingDomiciliumModel(_clientAddressKey, _newBusinessApplicant.LegalEntityKey);
            var command = new AddClientAddressAsPendingDomiciliumCommand(model, this.linkedGuid);
            base.Execute<AddClientAddressAsPendingDomiciliumCommand>(command).WithoutErrors();
            var clientDomiciliumAddresses = TestApiClient.Get<LegalEntityDomiciliumDataModel>(new { legalentityaddresskey = _clientAddressKey });
            Assert.That(clientDomiciliumAddresses.Where(x => x.GeneralStatusKey == (int)GeneralStatus.Pending
                && x.LegalEntityAddressKey == _clientAddressKey).First() != null, "Client Domicilium Address not created correctly.");
        }

        [Test]
        public void when_unsuccessful()
        {
            //make the address record inactive
            var setClientAddressToInactiveCommand = new SetClientAddressToInactiveCommand(_clientAddressKey);
            base.PerformCommand(setClientAddressToInactiveCommand);
            ClientAddressAsPendingDomiciliumModel model = new ClientAddressAsPendingDomiciliumModel(_clientAddressKey, _newBusinessApplicant.LegalEntityKey);
            AddClientAddressAsPendingDomiciliumCommand command = new AddClientAddressAsPendingDomiciliumCommand(model, this.linkedGuid);
            base.Execute<AddClientAddressAsPendingDomiciliumCommand>(command)
                .AndExpectThatErrorMessagesContain("An active client address must be provided.");
        }
    }
}