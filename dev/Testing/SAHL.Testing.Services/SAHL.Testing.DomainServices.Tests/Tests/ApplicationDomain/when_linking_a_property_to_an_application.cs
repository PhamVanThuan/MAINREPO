using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_linking_a_property_to_an_application : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetOpenNewBusinessApplicationQuery query = new GetOpenNewBusinessApplicationQuery(hasDebitOrder: false, hasMailingAddress: false, hasProperty: true, isAccepted: false,
                householdIncome: 0);
            base.PerformQuery(query);
            var application = query.Result.Results.First();
            var getPropertyQuery = new GetPropertyNotLinkedToAnApplicationQuery();
            base.PerformQuery(getPropertyQuery);
            PropertyDataModel property = getPropertyQuery.Result.Results.First();
            var model = new LinkPropertyToApplicationCommandModel(application.OfferKey, property.PropertyKey);
            var command = new LinkPropertyToApplicationCommand(model);
            base.Execute<LinkPropertyToApplicationCommand>(command);
            var offerMortgageLoan = TestApiClient.GetByKey<OfferMortgageLoanDataModel>(application.OfferKey);
            var propertyAdded = TestApiClient.GetByKey<PropertyDataModel>(offerMortgageLoan.PropertyKey.Value);
            Assert.That(propertyAdded.PropertyKey == command.PropertyKey);
        }
    }
}