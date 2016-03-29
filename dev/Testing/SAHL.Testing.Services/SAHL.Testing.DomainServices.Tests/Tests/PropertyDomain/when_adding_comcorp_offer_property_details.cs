using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.PropertyDomain;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using SAHL.Services.Interfaces.PropertyDomain.Models;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.PropertyDomain
{
    [TestFixture]
    public class when_adding_comcorp_offer_property_details : ServiceTestBase<IPropertyDomainServiceClient>
    {
        [Test]
        public void when_successfully_adding()
        {
            var query = new GetUnusedIDNumberQuery();
            base.PerformQuery(query);
            string sellerIdNumber = query.Result.Results.FirstOrDefault().IDNumber;
            var comcorpOfferPropertyDetails = new ComcorpOfferPropertyDetailsModel(sellerIDNo: sellerIdNumber, sahlOccupancyType: OccupancyType.OwnerOccupied.ToString(),
                sahlPropertyType: PropertyType.Flat.ToString(), sahlTitleType: TitleType.Freehold.ToString(), sectionalTitleUnitNo: "9", complexName: "SAHL Head Office", streetNo: "512", streetName: "Soofie Saheb",
                suburb: "Durban North", city: "Durban", province: "Kwazulu-Natal", postalCode: "4001", contactCellphone: "0721234567", contactName: "Test", namePropertyRegistered: "Test", standErfNo: "ERF12345", portionNo: "9");
            var applicationQuery = new GetApplicationWithoutComcorpOfferPropertyDetailsQuery();
            base.PerformQuery(applicationQuery);
            var application = applicationQuery.Result.Results.First();
            var command = new AddComcorpOfferPropertyDetailsCommand(application.OfferKey, comcorpOfferPropertyDetails);
            base.Execute<AddComcorpOfferPropertyDetailsCommand>(command);
            var getComcorpDetailsQuery = new GetComcorpOfferPropertyDetailsQuery(application.OfferKey);
            base.PerformQuery(getComcorpDetailsQuery);
            var newComcorpOfferPropertyDetails = getComcorpDetailsQuery.Result.Results.First();
            Assert.IsNotNull(newComcorpOfferPropertyDetails);
            AssertResults(comcorpOfferPropertyDetails, newComcorpOfferPropertyDetails);
        }

        [Test]
        public void when_successfully_updating()
        {
            var getUnusedIDNumberQuery = new GetUnusedIDNumberQuery();
            base.PerformQuery(getUnusedIDNumberQuery);
            string sellerIdNumber = getUnusedIDNumberQuery.Result.Results.FirstOrDefault().IDNumber;
            base.PerformQuery(getUnusedIDNumberQuery);
            string idNumber = getUnusedIDNumberQuery.Result.Results.FirstOrDefault().IDNumber;
            var expectedComcorpOfferPropertyDetails = new ComcorpOfferPropertyDetailsModel(sellerIDNo: sellerIdNumber, sahlOccupancyType: OccupancyType.OwnerOccupied.ToString(),
                sahlPropertyType: PropertyType.Flat.ToString(), sahlTitleType: TitleType.Freehold.ToString(), sectionalTitleUnitNo: "9", complexName: "SAHL Head Office", streetNo: "512", streetName: "Soofie Saheb",
                suburb: "Durban North", city: "Durban", province: "Kwazulu-Natal", postalCode: "4001", contactCellphone: "0721234567", contactName: "Test", namePropertyRegistered: "Test", standErfNo: "ERF12345", portionNo: "9");
            var applicationQuery = new GetApplicationWithoutComcorpOfferPropertyDetailsQuery();
            base.PerformQuery(applicationQuery);
            var applicationNumber = applicationQuery.Result.Results.First().OfferKey;
            var insertedComcorpOfferPropertyDetails = new ComcorpOfferPropertyDetailsDataModel(offerKey: applicationNumber, sellerIDNo: idNumber, sAHLOccupancyType: OccupancyType.Rental.ToString(),
                sAHLPropertyType: PropertyType.House.ToString(), sAHLTitleType: TitleType.FreeholdEstate.ToString(), sectionalTitleUnitNo: "10", complexName: "SAHL Branch Office", streetNo: "514", streetName: "Clark Road",
                suburb: "Constantia", city: "Cape Town", province: "Western Cape", postalCode: "6001", contactCellphone: "0727654321", contactName: "Tester", namePropertyRegistered: "Tester", standErfNo: "ERF54321",
                portionNo: "10", insertDate: DateTime.Now, changeDate: null);
            var insertComcorpOfferPropertyDetailsCommand = new InsertComcorpOfferPropertyDetailsCommand(insertedComcorpOfferPropertyDetails);
            base.PerformCommand(insertComcorpOfferPropertyDetailsCommand);
            var command = new AddComcorpOfferPropertyDetailsCommand(applicationNumber, expectedComcorpOfferPropertyDetails);
            base.Execute<AddComcorpOfferPropertyDetailsCommand>(command);
            var getComcorpDetailsQuery = new GetComcorpOfferPropertyDetailsQuery(applicationNumber);
            base.PerformQuery(getComcorpDetailsQuery);
            var actualComcorpOfferPropertyDetails = getComcorpDetailsQuery.Result.Results.First();
            Assert.IsNotNull(actualComcorpOfferPropertyDetails);
            AssertResults(expectedComcorpOfferPropertyDetails, actualComcorpOfferPropertyDetails);
        }

        private void AssertResults(ComcorpOfferPropertyDetailsModel comcorpOfferPropertyDetails, ComcorpOfferPropertyDetailsDataModel newComcorpOfferPropertyDetails)
        {
            Assert.That(comcorpOfferPropertyDetails.SellerIDNo == newComcorpOfferPropertyDetails.SellerIDNo &&
                comcorpOfferPropertyDetails.SAHLOccupancyType == newComcorpOfferPropertyDetails.SAHLOccupancyType &&
                comcorpOfferPropertyDetails.SAHLPropertyType == newComcorpOfferPropertyDetails.SAHLPropertyType &&
                comcorpOfferPropertyDetails.SAHLTitleType == newComcorpOfferPropertyDetails.SAHLTitleType &&
                comcorpOfferPropertyDetails.SectionalTitleUnitNo == newComcorpOfferPropertyDetails.SectionalTitleUnitNo &&
                comcorpOfferPropertyDetails.ComplexName == newComcorpOfferPropertyDetails.ComplexName &&
                comcorpOfferPropertyDetails.StreetNo == newComcorpOfferPropertyDetails.StreetNo &&
                comcorpOfferPropertyDetails.StreetName == newComcorpOfferPropertyDetails.StreetName &&
                comcorpOfferPropertyDetails.Suburb == newComcorpOfferPropertyDetails.Suburb &&
                comcorpOfferPropertyDetails.City == newComcorpOfferPropertyDetails.City &&
                comcorpOfferPropertyDetails.Province == newComcorpOfferPropertyDetails.Province &&
                comcorpOfferPropertyDetails.PostalCode == newComcorpOfferPropertyDetails.PostalCode &&
                comcorpOfferPropertyDetails.ContactCellphone == newComcorpOfferPropertyDetails.ContactCellphone &&
                comcorpOfferPropertyDetails.ContactName == newComcorpOfferPropertyDetails.ContactName &&
                comcorpOfferPropertyDetails.NamePropertyRegistered == newComcorpOfferPropertyDetails.NamePropertyRegistered &&
                comcorpOfferPropertyDetails.StandErfNo == newComcorpOfferPropertyDetails.StandErfNo);
        }
    }
}