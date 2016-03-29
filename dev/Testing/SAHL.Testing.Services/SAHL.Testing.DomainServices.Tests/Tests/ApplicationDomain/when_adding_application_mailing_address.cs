using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_adding_application_mailing_address : ServiceTestBase<IApplicationDomainServiceClient>
    {
        private GetApplicantWithApplicationCriteriaQueryResult _queryResult;
        private IEnumerable<GetClientAddressesQueryResult> _clientAddresses;

        [SetUp]
        public void OnSetup()
        {
            GetApplicantWithApplicationCriteriaQuery appQuery = new GetApplicantWithApplicationCriteriaQuery(true, false);
            base.PerformQuery(appQuery);
            _queryResult = appQuery.Result.Results.First();
            var getClientAddressesQuery = new GetClientAddressesQuery(_queryResult.LegalEntityKey);
            base.PerformQuery(getClientAddressesQuery);
            _clientAddresses = getClientAddressesQuery.Result.Results
                .Where(x => x.AddressFormatKey != (int)AddressFormat.FreeText);
        }

        [TearDown]
        public void OnTestComplete()
        {
            RemoveApplicationMailingAddressCommand command = new RemoveApplicationMailingAddressCommand(_queryResult.OfferKey);
            base.PerformCommand(command);
        }

        [Test]
        public void when_successful()
        {
            var model = new ApplicationMailingAddressModel(applicationNumber: _queryResult.OfferKey, clientAddressKey: _clientAddresses.First().LegalEntityAddressKey, correspondenceLanguage: CorrespondenceLanguage.English,
                onlineStatementFormat: OnlineStatementFormat.Text, correspondenceMedium: CorrespondenceMedium.Post, clientToUseForEmailCorrespondence: null, onlineStatementRequired: true);
            var command = new AddApplicationMailingAddressCommand(model);
            base.Execute<AddApplicationMailingAddressCommand>(command)
                .WithoutErrors();
            var mailingAddress = TestApiClient.Get<OfferMailingAddressDataModel>(new { offerkey = model.ApplicationNumber });
            Assert.IsNotNull(mailingAddress.FirstOrDefault());
        }

        [Test]
        public void when_unsuccessful()
        {
            var model = new ApplicationMailingAddressModel(applicationNumber: _queryResult.OfferKey, clientAddressKey: 999999, correspondenceLanguage: CorrespondenceLanguage.English,
                onlineStatementFormat: OnlineStatementFormat.Text, correspondenceMedium: CorrespondenceMedium.Post, clientToUseForEmailCorrespondence: null, onlineStatementRequired: true);
            var command = new AddApplicationMailingAddressCommand(model);
            base.Execute<AddApplicationMailingAddressCommand>(command);
            var mailingAddress = TestApiClient.Get<OfferMailingAddressDataModel>(new { offerkey = model.ApplicationNumber });
            Assert.IsNull(mailingAddress.FirstOrDefault());
        }
    }
}