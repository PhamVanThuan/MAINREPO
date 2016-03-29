using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_adding_a_new_purchase_application : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            NewPurchaseApplicationModel newPurchaseApplication = new NewPurchaseApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 125000, 750000, 750000, 240, Product.NewVariableLoan, "Reference", 1,
                "Transfer Attorney");

            AddNewPurchaseApplicationCommand command = new AddNewPurchaseApplicationCommand(newPurchaseApplication, this.linkedGuid);
            base.Execute<AddNewPurchaseApplicationCommand>(command);
            this.linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            OfferDataModel newApplication = TestApiClient.GetByKey<OfferDataModel>(linkedKey);
            var newApplicationMortgageLoan = TestApiClient.GetByKey<OfferMortgageLoanDataModel>(linkedKey);
            var applicationInfoQuery = new GetLatestApplicationInformationVariableLoanQuery(linkedKey);
            base.PerformQuery(applicationInfoQuery);
            var newApplicationInformation = applicationInfoQuery.Result.Results.First();
            var newApplicationInformationVariableLoan = TestApiClient.GetByKey<OfferInformationVariableLoanDataModel>(newApplicationInformation.OfferInformationKey);
            var newApplicationInformationInterestOnly = TestApiClient.GetByKey<OfferInformationInterestOnlyDataModel>(newApplicationInformation.OfferInformationKey);
            var newApplicationAttribute = TestApiClient.Get<OfferAttributeDataModel>(new { offerkey = linkedKey });

            Assert.NotNull(newApplication, "Application details were not added.");
            Assert.That(newApplication.OfferKey == linkedKey && newApplication.OfferTypeKey == (int)newPurchaseApplication.ApplicationType && newApplication.OfferStatusKey == (int)newPurchaseApplication.ApplicationStatus
                && newApplication.OfferSourceKey == newPurchaseApplication.ApplicationSourceKey && newApplication.ReservedAccountKey > 0
                && newApplication.OriginationSourceKey == (int)OriginationSource.SAHomeLoans, "Incorrect application details were added.");

            Assert.NotNull(newApplicationMortgageLoan, "Application mortgage loan details were not added.");
            Assert.That(newApplicationMortgageLoan.MortgageLoanPurposeKey == (int)MortgageLoanPurpose.Newpurchase && newApplicationMortgageLoan.ApplicantTypeKey == (int)ApplicantType.Single
                && newApplicationMortgageLoan.NumApplicants == newPurchaseApplication.ApplicantCount && newApplicationMortgageLoan.PurchasePrice == Convert.ToDouble(newPurchaseApplication.PurchasePrice)
                && newApplicationMortgageLoan.ResetConfigurationKey == (int)ResetConfiguration.Eighteenth && newApplicationMortgageLoan.ClientEstimatePropertyValuation == Convert.ToDouble(newPurchaseApplication.PurchasePrice)
                && newApplicationMortgageLoan.DocumentLanguageKey == (int)Language.English, "Incorrect application mortgage loan details were added.");

            Assert.NotNull(newApplicationInformation, "Application information details were not added.");
            Assert.That(newApplicationInformation.OfferKey == linkedKey && newApplicationInformation.OfferInformationTypeKey == (int)OfferInformationType.OriginalOffer
                && newApplicationInformation.ProductKey == (int)newPurchaseApplication.Product, "Incorrect application information details were added.");

            Assert.NotNull(newApplicationInformationVariableLoan, "Application information variable loan details were not added.");
            Assert.That(newApplicationInformationVariableLoan.OfferInformationKey == newApplicationInformation.OfferInformationKey && newApplicationInformationVariableLoan.Term == newPurchaseApplication.Term
                && newApplicationInformationVariableLoan.CashDeposit == Convert.ToDouble(newPurchaseApplication.Deposit)
                && newApplicationInformationVariableLoan.PropertyValuation == Convert.ToDouble(newPurchaseApplication.PurchasePrice), "Incorrect application information variable loan details were added.");

            Assert.NotNull(newApplicationInformationInterestOnly, "Application information interest only details were not added.");
            Assert.That(newApplicationInformationInterestOnly.OfferInformationKey == newApplicationInformation.OfferInformationKey, "Incorrect application information interest only details were added.");

            Assert.NotNull(newApplicationAttribute, "Application attribute details were not added.");
            Assert.That(newApplicationAttribute.Where(x => x.OfferAttributeTypeKey == (int)OfferAttributeType.ComcorpLoan).First() != null, "Incorrect application attribute details were added.");
        }
    }
}