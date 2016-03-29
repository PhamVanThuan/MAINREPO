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
    public class when_adding_a_switch_application : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            SwitchApplicationModel switchApplication = new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.Comcorp, 375000, 1000000, 240, 400000, Product.NewVariableLoan, "Reference", 1);

            AddSwitchApplicationCommand command = new AddSwitchApplicationCommand(switchApplication, this.linkedGuid);
            base.Execute<AddSwitchApplicationCommand>(command);
            linkedKey = linkedKeyManager.RetrieveLinkedKey(this.linkedGuid);
            var newApplication = TestApiClient.GetByKey<OfferDataModel>(linkedKey);
            var newApplicationMortgageLoan = TestApiClient.GetByKey<OfferMortgageLoanDataModel>(linkedKey);
            var applicationInfoQuery = new GetLatestApplicationInformationVariableLoanQuery(linkedKey);
            base.PerformQuery(applicationInfoQuery);
            var newApplicationInformation = applicationInfoQuery.Result.Results.First();
            var newApplicationInformationVariableLoan = TestApiClient.GetByKey<OfferInformationVariableLoanDataModel>(newApplicationInformation.OfferInformationKey);
            var newApplicationInformationInterestOnly = TestApiClient.GetByKey<OfferInformationInterestOnlyDataModel>(newApplicationInformation.OfferInformationKey);
            var newApplicationAttribute = TestApiClient.Get<OfferAttributeDataModel>(new { offerkey = linkedKey });

            Assert.NotNull(newApplication, "Application details were not added.");
            Assert.That(newApplication.OfferKey == linkedKey && newApplication.OfferTypeKey == (int)switchApplication.ApplicationType && newApplication.OfferStatusKey == (int)switchApplication.ApplicationStatus
                && newApplication.OfferSourceKey == switchApplication.ApplicationSourceKey && newApplication.ReservedAccountKey > 0
                && newApplication.OriginationSourceKey == (int)OriginationSource.SAHomeLoans, "Incorrect application details were added.");

            Assert.NotNull(newApplicationMortgageLoan, "Application mortgage loan details were not added.");
            Assert.That(newApplicationMortgageLoan.MortgageLoanPurposeKey == (int)MortgageLoanPurpose.Switchloan && newApplicationMortgageLoan.ApplicantTypeKey == (int)ApplicantType.Single
                && newApplicationMortgageLoan.NumApplicants == switchApplication.ApplicantCount && newApplicationMortgageLoan.ResetConfigurationKey == (int)ResetConfiguration.Eighteenth
                && newApplicationMortgageLoan.ClientEstimatePropertyValuation == Convert.ToDouble(switchApplication.EstimatedPropertyValue)
                && newApplicationMortgageLoan.DocumentLanguageKey == (int)Language.English, "Incorrect application mortgage loan details were added.");

            Assert.NotNull(newApplicationInformation, "Application information details were not added.");
            Assert.That(newApplicationInformation.OfferKey == linkedKey && newApplicationInformation.OfferInformationTypeKey == (int)OfferInformationType.OriginalOffer
                && newApplicationInformation.ProductKey == (int)switchApplication.Product, "Incorrect application information details were added.");

            Assert.NotNull(newApplicationInformationVariableLoan, "Application information variable loan details were not added.");
            Assert.That(newApplicationInformationVariableLoan.OfferInformationKey == newApplicationInformation.OfferInformationKey && newApplicationInformationVariableLoan.Term == switchApplication.Term
                && newApplicationInformationVariableLoan.ExistingLoan == Convert.ToDouble(switchApplication.ExistingLoan) && newApplicationInformationVariableLoan.PropertyValuation == Convert.ToDouble(switchApplication.EstimatedPropertyValue)
                && newApplicationInformationVariableLoan.RequestedCashAmount == Convert.ToDouble(switchApplication.CashOut), "Incorrect application information variable loan details were added.");

            Assert.NotNull(newApplicationInformationInterestOnly, "Application information interest only details were not added.");
            Assert.That(newApplicationInformationInterestOnly.OfferInformationKey == newApplicationInformation.OfferInformationKey, "Incorrect application information interest only details were added.");

            Assert.NotNull(newApplicationAttribute, "Application attribute details were not added.");
            Assert.That(newApplicationAttribute.Where(x => x.OfferAttributeTypeKey == (int)OfferAttributeType.ComcorpLoan).First() != null, "Incorrect application attribute details were added.");
        }
    }
}