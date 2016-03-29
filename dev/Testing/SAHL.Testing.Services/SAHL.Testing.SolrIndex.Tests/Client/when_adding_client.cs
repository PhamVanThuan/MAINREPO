using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests.Client
{
    [TestFixture]
    public class when_adding_client : SolrIndexTest
    {
        private Guid clientGuid;

        [TearDown]
        public void TearDown()
        {
            if (clientGuid != null)
            {
                _linkedKeyManager.DeleteLinkedKey(clientGuid);
            }
        }

        [Test]
        public void when_successful()
        {
            //Search for Open, New Purchase Offer on 2am using TestApiClient
            var offer = TestApiClient.GetAny<OfferDataModel>(new { OfferTypeKey = (int)OfferType.NewPurchaseLoan, OfferStatusKey = (int)OfferStatus.Open }, 1000);

            //Add new InsertClientCommand to FrontEndTestService that inserts a new LegalEntity and OfferRole record for an Offer
            var getUnusedIDNumber = new GetUnusedIDNumberQuery();
            _feTestClient.PerformQuery(getUnusedIDNumber).WithoutMessages();
            var idNumber = getUnusedIDNumber.Result.Results.FirstOrDefault().IDNumber;
            var firstName = DataGenerator.RandomString(10);
            var surname = DataGenerator.RandomString(10);
            LegalEntityDataModel legalEntity = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, (int)MaritalStatus.Single, (int)Gender.Male, (int)PopulationGroup.Unknown,
                                                                       DateTime.Now, null, firstName, "VP", surname, null, idNumber, null, null, null, null, null, null, "031", "5631970", null,
                                                                       null, "0745847268", null, null, null, null, (int)CitizenType.SACitizen, (int)LegalEntityStatus.Alive, "",
                                                                       null, "BCUser5", null, (int)Education.UniversityDegree, (int)Language.English,
                                                                       (int)Language.English, null);

            clientGuid = CombGuid.Instance.Generate();
            var icc = new InsertClientCommand(legalEntity, offer.OfferKey, clientGuid);
            _feTestClient.PerformCommand(icc, metadata).WithoutMessages();
            var offerRoleKey = _linkedKeyManager.RetrieveLinkedKey(clientGuid);
            var offerRole = TestApiClient.GetByKey<OfferRoleDataModel>(offerRoleKey);

            //Use SearchService to search for the new LegalEntity and check that it is associated with the Offer
            searchFilters.Add(new SearchFilter("OfferKey", offer.OfferKey.ToString()));
            var clientQuery = new SearchForClientQuery("Open", searchFilters, "Client");
            AssertLegalEntityReturnedInClientSearch(clientQuery, offerRole.LegalEntityKey, 1);
        }
    }
}